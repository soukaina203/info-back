﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace info_backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAdminAttr : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "Users",
                type: "boolean",
                nullable: true);
        }
    }
}
