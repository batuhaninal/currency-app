using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class precision2_mig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_current_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_current_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_cost_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_cost_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "count",
                table: "user_asset_item_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_sale_price",
                table: "user_asset_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_purchase_price",
                table: "user_asset_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_sale_price",
                table: "user_asset_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_purchase_price",
                table: "user_asset_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_sale_price",
                table: "user_asset_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_purchase_price",
                table: "user_asset_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "old_sale_price",
                table: "currency_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "old_purchase_price",
                table: "currency_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "new_sale_price",
                table: "currency_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "new_purchase_price",
                table: "currency_histories",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_price",
                table: "currencies",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "purchase_price",
                table: "currencies",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_price",
                table: "assets",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "purchase_price",
                table: "assets",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "current_sale_price",
                table: "assets",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "current_purchase_price",
                table: "assets",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "count",
                table: "assets",
                type: "numeric(24,8)",
                precision: 24,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,8)",
                oldPrecision: 18,
                oldScale: 8);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "total_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_current_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_current_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_cost_sale_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "item_avg_cost_purchase_price",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "count",
                table: "user_asset_item_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_sale_price",
                table: "user_asset_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_purchase_price",
                table: "user_asset_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_sale_price",
                table: "user_asset_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_current_purchase_price",
                table: "user_asset_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_sale_price",
                table: "user_asset_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "total_cost_purchase_price",
                table: "user_asset_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "old_sale_price",
                table: "currency_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "old_purchase_price",
                table: "currency_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "new_sale_price",
                table: "currency_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "new_purchase_price",
                table: "currency_histories",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_price",
                table: "currencies",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "purchase_price",
                table: "currencies",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "sale_price",
                table: "assets",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "purchase_price",
                table: "assets",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "current_sale_price",
                table: "assets",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "current_purchase_price",
                table: "assets",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);

            migrationBuilder.AlterColumn<decimal>(
                name: "count",
                table: "assets",
                type: "numeric(18,8)",
                precision: 18,
                scale: 8,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(24,8)",
                oldPrecision: 24,
                oldScale: 8);
        }
    }
}
