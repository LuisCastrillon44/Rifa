using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "talonarios",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    boletas_number = table.Column<int>(type: "integer", nullable: false),
                    boletas_value = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                    lottery_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    lottery_number = table.Column<string>(type: "text", nullable: true),
                    jackpot = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_talonarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_talonarios_usuarios_user_id",
                        column: x => x.user_id,
                        principalTable: "usuarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "boletas",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    talonario_id = table.Column<long>(type: "bigint", nullable: false),
                    number = table.Column<int>(type: "integer", nullable: false),
                    buyer_name = table.Column<string>(type: "text", nullable: true),
                    buyer_phone = table.Column<string>(type: "text", nullable: true),
                    buyer_address = table.Column<string>(type: "text", nullable: true),
                    sold = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_boletas", x => x.id);
                    table.ForeignKey(
                        name: "FK_boletas_talonarios_talonario_id",
                        column: x => x.talonario_id,
                        principalTable: "talonarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_boletas_sold",
                table: "boletas",
                column: "sold");

            migrationBuilder.CreateIndex(
                name: "IX_boletas_talonario_id",
                table: "boletas",
                column: "talonario_id");

            migrationBuilder.CreateIndex(
                name: "IX_boletas_talonario_id_number",
                table: "boletas",
                columns: new[] { "talonario_id", "number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_talonarios_user_id",
                table: "talonarios",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "boletas");

            migrationBuilder.DropTable(
                name: "talonarios");

            migrationBuilder.DropTable(
                name: "usuarios");
        }
    }
}
