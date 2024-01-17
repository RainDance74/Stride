using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stride.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserLinkingTable : Migration
    {
        private const string _addInsertUserTriggerCommand =
        """
            CREATE OR REPLACE FUNCTION stride_users_after_insert()
            RETURNS TRIGGER AS $$
            BEGIN
                INSERT INTO public.stride_users (id, created_date_time)
                VALUES (NEW.id, NOW());
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            CREATE TRIGGER stride_users_after_insert
            AFTER INSERT ON public."AspNetUsers"
            FOR EACH ROW EXECUTE PROCEDURE stride_users_after_insert();
        """;

        private const string _dropInsertUserTriggerCommand =
        """
            DROP TRIGGER stride_users_after_insert
            ON public."AspNetUsers";

            DROP FUNCTION stride_users_after_insert();
        """;

        private const string _addUpdateUserTriggerCommand =
        """
            CREATE OR REPLACE FUNCTION stride_users_updated_at_trigger()
            RETURNS TRIGGER AS $$
            BEGIN
                IF TG_TABLE_NAME = 'AspNetUsers' THEN
                    UPDATE public.stride_users SET updated_date_time = NOW() WHERE id = NEW.id;
                ELSE
                    NEW.updated_date_time = NOW();
                END IF;
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            CREATE TRIGGER stride_users_updated_at_trigger
            AFTER UPDATE ON public."AspNetUsers"
            FOR EACH ROW EXECUTE PROCEDURE stride_users_updated_at_trigger();

            CREATE TRIGGER stride_users_updated_at_trigger
            AFTER UPDATE ON public.stride_users
            FOR EACH ROW EXECUTE PROCEDURE stride_users_updated_at_trigger();
        """;

        private const string _dropUpdateUserTriggerCommand =
        """
            DROP TRIGGER stride_users_updated_at_trigger
            ON public."AspNetUsers";

            DROP TRIGGER stride_users_updated_at_trigger
            ON public.stride_users;

            DROP FUNCTION stride_users_updated_at_trigger();
        """;

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(_dropInsertUserTriggerCommand);
            migrationBuilder.Sql(_dropUpdateUserTriggerCommand);

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
                name: "fk_todo_lists_stride_users_owner_id",
                table: "todo_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_stride_users_updated_by_user_id",
                table: "todo_lists");

            migrationBuilder.DropTable(
                name: "stride_users");

            migrationBuilder.RenameColumn(
                name: "updated_by_user_id",
                table: "todo_lists",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "owner_id",
                table: "todo_lists",
                newName: "owner");

            migrationBuilder.RenameColumn(
                name: "created_by_user_id",
                table: "todo_lists",
                newName: "created_by");

            migrationBuilder.RenameIndex(
                name: "ix_todo_lists_updated_by_user_id",
                table: "todo_lists",
                newName: "ix_todo_lists_updated_by");

            migrationBuilder.RenameIndex(
                name: "ix_todo_lists_owner_id",
                table: "todo_lists",
                newName: "ix_todo_lists_owner");

            migrationBuilder.RenameIndex(
                name: "ix_todo_lists_created_by_user_id",
                table: "todo_lists",
                newName: "ix_todo_lists_created_by");

            migrationBuilder.RenameColumn(
                name: "updated_by_user_id",
                table: "todo_items",
                newName: "updated_by");

            migrationBuilder.RenameColumn(
                name: "created_by_user_id",
                table: "todo_items",
                newName: "created_by");

            migrationBuilder.RenameIndex(
                name: "ix_todo_items_updated_by_user_id",
                table: "todo_items",
                newName: "ix_todo_items_updated_by");

            migrationBuilder.RenameIndex(
                name: "ix_todo_items_created_by_user_id",
                table: "todo_items",
                newName: "ix_todo_items_created_by");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_users_created_by",
                table: "todo_items",
                column: "created_by",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_items_users_updated_by",
                table: "todo_items",
                column: "updated_by",
                principalTable: "AspNetUsers",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_users_created_by",
                table: "todo_lists",
                column: "created_by",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_users_owner",
                table: "todo_lists",
                column: "owner",
                principalTable: "AspNetUsers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_todo_lists_users_updated_by",
                table: "todo_lists",
                column: "updated_by",
                principalTable: "AspNetUsers",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(_addInsertUserTriggerCommand);
            migrationBuilder.Sql(_addUpdateUserTriggerCommand);

            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_users_created_by",
                table: "todo_items");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_items_users_updated_by",
                table: "todo_items");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_users_created_by",
                table: "todo_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_users_owner",
                table: "todo_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_todo_lists_users_updated_by",
                table: "todo_lists");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "todo_lists",
                newName: "updated_by_user_id");

            migrationBuilder.RenameColumn(
                name: "owner",
                table: "todo_lists",
                newName: "owner_id");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "todo_lists",
                newName: "created_by_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_lists_updated_by",
                table: "todo_lists",
                newName: "ix_todo_lists_updated_by_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_lists_owner",
                table: "todo_lists",
                newName: "ix_todo_lists_owner_id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_lists_created_by",
                table: "todo_lists",
                newName: "ix_todo_lists_created_by_user_id");

            migrationBuilder.RenameColumn(
                name: "updated_by",
                table: "todo_items",
                newName: "updated_by_user_id");

            migrationBuilder.RenameColumn(
                name: "created_by",
                table: "todo_items",
                newName: "created_by_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_items_updated_by",
                table: "todo_items",
                newName: "ix_todo_items_updated_by_user_id");

            migrationBuilder.RenameIndex(
                name: "ix_todo_items_created_by",
                table: "todo_items",
                newName: "ix_todo_items_created_by_user_id");

            migrationBuilder.CreateTable(
                name: "stride_users",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    created_date_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_date_time = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_stride_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_stride_users_users_id",
                        column: x => x.id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "fk_todo_lists_stride_users_owner_id",
                table: "todo_lists",
                column: "owner_id",
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
    }
}
