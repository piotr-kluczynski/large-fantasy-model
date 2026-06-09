using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class FixFriendMissingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipped",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "RaceId",
                table: "Races");

            migrationBuilder.DropColumn(
                name: "FeatureId",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ItemId",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Background_Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Background_Name",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Background_Option",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "WeaponId",
                table: "Weapons",
                newName: "WeaponName");

            migrationBuilder.RenameColumn(
                name: "SpellId",
                table: "Spells",
                newName: "SpellName");

            migrationBuilder.RenameColumn(
                name: "Subtype",
                table: "Classes",
                newName: "ClassName");


            migrationBuilder.AddColumn<string>(
                name: "FeatureName",
                table: "Features",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ItemName",
                table: "Equipment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<bool>(
                name: "SavingThrow_Wisdom",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "SavingThrow_Strength",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "SavingThrow_Intelligence",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "SavingThrow_Dexterity",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "SavingThrow_Constitution",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<bool>(
                name: "SavingThrow_Charisma",
                table: "Characters",
                type: "bit",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Backgrounds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BackgroundName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Backgrounds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Backgrounds_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Backgrounds_CharacterId",
                table: "Backgrounds",
                column: "CharacterId",
                unique: true);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Backgrounds");


            migrationBuilder.DropColumn(
                name: "FeatureName",
                table: "Features");

            migrationBuilder.DropColumn(
                name: "ItemName",
                table: "Equipment");

            migrationBuilder.DropColumn(
                name: "Level",
                table: "Characters");

            migrationBuilder.RenameColumn(
                name: "WeaponName",
                table: "Weapons",
                newName: "WeaponId");

            migrationBuilder.RenameColumn(
                name: "SpellName",
                table: "Spells",
                newName: "SpellId");

            migrationBuilder.RenameColumn(
                name: "ClassName",
                table: "Classes",
                newName: "Subtype");

            migrationBuilder.AddColumn<bool>(
                name: "Equipped",
                table: "Weapons",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "RaceId",
                table: "Races",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FeatureId",
                table: "Features",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ItemId",
                table: "Equipment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClassId",
                table: "Classes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Level",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "SavingThrow_Wisdom",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "SavingThrow_Strength",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "SavingThrow_Intelligence",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "SavingThrow_Dexterity",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "SavingThrow_Constitution",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<int>(
                name: "SavingThrow_Charisma",
                table: "Characters",
                type: "int",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddColumn<string>(
                name: "Background_Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Background_Name",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Background_Option",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
