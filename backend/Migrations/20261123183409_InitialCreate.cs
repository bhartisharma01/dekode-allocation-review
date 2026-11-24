using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AllocationRuns",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    RunIdentifier = table.Column<string>(type: "longtext", nullable: false),
                    RunDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationRuns", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "AllocationGaps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    GapIdentifier = table.Column<string>(type: "longtext", nullable: false),
                    Store = table.Column<string>(type: "longtext", nullable: false),
                    Brand = table.Column<string>(type: "longtext", nullable: false),
                    ProductType = table.Column<string>(type: "longtext", nullable: false),
                    OriginalShortfall = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RunId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllocationGaps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AllocationGaps_AllocationRuns_RunId",
                        column: x => x.RunId,
                        principalTable: "AllocationRuns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "BatchAllocations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    BatchIdentifier = table.Column<string>(type: "longtext", nullable: false),
                    AllocatedUnits = table.Column<int>(type: "int", nullable: false),
                    GapId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BatchAllocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BatchAllocations_AllocationGaps_GapId",
                        column: x => x.GapId,
                        principalTable: "AllocationGaps",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_AllocationGaps_RunId",
                table: "AllocationGaps",
                column: "RunId");

            migrationBuilder.CreateIndex(
                name: "IX_BatchAllocations_GapId",
                table: "BatchAllocations",
                column: "GapId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BatchAllocations");

            migrationBuilder.DropTable(
                name: "AllocationGaps");

            migrationBuilder.DropTable(
                name: "AllocationRuns");
        }
    }
}
