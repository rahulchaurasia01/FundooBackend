using Microsoft.EntityFrameworkCore.Migrations;

namespace FundooRepositoryLayer.Migrations
{
    public partial class AddedManyToManyRelationshipNotesAndLabelsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_NotesLabel_LabelId",
                table: "NotesLabel",
                column: "LabelId");

            migrationBuilder.CreateIndex(
                name: "IX_NotesLabel_NotesId",
                table: "NotesLabel",
                column: "NotesId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotesLabel_LabelDetails_LabelId",
                table: "NotesLabel",
                column: "LabelId",
                principalTable: "LabelDetails",
                principalColumn: "LabelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotesLabel_NotesDetails_NotesId",
                table: "NotesLabel",
                column: "NotesId",
                principalTable: "NotesDetails",
                principalColumn: "NotesId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotesLabel_LabelDetails_LabelId",
                table: "NotesLabel");

            migrationBuilder.DropForeignKey(
                name: "FK_NotesLabel_NotesDetails_NotesId",
                table: "NotesLabel");

            migrationBuilder.DropIndex(
                name: "IX_NotesLabel_LabelId",
                table: "NotesLabel");

            migrationBuilder.DropIndex(
                name: "IX_NotesLabel_NotesId",
                table: "NotesLabel");
        }
    }
}
