using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GYMSystem.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RenameSpecialitiesColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Sepecialites",
                table: "Trainers",
                newName: "Specialities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Specialities",
                table: "Trainers",
                newName: "Sepecialites");
        }
    }
}
