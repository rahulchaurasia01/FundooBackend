using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooRepositoryLayer.Migrations
{
    public partial class AddedProfilePicColumntoUserDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePic",
                table: "UserDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePic",
                table: "UserDetails");
        }
    }
}
