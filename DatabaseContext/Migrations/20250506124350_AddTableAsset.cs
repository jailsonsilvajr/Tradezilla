using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatabaseContext.Migrations
{
    /// <inheritdoc />
    public partial class AddTableAsset : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_Accounts_AccountId",
                table: "Deposits");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_AccountId",
                table: "Deposits");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Deposits");

            migrationBuilder.AlterColumn<Guid>(
                name: "AssetId",
                table: "Deposits",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(5)",
                oldMaxLength: 5);

            migrationBuilder.CreateTable(
                name: "Assets",
                columns: table => new
                {
                    AssetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssetName = table.Column<string>(type: "varchar(5)", maxLength: 5, nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.AssetId);
                    table.ForeignKey(
                        name: "FK_Assets_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "AccountId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_AssetId",
                table: "Deposits",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_AccountId",
                table: "Assets",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_Assets_AssetId",
                table: "Deposits",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "AssetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Deposits_Assets_AssetId",
                table: "Deposits");

            migrationBuilder.DropTable(
                name: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Deposits_AssetId",
                table: "Deposits");

            migrationBuilder.AlterColumn<string>(
                name: "AssetId",
                table: "Deposits",
                type: "varchar(5)",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                table: "Deposits",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_Deposits_AccountId",
                table: "Deposits",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Deposits_Accounts_AccountId",
                table: "Deposits",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
