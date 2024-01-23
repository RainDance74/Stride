using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stride.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_stride_users_created_by_id",
                table: "todo_items");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_stride_users_updated_by_id",
                table: "todo_items");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_stride_users_created_by_id",
                table: "todo_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_stride_users_updated_by_id",
                table: "todo_lists");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_stride_users_created_by_user_id",
                table: "todo_items",
                column: "created_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_stride_users_updated_by_user_id",
                table: "todo_items",
                column: "updated_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_stride_users_created_by_user_id",
                table: "todo_lists",
                column: "created_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_stride_users_updated_by_user_id",
                table: "todo_lists",
                column: "updated_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_stride_users_created_by_user_id",
                table: "todo_items");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_stride_users_updated_by_user_id",
                table: "todo_items");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_stride_users_created_by_user_id",
                table: "todo_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_stride_users_updated_by_user_id",
                table: "todo_lists");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_stride_users_created_by_id",
                table: "todo_items",
                column: "created_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_stride_users_updated_by_id",
                table: "todo_items",
                column: "updated_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_stride_users_created_by_id",
                table: "todo_lists",
                column: "created_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_stride_users_updated_by_id",
                table: "todo_lists",
                column: "updated_by_user_id",
                principalTable: "stride_users",
                principalColumn: "id");
        }
    }
}
