using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopFinance.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Mig01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false),
                    Birthdate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Frequencies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DaysInterval = table.Column<int>(type: "INTEGER", nullable: false),
                    PeriodsPerYear = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Frequencies", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InterestRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RateName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AnnualPercentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InterestRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTerms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    NumberOfPayments = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTerms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Phase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", nullable: false),
                    PhaseName = table.Column<string>(type: "TEXT", nullable: false),
                    Route = table.Column<string>(type: "TEXT", nullable: false),
                    IsInitial = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsFinal = table.Column<bool>(type: "INTEGER", nullable: false),
                    Required = table.Column<bool>(type: "INTEGER", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Phase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Key = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    IsEncrypted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TaxRates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Percentage = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: false),
                    EffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "date", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaxRates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CategoryId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CodeSku = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CostPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "INTEGER", nullable: false),
                    StockMin = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: true),
                    QuotationId = table.Column<Guid>(type: "TEXT", nullable: true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    OrderNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    RequiredDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    TotalAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PhaseState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PhaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    PhaseStateName = table.Column<string>(type: "TEXT", nullable: false),
                    Initial = table.Column<bool>(type: "INTEGER", nullable: false),
                    Edition = table.Column<bool>(type: "INTEGER", nullable: false),
                    Completed = table.Column<bool>(type: "INTEGER", nullable: false),
                    Canceled = table.Column<bool>(type: "INTEGER", nullable: false),
                    Refused = table.Column<bool>(type: "INTEGER", nullable: false),
                    PreviousPhase = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhaseState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhaseState_Phase_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FullName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    AvatarUrl = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QuotationPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TaxRateId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PlanName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    InitialEffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    FinalEffectiveDate = table.Column<DateTime>(type: "date", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationPlans_TaxRates_TaxRateId",
                        column: x => x.TaxRateId,
                        principalTable: "TaxRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockCounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CountNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    CountDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockCounts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTransfers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    TransferNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    TransferDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransfers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransfers_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseSummaries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalProducts = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    LowStockProducts = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    OutOfStockProducts = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    TotalInventoryValue = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false, defaultValue: 0m),
                    TotalMovementsToday = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseSummaries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseSummaries_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockAlerts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    AlertType = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentStock = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Threshold = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    AlertDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockAlerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockAlerts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockAlerts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockMovements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "TEXT", nullable: true),
                    MovementType = table.Column<int>(type: "INTEGER", nullable: false),
                    Source = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    PreviousStock = table.Column<int>(type: "INTEGER", nullable: false),
                    NewStock = table.Column<int>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    MovementDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockMovements_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockMovements_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseProducts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    StockMin = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    StockMax = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Location = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WarehouseProducts_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sales",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SaleChannel = table.Column<int>(type: "INTEGER", nullable: false),
                    SaleNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SaleDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    PaymentMethod = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sales_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Sales_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RoleId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationPlanFrequencies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    FrequencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    IsDefault = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationPlanFrequencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationPlanFrequencies_Frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "Frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationPlanFrequencies_QuotationPlans_QuotationPlanId",
                        column: x => x.QuotationPlanId,
                        principalTable: "QuotationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationPlanPaymentTerms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentTermId = table.Column<int>(type: "INTEGER", nullable: false),
                    InterestRateId = table.Column<int>(type: "INTEGER", nullable: false),
                    SpecialRateOverride = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    IsPromotional = table.Column<bool>(type: "INTEGER", nullable: false, defaultValue: false),
                    PromotionEndDate = table.Column<DateTime>(type: "date", nullable: true),
                    Order = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationPlanPaymentTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationPlanPaymentTerms_InterestRates_InterestRateId",
                        column: x => x.InterestRateId,
                        principalTable: "InterestRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationPlanPaymentTerms_PaymentTerms_PaymentTermId",
                        column: x => x.PaymentTermId,
                        principalTable: "PaymentTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_QuotationPlanPaymentTerms_QuotationPlans_QuotationPlanId",
                        column: x => x.QuotationPlanId,
                        principalTable: "QuotationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationPlanPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    PhaseId = table.Column<int>(type: "INTEGER", nullable: false),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationPlanPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationPlanPhases_Phase_PhaseId",
                        column: x => x.PhaseId,
                        principalTable: "Phase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QuotationPlanPhases_QuotationPlans_QuotationPlanId",
                        column: x => x.QuotationPlanId,
                        principalTable: "QuotationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Quotations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    OrderId = table.Column<Guid>(type: "TEXT", nullable: true),
                    QuotationPlanId = table.Column<int>(type: "INTEGER", nullable: false),
                    FrequencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    State = table.Column<int>(type: "INTEGER", nullable: false),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false),
                    TaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    Term = table.Column<int>(type: "INTEGER", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    MonthlyPayment = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Quotations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Quotations_Frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "Frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Quotations_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Quotations_QuotationPlans_QuotationPlanId",
                        column: x => x.QuotationPlanId,
                        principalTable: "QuotationPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockCountItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StockCountId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    SystemQuantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    PhysicalQuantity = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockCountItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockCountItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockCountItems_StockCounts_StockCountId",
                        column: x => x.StockCountId,
                        principalTable: "StockCounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransferItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    StockTransferId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    QuantitySent = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    QuantityReceived = table.Column<int>(type: "INTEGER", nullable: false, defaultValue: 0),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransferItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransferItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTransferItems_StockTransfers_StockTransferId",
                        column: x => x.StockTransferId,
                        principalTable: "StockTransfers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaleItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    SaleId = table.Column<Guid>(type: "TEXT", nullable: false),
                    ProductId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    CostPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaleItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaleItems_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SaleItems_Sales_SaleId",
                        column: x => x.SaleId,
                        principalTable: "Sales",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreditId = table.Column<Guid>(type: "TEXT", nullable: true),
                    PhaseStateId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsDeleted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditRequests_PhaseState_PhaseStateId",
                        column: x => x.PhaseStateId,
                        principalTable: "PhaseState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CreditRequests_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "QuotationAdditionalCosts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CostType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    IsPercentage = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationAdditionalCosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationAdditionalCosts_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationAmortizationScheduleEntries",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PeriodNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Principal = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    Interest = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    InterestTax = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false),
                    TotalDue = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationAmortizationScheduleEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationAmortizationScheduleEntries_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "QuotationDiscounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    DiscountType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Value = table.Column<decimal>(type: "TEXT", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QuotationDiscounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QuotationDiscounts_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CreditRequestPhases",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreditRequestId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PhaseStateId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditRequestPhases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CreditRequestPhases_CreditRequests_CreditRequestId",
                        column: x => x.CreditRequestId,
                        principalTable: "CreditRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CreditRequestPhases_PhaseState_PhaseStateId",
                        column: x => x.PhaseStateId,
                        principalTable: "PhaseState",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Credits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CustomerId = table.Column<Guid>(type: "TEXT", nullable: false),
                    FrequencyId = table.Column<int>(type: "INTEGER", nullable: false),
                    CreditRequestId = table.Column<Guid>(type: "TEXT", nullable: false),
                    QuotationId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreditKey = table.Column<string>(type: "TEXT", nullable: false),
                    Rate = table.Column<decimal>(type: "TEXT", nullable: false),
                    TaxRate = table.Column<decimal>(type: "TEXT", nullable: false),
                    Term = table.Column<int>(type: "INTEGER", nullable: false),
                    PrincipalAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    FinancedAmount = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreditState = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Credits_CreditRequests_CreditRequestId",
                        column: x => x.CreditRequestId,
                        principalTable: "CreditRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Credits_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Credits_Frequencies_FrequencyId",
                        column: x => x.FrequencyId,
                        principalTable: "Frequencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Credits_Quotations_QuotationId",
                        column: x => x.QuotationId,
                        principalTable: "Quotations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AmortizationSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreditId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PeriodNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    DueDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Principal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Interest = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalDue = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrincipalBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestTaxBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AmortizationSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AmortizationSchedules_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Movements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreditId = table.Column<Guid>(type: "TEXT", nullable: false),
                    Principal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Interest = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    PrincipalBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestTaxBalance = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalBalance = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Movements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Movements_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreditId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Principal = table.Column<decimal>(type: "TEXT", nullable: false),
                    Interest = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestTax = table.Column<decimal>(type: "TEXT", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Credits_CreditId",
                        column: x => x.CreditId,
                        principalTable: "Credits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PaymentApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MovementId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PaymentId = table.Column<Guid>(type: "TEXT", nullable: false),
                    PrincipalPaid = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestPaid = table.Column<decimal>(type: "TEXT", nullable: false),
                    InterestTaxPaid = table.Column<decimal>(type: "TEXT", nullable: false),
                    TotalTaxPaid = table.Column<decimal>(type: "TEXT", nullable: false),
                    AmortizationScheduleEntryId = table.Column<Guid>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentApplications_AmortizationSchedules_AmortizationScheduleEntryId",
                        column: x => x.AmortizationScheduleEntryId,
                        principalTable: "AmortizationSchedules",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PaymentApplications_Movements_MovementId",
                        column: x => x.MovementId,
                        principalTable: "Movements",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentApplications_Payments_PaymentId",
                        column: x => x.PaymentId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Frequencies",
                columns: new[] { "Id", "Code", "DaysInterval", "Description", "IsActive", "Name", "PeriodsPerYear" },
                values: new object[,]
                {
                    { 1, "DAILY", 1, "", false, "Diario", 365 },
                    { 2, "WEEKLY", 7, "", false, "Semanal", 52 },
                    { 3, "BIWEEKLY", 15, "", false, "Quincenal", 24 },
                    { 4, "MONTHLY", 30, "", false, "Mensual", 12 },
                    { 5, "BIMONTHLY", 60, "", false, "Bimestral", 6 },
                    { 6, "QUARTERLY", 90, "", false, "Trimestral", 4 },
                    { 7, "SEMIANNUAL", 180, "", false, "Semestral", 2 },
                    { 8, "ANNUAL", 365, "", false, "Anual", 1 }
                });

            migrationBuilder.InsertData(
                table: "PaymentTerms",
                columns: new[] { "Id", "Code", "IsActive", "Name", "NumberOfPayments" },
                values: new object[,]
                {
                    { 1, "TERM_1", false, "Un Pago", 1 },
                    { 2, "TERM_3", false, "3 Pagos", 3 },
                    { 3, "TERM_6", false, "6 Pagos", 6 },
                    { 4, "TERM_12", false, "12 Pagos", 12 },
                    { 5, "TERM_18", false, "18 Pagos", 18 },
                    { 6, "TERM_24", false, "24 Pagos", 24 },
                    { 7, "TERM_36", false, "36 Pagos", 36 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AmortizationSchedules_CreditId",
                table: "AmortizationSchedules",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Code",
                table: "Categories",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_CreditRequestPhases_CreditRequestId",
                table: "CreditRequestPhases",
                column: "CreditRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditRequestPhases_PhaseStateId",
                table: "CreditRequestPhases",
                column: "PhaseStateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditRequests_PhaseStateId",
                table: "CreditRequests",
                column: "PhaseStateId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditRequests_QuotationId",
                table: "CreditRequests",
                column: "QuotationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credits_CreditRequestId",
                table: "Credits",
                column: "CreditRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Credits_CustomerId",
                table: "Credits",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_FrequencyId",
                table: "Credits",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Credits_QuotationId",
                table: "Credits",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_Frequencies_Code",
                table: "Frequencies",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Frequencies_DaysInterval",
                table: "Frequencies",
                column: "DaysInterval");

            migrationBuilder.CreateIndex(
                name: "IX_InterestRates_EffectiveDate",
                table: "InterestRates",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_InterestRates_IsActive",
                table: "InterestRates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "UK_InterestRates_Name",
                table: "InterestRates",
                column: "RateName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Movements_CreditId",
                table: "Movements",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_ProductId",
                table: "OrderItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderDate",
                table: "Orders",
                column: "OrderDate");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status",
                table: "Orders",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentApplications_AmortizationScheduleEntryId",
                table: "PaymentApplications",
                column: "AmortizationScheduleEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentApplications_MovementId",
                table: "PaymentApplications",
                column: "MovementId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentApplications_PaymentId",
                table: "PaymentApplications",
                column: "PaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CreditId",
                table: "Payments",
                column: "CreditId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerms_Code",
                table: "PaymentTerms",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PaymentTerms_NumberOfPayments",
                table: "PaymentTerms",
                column: "NumberOfPayments");

            migrationBuilder.CreateIndex(
                name: "IX_PhaseState_PhaseId",
                table: "PhaseState",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_Code",
                table: "Products",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_CodeSku",
                table: "Products",
                column: "CodeSku",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Products_Name",
                table: "Products",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Products_State",
                table: "Products",
                column: "State");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationAdditionalCosts_CostType",
                table: "QuotationAdditionalCosts",
                column: "CostType");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationAdditionalCosts_QuotationId",
                table: "QuotationAdditionalCosts",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationAmortizationScheduleEntries_DueDate",
                table: "QuotationAmortizationScheduleEntries",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationAmortizationScheduleEntries_QuotationId_PeriodNumber",
                table: "QuotationAmortizationScheduleEntries",
                columns: new[] { "QuotationId", "PeriodNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDiscounts_DiscountType",
                table: "QuotationDiscounts",
                column: "DiscountType");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationDiscounts_QuotationId",
                table: "QuotationDiscounts",
                column: "QuotationId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanFrequencies_FrequencyId",
                table: "QuotationPlanFrequencies",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanFrequencies_QuotationPlanId_FrequencyId",
                table: "QuotationPlanFrequencies",
                columns: new[] { "QuotationPlanId", "FrequencyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanPaymentTerms_InterestRateId",
                table: "QuotationPlanPaymentTerms",
                column: "InterestRateId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanPaymentTerms_PaymentTermId",
                table: "QuotationPlanPaymentTerms",
                column: "PaymentTermId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanPaymentTerms_QuotationPlanId_PaymentTermId",
                table: "QuotationPlanPaymentTerms",
                columns: new[] { "QuotationPlanId", "PaymentTermId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanPhases_PhaseId",
                table: "QuotationPlanPhases",
                column: "PhaseId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlanPhases_QuotationPlanId",
                table: "QuotationPlanPhases",
                column: "QuotationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlans_Code",
                table: "QuotationPlans",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_QuotationPlans_TaxRateId",
                table: "QuotationPlans",
                column: "TaxRateId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_FrequencyId",
                table: "Quotations",
                column: "FrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_OrderId",
                table: "Quotations",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Quotations_QuotationPlanId",
                table: "Quotations",
                column: "QuotationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_IsActive",
                table: "Roles",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_IsDeleted",
                table: "Roles",
                column: "IsDeleted");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Roles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_ProductId",
                table: "SaleItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_SaleItems_SaleId_ProductId",
                table: "SaleItems",
                columns: new[] { "SaleId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_InvoiceNumber",
                table: "Sales",
                column: "InvoiceNumber",
                unique: true,
                filter: "[InvoiceNumber] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_OrderId",
                table: "Sales",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleDate",
                table: "Sales",
                column: "SaleDate");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_SaleNumber",
                table: "Sales",
                column: "SaleNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sales_Status",
                table: "Sales",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Sales_WarehouseId",
                table: "Sales",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_Settings_Key",
                table: "Settings",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_AlertDate",
                table: "StockAlerts",
                column: "AlertDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_AlertType",
                table: "StockAlerts",
                column: "AlertType");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_ProductId_AlertType",
                table: "StockAlerts",
                columns: new[] { "ProductId", "AlertType" });

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_WarehouseId_AlertDate",
                table: "StockAlerts",
                columns: new[] { "WarehouseId", "AlertDate" },
                filter: "[AlertType] IN (0,1,2)");

            migrationBuilder.CreateIndex(
                name: "IX_StockAlerts_WarehouseId_AlertType",
                table: "StockAlerts",
                columns: new[] { "WarehouseId", "AlertType" });

            migrationBuilder.CreateIndex(
                name: "IX_StockCountItems_ProductId",
                table: "StockCountItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCountItems_StockCountId_ProductId",
                table: "StockCountItems",
                columns: new[] { "StockCountId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockCountItems_StockCountId_SystemQuantity_PhysicalQuantity",
                table: "StockCountItems",
                columns: new[] { "StockCountId", "SystemQuantity", "PhysicalQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_CountDate",
                table: "StockCounts",
                column: "CountDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_CountNumber",
                table: "StockCounts",
                column: "CountNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_Status",
                table: "StockCounts",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_WarehouseId",
                table: "StockCounts",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockCounts_WarehouseId_Status",
                table: "StockCounts",
                columns: new[] { "WarehouseId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_MovementDate",
                table: "StockMovements",
                column: "MovementDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_MovementType",
                table: "StockMovements",
                column: "MovementType");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductId_MovementDate",
                table: "StockMovements",
                columns: new[] { "ProductId", "MovementDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ReferenceId_Source",
                table: "StockMovements",
                columns: new[] { "ReferenceId", "Source" },
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_Source",
                table: "StockMovements",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_WarehouseId_MovementDate",
                table: "StockMovements",
                columns: new[] { "WarehouseId", "MovementDate" });

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferItems_ProductId",
                table: "StockTransferItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransferItems_StockTransferId_ProductId",
                table: "StockTransferItems",
                columns: new[] { "StockTransferId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_FromWarehouseId",
                table: "StockTransfers",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_Status",
                table: "StockTransfers",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_ToWarehouseId",
                table: "StockTransfers",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_TransferDate",
                table: "StockTransfers",
                column: "TransferDate");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransfers_TransferNumber",
                table: "StockTransfers",
                column: "TransferNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_Code",
                table: "TaxRates",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_EffectiveDate",
                table: "TaxRates",
                column: "EffectiveDate");

            migrationBuilder.CreateIndex(
                name: "IX_TaxRates_IsActive",
                table: "TaxRates",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "Users",
                column: "NormalizedEmail",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "Users",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_Location",
                table: "WarehouseProducts",
                column: "Location",
                filter: "[Location] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_ProductId",
                table: "WarehouseProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_StockQuantity",
                table: "WarehouseProducts",
                column: "StockQuantity");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_WarehouseId_ProductId",
                table: "WarehouseProducts",
                columns: new[] { "WarehouseId", "ProductId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseProducts_WarehouseId_StockQuantity",
                table: "WarehouseProducts",
                columns: new[] { "WarehouseId", "StockQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Code",
                table: "Warehouses",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_IsActive",
                table: "Warehouses",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Name",
                table: "Warehouses",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_Type",
                table: "Warehouses",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseSummaries_WarehouseId",
                table: "WarehouseSummaries",
                column: "WarehouseId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CreditRequestPhases");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PaymentApplications");

            migrationBuilder.DropTable(
                name: "QuotationAdditionalCosts");

            migrationBuilder.DropTable(
                name: "QuotationAmortizationScheduleEntries");

            migrationBuilder.DropTable(
                name: "QuotationDiscounts");

            migrationBuilder.DropTable(
                name: "QuotationPlanFrequencies");

            migrationBuilder.DropTable(
                name: "QuotationPlanPaymentTerms");

            migrationBuilder.DropTable(
                name: "QuotationPlanPhases");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "SaleItems");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "StockAlerts");

            migrationBuilder.DropTable(
                name: "StockCountItems");

            migrationBuilder.DropTable(
                name: "StockMovements");

            migrationBuilder.DropTable(
                name: "StockTransferItems");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WarehouseProducts");

            migrationBuilder.DropTable(
                name: "WarehouseSummaries");

            migrationBuilder.DropTable(
                name: "AmortizationSchedules");

            migrationBuilder.DropTable(
                name: "Movements");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "InterestRates");

            migrationBuilder.DropTable(
                name: "PaymentTerms");

            migrationBuilder.DropTable(
                name: "Sales");

            migrationBuilder.DropTable(
                name: "StockCounts");

            migrationBuilder.DropTable(
                name: "StockTransfers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Credits");

            migrationBuilder.DropTable(
                name: "Warehouses");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "CreditRequests");

            migrationBuilder.DropTable(
                name: "PhaseState");

            migrationBuilder.DropTable(
                name: "Quotations");

            migrationBuilder.DropTable(
                name: "Phase");

            migrationBuilder.DropTable(
                name: "Frequencies");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "QuotationPlans");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "TaxRates");
        }
    }
}
