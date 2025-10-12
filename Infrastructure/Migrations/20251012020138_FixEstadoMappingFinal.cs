using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixEstadoMappingFinal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "is_active",
                table: "proveedores",
                newName: "IsActiveBool");

            migrationBuilder.AlterColumn<bool>(
                name: "IsActiveBool",
                table: "proveedores",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "proveedores",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "proveedores");

            migrationBuilder.RenameColumn(
                name: "IsActiveBool",
                table: "proveedores",
                newName: "is_active");

            migrationBuilder.AlterColumn<bool>(
                name: "is_active",
                table: "proveedores",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");
        }
    }
}
