using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stride.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUsersRelationship : Migration
    {
        private const string _addInsertUserTriggerCommand =
        """
            CREATE TRIGGER stride_users_after_insert
            AFTER INSERT ON public.AspNetUsers
            INSERT INTO public.stride_users (id)
            VALUES (NEW.id);
        """;

        private const string _dropInsertUserTriggerCommand =
        """
            DROP TRIGGER stride_users_after_insert
            ON public.AspNetUsers
        """;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_stride_users_id",
                table: "AspNetUsers");

            migrationBuilder.AddForeignKey(
                name: "fk_stride_users_users_id",
                table: "stride_users",
                column: "id",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(_addInsertUserTriggerCommand);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_stride_users_users_id",
                table: "stride_users");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_stride_users_id",
                table: "AspNetUsers",
                column: "id",
                principalTable: "stride_users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.Sql(_dropInsertUserTriggerCommand);
        }
    }
}
