using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class FixedTableNaming2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Silver",
                table: "Characters",
                newName: "Currency_Silver");

            migrationBuilder.RenameColumn(
                name: "Platinum",
                table: "Characters",
                newName: "Currency_Platinum");

            migrationBuilder.RenameColumn(
                name: "Gold",
                table: "Characters",
                newName: "Currency_Gold");

            migrationBuilder.RenameColumn(
                name: "Electrum",
                table: "Characters",
                newName: "Currency_Electrum");

            migrationBuilder.RenameColumn(
                name: "Copper",
                table: "Characters",
                newName: "Currency_Copper");

            migrationBuilder.RenameColumn(
                name: "BackgroundOption",
                table: "Characters",
                newName: "Background_Option");

            migrationBuilder.RenameColumn(
                name: "BackgroundName",
                table: "Characters",
                newName: "Background_Name");

            migrationBuilder.RenameColumn(
                name: "BackgroundDescription",
                table: "Characters",
                newName: "Background_Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Currency_Silver",
                table: "Characters",
                newName: "Silver");

            migrationBuilder.RenameColumn(
                name: "Currency_Platinum",
                table: "Characters",
                newName: "Platinum");

            migrationBuilder.RenameColumn(
                name: "Currency_Gold",
                table: "Characters",
                newName: "Gold");

            migrationBuilder.RenameColumn(
                name: "Currency_Electrum",
                table: "Characters",
                newName: "Electrum");

            migrationBuilder.RenameColumn(
                name: "Currency_Copper",
                table: "Characters",
                newName: "Copper");

            migrationBuilder.RenameColumn(
                name: "Background_Option",
                table: "Characters",
                newName: "BackgroundOption");

            migrationBuilder.RenameColumn(
                name: "Background_Name",
                table: "Characters",
                newName: "BackgroundName");

            migrationBuilder.RenameColumn(
                name: "Background_Description",
                table: "Characters",
                newName: "BackgroundDescription");
        }
    }
}
