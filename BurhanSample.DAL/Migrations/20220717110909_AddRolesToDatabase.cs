using Microsoft.EntityFrameworkCore.Migrations;

namespace BurhanSample.DAL.Migrations
{
    public partial class AddRolesToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f2aae4fb-08fc-4f96-baf1-b42fb646c860", "c1f421c4-d151-4500-b40f-b07720aaadbe", "Manager", "MANAGER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "798994f2-a104-454d-a037-b185468e794e", "54e84ee7-21a1-421e-9855-7f60a8325f1a", "Administrator", "ADMINISTRATOR" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "798994f2-a104-454d-a037-b185468e794e");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f2aae4fb-08fc-4f96-baf1-b42fb646c860");
        }
    }
}
