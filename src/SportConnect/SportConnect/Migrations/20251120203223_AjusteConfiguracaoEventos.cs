using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SportConnect.Migrations
{
    /// <inheritdoc />
    public partial class AjusteConfiguracaoEventos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Grupos_GrupoId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Eventos_Grupos_UsuarioId",
                table: "Eventos");

            migrationBuilder.DropForeignKey(
                name: "FK_Grupos_Usuarios_UsuarioId",
                table: "Grupos");

            migrationBuilder.DropIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos");

            migrationBuilder.DropColumn(
                name: "UsuarioId",
                table: "Eventos");

            migrationBuilder.AlterColumn<int>(
                name: "GrupoId",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CriadorId",
                table: "Eventos",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_CriadorId",
                table: "Eventos",
                column: "CriadorId");

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

            migrationBuilder.DropColumn(
                name: "CriadorId",
                table: "Eventos");

            migrationBuilder.AlterColumn<int>(
                name: "GrupoId",
                table: "Eventos",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "UsuarioId",
                table: "Eventos",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Eventos_UsuarioId",
                table: "Eventos",
                column: "UsuarioId");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Grupos_GrupoId",
                table: "Eventos",
                column: "GrupoId",
                principalTable: "Grupos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Eventos_Grupos_UsuarioId",
                table: "Eventos",
                column: "UsuarioId",
                principalTable: "Grupos",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Grupos_Usuarios_UsuarioId",
                table: "Grupos",
                column: "UsuarioId",
                principalTable: "Usuarios",
                principalColumn: "Id");
        }
    }
}
