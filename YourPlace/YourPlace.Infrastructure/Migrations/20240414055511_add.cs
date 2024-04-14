using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserID",
                table: "Receptionist",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "136a14f2-521f-4666-bac2-c53b91b7ead7", "AQAAAAIAAYagAAAAEAFyiMFowqLo8yaBBq3/UjRUf+2UtNv44+kQZQADaC+ILtOSp1j3zEfTNOnckd1pHQ==" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Receptionist",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a20793fd-41f5-40ac-8ce5-ca5e57a66cbb", "AQAAAAIAAYagAAAAEPFV9bQDHJh/vDUIgEVM6dTxYqaXHPNBAPhZw4itAa0n5fOL+Vt1G2d7hFkgvEe8BQ==" });
        }
    }
}
