using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bitra.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blocks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PreviousHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nonce = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blocks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TotalMinings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CurrentTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalPeek = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalMinings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Address = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Address);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Hash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SignKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Receiver = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    BlockId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Transactions_Blocks_BlockId",
                        column: x => x.BlockId,
                        principalTable: "Blocks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Blocks",
                columns: new[] { "Id", "Hash", "Nonce", "PreviousHash", "Timestamp" },
                values: new object[] { 1, "e96a594fe292d9ea7fc79b44adf05dd0c26c119ce777bf662cfe0c93b8ef0800", 0, "0", new DateTime(2025, 3, 17, 15, 43, 31, 83, DateTimeKind.Utc).AddTicks(801) });

            migrationBuilder.InsertData(
                table: "TotalMinings",
                columns: new[] { "Id", "CreatedAt", "CurrentTotal", "TotalPeek", "UpdateAt" },
                values: new object[] { 1, new DateTime(2025, 3, 17, 15, 43, 31, 83, DateTimeKind.Utc).AddTicks(3027), 100m, 5000000m, new DateTime(2025, 3, 17, 15, 43, 31, 83, DateTimeKind.Utc).AddTicks(3029) });

            migrationBuilder.InsertData(
                table: "Wallets",
                columns: new[] { "Address", "Balance", "IsActive", "PrivateKey", "PublicKey", "Type" },
                values: new object[] { "BitraServerSecurityAddress", 100m, true, "$2a$11$nrxarLvsM1AO35ybCyvHYudemahhW0F.t9oqKr/fSGBxE6nbsEnWK", "624002ee-f952-46fe-b770-3c91af29966a", "server" });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_BlockId",
                table: "Transactions",
                column: "BlockId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TotalMinings");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "Blocks");
        }
    }
}
