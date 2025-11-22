using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SportConnect.Migrations
{
    /// <inheritdoc />
    public partial class addmodalidades03 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                    { 46, "Iatismo" },
                    { 47, "Automobilismo" },
                    { 48, "Motociclismo" },
                    { 49, "Breakdance" },
                    { 50, "Xadrez" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 31);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 32);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 33);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 34);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 35);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 36);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 37);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 38);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 39);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 42);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 49);

            migrationBuilder.DeleteData(
                table: "Modalidades",
                keyColumn: "Id",
                keyValue: 50);
        }
    }
}
