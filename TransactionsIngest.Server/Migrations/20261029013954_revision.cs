using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TransactionsIngest.Server.Migrations
{
    /// <inheritdoc />
    public partial class revision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumns: new[] { "SourceSystem", "TransactionId" },
                keyValues: new object[] { "Online", 100001 });

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumns: new[] { "SourceSystem", "TransactionId" },
                keyValues: new object[] { "Store Downtown", 100001 });

            migrationBuilder.DeleteData(
                table: "Transactions",
                keyColumns: new[] { "SourceSystem", "TransactionId" },
                keyValues: new object[] { "Store Uptown", 100001 });

            migrationBuilder.AddColumn<string>(
                name: "AuditLog",
                table: "Transactions",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditLog",
                table: "Transactions");

            migrationBuilder.InsertData(
                table: "Transactions",
                columns: new[] { "SourceSystem", "TransactionId", "Amount", "CardNumber", "LocationCode", "ProductName", "TransactionTime" },
                values: new object[,]
                {
                    { "Online", 100001, 129.95m, "***4444", "ONLINE", "USB-C Docking Station", new DateTime(2026, 2, 27, 5, 20, 5, 0, DateTimeKind.Local) },
                    { "Store Downtown", 100001, 89.99m, "***0002", "DOWNTOWN", "Mechanical Keyboard", new DateTime(2026, 2, 27, 3, 45, 12, 0, DateTimeKind.Local) },
                    { "Store Uptown", 100001, 24.99m, "***1111", "UPTOWN", "Wireless Mouse", new DateTime(2026, 2, 27, 2, 15, 30, 0, DateTimeKind.Local) }
                });
        }
    }
}
