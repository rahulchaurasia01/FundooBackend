using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooRepositoryLayer.Migrations
{
    public partial class RemovedGuidColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails");

            migrationBuilder.DeleteData(
                table: "UserDetails",
                keyColumn: "UserId",
                keyValue: new Guid("7cc7fcb3-d6c0-4984-96d7-b4459ac0cc2f"));

            migrationBuilder.DeleteData(
                table: "UserDetails",
                keyColumn: "UserId",
                keyValue: new Guid("af1e0f0a-f4d8-404a-b346-d2970c4079fb"));

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserDetails");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserDetails",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails",
                column: "FirstName");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "UserDetails",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "UserDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDetails",
                table: "UserDetails",
                column: "UserId");

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "UserId", "CreatedAt", "EmailId", "FirstName", "IsActive", "LastName", "ModifiedAt", "Password", "Type" },
                values: new object[] { new Guid("af1e0f0a-f4d8-404a-b346-d2970c4079fb"), new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "rahulchaurasia@hotmail.com", "Rahul", true, "Chaurasia", new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "123456789", "Basic" });

            migrationBuilder.InsertData(
                table: "UserDetails",
                columns: new[] { "UserId", "CreatedAt", "EmailId", "FirstName", "IsActive", "LastName", "ModifiedAt", "Password", "Type" },
                values: new object[] { new Guid("7cc7fcb3-d6c0-4984-96d7-b4459ac0cc2f"), new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "rahulchaurasia92@hotmail.com", "Rahul", true, "Chaurasia", new DateTime(2020, 1, 15, 19, 5, 42, 677, DateTimeKind.Local), "123456789", "Advanced" });
        }
    }
}
