using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace info_backend.Migrations
{
    /// <inheritdoc />
    public partial class changeProfProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Methods_ProfProfiles_ProfProfileId",
                table: "Methods");

            migrationBuilder.DropForeignKey(
                name: "FK_Niveaux_ProfProfiles_ProfProfileId",
                table: "Niveaux");

            migrationBuilder.DropForeignKey(
                name: "FK_Services_ProfProfiles_ProfProfileId",
                table: "Services");

            migrationBuilder.DropForeignKey(
                name: "FK_Specialities_ProfProfiles_ProfProfileId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Specialities_ProfProfileId",
                table: "Specialities");

            migrationBuilder.DropIndex(
                name: "IX_Services_ProfProfileId",
                table: "Services");

            migrationBuilder.DropIndex(
                name: "IX_Niveaux_ProfProfileId",
                table: "Niveaux");

            migrationBuilder.DropIndex(
                name: "IX_Methods_ProfProfileId",
                table: "Methods");

            migrationBuilder.DropColumn(
                name: "ProfProfileId",
                table: "Specialities");

            migrationBuilder.DropColumn(
                name: "ProfProfileId",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "ProfProfileId",
                table: "Niveaux");

            migrationBuilder.DropColumn(
                name: "ProfProfileId",
                table: "Methods");

            migrationBuilder.RenameColumn(
                name: "image",
                table: "Services",
                newName: "Image");

            migrationBuilder.AddColumn<int[]>(
                name: "Methodes",
                table: "ProfProfiles",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int[]>(
                name: "Niveaux",
                table: "ProfProfiles",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int[]>(
                name: "Services",
                table: "ProfProfiles",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);

            migrationBuilder.AddColumn<int[]>(
                name: "Specialities",
                table: "ProfProfiles",
                type: "integer[]",
                nullable: false,
                defaultValue: new int[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Methodes",
                table: "ProfProfiles");

            migrationBuilder.DropColumn(
                name: "Niveaux",
                table: "ProfProfiles");

            migrationBuilder.DropColumn(
                name: "Services",
                table: "ProfProfiles");

            migrationBuilder.DropColumn(
                name: "Specialities",
                table: "ProfProfiles");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Services",
                newName: "image");

            migrationBuilder.AddColumn<int>(
                name: "ProfProfileId",
                table: "Specialities",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfProfileId",
                table: "Services",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfProfileId",
                table: "Niveaux",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfProfileId",
                table: "Methods",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Specialities_ProfProfileId",
                table: "Specialities",
                column: "ProfProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Services_ProfProfileId",
                table: "Services",
                column: "ProfProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Niveaux_ProfProfileId",
                table: "Niveaux",
                column: "ProfProfileId");

            migrationBuilder.CreateIndex(
                name: "IX_Methods_ProfProfileId",
                table: "Methods",
                column: "ProfProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Methods_ProfProfiles_ProfProfileId",
                table: "Methods",
                column: "ProfProfileId",
                principalTable: "ProfProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Niveaux_ProfProfiles_ProfProfileId",
                table: "Niveaux",
                column: "ProfProfileId",
                principalTable: "ProfProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Services_ProfProfiles_ProfProfileId",
                table: "Services",
                column: "ProfProfileId",
                principalTable: "ProfProfiles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Specialities_ProfProfiles_ProfProfileId",
                table: "Specialities",
                column: "ProfProfileId",
                principalTable: "ProfProfiles",
                principalColumn: "Id");
        }
    }
}
