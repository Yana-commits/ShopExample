using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FruitShop.Host.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence(
                name: "fruit_item_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "fruit_sort_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "fruit_type_hilo",
                incrementBy: 10);

            migrationBuilder.CreateSequence(
                name: "provider_hilo",
                incrementBy: 10);

            migrationBuilder.CreateTable(
                name: "FruitSorts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Sort = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitSorts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FruitTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Providers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Providers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FruitItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    FruitTypeId = table.Column<int>(type: "integer", nullable: false),
                    FruitSortId = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Price = table.Column<decimal>(type: "numeric", nullable: false),
                    ProviderId = table.Column<int>(type: "integer", nullable: false),
                    PictureUrl = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FruitItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FruitItems_FruitSorts_FruitSortId",
                        column: x => x.FruitSortId,
                        principalTable: "FruitSorts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FruitItems_FruitTypes_FruitTypeId",
                        column: x => x.FruitTypeId,
                        principalTable: "FruitTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FruitItems_Providers_ProviderId",
                        column: x => x.ProviderId,
                        principalTable: "Providers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FruitItems_FruitSortId",
                table: "FruitItems",
                column: "FruitSortId");

            migrationBuilder.CreateIndex(
                name: "IX_FruitItems_FruitTypeId",
                table: "FruitItems",
                column: "FruitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FruitItems_ProviderId",
                table: "FruitItems",
                column: "ProviderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FruitItems");

            migrationBuilder.DropTable(
                name: "FruitSorts");

            migrationBuilder.DropTable(
                name: "FruitTypes");

            migrationBuilder.DropTable(
                name: "Providers");

            migrationBuilder.DropSequence(
                name: "fruit_item_hilo");

            migrationBuilder.DropSequence(
                name: "fruit_sort_hilo");

            migrationBuilder.DropSequence(
                name: "fruit_type_hilo");

            migrationBuilder.DropSequence(
                name: "provider_hilo");
        }
    }
}
