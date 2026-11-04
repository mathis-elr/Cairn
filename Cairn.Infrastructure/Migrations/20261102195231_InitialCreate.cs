using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Cairn.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Actif",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPowens = table.Column<int>(type: "int", nullable: false),
                    Libelle = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CodePowens = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Symbole = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    DateRechercheSymbole = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<byte>(type: "tinyint", nullable: false),
                    LogoSource = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Actif", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Utilisateur",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Prenom = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MdpHash = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    DateCreationCompte = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Utilisateur", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoursActif",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActifId = table.Column<int>(type: "int", nullable: false),
                    Prix = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoursActif", x => new { x.ActifId, x.Date });
                    table.ForeignKey(
                        name: "FK_CoursActif_Actif_ActifId",
                        column: x => x.ActifId,
                        principalTable: "Actif",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdConnectionPowens = table.Column<int>(type: "int", nullable: false),
                    IdConnectorPowens = table.Column<int>(type: "int", nullable: false),
                    Nom = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StatutConnexion = table.Column<byte>(type: "tinyint", nullable: false),
                    DerniereSynchro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banque_Utilisateur_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MacroCategorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Role = table.Column<byte>(type: "tinyint", nullable: false),
                    Ordre = table.Column<byte>(type: "tinyint", nullable: false),
                    CouleurHexa = table.Column<string>(type: "char(7)", unicode: false, fixedLength: true, maxLength: 7, nullable: false),
                    EstActive = table.Column<bool>(type: "bit", nullable: false),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MacroCategorie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MacroCategorie_Utilisateur_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UtilisateurPowens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPowens = table.Column<int>(type: "int", nullable: false),
                    AccessTokenCrypte = table.Column<string>(type: "nvarchar(max)", maxLength: 200, nullable: false),
                    UtilisateurId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UtilisateurPowens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UtilisateurPowens_Utilisateur_UtilisateurId",
                        column: x => x.UtilisateurId,
                        principalTable: "Utilisateur",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CompteBanque",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPowens = table.Column<int>(type: "int", nullable: false),
                    TypePowens = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    TypeCompte = table.Column<byte>(type: "tinyint", nullable: false),
                    NomPowens = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    NomPersonnalise = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Solde = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DerniereSynchro = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BanqueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompteBanque", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompteBanque_Banque_BanqueId",
                        column: x => x.BanqueId,
                        principalTable: "Banque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MicroCategorie",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Libelle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Ordre = table.Column<byte>(type: "tinyint", nullable: false),
                    EstActive = table.Column<bool>(type: "bit", nullable: false),
                    MacroCategorieId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MicroCategorie", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MicroCategorie_MacroCategorie_MacroCategorieId",
                        column: x => x.MacroCategorieId,
                        principalTable: "MacroCategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "HistoriqueSoldeCompte",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    CompteBanqueId = table.Column<int>(type: "int", nullable: false),
                    Solde = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MontantInvesti = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HistoriqueSoldeCompte", x => new { x.CompteBanqueId, x.Date });
                    table.ForeignKey(
                        name: "FK_HistoriqueSoldeCompte_CompteBanque_CompteBanqueId",
                        column: x => x.CompteBanqueId,
                        principalTable: "CompteBanque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PositionInvestissement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPowens = table.Column<int>(type: "int", nullable: false),
                    Quantite = table.Column<decimal>(type: "decimal(18,8)", precision: 18, scale: 8, nullable: false),
                    PrixAchat = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    PrixCourant = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false),
                    Valorisation = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DateCours = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DerniereMaj = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ActifId = table.Column<int>(type: "int", nullable: false),
                    CompteBanqueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PositionInvestissement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PositionInvestissement_Actif_ActifId",
                        column: x => x.ActifId,
                        principalTable: "Actif",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PositionInvestissement_CompteBanque_CompteBanqueId",
                        column: x => x.CompteBanqueId,
                        principalTable: "CompteBanque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FluxBancaire",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IdPowens = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Valeur = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    LibellePowens = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    LibellePersonnalise = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    EstSuggestion = table.Column<bool>(type: "bit", nullable: false),
                    MicroCategorieId = table.Column<int>(type: "int", nullable: true),
                    CompteBanqueId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FluxBancaire", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FluxBancaire_CompteBanque_CompteBanqueId",
                        column: x => x.CompteBanqueId,
                        principalTable: "CompteBanque",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FluxBancaire_MicroCategorie_MicroCategorieId",
                        column: x => x.MicroCategorieId,
                        principalTable: "MicroCategorie",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Actif_IdPowens",
                table: "Actif",
                column: "IdPowens",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banque_IdConnectionPowens",
                table: "Banque",
                column: "IdConnectionPowens",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Banque_UtilisateurId",
                table: "Banque",
                column: "UtilisateurId");

            migrationBuilder.CreateIndex(
                name: "IX_CompteBanque_BanqueId",
                table: "CompteBanque",
                column: "BanqueId");

            migrationBuilder.CreateIndex(
                name: "IX_CompteBanque_IdPowens",
                table: "CompteBanque",
                column: "IdPowens",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FluxBancaire_CompteBanqueId_Date",
                table: "FluxBancaire",
                columns: new[] { "CompteBanqueId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_FluxBancaire_IdPowens",
                table: "FluxBancaire",
                column: "IdPowens",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FluxBancaire_MicroCategorieId",
                table: "FluxBancaire",
                column: "MicroCategorieId");

            migrationBuilder.CreateIndex(
                name: "IX_MacroCategorie_UtilisateurId_Libelle",
                table: "MacroCategorie",
                columns: new[] { "UtilisateurId", "Libelle" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MicroCategorie_MacroCategorieId_Libelle",
                table: "MicroCategorie",
                columns: new[] { "MacroCategorieId", "Libelle" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PositionInvestissement_ActifId",
                table: "PositionInvestissement",
                column: "ActifId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionInvestissement_CompteBanqueId",
                table: "PositionInvestissement",
                column: "CompteBanqueId");

            migrationBuilder.CreateIndex(
                name: "IX_PositionInvestissement_IdPowens",
                table: "PositionInvestissement",
                column: "IdPowens",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Utilisateur_Email",
                table: "Utilisateur",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateurPowens_IdPowens",
                table: "UtilisateurPowens",
                column: "IdPowens",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UtilisateurPowens_UtilisateurId",
                table: "UtilisateurPowens",
                column: "UtilisateurId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoursActif");

            migrationBuilder.DropTable(
                name: "FluxBancaire");

            migrationBuilder.DropTable(
                name: "HistoriqueSoldeCompte");

            migrationBuilder.DropTable(
                name: "PositionInvestissement");

            migrationBuilder.DropTable(
                name: "UtilisateurPowens");

            migrationBuilder.DropTable(
                name: "MicroCategorie");

            migrationBuilder.DropTable(
                name: "Actif");

            migrationBuilder.DropTable(
                name: "CompteBanque");

            migrationBuilder.DropTable(
                name: "MacroCategorie");

            migrationBuilder.DropTable(
                name: "Banque");

            migrationBuilder.DropTable(
                name: "Utilisateur");
        }
    }
}
