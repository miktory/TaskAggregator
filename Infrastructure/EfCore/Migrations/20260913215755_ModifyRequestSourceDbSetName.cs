using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskAggregator.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ModifyRequestSourceDbSetName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_requests_task_sources_request_source_id",
                table: "requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_task_sources",
                table: "task_sources");

            migrationBuilder.RenameTable(
                name: "task_sources",
                newName: "request_sources");

            migrationBuilder.AddPrimaryKey(
                name: "pk_request_sources",
                table: "request_sources",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_requests_request_sources_request_source_id",
                table: "requests",
                column: "request_source_id",
                principalTable: "request_sources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_requests_request_sources_request_source_id",
                table: "requests");

            migrationBuilder.DropPrimaryKey(
                name: "pk_request_sources",
                table: "request_sources");

            migrationBuilder.RenameTable(
                name: "request_sources",
                newName: "task_sources");

            migrationBuilder.AddPrimaryKey(
                name: "pk_task_sources",
                table: "task_sources",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_requests_task_sources_request_source_id",
                table: "requests",
                column: "request_source_id",
                principalTable: "task_sources",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
