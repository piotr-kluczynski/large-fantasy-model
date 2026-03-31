using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class FixedTableNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterRace_Characters_CharacterId",
                table: "CharacterRace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterRace",
                table: "CharacterRace");

            migrationBuilder.RenameTable(
                name: "CharacterRace",
                newName: "Races");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterRace_CharacterId",
                table: "Races",
                newName: "IX_Races_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Races",
                table: "Races",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Races_Characters_CharacterId",
                table: "Races",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Races_Characters_CharacterId",
                table: "Races");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Races",
                table: "Races");

            migrationBuilder.RenameTable(
                name: "Races",
                newName: "CharacterRace");

            migrationBuilder.RenameIndex(
                name: "IX_Races_CharacterId",
                table: "CharacterRace",
                newName: "IX_CharacterRace_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterRace",
                table: "CharacterRace",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterRace_Characters_CharacterId",
                table: "CharacterRace",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
