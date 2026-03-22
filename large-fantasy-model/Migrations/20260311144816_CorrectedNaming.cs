using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace large_fantasy_model.Migrations
{
    /// <inheritdoc />
    public partial class CorrectedNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Conversation_ConversationsConversationId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_User_UsersUserId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_Games_GamesGameKey",
                table: "GameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_User_UsersUserId",
                table: "GameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_User_SenderUserId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_User_FriendOfUserId",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_User_FriendsUserId",
                table: "UserUser");

            migrationBuilder.RenameColumn(
                name: "FriendsUserId",
                table: "UserUser",
                newName: "FriendsId");

            migrationBuilder.RenameColumn(
                name: "FriendOfUserId",
                table: "UserUser",
                newName: "FriendOfId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_FriendsUserId",
                table: "UserUser",
                newName: "IX_UserUser_FriendsId");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "User",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "SenderUserId",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameColumn(
                name: "MessageId",
                table: "Messages",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderUserId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameColumn(
                name: "UsersUserId",
                table: "GameUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "GamesGameKey",
                table: "GameUser",
                newName: "GamesId");

            migrationBuilder.RenameIndex(
                name: "IX_GameUser_UsersUserId",
                table: "GameUser",
                newName: "IX_GameUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "GameKey",
                table: "Games",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UsersUserId",
                table: "ConversationUser",
                newName: "UsersId");

            migrationBuilder.RenameColumn(
                name: "ConversationsConversationId",
                table: "ConversationUser",
                newName: "ConversationsId");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationUser_UsersUserId",
                table: "ConversationUser",
                newName: "IX_ConversationUser_UsersId");

            migrationBuilder.RenameColumn(
                name: "ConversationId",
                table: "Conversation",
                newName: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Conversation_ConversationsId",
                table: "ConversationUser",
                column: "ConversationsId",
                principalTable: "Conversation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_User_UsersId",
                table: "ConversationUser",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_Games_GamesId",
                table: "GameUser",
                column: "GamesId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_User_UsersId",
                table: "GameUser",
                column: "UsersId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_User_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_User_FriendOfId",
                table: "UserUser",
                column: "FriendOfId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_User_FriendsId",
                table: "UserUser",
                column: "FriendsId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_Conversation_ConversationsId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_ConversationUser_User_UsersId",
                table: "ConversationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_Games_GamesId",
                table: "GameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_GameUser_User_UsersId",
                table: "GameUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_User_SenderId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_User_FriendOfId",
                table: "UserUser");

            migrationBuilder.DropForeignKey(
                name: "FK_UserUser_User_FriendsId",
                table: "UserUser");

            migrationBuilder.RenameColumn(
                name: "FriendsId",
                table: "UserUser",
                newName: "FriendsUserId");

            migrationBuilder.RenameColumn(
                name: "FriendOfId",
                table: "UserUser",
                newName: "FriendOfUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserUser_FriendsId",
                table: "UserUser",
                newName: "IX_UserUser_FriendsUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "User",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "SenderUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Messages",
                newName: "MessageId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                newName: "IX_Messages_SenderUserId");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "GameUser",
                newName: "UsersUserId");

            migrationBuilder.RenameColumn(
                name: "GamesId",
                table: "GameUser",
                newName: "GamesGameKey");

            migrationBuilder.RenameIndex(
                name: "IX_GameUser_UsersId",
                table: "GameUser",
                newName: "IX_GameUser_UsersUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Games",
                newName: "GameKey");

            migrationBuilder.RenameColumn(
                name: "UsersId",
                table: "ConversationUser",
                newName: "UsersUserId");

            migrationBuilder.RenameColumn(
                name: "ConversationsId",
                table: "ConversationUser",
                newName: "ConversationsConversationId");

            migrationBuilder.RenameIndex(
                name: "IX_ConversationUser_UsersId",
                table: "ConversationUser",
                newName: "IX_ConversationUser_UsersUserId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Conversation",
                newName: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_Conversation_ConversationsConversationId",
                table: "ConversationUser",
                column: "ConversationsConversationId",
                principalTable: "Conversation",
                principalColumn: "ConversationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ConversationUser_User_UsersUserId",
                table: "ConversationUser",
                column: "UsersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_Games_GamesGameKey",
                table: "GameUser",
                column: "GamesGameKey",
                principalTable: "Games",
                principalColumn: "GameKey",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GameUser_User_UsersUserId",
                table: "GameUser",
                column: "UsersUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_User_SenderUserId",
                table: "Messages",
                column: "SenderUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_User_FriendOfUserId",
                table: "UserUser",
                column: "FriendOfUserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserUser_User_FriendsUserId",
                table: "UserUser",
                column: "FriendsUserId",
                principalTable: "User",
                principalColumn: "UserId");
        }
    }
}
