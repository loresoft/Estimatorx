using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EstimatorX.Migrations.Sqlite.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataProtectionKey",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FriendlyName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Xml = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKey", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Organization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 255, nullable: true),
                    HostMatches = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organization", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    IsAdministrator = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    IsDisabled = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Invite",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    OrganizationId = table.Column<long>(type: "INTEGER", nullable: false),
                    SecurityKey = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Expiration = table.Column<long>(type: "INTEGER", nullable: false),
                    LastSent = table.Column<long>(type: "INTEGER", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Invite", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Invite_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Project",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    OrganizationId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: true),
                    EstimatedTotal = table.Column<int>(type: "INTEGER", nullable: true),
                    WeightedTotal = table.Column<int>(type: "INTEGER", nullable: true),
                    EstimatedWeeks = table.Column<double>(type: "REAL", nullable: true),
                    WeightedWeeks = table.Column<double>(type: "REAL", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    WeightedCost = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    SecurityKey = table.Column<string>(type: "TEXT", maxLength: 64, nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Epics = table.Column<string>(type: "TEXT", nullable: false),
                    Settings = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Project", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Project_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Project_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Template",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    OrganizationId = table.Column<long>(type: "INTEGER", nullable: true),
                    UserId = table.Column<long>(type: "INTEGER", nullable: true),
                    EstimatedTotal = table.Column<int>(type: "INTEGER", nullable: true),
                    WeightedTotal = table.Column<int>(type: "INTEGER", nullable: true),
                    EstimatedWeeks = table.Column<double>(type: "REAL", nullable: true),
                    WeightedWeeks = table.Column<double>(type: "REAL", nullable: true),
                    EstimatedCost = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    WeightedCost = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Epics = table.Column<string>(type: "TEXT", nullable: false),
                    Settings = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Template", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Template_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Template_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserLogin",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", nullable: true),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogin", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogin_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserOrganization",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    OrganizationId = table.Column<long>(type: "INTEGER", nullable: false),
                    IsOwner = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserOrganization", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserOrganization_Organization_OrganizationId",
                        column: x => x.OrganizationId,
                        principalTable: "Organization",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserOrganization_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProvider",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false),
                    Provider = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", maxLength: 450, nullable: false),
                    UserId = table.Column<long>(type: "INTEGER", nullable: false),
                    Created = table.Column<long>(type: "INTEGER", nullable: false),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Updated = table.Column<long>(type: "INTEGER", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProvider_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Organization",
                columns: new[] { "Id", "Created", "CreatedBy", "Description", "HostMatches", "Name", "Updated", "UpdatedBy" },
                values: new object[] { 364914440505659392L, 1308729802752000000L, "system", "Default testing organization", null, "Testing", 1308729802752000000L, "system" });

            migrationBuilder.CreateIndex(
                name: "UX_Invite_OrganizationId_Email",
                table: "Invite",
                columns: new[] { "OrganizationId", "Email" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UX_Organization_Name",
                table: "Organization",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_OrganizationId",
                table: "Project",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserId",
                table: "Project",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_Project_SecurityKey",
                table: "Project",
                column: "SecurityKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Template_OrganizationId",
                table: "Template",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_Template_UserId",
                table: "Template",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLogin_UserId",
                table: "UserLogin",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserOrganization_OrganizationId",
                table: "UserOrganization",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "UX_UserOrganization_UserId_OrganizationId",
                table: "UserOrganization",
                columns: new[] { "UserId", "OrganizationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProvider_UserId",
                table: "UserProvider",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "UX_UserProvider_Provider_ProviderKey",
                table: "UserProvider",
                columns: new[] { "Provider", "ProviderKey" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKey");

            migrationBuilder.DropTable(
                name: "Invite");

            migrationBuilder.DropTable(
                name: "Project");

            migrationBuilder.DropTable(
                name: "Template");

            migrationBuilder.DropTable(
                name: "UserLogin");

            migrationBuilder.DropTable(
                name: "UserOrganization");

            migrationBuilder.DropTable(
                name: "UserProvider");

            migrationBuilder.DropTable(
                name: "Organization");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
