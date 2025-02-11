using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Application.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ORDER",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TOTAL_PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    STATUS = table.Column<int>(type: "int", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LAST_MODIFIED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_MODIFIED_BY = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER", x => x.ID);
                }
            );

            migrationBuilder.CreateTable(
                name: "PRODUCT",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    INVENTORY_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NAME = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LAST_MODIFIED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_MODIFIED_BY = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUCT", x => x.ID);
                }
            );

            migrationBuilder.CreateTable(
                name: "INVENTORY",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRODUCT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QUANTITY_AVAILABLE = table.Column<int>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_INVENTORY", x => x.ID);
                    table.ForeignKey(
                        name: "FK_INVENTORY_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateTable(
                name: "ORDER_ITEM",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ORDER_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PRODUCT_ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    QUANTITY = table.Column<int>(type: "int", nullable: false),
                    UNIT_PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PRICE = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CREATED = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CREATED_BY = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LAST_MODIFIED = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LAST_MODIFIED_BY = table.Column<Guid>(
                        type: "uniqueidentifier",
                        nullable: false
                    ),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ORDER_ITEM", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ORDER_ITEM_ORDER_ORDER_ID",
                        column: x => x.ORDER_ID,
                        principalTable: "ORDER",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict
                    );
                    table.ForeignKey(
                        name: "FK_ORDER_ITEM_PRODUCT_PRODUCT_ID",
                        column: x => x.PRODUCT_ID,
                        principalTable: "PRODUCT",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict
                    );
                }
            );

            migrationBuilder.CreateIndex(name: "IX_INVENTORY_ID", table: "INVENTORY", column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_INVENTORY_PRODUCT_ID",
                table: "INVENTORY",
                column: "PRODUCT_ID",
                unique: true
            );

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEM_ID",
                table: "ORDER_ITEM",
                column: "ID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEM_ORDER_ID",
                table: "ORDER_ITEM",
                column: "ORDER_ID"
            );

            migrationBuilder.CreateIndex(
                name: "IX_ORDER_ITEM_PRODUCT_ID",
                table: "ORDER_ITEM",
                column: "PRODUCT_ID"
            );

            migrationBuilder.CreateIndex(name: "IX_PRODUCT_ID", table: "PRODUCT", column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_PRODUCT_NAME",
                table: "PRODUCT",
                column: "NAME",
                unique: true
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "INVENTORY");

            migrationBuilder.DropTable(name: "ORDER_ITEM");

            migrationBuilder.DropTable(name: "ORDER");

            migrationBuilder.DropTable(name: "PRODUCT");
        }
    }
}
