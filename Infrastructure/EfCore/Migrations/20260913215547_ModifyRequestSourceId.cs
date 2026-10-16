using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TaskAggregator.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequestSourceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_requests_task_sources_task_source_id",
                table: "requests");

            migrationBuilder.DropIndex(
                name: "ix_requests_task_source_id_external_id",
                table: "requests");

            migrationBuilder.DropColumn(
                name: "task_source_id",
                table: "requests");

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "task_sources",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<long>(
                name: "id",
                table: "requests",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddColumn<long>(
                name: "request_source_id",
                table: "requests",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_requests_request_source_id_external_id",
                table: "requests",
                columns: new[] { "request_source_id", "external_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_requests_task_sources_request_source_id",
                table: "requests",
                column: "request_source_id",
                principalTable: "task_sources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_requests_task_sources_request_source_id",
                table: "requests");

            migrationBuilder.DropIndex(
                name: "ix_requests_request_source_id_external_id",
                table: "requests");

            migrationBuilder.DropColumn(
                name: "request_source_id",
                table: "requests");

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "task_sources",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "requests",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn);

            migrationBuilder.AddColumn<int>(
                name: "task_source_id",
                table: "requests",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_requests_task_source_id_external_id",
                table: "requests",
                columns: new[] { "task_source_id", "external_id" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_requests_task_sources_task_source_id",
                table: "requests",
                column: "task_source_id",
                principalTable: "task_sources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
