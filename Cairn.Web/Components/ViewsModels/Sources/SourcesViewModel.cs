using Cairn.Application.Interfaces.Services;
using Cairn.Application.DTO.FluxBancaires;
using Cairn.Web.GestionSession;

namespace Cairn.Web.Components.ViewsModels.Sources
{
    public class SourcesViewModel(ISynchronisationPowensService synchronisationPowensService,
                                  ICompteBanqueService compteBanqueService,
                                  IBanqueService banqueService,
                                  SessionService sessionService)
    {
        private readonly ISynchronisationPowensService _synchronisationPowensService = synchronisationPowensService;
        private readonly ICompteBanqueService _compteBanqueService = compteBanqueService;
        private readonly IBanqueService _banqueService = banqueService;
        private readonly SessionService _sessionService = sessionService;

        // CONNEXION BANQUE
        public string UrlConnexionPowens { get; set; } = string.Empty;
        public List<SourceDto> Sources { get; set; } = [];
        public bool AucuneSource => Sources.Count == 0;
        public SourceDto? SourceSelectionne { get; set; }


        // USER CONNECTE
        public int IdUser { get; set; }
        public string PrenomUser { get; set; } = string.Empty;

        //MAJ VUE
        public bool Chargement { get; set; } = false;
        public event Action OnChange = null!;
        public void NotifyStateChanged() => OnChange?.Invoke();

        // GESTION D'ERREUR
        public string MessageErreur { get; set; } = string.Empty;
        public bool HasErreur { get; set; } = false;


        public async Task StartLoadData()
        {
            try
            {
                await InitialiserSession();

                await LoadComptesBanque();

                if (AucuneSource)
                    return;

                SourceSelectionne = Sources.First();
            }
            finally
            {
                Chargement = false;
            }
        }

        public async Task FinaliserAjoutBanque(int connectionBanqueId)
        {
            await InitialiserSession();

            try
            {
                await _banqueService.SaveBanque(connectionBanqueId, IdUser);
            }
            catch (Exception ex)
            {
                HasErreur = true;
                MessageErreur = ex.Message;
            }
        }     

        public async Task ChangerSourceSelectionne(SourceDto source)
        {
            SourceSelectionne = source;
        }

        public async Task InitialiserUrlConnexionPowens()
        {
            try
            {
                UrlConnexionPowens = await _synchronisationPowensService.GetUrlConnexionPowens(IdUser);
            }
            catch (Exception ex)
            {
                HasErreur = true;
                MessageErreur = ex.Message;
            }
        }

        public async Task MettreAJourNomSource(SourceDto source)
        {
            await _compteBanqueService.UpdateNom(source, IdUser);
        }

        private async Task InitialiserSession()
        {
            await _sessionService.VerifierInitialisation();
            IdUser = _sessionService.Id;
        }

        private async Task LoadComptesBanque()
        {
            Sources = await _compteBanqueService.GetAllByUserId(IdUser);
        } 
    }
}