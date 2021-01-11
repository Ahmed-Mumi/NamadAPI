using Microsoft.EntityFrameworkCore.Migrations;

namespace NomadAPI.Migrations
{
    public partial class ReactionUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PositiveReaction",
                table: "UserReactions",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositiveReaction",
                table: "UserReactions");
        }
    }
}
