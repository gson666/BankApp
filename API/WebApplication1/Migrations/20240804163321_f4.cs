using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication1.Migrations
{
    public partial class f4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop existing constraints and columns
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReceiverBankId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderBankId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Mask",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "ShareableId",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "Subtype",
                table: "Accounts");

            // Add new columns without constraints
            migrationBuilder.AddColumn<int>(
                name: "TransactionId",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ReceiverAccountId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SenderAccountId",
                table: "Transactions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            // Add primary keys
            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "TransactionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "AccountId");

            // Update the new columns with valid data
            migrationBuilder.Sql(@"
                UPDATE Transactions
                SET ReceiverAccountId = (
                    SELECT TOP 1 AccountId
                    FROM Accounts
                    ORDER BY NEWID()
                )
                WHERE ReceiverAccountId IS NULL
                    OR ReceiverAccountId NOT IN (SELECT AccountId FROM Accounts);
                
                UPDATE Transactions
                SET SenderAccountId = (
                    SELECT TOP 1 AccountId
                    FROM Accounts
                    ORDER BY NEWID()
                )
                WHERE SenderAccountId IS NULL
                    OR SenderAccountId NOT IN (SELECT AccountId FROM Accounts);
            ");

            // Make the new columns non-nullable
            migrationBuilder.AlterColumn<int>(
                name: "ReceiverAccountId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SenderAccountId",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_ReceiverAccountId",
                table: "Transactions",
                column: "ReceiverAccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SenderAccountId",
                table: "Transactions",
                column: "SenderAccountId");

            // Add foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_ReceiverAccountId",
                table: "Transactions",
                column: "ReceiverAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_SenderAccountId",
                table: "Transactions",
                column: "SenderAccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the changes
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_ReceiverAccountId",
                table: "Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Accounts_SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_ReceiverAccountId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "TransactionId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ReceiverAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SenderAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "Accounts");

            // Re-add old columns
            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AccountId",
                table: "Transactions",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transactions",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReceiverBankId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SenderBankId",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Mask",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ShareableId",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Subtype",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            // Re-add primary keys
            migrationBuilder.AddPrimaryKey(
                name: "PK_Transactions",
                table: "Transactions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Accounts",
                table: "Accounts",
                column: "Id");

            // Re-create indexes
            migrationBuilder.CreateIndex(
                name: "IX_Transactions_AccountId",
                table: "Transactions",
                column: "AccountId");

            // Re-add foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Accounts_AccountId",
                table: "Transactions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id");
        }
    }
}
