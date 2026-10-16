using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskAggregator.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class ChangeRequestSourceModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "code",
                table: "request_sources",
                newName: "external_id");

            migrationBuilder.RenameIndex(
                name: "ix_request_sources_code",
                table: "request_sources",
                newName: "ix_request_sources_external_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "external_id",
                table: "request_sources",
                newName: "code");

            migrationBuilder.RenameIndex(
                name: "ix_request_sources_external_id",
                table: "request_sources",
                newName: "ix_request_sources_code");
        }
    }
}
