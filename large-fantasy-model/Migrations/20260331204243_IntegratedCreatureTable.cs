using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class IntegratedCreatureTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Creature_AbilityScores_Cha",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_AbilityScores_Con",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_AbilityScores_Dex",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_AbilityScores_Int",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_AbilityScores_Str",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_AbilityScores_Wis",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creature_Alignment",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Creature_ArmorClass_Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Creature_ArmorClass_Value",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Blinded",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Charmed",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Deafened",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Exhausted",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Frightened",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Grappled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Incapacitated",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Invisible",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Necrotic",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Paralyzed",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Petrified",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Poisoned",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Prone",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Restrained",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Stunned",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_ConditionImmunities_Unconscious",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Blinded",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Charmed",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Deafened",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Exhausted",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Frightened",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Grappled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Incapacitated",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Invisible",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Necrotic",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Paralyzed",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Petrified",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Poisoned",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Prone",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Restrained",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Stunned",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Conditions_Unconscious",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_HitPoints_Current",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_HitPoints_Max",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_HitPoints_Temporary",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Creature_Inspiration",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Creature_Name",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Creature_SavingThrows_Cha_Amount",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_SavingThrows_Cha_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_SavingThrows_Con_Amount",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_SavingThrows_Con_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_SavingThrows_Dex_Amount",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_SavingThrows_Dex_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_SavingThrows_Int_Amount",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_SavingThrows_Int_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_SavingThrows_Str_Amount",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_SavingThrows_Str_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_SavingThrows_Wis_Amount",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_SavingThrows_Wis_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Creature_Senses_Blindsight_Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Senses_Blindsight_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Creature_Senses_Darkvision_Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Senses_Darkvision_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Creature_Senses_Tremorsense_Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Senses_Tremorsense_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Creature_Senses_Truesight_Description",
                table: "Characters",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Senses_Truesight_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Shield",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Acrobatics_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Acrobatics_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_AnimalHandling_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_AnimalHandling_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Arcana_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Arcana_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Athletics_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Athletics_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Deception_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Deception_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_History_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_History_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Insight_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Insight_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Intimidation_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Intimidation_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Investigation_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Investigation_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Medicine_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Medicine_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Nature_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Nature_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Perception_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Perception_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Performance_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Performance_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Persuasion_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Persuasion_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Religion_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Religion_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_SleightOfHand_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_SleightOfHand_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Stealth_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Stealth_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Skills_Survival_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Skills_Survival_Level",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Speed_Burrow_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Speed_Burrow_Speed",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Speed_Climb_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Speed_Climb_Speed",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Speed_Fly_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Speed_Fly_Speed",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Speed_Hover_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Speed_Hover_Speed",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Creature_Speed_Swim_Enabled",
                table: "Characters",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Speed_Swim_Speed",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Creature_Speed_Walk",
                table: "Characters",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CharacterDamageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CharacterId = table.Column<int>(type: "int", nullable: false),
                    DamageTypeId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterDamageType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CharacterDamageType_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Dice",
                columns: table => new
                {
                    HitPointsCreatureCharacterId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Sides = table.Column<int>(type: "int", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false),
                    Mod = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dice", x => new { x.HitPointsCreatureCharacterId, x.Id });
                    table.ForeignKey(
                        name: "FK_Dice_Characters_HitPointsCreatureCharacterId",
                        column: x => x.HitPointsCreatureCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    CreatureCharacterId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => new { x.CreatureCharacterId, x.Id });
                    table.ForeignKey(
                        name: "FK_Language_Characters_CreatureCharacterId",
                        column: x => x.CreatureCharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CharacterDamageType_CharacterId",
                table: "CharacterDamageType",
                column: "CharacterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterDamageType");

            migrationBuilder.DropTable(
                name: "Dice");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropColumn(
                name: "Creature_AbilityScores_Cha",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_AbilityScores_Con",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_AbilityScores_Dex",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_AbilityScores_Int",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_AbilityScores_Str",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_AbilityScores_Wis",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Alignment",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ArmorClass_Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ArmorClass_Value",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Blinded",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Charmed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Deafened",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Exhausted",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Frightened",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Grappled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Incapacitated",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Invisible",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Necrotic",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Paralyzed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Petrified",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Poisoned",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Prone",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Restrained",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Stunned",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_ConditionImmunities_Unconscious",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Blinded",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Charmed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Deafened",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Exhausted",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Frightened",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Grappled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Incapacitated",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Invisible",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Necrotic",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Paralyzed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Petrified",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Poisoned",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Prone",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Restrained",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Stunned",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Conditions_Unconscious",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_HitPoints_Current",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_HitPoints_Max",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_HitPoints_Temporary",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Inspiration",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Name",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Cha_Amount",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Cha_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Con_Amount",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Con_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Dex_Amount",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Dex_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Int_Amount",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Int_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Str_Amount",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Str_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Wis_Amount",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_SavingThrows_Wis_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Blindsight_Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Blindsight_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Darkvision_Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Darkvision_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Tremorsense_Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Tremorsense_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Truesight_Description",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Senses_Truesight_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Shield",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Acrobatics_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Acrobatics_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_AnimalHandling_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_AnimalHandling_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Arcana_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Arcana_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Athletics_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Athletics_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Deception_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Deception_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_History_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_History_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Insight_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Insight_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Intimidation_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Intimidation_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Investigation_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Investigation_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Medicine_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Medicine_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Nature_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Nature_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Perception_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Perception_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Performance_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Performance_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Persuasion_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Persuasion_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Religion_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Religion_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_SleightOfHand_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_SleightOfHand_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Stealth_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Stealth_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Survival_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Skills_Survival_Level",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Burrow_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Burrow_Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Climb_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Climb_Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Fly_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Fly_Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Hover_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Hover_Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Swim_Enabled",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Swim_Speed",
                table: "Characters");

            migrationBuilder.DropColumn(
                name: "Creature_Speed_Walk",
                table: "Characters");
        }
    }
}
