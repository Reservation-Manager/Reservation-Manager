using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YourPlace.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Hotels_HotelID",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_HotelID",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "HotelID",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "Receptionist",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<int>(type: "int", nullable: false),
                    HotelID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receptionist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receptionist_Hotels_HotelID",
                        column: x => x.HotelID,
                        principalTable: "Hotels",
                        principalColumn: "HotelID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1",
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a20793fd-41f5-40ac-8ce5-ca5e57a66cbb", "AQAAAAIAAYagAAAAEPFV9bQDHJh/vDUIgEVM6dTxYqaXHPNBAPhZw4itAa0n5fOL+Vt1G2d7hFkgvEe8BQ==" });

            migrationBuilder.CreateIndex(
                name: "IX_Receptionist_HotelID",
                table: "Receptionist",
                column: "HotelID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Receptionist");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "1");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(13)",
                maxLength: 13,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "HotelID",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Discriminator", "Email", "EmailConfirmed", "FirstName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "Surname", "TwoFactorEnabled", "UserName" },
                values: new object[] { "1", 0, "c13c1120-02e9-40a7-87e6-0a8472a0d0bf", "User", "admin@gmail.com", true, "Admin", false, null, "ADMIN@GMAIL.COM", "Admin", "AQAAAAIAAYagAAAAELOjByJ9RdIIKhxVOD9BRbvxB6kGQN5Dv4X5L6LT34McT9TI8WQk+KUA235z0H/Esg==", null, false, "", "User", false, "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_HotelID",
                table: "AspNetUsers",
                column: "HotelID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Hotels_HotelID",
                table: "AspNetUsers",
                column: "HotelID",
                principalTable: "Hotels",
                principalColumn: "HotelID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
