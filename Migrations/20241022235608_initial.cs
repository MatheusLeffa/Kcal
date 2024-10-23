using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Kcal.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Marca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gramas = table.Column<int>(type: "int", nullable: true),
                    Kcal = table.Column<int>(type: "int", nullable: false),
                    Categoria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DataNascimento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sexo = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Peso = table.Column<int>(type: "int", nullable: false),
                    Altura = table.Column<int>(type: "int", nullable: false),
                    MetabolismoBasal = table.Column<int>(type: "int", nullable: false),
                    DataCadastro = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsumedProducts",
                columns: table => new
                {
                    ProductId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantidade = table.Column<int>(type: "int", nullable: false),
                    DataConsumo = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsumedProducts", x => new { x.ProductId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ConsumedProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConsumedProducts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Categoria", "DataCadastro", "Gramas", "Kcal", "Marca", "Nome" },
                values: new object[,]
                {
                    { new Guid("094d7335-7b16-4d5b-a5fe-7447e7d17eb1"), "Fruta", new DateTime(2024, 10, 22, 23, 56, 8, 390, DateTimeKind.Utc).AddTicks(5926), 50, 20, "Natural", "Laranja" },
                    { new Guid("dbb17be4-0d21-47a6-9513-c935e4a9b62b"), "Grãos", new DateTime(2024, 10, 22, 23, 56, 8, 390, DateTimeKind.Utc).AddTicks(5929), 100, 80, "Namorado", "Arroz" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Altura", "DataCadastro", "DataNascimento", "MetabolismoBasal", "Name", "Peso", "Sexo" },
                values: new object[] { new Guid("6af0fffc-0f59-4298-9528-eeae4856d7f2"), 180, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 10, 22, 23, 56, 8, 390, DateTimeKind.Utc).AddTicks(5917), 0, "Matheus", 80, "M" });

            migrationBuilder.InsertData(
                table: "ConsumedProducts",
                columns: new[] { "ProductId", "UserId", "DataConsumo", "Quantidade" },
                values: new object[,]
                {
                    { new Guid("094d7335-7b16-4d5b-a5fe-7447e7d17eb1"), new Guid("6af0fffc-0f59-4298-9528-eeae4856d7f2"), new DateTime(2024, 10, 22, 23, 56, 8, 390, DateTimeKind.Utc).AddTicks(6140), 1 },
                    { new Guid("dbb17be4-0d21-47a6-9513-c935e4a9b62b"), new Guid("6af0fffc-0f59-4298-9528-eeae4856d7f2"), new DateTime(2024, 10, 22, 23, 56, 8, 390, DateTimeKind.Utc).AddTicks(6143), 2 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsumedProducts_UserId",
                table: "ConsumedProducts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsumedProducts");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
