using Microsoft.EntityFrameworkCore.Migrations;

namespace LibTads.Migrations
{
    public partial class Coluna_quantidade_disponivel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "QuantidadeDisponivel",
                table: "Livros",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "QuantidadeDisponivel",
                table: "Livros");
        }
    }
}
