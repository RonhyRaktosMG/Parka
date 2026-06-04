using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkaApp.Migrations
{
    /// <inheritdoc />
    public partial class AlterOnDeletePolicy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occupations_Clients_ClientId",
                table: "Occupations");

            migrationBuilder.DropForeignKey(
                name: "FK_Occupations_Places_PlaceId",
                table: "Occupations");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupations_Clients_ClientId",
                table: "Occupations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Occupations_Places_PlaceId",
                table: "Occupations",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occupations_Clients_ClientId",
                table: "Occupations");

            migrationBuilder.DropForeignKey(
                name: "FK_Occupations_Places_PlaceId",
                table: "Occupations");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupations_Clients_ClientId",
                table: "Occupations",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Occupations_Places_PlaceId",
                table: "Occupations",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
