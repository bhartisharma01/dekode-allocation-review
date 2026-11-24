using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddSchemaConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "BatchIdentifier",
                table: "BatchAllocations",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "RunIdentifier",
                table: "AllocationRuns",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "Store",
                table: "AllocationGaps",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "AllocationGaps",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "GapIdentifier",
                table: "AllocationGaps",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "AllocationGaps",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext");

            // Create replacement index first.
            migrationBuilder.CreateIndex(
                name: "IX_AllocationGaps_RunId_GapIdentifier",
                table: "AllocationGaps",
                columns: new[] { "RunId", "GapIdentifier" },
                unique: true);

            // Now the composite index can support the foreign key.
            migrationBuilder.DropIndex(
                name: "IX_AllocationGaps_RunId",
                table: "AllocationGaps");

            // Add unique constraint for run identifier.
            migrationBuilder.CreateIndex(
                name: "IX_AllocationRuns_RunIdentifier",
                table: "AllocationRuns",
                column: "RunIdentifier",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AllocationRuns_RunIdentifier",
                table: "AllocationRuns");

            migrationBuilder.DropIndex(
                name: "IX_AllocationGaps_RunId_GapIdentifier",
                table: "AllocationGaps");

            migrationBuilder.AlterColumn<string>(
                name: "BatchIdentifier",
                table: "BatchAllocations",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "RunIdentifier",
                table: "AllocationRuns",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Store",
                table: "AllocationGaps",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ProductType",
                table: "AllocationGaps",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "GapIdentifier",
                table: "AllocationGaps",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "AllocationGaps",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_AllocationGaps_RunId",
                table: "AllocationGaps",
                column: "RunId");
        }
    }
}
