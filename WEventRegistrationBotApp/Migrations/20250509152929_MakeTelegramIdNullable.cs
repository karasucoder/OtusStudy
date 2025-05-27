using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WEventRegistrationBotApp.Migrations
{
    /// <inheritdoc />
    public partial class MakeTelegramIdNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Сначала создаем временный столбец
            migrationBuilder.AddColumn<long>(
                name: "TempTelegramId",
                table: "Guests",
                type: "bigint",
                nullable: true);

            // Копируем и преобразуем данные (только для числовых значений)
            migrationBuilder.Sql(@"
        UPDATE ""Guests""
        SET ""TempTelegramId"" = CAST(""TelegramId"" AS bigint)
        WHERE ""TelegramId"" ~ '^\d+$' AND ""TelegramId"" IS NOT NULL;
    ");

            // Удаляем старый столбец
            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "Guests");

            // Переименовываем временный столбец
            migrationBuilder.RenameColumn(
                name: "TempTelegramId",
                table: "Guests",
                newName: "TelegramId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Аналогичная логика для отката
            migrationBuilder.AddColumn<string>(
                name: "TempTelegramId",
                table: "Guests",
                type: "text",
                nullable: true);

            migrationBuilder.Sql(@"
        UPDATE ""Guests""
        SET ""TempTelegramId"" = ""TelegramId""::text
        WHERE ""TelegramId"" IS NOT NULL;
    ");

            migrationBuilder.DropColumn(
                name: "TelegramId",
                table: "Guests");

            migrationBuilder.RenameColumn(
                name: "TempTelegramId",
                table: "Guests",
                newName: "TelegramId");
        }
    }
}
