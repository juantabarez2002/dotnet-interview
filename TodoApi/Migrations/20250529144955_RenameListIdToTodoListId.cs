using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TodoApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameListIdToTodoListId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoList_TodoListId",
                table: "TodoItems");

            migrationBuilder.DropColumn(
                name: "ListId",
                table: "TodoItems");

            migrationBuilder.AlterColumn<long>(
                name: "TodoListId",
                table: "TodoItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoList_TodoListId",
                table: "TodoItems",
                column: "TodoListId",
                principalTable: "TodoList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoItems_TodoList_TodoListId",
                table: "TodoItems");

            migrationBuilder.AlterColumn<long>(
                name: "TodoListId",
                table: "TodoItems",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "ListId",
                table: "TodoItems",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddForeignKey(
                name: "FK_TodoItems_TodoList_TodoListId",
                table: "TodoItems",
                column: "TodoListId",
                principalTable: "TodoList",
                principalColumn: "Id");
        }
    }
}
