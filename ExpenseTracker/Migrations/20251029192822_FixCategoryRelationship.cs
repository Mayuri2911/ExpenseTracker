using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Migrations
{
    /// <inheritdoc />
    public partial class FixCategoryRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategoryId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoriesCategoryId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "CategoriesCategoryId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoryId",
                schema: "dbo",
                table: "Transactions",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                schema: "dbo",
                table: "Transactions",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Categories_CategoryId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_CategoryId",
                schema: "dbo",
                table: "Transactions");

            migrationBuilder.AddColumn<int>(
                name: "CategoriesCategoryId",
                schema: "dbo",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_CategoriesCategoryId",
                schema: "dbo",
                table: "Transactions",
                column: "CategoriesCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Categories_CategoriesCategoryId",
                schema: "dbo",
                table: "Transactions",
                column: "CategoriesCategoryId",
                principalTable: "Categories",
                principalColumn: "CategoryId");
        }
    }
}
