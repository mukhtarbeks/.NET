using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MuhktarWebApp.Migrations
{
    public partial class addCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CancelCount",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "CompleteCount",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "RefundCount",
                table: "Route");

            migrationBuilder.DropColumn(
                name: "TotalCount",
                table: "Route");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CancelCount",
                table: "Route",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompleteCount",
                table: "Route",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "Percent",
                table: "Route",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "RefundCount",
                table: "Route",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalCount",
                table: "Route",
                nullable: false,
                defaultValue: 0);
        }
    }
}
