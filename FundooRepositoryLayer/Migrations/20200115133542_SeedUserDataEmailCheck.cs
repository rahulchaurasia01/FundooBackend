using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooRepositoryLayer.Migrations
{
    public partial class SeedUserDataEmailCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserDetails",
                columns: table => new
                {
                    UserId = table.Column<Guid>(nullable: false),
                    FirstName = table.Column<string>(nullable: false),
                    LastName = table.Column<string>(nullable: false),
                    EmailId = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Type = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    ModifiedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDetails", x => x.UserId);
                });

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "UserId", "CreatedAt", "EmailId", "FirstName", "IsActive", "LastName", "ModifiedAt", "Password", "Type" },
                values: new object[] { new Guid("af1e0f0a-f4d8-404a-b346-d2970c4079fb"), new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "rahulchaurasia@hotmail.com", "Rahul", true, "Chaurasia", new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "123456789", "Basic" });

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "UserId", "CreatedAt", "EmailId", "FirstName", "IsActive", "LastName", "ModifiedAt", "Password", "Type" },
                values: new object[] { new Guid("7cc7fcb3-d6c0-4984-96d7-b4459ac0cc2f"), new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "rahulchaurasia92@hotmail.com", "Rahul", true, "Chaurasia", new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "123456789", "Advanced" });

            migrationBuilder.CreateIndex(
                name: "IX_UserDetails_EmailId",
                table: "UserDetails",
                column: "EmailId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserDetails");
        }
    }
}
