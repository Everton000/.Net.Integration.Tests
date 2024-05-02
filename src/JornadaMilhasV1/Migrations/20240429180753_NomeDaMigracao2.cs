using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JornadaMilhas.Migrations
{
    /// <inheritdoc />
    public partial class NomeDaMigracao2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataIniciasl",
                table: "OfertasViagem",
                newName: "DataInicial");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataInicial",
                table: "OfertasViagem",
                newName: "DataIniciasl");
        }
    }
}
