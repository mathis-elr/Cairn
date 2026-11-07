using Cairn.Application.Interfaces.Services;
using Cairn.Application.DTO.FluxBancaires;
using Cairn.Application.Interfaces.APIs;
using Cairn.Web.GestionSession;
using Cairn.Domain.Enums;
using Cairn.Application.DTO.MacroCategories;
using Cairn.Application.DTO.MicroCategories;

namespace Cairn.Web.Components.ViewsModels.Sources.CompteCourant
{
    public class CompteCourantViewModel(IMacroCategorieService macroCategorieService,
                                        IMicroCategorieService microCategorieService,
                                        IFluxBancaireService fluxBancaireService,
                                        IPowensApiService powensApiService,
                                        SessionService sessionService)
    {
        private readonly IMacroCategorieService _macroCategorieService = macroCategorieService;
        private readonly IMicroCategorieService _microCategorieService = microCategorieService;
        private readonly IFluxBancaireService _fluxBancaireService = fluxBancaireService;
        private readonly IPowensApiService _powensApiService = powensApiService;
        private readonly SessionService _sessionService = sessionService;

        public SourceDto CompteCourant { get; set; } = null!;

        // USER CONNECTE
        public int IdUser { get; set; }
        public string PrenomUser { get; set; } = string.Empty;

        //MAJ VUE
        public event Action OnChange = null!;
        public void NotifyStateChanged() => OnChange?.Invoke();

        // FLUX BANCAIRES
        public List<FluxBancaireDto> FluxCompte { get; set; } = [];


        // RECAPITULATIF GLOBAL
        public IEnumerable<PeriodeBudget> PeriodeBudgetPossibles => Enum.GetValues<PeriodeBudget>();
        public PeriodeBudget PeriodeBudgetSelectionnee { get; set; } = PeriodeBudget.Tout;

        public IEnumerable<BudgetsByRoleMacroCategorieDto> BudgetLineCharts { get; set; } = [];

        public decimal DepensesMoyennnes => BudgetLineCharts
            .Where(b => b.RoleCategorie == RoleCategorie.Depense)
            .SelectMany(b => b.BudgetCategorieParMois)
            .GroupBy(m => m.Date)
            .Select(g => g.Sum(m => m.Budget))
            .DefaultIfEmpty(0)
            .Average();

        public decimal TauxEpargne => BudgetLineCharts
            .SelectMany(b => b.BudgetCategorieParMois, (b, m) => new { b.RoleCategorie, m.Budget })
            .GroupBy(_ => 1)
            .Select(g =>
            {
                var revenus = g.Where(x => x.RoleCategorie == RoleCategorie.Revenu)
                               .Sum(x => x.Budget);
                var epargne = g.Where(x => x.RoleCategorie == RoleCategorie.Epargne || x.RoleCategorie == RoleCategorie.Investissement)
                               .Sum(x => x.Budget);
                return revenus == 0 ? 0 : Math.Abs(epargne) / revenus;
            })
            .FirstOrDefault();

        public decimal SoldeMoyenFinMois => BudgetLineCharts
            .SelectMany(b => b.BudgetCategorieParMois)
            .GroupBy(m => m.Date)
            .Select(g => g.Sum(m => m.Budget))
            .DefaultIfEmpty(0)
            .Average();


        // HISTORIQUE MENSUEL
        public DateTime DateDebut { get; set; } = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1).AddMonths(-2);
        public List<StatutParMoisVM> StatutsParMois { get; set; } = [];
        public StatutParMoisVM? StatutMoisActif { get; set; } = null;
        public DateTime? DateActive { get; set; } = null;
        public string? DateActiveString => DateActive?.ToString("MMMM yyyy");

        // ENREGISTREMENT MENSUEL
        public DateTime DateEditMensuel { get; set; } = DateTime.UtcNow;
        public List<FluxBancaireDto> FluxMensuel { get; set; } = [];
        public List<FluxBancaireDto> CreditsFluxMensuel => FluxMensuel.Where(f => f.Valeur >= 0).ToList();
        public List<FluxBancaireDto> DebitsFluxMensuel => FluxMensuel.Where(f => f.Valeur < 0).ToList();
        public IEnumerable<MacroCategorieDto> MacroCategories { get; set; } = [];
        public IEnumerable<MicroCategorieDto> MicroCategories { get; set; } = [];
        public List<ValeurParCategorieBarChartDto> StatsGraphique { get; set; } = [];
        public bool ActionEnCours { get; set; } = false;

        // GESTION D'ERREUR
        public string MessageErreur { get; set; } = string.Empty;
        public bool HasErreur { get; set; } = false;

        public async Task StartLoadData(SourceDto sourceSelectionnee)
        {
            ActionEnCours = true;

            try
            {
                await InitialiserSession();

                CompteCourant = sourceSelectionnee;

                await Task.WhenAll(
                    LoadFluxCompte(),
                    LoadMacroCategories(),
                    LoadMicroCategories()
                );

                await LoadBudgetParCategorie();

                DateDebut = FluxCompte.Count != 0 ? FluxCompte.Min(f => f.Date) : DateDebut;
                DeterminerStatutMois();
            }
            finally
            {
                ActionEnCours = false;
            }
        }

        public void SetRecapGlobal()
        {
            FluxMensuel = [];
            DateActive = null;
            StatutMoisActif = null;

            NotifyStateChanged();
        }

        public void LoadFluxUnMois(StatutParMoisVM statutMoisDto)
        {
            StatutMoisActif = statutMoisDto;
            var date = statutMoisDto.Date;

            DateActive = date;

            DateEditMensuel = date;
            FluxMensuel = FluxCompte
                .Where(f => f.Date.Month == date.Month && f.Date.Year == date.Year)
                .OrderByDescending(f => f.Date)
                .ToList();

            if (statutMoisDto.Statut == Statut.complete)
                CalculerStatsGraphique();

            NotifyStateChanged();
        }

        public async Task UpdateFluxMensuel()
        {
            ActionEnCours = true;

            if (FluxMensuel == null)
                throw new Exception("Aucune données mensuel");

            await _fluxBancaireService.UpdateRange(FluxMensuel, IdUser);

            await LoadFluxCompte();
            LoadFluxUnMois(StatutMoisActif!);
            CalculerStatsGraphique();

            DeterminerStatutMois();

            ActionEnCours = false;

            NotifyStateChanged();
        }


        public void EditerMoisComplete()
        {
            StatutMoisActif!.Statut = Statut.edition;
            NotifyStateChanged();
        }

        public string GetLibellePeriodeBudget(PeriodeBudget periode)
        {
            return periode switch
            {
                PeriodeBudget.TroisMois => "3M",
                PeriodeBudget.SixMois => "6M",
                PeriodeBudget.Ans => "1A",
                PeriodeBudget.Tout => "Tout",
                _ => string.Empty
            };
        }

        public async Task ChangerPeriodeBudgetSelectionnee(PeriodeBudget periode)
        {
            if (PeriodeBudgetSelectionnee == periode)
                return;

            PeriodeBudgetSelectionnee = periode;

            //await RafraichirGraphique();
        }

        private async Task InitialiserSession()
        {
            await _sessionService.VerifierInitialisation();
            IdUser = _sessionService.Id;
        }

        private async Task LoadFluxCompte()
        {
            FluxCompte = await _fluxBancaireService.GetFluxByCompteId(CompteCourant.Id);
        }

        private async Task LoadMicroCategories()
        {
            MicroCategories = await _microCategorieService.GetAll();
        }

        private async Task LoadMacroCategories()
        {
            MacroCategories = await _macroCategorieService.GetAll();
        }

        private async Task LoadBudgetParCategorie()
        {
            BudgetLineCharts = await _fluxBancaireService.GetBudgetMensuelByMacroCategorie(CompteCourant.Id);
        }

        //private async Task RafraichirGraphique()
        //{
        //    ChargementGraphique = true;

        //    try
        //    {
        //        await LoadInvestissementsParMois();

        //        NotifyStateChanged();
        //    }
        //    finally
        //    {
        //        ChargementGraphique = false;
        //    }
        //}

        private void CalculerStatsGraphique()
        {
            StatsGraphique = FluxMensuel
                .GroupBy(f => f.IdCategorie)
                .Select(g => new ValeurParCategorieBarChartDto
                {
                    Categorie = MicroCategories.FirstOrDefault(c => c.Id == g.Key)?.Libelle ?? "Inconnu",
                    Valeur = g.Sum(f => f.Valeur),
                })
                .Where(g => g.Valeur != 0)
                .OrderByDescending(x => x.Valeur)
                .ToList();
        }

        private void DeterminerStatutMois()
        {
            StatutsParMois.Clear();

            var aujourdHui = DateTime.UtcNow;
            var moisCourant = new DateTime(aujourdHui.Year, aujourdHui.Month, 1);
            var moisPrecedent = moisCourant.AddMonths(-1);

            var dateCourante = moisCourant;

            while (dateCourante >= new DateTime(DateDebut.Year, DateDebut.Month, 1))
            {
                var unMois = new StatutParMoisVM { Date = dateCourante };

                bool estMoisCourant =
                    dateCourante.Year == moisCourant.Year &&
                    dateCourante.Month == moisCourant.Month;

                bool estMoisPrecedent =
                    dateCourante.Year == moisPrecedent.Year &&
                    dateCourante.Month == moisPrecedent.Month;

                bool estIndisponible =
                    estMoisCourant ||
                    aujourdHui.Day < 5 && estMoisPrecedent;

                var fluxDuMois = FluxCompte
                    .Where(f => f.Date.Year == dateCourante.Year &&
                                f.Date.Month == dateCourante.Month)
                    .ToList();

                bool fluxExiste = fluxDuMois.Count != 0;
                bool toutesCategoriesCompletes = fluxDuMois.All(f => f.IdCategorie != 0);

                if (estIndisponible)
                    unMois.Statut = Statut.indisponible;
                else if (fluxExiste && toutesCategoriesCompletes)
                    unMois.Statut = Statut.complete;
                else if (fluxExiste)
                    unMois.Statut = Statut.a_completer;
                else
                    unMois.Statut = Statut.aucune_donnees;

                StatutsParMois.Add(unMois);

                dateCourante = dateCourante.AddMonths(-1);
            }

            if (DateActive.HasValue)
            {
                StatutMoisActif = StatutsParMois.FirstOrDefault(m =>
                    m.Date.Month == DateActive.Value.Month &&
                    m.Date.Year == DateActive.Value.Year);
            }
        }
    }
}
