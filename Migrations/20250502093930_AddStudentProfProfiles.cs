using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace info_backend.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentProfProfiles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.CreateTable(
                name: "ProfProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Cv = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfProfiles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StudentProfile",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentProfile_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ProfProfiles_UserId",
                table: "ProfProfiles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentProfile_UserId",
                table: "StudentProfile",
                column: "UserId");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropTable(
                name: "ProfProfiles");

            migrationBuilder.DropTable(
                name: "StudentProfile");

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
        }
    }
}
