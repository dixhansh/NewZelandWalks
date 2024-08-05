using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalks.API.Migrations.NZWalksAuthDb
{
    /// <inheritdoc />
    public partial class CreatingAuthDatabase2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "1dc38c61-1a74-48c8-bdca-e10d18a2cdda", "1dc38c61-1a74-48c8-bdca-e10d18a2cdda", "Writer", "WRITER" },
                    { "ae46930c-eeed-4603-9153-d18dae47def7", "ae46930c-eeed-4603-9153-d18dae47def7", "Reader", "READER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1dc38c61-1a74-48c8-bdca-e10d18a2cdda");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ae46930c-eeed-4603-9153-d18dae47def7");
        }
    }
}
