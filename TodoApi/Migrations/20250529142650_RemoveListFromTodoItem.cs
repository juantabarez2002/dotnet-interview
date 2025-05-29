using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveListFromTodoItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoList_ListId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_ListId",
                table: "TodoItems");

            migrationBuilder.AddColumn<long>(
                name: "TodoListId",
                table: "TodoItems",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_TodoListId",
                table: "TodoItems",
                column: "TodoListId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoList_TodoListId",
                table: "TodoItems",
                column: "TodoListId",
                principalTable: "TodoList",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoList_TodoListId",
                table: "TodoItems");

            migrationBuilder.DropIndex(
                name: "IX_TodoItems_TodoListId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "TodoListId",
                table: "TodoItems");

            migrationBuilder.CreateIndex(
                name: "IX_TodoItems_ListId",
                table: "TodoItems",
                column: "ListId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoList_ListId",
                table: "TodoItems",
                column: "ListId",
                principalTable: "TodoList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
