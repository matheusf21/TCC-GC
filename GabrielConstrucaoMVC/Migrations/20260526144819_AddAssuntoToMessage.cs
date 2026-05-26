using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GabrielConstrucaoMVC.Migrations
{
    /// <inheritdoc />
    public partial class AddAssuntoToMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Assunto",
                table: "Messages",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Assunto",
                table: "Messages");
        }
    }
}
