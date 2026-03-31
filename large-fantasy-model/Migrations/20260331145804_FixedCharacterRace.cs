using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class FixedCharacterRace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Race_Id",
                table: "Characters");

            migrationBuilder.CreateTable(
                name: "CharacterRace",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    RaceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterRace", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterRace_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterRace_CharacterId",
                table: "CharacterRace",
                column: "CharacterId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterRace");

            migrationBuilder.AddColumn<int>(
                name: "RaceId",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Race_Id",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
