using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Stride.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixUsersRelationship : Migration
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
            migrationBuilder.Sql(_addUpdateUserTriggerCommand);
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
            migrationBuilder.Sql(_dropUpdateUserTriggerCommand);
        }
    }
}
