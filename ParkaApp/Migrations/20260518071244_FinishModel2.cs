using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkaApp.Migrations
{
    /// <inheritdoc />
    public partial class FinishModel2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Occupation_Clients_ClientId",
                table: "Occupation");

            migrationBuilder.DropForeignKey(
                name: "FK_Occupation_Places_PlaceId",
                table: "Occupation");

            migrationBuilder.DropForeignKey(
                name: "FK_Payment_Clients_ClientId",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payment",
                table: "Payment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occupation",
                table: "Occupation");

            migrationBuilder.RenameTable(
                name: "Payment",
                newName: "Payments");

            migrationBuilder.RenameTable(
                name: "Occupation",
                newName: "Occupations");

            migrationBuilder.RenameIndex(
                name: "IX_Payment_ClientId",
                table: "Payments",
                newName: "IX_Payments_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Occupation_PlaceId",
                table: "Occupations",
                newName: "IX_Occupations_PlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Occupation_ClientId",
                table: "Occupations",
                newName: "IX_Occupations_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payments",
                table: "Payments",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occupations",
                table: "Occupations",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments",
                column: "ClientId",
                principalTable: "Clients",
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

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Clients_ClientId",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Payments",
                table: "Payments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Occupations",
                table: "Occupations");

            migrationBuilder.RenameTable(
                name: "Payments",
                newName: "Payment");

            migrationBuilder.RenameTable(
                name: "Occupations",
                newName: "Occupation");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ClientId",
                table: "Payment",
                newName: "IX_Payment_ClientId");

            migrationBuilder.RenameIndex(
                name: "IX_Occupations_PlaceId",
                table: "Occupation",
                newName: "IX_Occupation_PlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Occupations_ClientId",
                table: "Occupation",
                newName: "IX_Occupation_ClientId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Payment",
                table: "Payment",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Occupation",
                table: "Occupation",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Occupation_Clients_ClientId",
                table: "Occupation",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Occupation_Places_PlaceId",
                table: "Occupation",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Payment_Clients_ClientId",
                table: "Payment",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
