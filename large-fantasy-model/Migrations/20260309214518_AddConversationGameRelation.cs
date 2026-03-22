using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationGameRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ConversationId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Games_ConversationId",
                table: "Games",
                column: "ConversationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Conversation_ConversationId",
                table: "Games",
                column: "ConversationId",
                principalTable: "Conversation",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Conversation_ConversationId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_Games_ConversationId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Games");
        }
    }
}
