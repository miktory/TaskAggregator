using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskAggregator.Infrastructure.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddUqConstraintForRequestSource : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "ix_request_sources_code",
                table: "request_sources",
                column: "code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_request_sources_code",
                table: "request_sources");
        }
    }
}
