using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Benchy.DapperVsEfCore.Database.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "DapperVsEfCore");

            migrationBuilder.CreateTable(
                name: "EngineDetails",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pk = table.Column<int>(type: "int", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    NumberOfCilinders = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EngineDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Makes",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Makes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Options",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Options", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Models",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MakeId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Models", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Models_Makes_MakeId",
                        column: x => x.MakeId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Makes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DamageImage",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmallUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MediumUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OriginalUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DamageImage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmallUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MediumUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OriginalUrl = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ModelId = table.Column<long>(type: "bigint", nullable: false),
                    ThumbnailId = table.Column<long>(type: "bigint", nullable: false),
                    LicensePlate = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    EngineDetailsId = table.Column<long>(type: "bigint", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,6)", precision: 18, scale: 6, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_EngineDetails_EngineDetailsId",
                        column: x => x.EngineDetailsId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "EngineDetails",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_Images_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vehicles_Models_ModelId",
                        column: x => x.ModelId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Models",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OptionVehicle",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    OptionsId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OptionVehicle", x => new { x.OptionsId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_OptionVehicle_Options_OptionsId",
                        column: x => x.OptionsId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Options",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OptionVehicle_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagVehicle",
                schema: "DapperVsEfCore",
                columns: table => new
                {
                    TagsId = table.Column<long>(type: "bigint", nullable: false),
                    VehicleId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagVehicle", x => new { x.TagsId, x.VehicleId });
                    table.ForeignKey(
                        name: "FK_TagVehicle_Tags_TagsId",
                        column: x => x.TagsId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagVehicle_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalSchema: "DapperVsEfCore",
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DamageImage_VehicleId",
                schema: "DapperVsEfCore",
                table: "DamageImage",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_VehicleId",
                schema: "DapperVsEfCore",
                table: "Images",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Models_MakeId",
                schema: "DapperVsEfCore",
                table: "Models",
                column: "MakeId");

            migrationBuilder.CreateIndex(
                name: "IX_OptionVehicle_VehicleId",
                schema: "DapperVsEfCore",
                table: "OptionVehicle",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TagVehicle_VehicleId",
                schema: "DapperVsEfCore",
                table: "TagVehicle",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_EngineDetailsId",
                schema: "DapperVsEfCore",
                table: "Vehicles",
                column: "EngineDetailsId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ModelId",
                schema: "DapperVsEfCore",
                table: "Vehicles",
                column: "ModelId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ThumbnailId",
                schema: "DapperVsEfCore",
                table: "Vehicles",
                column: "ThumbnailId");

            migrationBuilder.AddForeignKey(
                name: "FK_DamageImage_Vehicles_VehicleId",
                schema: "DapperVsEfCore",
                table: "DamageImage",
                column: "VehicleId",
                principalSchema: "DapperVsEfCore",
                principalTable: "Vehicles",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Vehicles_VehicleId",
                schema: "DapperVsEfCore",
                table: "Images",
                column: "VehicleId",
                principalSchema: "DapperVsEfCore",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Vehicles_VehicleId",
                schema: "DapperVsEfCore",
                table: "Images");

            migrationBuilder.DropTable(
                name: "DamageImage",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "OptionVehicle",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "TagVehicle",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "Options",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "Vehicles",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "EngineDetails",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "Images",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "Models",
                schema: "DapperVsEfCore");

            migrationBuilder.DropTable(
                name: "Makes",
                schema: "DapperVsEfCore");
        }
    }
}
