using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Blasterify.Services.Migrations
{
    public partial class Update_Client_User : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClientUsers_MerchantOrderId",
                table: "ClientUsers");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantOrderId",
                table: "ClientUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "ClientUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "ClientUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LogInAttempts",
                table: "ClientUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResetPasswordAttempts",
                table: "ClientUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockDate",
                table: "ClientUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VerificationCode",
                table: "ClientUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeExpiration",
                table: "ClientUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "AdministratorUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVerified",
                table: "AdministratorUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LogInAttempts",
                table: "AdministratorUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResetPasswordAttempts",
                table: "AdministratorUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "UnlockDate",
                table: "AdministratorUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "VerificationCode",
                table: "AdministratorUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerificationCodeExpiration",
                table: "AdministratorUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_ClientUsers_MerchantOrderId",
                table: "ClientUsers",
                column: "MerchantOrderId",
                unique: true,
                filter: "[MerchantOrderId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ClientUsers_MerchantOrderId",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "LogInAttempts",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordAttempts",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "UnlockDate",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpiration",
                table: "ClientUsers");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "AdministratorUsers");

            migrationBuilder.DropColumn(
                name: "IsVerified",
                table: "AdministratorUsers");

            migrationBuilder.DropColumn(
                name: "LogInAttempts",
                table: "AdministratorUsers");

            migrationBuilder.DropColumn(
                name: "ResetPasswordAttempts",
                table: "AdministratorUsers");

            migrationBuilder.DropColumn(
                name: "UnlockDate",
                table: "AdministratorUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AdministratorUsers");

            migrationBuilder.DropColumn(
                name: "VerificationCodeExpiration",
                table: "AdministratorUsers");

            migrationBuilder.AlterColumn<string>(
                name: "MerchantOrderId",
                table: "ClientUsers",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ClientUsers_MerchantOrderId",
                table: "ClientUsers",
                column: "MerchantOrderId",
                unique: true);
        }
    }
}
