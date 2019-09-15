using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SYS_CONFIG",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    DELETED = table.Column<bool>(nullable: false),
                    CREATED_USER = table.Column<string>(maxLength: 2048, nullable: true),
                    CREATED_TM = table.Column<DateTime>(nullable: false),
                    LAST_MDF_USER = table.Column<string>(maxLength: 2048, nullable: true),
                    LAST_MDF_TM = table.Column<DateTime>(nullable: true),
                    KEY = table.Column<string>(maxLength: 2048, nullable: true),
                    VALUE = table.Column<string>(maxLength: 2048, nullable: true),
                    VALUE_UNIT = table.Column<string>(maxLength: 2048, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SYS_CONFIG", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USER",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    DELETED = table.Column<bool>(nullable: false),
                    CREATED_USER = table.Column<string>(maxLength: 2048, nullable: true),
                    CREATED_TM = table.Column<DateTime>(nullable: false),
                    LAST_MDF_USER = table.Column<string>(maxLength: 2048, nullable: true),
                    LAST_MDF_TM = table.Column<DateTime>(nullable: true),
                    LOGIN_FAILED_NR = table.Column<int>(nullable: true),
                    TOKEN = table.Column<string>(maxLength: 2048, nullable: true),
                    SUBCRISE_TOKEN = table.Column<string>(maxLength: 2048, nullable: true),
                    TOKEN_EXPIRED_DT = table.Column<DateTime>(nullable: true),
                    LOGIN_TM = table.Column<DateTime>(nullable: true),
                    PASSWORD = table.Column<string>(maxLength: 1024, nullable: false),
                    PASSWORD_LAST_UDT = table.Column<DateTime>(nullable: true),
                    USERNAME = table.Column<string>(maxLength: 2048, nullable: false),
                    CODE = table.Column<string>(maxLength: 128, nullable: false),
                    FULL_NAME = table.Column<string>(maxLength: 2048, nullable: false),
                    USER_TYP = table.Column<string>(maxLength: 2048, nullable: false),
                    STATUS = table.Column<string>(maxLength: 2048, nullable: false),
                    ADDRESS = table.Column<string>(maxLength: 2048, nullable: false),
                    EMAIL = table.Column<string>(maxLength: 2048, nullable: true),
                    PHONE = table.Column<string>(maxLength: 128, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USER", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SYS_CONFIG");

            migrationBuilder.DropTable(
                name: "USER");
        }
    }
}
