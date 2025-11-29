using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SportConnect.Migrations
{
    /// <inheritdoc />
    public partial class sportconnectmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Modalidades",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modalidades", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notificacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    Mensagem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DataEnvio = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    Lida = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notificacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participacoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UsuarioId = table.Column<int>(type: "int", nullable: false),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    StatusParticipacao = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    DataInscricao = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participacoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cpf = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataDeNascimento = table.Column<DateOnly>(type: "date", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Senha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Grupos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroMaximoParticipantes = table.Column<int>(type: "int", nullable: false),
                    ListaEspera = table.Column<bool>(type: "bit", nullable: false),
                    Modalidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UsuarioId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grupos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Grupos_Usuarios_UsuarioId",
                        column: x => x.UsuarioId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Eventos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nome = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descricao = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataEvento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Cidade = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bairro = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rua = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: true),
                    Longitude = table.Column<double>(type: "float", nullable: true),
                    GrupoId = table.Column<int>(type: "int", nullable: false),
                    CriadorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Eventos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Eventos_Grupos_GrupoId",
                        column: x => x.GrupoId,
                        principalTable: "Grupos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Eventos_Usuarios_CriadorId",
                        column: x => x.CriadorId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Modalidades",
                columns: new[] { "Id", "Nome" },
                values: new object[,]
                {
                    { 1, "Futebol" },
                    { 2, "Vôlei" },
                    { 3, "Futsal" },
                    { 4, "Basquete" },
                    { 5, "Natação" },
                    { 6, "Caminhada e Corrida" },
                    { 7, "Ciclismo" },
                    { 8, "Tênis de Mesa" },
                    { 9, "Musculação" },
                    { 10, "Surf" },
                    { 11, "Skate" },
                    { 12, "Judô" },
                    { 13, "Jiu-Jitsu" },
                    { 14, "Boxe" },
                    { 15, "Capoeira" },
                    { 16, "Handebol" },
                    { 17, "Vôlei de Praia" },
                    { 18, "Futevôlei" },
                    { 19, "Tênis" },
                    { 20, "Atletismo" },
                    { 21, "Ginástica Artística" },
                    { 22, "Ginástica Rítmica" },
                    { 23, "Taekwondo" },
                    { 24, "Karatê" },
                    { 25, "Mountain Bike" },
                    { 26, "Canoagem" },
                    { 27, "Remo" },
                    { 28, "Polo Aquático" },
                    { 29, "Halterofilismo" },
                    { 30, "Golfe" },
                    { 31, "Beisebol" },
                    { 32, "Softbol" },
                    { 33, "Futebol Americano" },
                    { 34, "Rugby" },
                    { 35, "Esgrima" },
                    { 36, "Tiro Esportivo" },
                    { 37, "Hipismo" },
                    { 38, "Badminton" },
                    { 39, "Squash" },
                    { 40, "Peteca" },
                    { 41, "Beach Tennis" },
                    { 42, "Padel" },
                    { 43, "Pesca Esportiva" },
                    { 44, "Triatlo" },
                    { 45, "Muay Thai" },
                    { 46, "Iatismo/Vela" },
                    { 47, "Automobilismo" },
                    { 48, "Motociclismo" },
                    { 49, "Dança Esportiva" },
                    { 50, "Xadrez" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CriadorId",
                table: "Eventos",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_GrupoId",
                table: "Eventos",
                column: "GrupoId");

            migrationBuilder.CreateIndex(
                name: "IX_Grupos_UsuarioId",
                table: "Grupos",
                column: "UsuarioId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Eventos");

            migrationBuilder.DropTable(
                name: "Modalidades");

            migrationBuilder.DropTable(
                name: "Notificacoes");

            migrationBuilder.DropTable(
                name: "Participacoes");

            migrationBuilder.DropTable(
                name: "Grupos");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
