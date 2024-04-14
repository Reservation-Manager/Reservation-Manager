using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class verified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "8b7f2c2b-a4f9-4c02-bf93-cdabd397714c", "AQAAAAIAAYagAAAAEDqfNTTYFe/FPdECvOH0JDM5o7olADhryJY9gAPPJ0y/9/UFC0RwjsmF3/bXcgKlOQ==" });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationID",
                keyValue: 1,
                columns: new[] { "Email", "PhoneNumber", "Verified" },
                values: new object[] { "ipetrov@gmail.com", "0876155488", true });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationID",
                keyValue: 2,
                columns: new[] { "Email", "PhoneNumber", "Verified" },
                values: new object[] { "mariaivanova@gmail.com", "0876155489", true });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationID",
                keyValue: 3,
                columns: new[] { "Email", "PhoneNumber", "Verified" },
                values: new object[] { "peturivanov@gmail.com", "0876155648", true });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationID",
                keyValue: 4,
                columns: new[] { "Email", "PhoneNumber", "Verified" },
                values: new object[] { "gerip@gmail.com", "0876155428", true });

            migrationBuilder.UpdateData(
                table: "Reservations",
                keyColumn: "ReservationID",
                keyValue: 5,
                columns: new[] { "Email", "PhoneNumber", "Verified" },
                values: new object[] { "stefangeorgiev@gmail.com", "0876156489", true });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "Verified",
                table: "Reservations");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "136a14f2-521f-4666-bac2-c53b91b7ead7", "AQAAAAIAAYagAAAAEAFyiMFowqLo8yaBBq3/UjRUf+2UtNv44+kQZQADaC+ILtOSp1j3zEfTNOnckd1pHQ==" });
        }
    }
}
