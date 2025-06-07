using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UnitsConverterWebApp.Migrations
{
    /// <inheritdoc />
    public partial class AddCategoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Unit");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Unit",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Unit_CategoryId",
                table: "Unit",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Unit_Categories_CategoryId",
                table: "Unit",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Unit_Categories_CategoryId",
                table: "Unit");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Unit_CategoryId",
                table: "Unit");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Unit");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Unit",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
