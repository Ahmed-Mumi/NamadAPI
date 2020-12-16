using Microsoft.EntityFrameworkCore.Migrations;

namespace NomadAPI.Migrations
{
    public partial class MeansOfTravelUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Travels_MeansOfTravels_MeansOfTravelId",
                table: "Travels");

            migrationBuilder.AlterColumn<int>(
                name: "MeansOfTravelId",
                table: "Travels",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Travels_MeansOfTravels_MeansOfTravelId",
                table: "Travels",
                column: "MeansOfTravelId",
                principalTable: "MeansOfTravels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Travels_MeansOfTravels_MeansOfTravelId",
                table: "Travels");

            migrationBuilder.AlterColumn<int>(
                name: "MeansOfTravelId",
                table: "Travels",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Travels_MeansOfTravels_MeansOfTravelId",
                table: "Travels",
                column: "MeansOfTravelId",
                principalTable: "MeansOfTravels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
