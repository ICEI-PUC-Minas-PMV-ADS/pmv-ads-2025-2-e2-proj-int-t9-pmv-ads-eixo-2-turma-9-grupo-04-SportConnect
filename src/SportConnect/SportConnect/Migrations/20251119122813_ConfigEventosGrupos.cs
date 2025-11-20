using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportConnect.Migrations
{
    /// <inheritdoc />
    public partial class ConfigEventosGrupos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_Usuarios_UsuarioId",
                table: "Grupos");

            migrationBuilder.AddColumn<int>(
                name: "CriadorId",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "GrupoId",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CriadorId",
                table: "Eventos",
                column: "CriadorId");

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_GrupoId",
                table: "Eventos",
                column: "GrupoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Grupos_GrupoId",
                table: "Eventos",
                column: "GrupoId",
                principalTable: "Grupos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Usuarios_CriadorId",
                table: "Eventos",
                column: "CriadorId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_Usuarios_UsuarioId",
                table: "Grupos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Grupos_GrupoId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Usuarios_CriadorId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_Usuarios_UsuarioId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_CriadorId",
                table: "Eventos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_GrupoId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "CriadorId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "GrupoId",
                table: "Eventos");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_Usuarios_UsuarioId",
                table: "Grupos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
