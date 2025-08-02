using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechZone.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ModifyFirstName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FisrtName",
                table: "AspNetUsers",
                newName: "FirstName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FirstName",
                table: "AspNetUsers",
                newName: "FisrtName");
        }
    }
}
