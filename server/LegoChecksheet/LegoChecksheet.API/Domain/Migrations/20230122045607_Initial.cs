using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LegoChecksheet.API.Domain.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LegoSets",
                columns: table => new
                {
                    LegoSetId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegoSets", x => x.LegoSetId);
                });

            migrationBuilder.CreateTable(
                name: "LegoPieces",
                columns: table => new
                {
                    LegoPieceId = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ElementId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Owned = table.Column<bool>(type: "bit", nullable: false),
                    NumberMissing = table.Column<int>(type: "int", nullable: false),
                    LegoSetId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegoPieces", x => x.LegoPieceId);
                    table.ForeignKey(
                        name: "FK_LegoPieces_LegoSets_LegoSetId",
                        column: x => x.LegoSetId,
                        principalTable: "LegoSets",
                        principalColumn: "LegoSetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LegoPieces_LegoSetId",
                table: "LegoPieces",
                column: "LegoSetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LegoPieces");

            migrationBuilder.DropTable(
                name: "LegoSets");
        }
    }
}
