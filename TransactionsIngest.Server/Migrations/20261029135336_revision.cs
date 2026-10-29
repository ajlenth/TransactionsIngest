using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TransactionsIngest.Server.Migrations
{
    /// <inheritdoc />
    public partial class revision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditLog",
                table: "Transactions");

            migrationBuilder.CreateTable(
                name: "Revision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Field = table.Column<string>(type: "TEXT", nullable: false),
                    From = table.Column<string>(type: "TEXT", nullable: false),
                    To = table.Column<string>(type: "TEXT", nullable: false),
                    TransactionId = table.Column<int>(type: "INTEGER", nullable: true),
                    TransactionSourceSystem = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Revision", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Revision_Transactions_TransactionSourceSystem_TransactionId",
                        columns: x => new { x.TransactionSourceSystem, x.TransactionId },
                        principalTable: "Transactions",
                        principalColumns: new[] { "SourceSystem", "TransactionId" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_Revision_TransactionSourceSystem_TransactionId",
                table: "Revision",
                columns: new[] { "TransactionSourceSystem", "TransactionId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Revision");

            migrationBuilder.AddColumn<string>(
                name: "AuditLog",
                table: "Transactions",
                type: "TEXT",
                nullable: true);
        }
    }
}
