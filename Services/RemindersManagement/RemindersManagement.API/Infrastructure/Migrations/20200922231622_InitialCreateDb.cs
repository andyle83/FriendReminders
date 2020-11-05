using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace RemindersManagement.API.Infrastructure.Migrations
{
    public partial class InitialCreateDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "reminders");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "reminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", nullable: false),
                    Icon = table.Column<string>(nullable: true),
                    HexaColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Reminders",
                schema: "reminders",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Status = table.Column<int>(type: "varchar(50)", nullable: false),
                    Priority = table.Column<int>(nullable: false),
                    RemiderTime = table.Column<DateTime>(nullable: true),
                    CategoryId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reminders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reminders_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "reminders",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "reminders",
                table: "Categories",
                columns: new[] { "Id", "HexaColor", "Icon", "Name" },
                values: new object[] { new Guid("e76910df-ff41-483c-b284-0873b55a936b"), null, null, "Default" });

            migrationBuilder.InsertData(
                schema: "reminders",
                table: "Reminders",
                columns: new[] { "Id", "CategoryId", "Description", "Priority", "RemiderTime", "Status" },
                values: new object[] { new Guid("8b5d82df-e621-4b9d-96a4-1c67301e5fa5"), new Guid("e76910df-ff41-483c-b284-0873b55a936b"), "Learning Microservices", 1, null, 1 });

            migrationBuilder.InsertData(
                schema: "reminders",
                table: "Reminders",
                columns: new[] { "Id", "CategoryId", "Description", "Priority", "RemiderTime", "Status" },
                values: new object[] { new Guid("7170f7b0-60d5-4f7e-a4f5-2cbdc0dd5d65"), new Guid("e76910df-ff41-483c-b284-0873b55a936b"), "Writing Blog", 1, null, 0 });

            migrationBuilder.InsertData(
                schema: "reminders",
                table: "Reminders",
                columns: new[] { "Id", "CategoryId", "Description", "Priority", "RemiderTime", "Status" },
                values: new object[] { new Guid("ab78c3ec-c7e9-44dd-8e58-ce40444a6d5c"), new Guid("e76910df-ff41-483c-b284-0873b55a936b"), "Presentation prepare", 1, null, 0 });

            migrationBuilder.CreateIndex(
                name: "IX_Reminders_CategoryId",
                schema: "reminders",
                table: "Reminders",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reminders",
                schema: "reminders");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "reminders");
        }
    }
}