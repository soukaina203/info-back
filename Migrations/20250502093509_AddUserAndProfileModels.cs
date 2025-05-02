using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace info_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddUserAndProfileModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_Users_UserId",
                table: "Methods");

            migrationBuilder.DropForeignKey(
                name: "FK_Niveaux_Users_UserId",
                table: "Niveaux");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_Users_UserId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_Users_UserId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_UserId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Services_UserId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Niveaux_UserId",
                table: "Niveaux");

            migrationBuilder.DropIndex(
                name: "IX_Methods_UserId",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Cv",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Niveaux");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Methods");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cv",
                table: "Users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Specialities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Services",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Niveaux",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Methods",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_UserId",
                table: "Specialities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_UserId",
                table: "Services",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Niveaux_UserId",
                table: "Niveaux",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Methods_UserId",
                table: "Methods",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_Users_UserId",
                table: "Methods",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Niveaux_Users_UserId",
                table: "Niveaux",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_Users_UserId",
                table: "Services",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_Users_UserId",
                table: "Specialities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
