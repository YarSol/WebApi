using Microsoft.EntityFrameworkCore.Migrations;

namespace Jax3.Migrations
{
    public partial class Rename_Index_For_User_Entity_Field_Email : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                table: "Users",
                newName: "UniqueError_User_Email");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "UniqueError_User_Email",
                table: "Users",
                newName: "IX_Users_Email");
        }
    }
}
