using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatWebAPI.Migrations
{
    public partial class init0 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Contact_ContactId",
                table: "Message");

            migrationBuilder.DropColumn(
                name: "Contact",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Message",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Contact_ContactId",
                table: "Message",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Message_Contact_ContactId",
                table: "Message");

            migrationBuilder.AlterColumn<int>(
                name: "ContactId",
                table: "Message",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "Contact",
                table: "Message",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Message_Contact_ContactId",
                table: "Message",
                column: "ContactId",
                principalTable: "Contact",
                principalColumn: "ContactId");
        }
    }
}
