using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class FixedTableNaming1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CharacterProficiency_Characters_CharacterId",
                table: "CharacterProficiency");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CharacterProficiency",
                table: "CharacterProficiency");

            migrationBuilder.RenameTable(
                name: "CharacterProficiency",
                newName: "Proficiencies");

            migrationBuilder.RenameIndex(
                name: "IX_CharacterProficiency_CharacterId",
                table: "Proficiencies",
                newName: "IX_Proficiencies_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Proficiencies",
                table: "Proficiencies",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Proficiencies_Characters_CharacterId",
                table: "Proficiencies",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proficiencies_Characters_CharacterId",
                table: "Proficiencies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Proficiencies",
                table: "Proficiencies");

            migrationBuilder.RenameTable(
                name: "Proficiencies",
                newName: "CharacterProficiency");

            migrationBuilder.RenameIndex(
                name: "IX_Proficiencies_CharacterId",
                table: "CharacterProficiency",
                newName: "IX_CharacterProficiency_CharacterId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CharacterProficiency",
                table: "CharacterProficiency",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CharacterProficiency_Characters_CharacterId",
                table: "CharacterProficiency",
                column: "CharacterId",
                principalTable: "Characters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
