using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Pgvector;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:vector", ",,");

            migrationBuilder.CreateTable(
                name: "BannerConfigurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    MaxNumberOfBanner = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerConfigurations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BannerRequestPrices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerRequestPrices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Brands",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Brands", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SellerSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: false),
                    Duration = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Shops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    WebsiteUrl = table.Column<string>(type: "text", nullable: false),
                    LogoUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shops", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: true),
                    Role = table.Column<string>(type: "text", nullable: false),
                    LoginMethod = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryBrands",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryBrands", x => new { x.BrandId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_CategoryBrands_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CategoryBrands_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GadgetFilters",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetFilters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetFilters_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false),
                    AvatarUrl = table.Column<string>(type: "text", nullable: true),
                    Address = table.Column<string>(type: "text", nullable: true),
                    CCCD = table.Column<string>(type: "text", nullable: true),
                    Gender = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Managers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    FullName = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Managers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Managers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    IsSent = table.Column<bool>(type: "boolean", nullable: false),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SellerApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    ShopName = table.Column<string>(type: "text", nullable: false),
                    ShopAddress = table.Column<string>(type: "text", nullable: false),
                    BusinessModel = table.Column<string>(type: "text", nullable: false),
                    BusinessRegistrationCertificateUrl = table.Column<string>(type: "text", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    RejectReason = table.Column<string>(type: "text", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerApplications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sellers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyName = table.Column<string>(type: "text", nullable: true),
                    ShopName = table.Column<string>(type: "text", nullable: false),
                    ShopAddress = table.Column<string>(type: "text", nullable: false),
                    BusinessModel = table.Column<string>(type: "text", nullable: false),
                    BusinessRegistrationCertificateUrl = table.Column<string>(type: "text", nullable: true),
                    TaxCode = table.Column<string>(type: "text", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sellers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserVerifies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    VerifyCode = table.Column<string>(type: "text", nullable: false),
                    VerifyStatus = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVerifies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserVerifies_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GadgetFilterOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetFilterId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetFilterOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetFilterOptions_GadgetFilters_GadgetFilterId",
                        column: x => x.GadgetFilterId,
                        principalTable: "GadgetFilters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerSubscriptionTrackers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionStatus = table.Column<string>(type: "text", nullable: false),
                    ValidityStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidityEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OverdueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerSubscriptionTrackers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerSubscriptionTrackers_CustomerSubscriptions_Customer~",
                        column: x => x.CustomerSubscriptionId,
                        principalTable: "CustomerSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CustomerSubscriptionTrackers_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "KeywordHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Keyword = table.Column<string>(type: "text", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeywordHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_KeywordHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingMailApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingMailApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingMailApplications_SellerApplications_SellerApplicatio~",
                        column: x => x.SellerApplicationId,
                        principalTable: "SellerApplications",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BannerRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    BannerRequestPriceId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    ExpiredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BannerRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BannerRequests_BannerRequestPrices_BannerRequestPriceId",
                        column: x => x.BannerRequestPriceId,
                        principalTable: "BannerRequestPrices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BannerRequests_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BillingMails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    Mail = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingMails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BillingMails_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Gadgets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: true),
                    BrandId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<int>(type: "integer", nullable: true),
                    ThumbnailUrl = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Url = table.Column<string>(type: "text", nullable: true),
                    ShopId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Vector = table.Column<Vector>(type: "vector(384)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Gadgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Gadgets_Brands_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Brands",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gadgets_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Gadgets_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Gadgets_Shops_ShopId",
                        column: x => x.ShopId,
                        principalTable: "Shops",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SellerSubscriptionTrackers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SellerSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaymentMethod = table.Column<string>(type: "text", nullable: false),
                    PaymentStatus = table.Column<string>(type: "text", nullable: false),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    NumberOfMailLeft = table.Column<int>(type: "integer", nullable: false),
                    SubscriptionStatus = table.Column<string>(type: "text", nullable: false),
                    ValidityStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ValidityEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    OverdueTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SellerSubscriptionTrackers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SellerSubscriptionTrackers_SellerSubscriptions_SellerSubscr~",
                        column: x => x.SellerSubscriptionId,
                        principalTable: "SellerSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SellerSubscriptionTrackers_Sellers_SellerId",
                        column: x => x.SellerId,
                        principalTable: "Sellers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchHistoryResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchHistoryId = table.Column<Guid>(type: "uuid", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchHistoryResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchHistoryResponses_SearchHistories_SearchHistoryId",
                        column: x => x.SearchHistoryId,
                        principalTable: "SearchHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BannerRequestId = table.Column<Guid>(type: "uuid", nullable: true),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Banners_BannerRequests_BannerRequestId",
                        column: x => x.BannerRequestId,
                        principalTable: "BannerRequests",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FavoriteGadgets",
                columns: table => new
                {
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FavoriteGadgets", x => new { x.CustomerId, x.GadgetId });
                    table.ForeignKey(
                        name: "FK_FavoriteGadgets_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FavoriteGadgets_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GadgetDescriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    Type = table.Column<string>(type: "text", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetDescriptions_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GadgetHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetHistories_Customers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Customers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GadgetHistories_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GadgetImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GadgetImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GadgetImages_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Specifications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Specifications_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchGadgetResponses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SearchHistoryResponseId = table.Column<Guid>(type: "uuid", nullable: false),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchGadgetResponses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchGadgetResponses_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SearchGadgetResponses_SearchHistoryResponses_SearchHistoryR~",
                        column: x => x.SearchHistoryResponseId,
                        principalTable: "SearchHistoryResponses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SpecificationKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecificationId = table.Column<Guid>(type: "uuid", nullable: true),
                    GadgetId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationKeys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationKeys_Gadgets_GadgetId",
                        column: x => x.GadgetId,
                        principalTable: "Gadgets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SpecificationKeys_Specifications_SpecificationId",
                        column: x => x.SpecificationId,
                        principalTable: "Specifications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SpecificationValues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SpecificationKeyId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecificationValues", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecificationValues_SpecificationKeys_SpecificationKeyId",
                        column: x => x.SpecificationKeyId,
                        principalTable: "SpecificationKeys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BannerConfigurations",
                columns: new[] { "Id", "MaxNumberOfBanner" },
                values: new object[] { new Guid("13f4ef9b-279e-42ab-9993-daa3ed0602fe"), 5 });

            migrationBuilder.InsertData(
                table: "Brands",
                columns: new[] { "Id", "LogoUrl", "Name" },
                values: new object[,]
                {
                    { new Guid("00e98472-9171-4111-a63a-925c3610563e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Edifier.jpg", "Edifier" },
                    { new Guid("0178b31e-208f-4019-8598-04d6302c5ec3"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Devia.png", "Devia" },
                    { new Guid("01e2ab86-8095-438c-8caf-19d0a9a452a8"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Rock.png", "Rock" },
                    { new Guid("057baa12-e0f6-47f7-80b3-479a6eb3f42d"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Microtek.jpg", "Microtek" },
                    { new Guid("0652a4b5-129c-4188-bee7-71d6985db1f8"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/JBL.jpg", "JBL" },
                    { new Guid("08f2e776-516c-4a40-8e23-9b6aedd2c6de"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Sennheiser.jpg", "Sennheiser" },
                    { new Guid("10fdeaaa-b502-4a8c-b376-49da96397dff"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Sumico.jpg", "Sumico" },
                    { new Guid("122cc69b-2045-4c82-8d77-cd6e9245b25d"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Soul.png", "Soul" },
                    { new Guid("14f7fc83-64ff-45c9-8471-1f4a642ef686"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Jammy.jpg", "Jammy" },
                    { new Guid("1508b3dc-da9c-45c3-899c-7200dc045d2f"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Logitech.jpg", "Logitech" },
                    { new Guid("15f73a3b-8436-4010-b9f1-9ebcb7a710dd"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Harman%20Kardon.png", "Harman Kardon" },
                    { new Guid("1643db3f-45b8-4df8-860a-d7e95bdf2651"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Gigabyte.png", "Gigabyte" },
                    { new Guid("17a5fdab-508b-455a-b024-8fd558c7698c"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Microlab.jpg", "Microlab" },
                    { new Guid("1bd9813b-411a-4029-9de3-cf864bf43f51"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Dareu.jpg", "Dareu" },
                    { new Guid("1c2181c6-cf60-46dc-b973-c34c8fb53478"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Paramax.jpg", "Paramax" },
                    { new Guid("1ce7f45e-ec04-4467-9794-574109271fac"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Havit.jpeg", "Havit" },
                    { new Guid("20c7edea-efa4-4c66-b351-820be6ab9084"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Beats.jpg", "Beats" },
                    { new Guid("2301778e-1802-4086-afb8-818746f11db0"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Bose.jpg", "Bose" },
                    { new Guid("2499794c-4804-4364-baad-ae65890d77d7"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Dell.jpg", "Dell" },
                    { new Guid("26ee773d-e35e-4f6b-885f-03f732e0096c"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Soundmax.jpg", "Soundmax" },
                    { new Guid("39600d1f-9a07-4535-b5d6-6298647788fa"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Pro%20One.png", "Pro One" },
                    { new Guid("3b0410e4-3c29-42ee-b9af-db4d42f37f46"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/LG.jpg", "LG" },
                    { new Guid("3b154b23-3bd4-44d8-8943-c1c02f1668a0"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Belkin.jpg", "Belkin" },
                    { new Guid("3df4ba15-3155-40a8-98c4-f64aef35f62a"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Pisen.jpg", "Pisen" },
                    { new Guid("403d5c4e-01ed-460d-8723-f219658b8147"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Masstel.jpg", "Masstel" },
                    { new Guid("419bccb1-65bf-49b2-a3c1-d380a7be2e68"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/AVA.jpg", "AVA" },
                    { new Guid("45e86cf4-0e1f-44cc-bee6-239ea5520397"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Pack%26Go.jpg", "Pack&Go" },
                    { new Guid("4ab1a520-89b7-42d3-ba98-e3a501abe107"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Rezo.jpg", "Rezo" },
                    { new Guid("4b1424ad-ba91-403f-8093-1a479558e299"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Ovan.jpg", "Ovan" },
                    { new Guid("4d86b327-0a41-4eb2-95f1-4dc1c55059db"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Rapoo.png", "Rapoo" },
                    { new Guid("52b8e46c-ed0a-40df-bb25-216c07b39098"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Soundcore.png", "Soundcore" },
                    { new Guid("532ff7b1-71de-46e1-a4c0-d217140cd472"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Itel.jpg", "Itel" },
                    { new Guid("534611d3-4be3-4127-8839-3b5cf6ad50b9"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Divoom.jpg", "Divoom" },
                    { new Guid("5534e6ee-eb0f-4787-b519-a8cfb18682ae"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Klipsch.jpg", "Klipsch" },
                    { new Guid("589165e2-9282-4c48-80b3-1e25fd9102a8"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Zadez.jpg", "Zadez" },
                    { new Guid("5a01f7f3-14ed-4ef6-8a15-4a2415cd9fb4"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Cooler%20Master.jpg", "Cooler Master" },
                    { new Guid("5a90a711-7b72-4fa3-95b3-c7bcd91cc960"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/IValue.jpg", "IValue" },
                    { new Guid("5f74107e-ea36-43da-a7a0-4c43e8aef224"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Sdrd.png", "Sdrd" },
                    { new Guid("60495ab4-ffe4-404c-9ec2-e8e2d48c75c4"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Sony.jpg", "Sony" },
                    { new Guid("6158ec00-90e6-4b30-b20c-91aeb980dd26"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Nokia.png", "Nokia" },
                    { new Guid("65dc012f-c486-4b84-8bc7-10a15b8a4964"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Celebrat.jpeg", "Celebrat" },
                    { new Guid("66ef437d-bae6-460c-adfb-0ace2b83b73e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Remax.png", "Remax" },
                    { new Guid("680f7bd9-82fd-4463-9620-292062bdbc21"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Hp.png", "Hp" },
                    { new Guid("68b782b1-eda2-4f0d-88c6-e9d74a39a397"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Honor.jpg", "Honor" },
                    { new Guid("6b3256ea-fb3f-4fd7-adb0-80a6a18b864e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Creative.jpg", "Creative" },
                    { new Guid("6dc7e374-ece0-4779-81a1-5eb40d6f0608"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Marshall.png", "Marshall" },
                    { new Guid("710f0631-bc09-4837-ae35-82d87b34c411"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/E-Dra.jpg", "E-Dra" },
                    { new Guid("73f2c1a2-a6d3-4606-a09d-6c4645c243a9"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Prolink.jpg", "ProLink" },
                    { new Guid("7537c078-2b87-4286-9b74-be18ad3fc912"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Enkor.jpg", "Enkor" },
                    { new Guid("76c2e452-64fb-4c0e-b131-fae9781d901e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Inoi.png", "Inoi" },
                    { new Guid("78142080-67b6-4be9-886c-6a41a7678f3a"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Corsair.png", "Corsair" },
                    { new Guid("79a4d1e8-cabc-4b86-9ec5-d259e241d6d4"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Audio-technica.jpg", "Audio-technica" },
                    { new Guid("7decf70d-79ff-4af8-8a7a-00c206b9d123"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Mad%20Catz.jpg", "Mad Catz" },
                    { new Guid("7ed8e47c-04d8-4197-aa96-b75b305485d0"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Sounarc.jpg", "Sounarc" },
                    { new Guid("803f2378-adc8-4bc2-ace0-5d00ce66a22b"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/UmeTravel.jpg", "UmeTravel" },
                    { new Guid("80d20e0e-31e2-406b-b21b-476fc3eeb34c"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Tplink.png", "Tplink" },
                    { new Guid("81f79d0f-0567-453e-b946-441d81f1b65b"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Defunc.jpg", "Defunc" },
                    { new Guid("82817f1b-be6c-4b9b-9f8f-49839cbf4bf8"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/HyperX.jpg", "HyperX" },
                    { new Guid("82a8b1ca-fe27-406b-bcb4-22b11548cdfa"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Acer.jpg", "Acer" },
                    { new Guid("8968277e-cb34-4f11-aaba-191011f8914a"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Birici.jpg", "Birici" },
                    { new Guid("8d453e47-d824-4c81-a0ea-14272c738979"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Vaio.jpg", "Vaio" },
                    { new Guid("901a9e9f-5545-475e-b591-b4dd01527a95"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Oppo.png", "Oppo" },
                    { new Guid("904b2351-951b-4efa-877d-d759a0772958"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Monster.jpg", "Monster" },
                    { new Guid("93d79b77-85a3-46d3-bd3a-98844c13f2d6"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Shokz.png", "Shokz" },
                    { new Guid("9f112f02-284a-49d9-8e90-b4e1402ec943"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/KZ.png", "KZ" },
                    { new Guid("a2eecdba-0c2f-4681-bb6f-b5d2df684d8a"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Viettel.jpg", "Viettel" },
                    { new Guid("a76154d5-bb61-439d-bd99-35df70b8d616"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Denon.jpg", "Denon" },
                    { new Guid("a953e193-a6f6-4aa0-b804-ed577c39aea1"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/F.Power.png", "F.Power" },
                    { new Guid("aa8a3a33-47be-4e0f-a148-ee3c3c9c82d3"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Fenda.png", "Fenda" },
                    { new Guid("aaa48259-deb6-4fbb-804a-82b24bf006f2"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Huawei.jpg", "Huawei" },
                    { new Guid("ac7694d0-0ea2-455a-b1b4-13b8e6c75721"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Alpha%20Works.jpg", "Alpha Works" },
                    { new Guid("ac8e62e6-17bd-4484-8a03-8087ec593992"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Bagitech.png", "Bagitech" },
                    { new Guid("ad805cc1-06c9-4080-928f-ac76293d0aab"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Disney.jpg", "Disney" },
                    { new Guid("ade6396d-9078-4575-9eb1-90402000f1ce"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Zidli.png", "Zidli" },
                    { new Guid("aed04607-3957-4214-8fb4-c79e1a8dfaf3"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Aukey.jpg", "Aukey" },
                    { new Guid("af97baf7-367d-4887-a7e6-9dbb65c8ed2d"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Lenovo.png", "Lenovo" },
                    { new Guid("b2caff82-b89e-4ca8-8ad2-433c10b9eeda"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Steelseries.jpg", "Steelseries" },
                    { new Guid("b33ff055-5c7d-426a-b4fe-7a08290ee78d"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Baseus.png", "Baseus" },
                    { new Guid("b34a95a5-f4bf-4189-8f1c-feb6bd7caaf7"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Xiaomi.png", "Xiaomi" },
                    { new Guid("b5a610d7-6592-4be5-a4a7-ce9e945d7328"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Soundpeats.png", "Soundpeats" },
                    { new Guid("b72baf1d-38e6-48da-8525-d575005f0165"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Dalton.jpg", "Dalton" },
                    { new Guid("bb8a59c2-b165-45b3-801b-7592666c224a"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Somic.jpg", "Somic" },
                    { new Guid("bd300cf8-5d59-46ff-b501-6e9e71bcadca"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Klatre.jpg", "Klatre" },
                    { new Guid("bdbbaea4-fa7d-46b8-bb70-f548322fc3a2"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Asus.png", "Asus" },
                    { new Guid("c32d3ba9-95d7-4cd5-a8f7-4dd10e024da0"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Tako.jpg", "Tako" },
                    { new Guid("c5cd02d6-2c9b-41f0-ba81-4c25097833be"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/X-Mini.jpg", "X-Mini" },
                    { new Guid("c63835ee-bcce-4d8b-85f8-e14678b530ce"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Benco.png", "Benco" },
                    { new Guid("c91d0f29-0944-4534-9430-dc577bccdd3f"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Realme.png", "Realme" },
                    { new Guid("cf2b71ae-c67f-43e3-9df9-b42573c09488"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Mobell.jpg", "Mobell" },
                    { new Guid("d40f2c4c-bbd3-4989-8e2c-43661c8a7b28"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Zenbos.jpg", "Zenbos" },
                    { new Guid("d5ecc812-3dd2-4809-a5f4-11cb3c0d3595"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Vivo.jpg", "Vivo" },
                    { new Guid("d6462b84-45a9-4d69-8e96-a2b4d0454ced"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Plantronics.png", "Plantronics" },
                    { new Guid("d6c89b99-409b-48ec-8fc0-a2e34c73ee1c"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Nanomax.jpg", "Nanomax" },
                    { new Guid("d9e044bb-aa3f-44d4-ae9e-436d80905958"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Mozard.jpg", "Mozard" },
                    { new Guid("e800ce83-2289-497d-80fe-f5f713890ef6"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Apple.jpg", "Apple" },
                    { new Guid("e9d16909-dfe9-49f2-81ca-44e8504fb304"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Razer.jpg", "Razer" },
                    { new Guid("eb11099c-8c02-433c-a522-62d6898dd64e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Sudio.jpg", "Sudio" },
                    { new Guid("eb8928a3-bfb8-4f87-a24d-2fc2fb7943fc"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Icore.jpg", "ICore" },
                    { new Guid("ec2ba326-c452-4a5a-94a2-6a46f0e81282"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/MSI.jpg", "MSI" },
                    { new Guid("ed1bdc18-20ce-4707-9e49-eefb7475e702"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Elecom.png", "Elecom" },
                    { new Guid("f1f72e2b-ef64-4583-8907-5f9c9df5b9fd"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Samsung.jpg", "Samsung" },
                    { new Guid("f2671cf2-fc2e-4374-945f-e63bed491ad2"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/ZTE.png", "ZTE" },
                    { new Guid("f96ce9ef-0fd1-4549-a853-bc768c0c337e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Jabra.png", "Jabra" },
                    { new Guid("f9d44622-20bb-472b-aac9-02b6c52c3f1e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Pioneer.jpg", "Pioneer" },
                    { new Guid("fba38fed-c1b1-4ccb-85e1-f63553a838f7"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Tecno.png", "Tecno" },
                    { new Guid("fc1da7de-b46f-4a61-b3a9-ba1db77a0a44"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/TCL.png", "TCL" },
                    { new Guid("ff25e48b-b326-4a63-9bcf-a5cb2c51886b"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Brands/Anker.jpg", "Anker" }
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Laptop" },
                    { new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Điện thoại" },
                    { new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Thiết bị âm thanh" }
                });

            migrationBuilder.InsertData(
                table: "CustomerSubscriptions",
                columns: new[] { "Id", "Description", "Duration", "Name", "Price", "Status", "Type" },
                values: new object[] { new Guid("53e4a057-7dc4-4c0d-9ef6-d8f6b97fb7d8"), "Sử dụng tính năng tìm kiếm bằng ngôn ngữ tự nhiên không giới hạn", 30, "Gói Standard", 29000, "Active", "Standard" });

            migrationBuilder.InsertData(
                table: "SellerSubscriptions",
                columns: new[] { "Id", "Description", "Duration", "Name", "Price", "Status", "Type" },
                values: new object[,]
                {
                    { new Guid("689bf59d-1dbf-41f7-8d21-93f8bb92b999"), "Gửi 10 mail/ngày", 30, "Gói Standard", 29000, "Active", "Standard" },
                    { new Guid("76636ac8-444a-478b-8973-ef3da2925c53"), "Gửi 25 mail/ngày", 30, "Gói Premium", 49000, "Active", "Standard" }
                });

            migrationBuilder.InsertData(
                table: "Shops",
                columns: new[] { "Id", "LogoUrl", "Name", "WebsiteUrl" },
                values: new object[,]
                {
                    { new Guid("1f3c2205-1e9c-4efa-9c6b-57819c114793"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Shops/Thegioididong.jpg", "Thế Giới Di Động", "https://www.thegioididong.com" },
                    { new Guid("bafc41ac-9b92-4af3-a8da-84cac529be43"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Shops/Phongvu.jpg", "Phong Vũ", "https://phongvu.vn" },
                    { new Guid("e5233830-7d0b-45d2-953e-0fe3bb3cc09e"), "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/Shops/Fptshop.jpg", "FPT Shop", "https://fptshop.com.vn" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "LoginMethod", "Password", "Role", "Status" },
                values: new object[,]
                {
                    { new Guid("0a4590ef-a843-4489-94ef-762259b78688"), "seller2@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Seller", "Active" },
                    { new Guid("27a15668-0d9e-4276-a0df-791b7dfeed9e"), "manager1@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Manager", "Active" },
                    { new Guid("4808ef8f-f46f-461f-ba41-962e16aec45b"), "admin2@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Admin", "Active" },
                    { new Guid("5a57223a-6e7d-401b-a19e-bf9282db69fe"), "customer1@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Customer", "Active" },
                    { new Guid("638eadf4-a17f-4f16-a9dd-6d12a5b5a80a"), "manager2@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Manager", "Active" },
                    { new Guid("69f7c054-00d2-48f3-9e86-21081f095340"), "admin1@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Admin", "Active" },
                    { new Guid("8d8707e4-299d-450b-bc5c-f8ab49504fce"), "customer2@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Customer", "Active" },
                    { new Guid("f56cc7e6-725c-4090-83b8-77f5ce6a53c8"), "seller1@gmail.com", "Default", "5wJ0xMM/o1DPaTby8haqjIeEx0hqnJfyw4SmivYCGT17khWSPTXkR+56laWZr3/U", "Seller", "Active" }
                });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "FullName", "UserId" },
                values: new object[,]
                {
                    { new Guid("ac09dfb9-0216-4b11-81a6-42c959d95ccb"), "Thái Hưng Đạo", new Guid("4808ef8f-f46f-461f-ba41-962e16aec45b") },
                    { new Guid("c3279608-d5f2-4da9-a942-e14573fa41e7"), "Liêu Bình An", new Guid("69f7c054-00d2-48f3-9e86-21081f095340") }
                });

            migrationBuilder.InsertData(
                table: "CategoryBrands",
                columns: new[] { "BrandId", "CategoryId" },
                values: new object[,]
                {
                    { new Guid("00e98472-9171-4111-a63a-925c3610563e"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("0178b31e-208f-4019-8598-04d6302c5ec3"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("01e2ab86-8095-438c-8caf-19d0a9a452a8"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("057baa12-e0f6-47f7-80b3-479a6eb3f42d"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("0652a4b5-129c-4188-bee7-71d6985db1f8"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("08f2e776-516c-4a40-8e23-9b6aedd2c6de"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("10fdeaaa-b502-4a8c-b376-49da96397dff"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("122cc69b-2045-4c82-8d77-cd6e9245b25d"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("14f7fc83-64ff-45c9-8471-1f4a642ef686"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("1508b3dc-da9c-45c3-899c-7200dc045d2f"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("15f73a3b-8436-4010-b9f1-9ebcb7a710dd"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("1643db3f-45b8-4df8-860a-d7e95bdf2651"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("17a5fdab-508b-455a-b024-8fd558c7698c"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("1bd9813b-411a-4029-9de3-cf864bf43f51"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("1c2181c6-cf60-46dc-b973-c34c8fb53478"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("1ce7f45e-ec04-4467-9794-574109271fac"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("20c7edea-efa4-4c66-b351-820be6ab9084"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("2301778e-1802-4086-afb8-818746f11db0"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("2499794c-4804-4364-baad-ae65890d77d7"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("2499794c-4804-4364-baad-ae65890d77d7"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("26ee773d-e35e-4f6b-885f-03f732e0096c"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("39600d1f-9a07-4535-b5d6-6298647788fa"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("3b0410e4-3c29-42ee-b9af-db4d42f37f46"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("3b0410e4-3c29-42ee-b9af-db4d42f37f46"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("3b154b23-3bd4-44d8-8943-c1c02f1668a0"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("3df4ba15-3155-40a8-98c4-f64aef35f62a"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("403d5c4e-01ed-460d-8723-f219658b8147"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("403d5c4e-01ed-460d-8723-f219658b8147"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("419bccb1-65bf-49b2-a3c1-d380a7be2e68"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("45e86cf4-0e1f-44cc-bee6-239ea5520397"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("4ab1a520-89b7-42d3-ba98-e3a501abe107"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("4b1424ad-ba91-403f-8093-1a479558e299"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("4d86b327-0a41-4eb2-95f1-4dc1c55059db"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("52b8e46c-ed0a-40df-bb25-216c07b39098"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("532ff7b1-71de-46e1-a4c0-d217140cd472"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("534611d3-4be3-4127-8839-3b5cf6ad50b9"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("5534e6ee-eb0f-4787-b519-a8cfb18682ae"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("589165e2-9282-4c48-80b3-1e25fd9102a8"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("5a01f7f3-14ed-4ef6-8a15-4a2415cd9fb4"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("5a90a711-7b72-4fa3-95b3-c7bcd91cc960"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("5f74107e-ea36-43da-a7a0-4c43e8aef224"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("60495ab4-ffe4-404c-9ec2-e8e2d48c75c4"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("6158ec00-90e6-4b30-b20c-91aeb980dd26"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("65dc012f-c486-4b84-8bc7-10a15b8a4964"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("66ef437d-bae6-460c-adfb-0ace2b83b73e"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("680f7bd9-82fd-4463-9620-292062bdbc21"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("68b782b1-eda2-4f0d-88c6-e9d74a39a397"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("6b3256ea-fb3f-4fd7-adb0-80a6a18b864e"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("6dc7e374-ece0-4779-81a1-5eb40d6f0608"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("710f0631-bc09-4837-ae35-82d87b34c411"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("73f2c1a2-a6d3-4606-a09d-6c4645c243a9"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("7537c078-2b87-4286-9b74-be18ad3fc912"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("76c2e452-64fb-4c0e-b131-fae9781d901e"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("78142080-67b6-4be9-886c-6a41a7678f3a"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("79a4d1e8-cabc-4b86-9ec5-d259e241d6d4"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("7decf70d-79ff-4af8-8a7a-00c206b9d123"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("7ed8e47c-04d8-4197-aa96-b75b305485d0"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("803f2378-adc8-4bc2-ace0-5d00ce66a22b"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("80d20e0e-31e2-406b-b21b-476fc3eeb34c"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("81f79d0f-0567-453e-b946-441d81f1b65b"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("82817f1b-be6c-4b9b-9f8f-49839cbf4bf8"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("82a8b1ca-fe27-406b-bcb4-22b11548cdfa"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("82a8b1ca-fe27-406b-bcb4-22b11548cdfa"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("8968277e-cb34-4f11-aaba-191011f8914a"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("8d453e47-d824-4c81-a0ea-14272c738979"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("901a9e9f-5545-475e-b591-b4dd01527a95"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("901a9e9f-5545-475e-b591-b4dd01527a95"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("904b2351-951b-4efa-877d-d759a0772958"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("93d79b77-85a3-46d3-bd3a-98844c13f2d6"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("9f112f02-284a-49d9-8e90-b4e1402ec943"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("a2eecdba-0c2f-4681-bb6f-b5d2df684d8a"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("a76154d5-bb61-439d-bd99-35df70b8d616"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("a953e193-a6f6-4aa0-b804-ed577c39aea1"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("aa8a3a33-47be-4e0f-a148-ee3c3c9c82d3"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("aaa48259-deb6-4fbb-804a-82b24bf006f2"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("ac7694d0-0ea2-455a-b1b4-13b8e6c75721"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("ac8e62e6-17bd-4484-8a03-8087ec593992"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("ad805cc1-06c9-4080-928f-ac76293d0aab"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("ade6396d-9078-4575-9eb1-90402000f1ce"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("aed04607-3957-4214-8fb4-c79e1a8dfaf3"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("af97baf7-367d-4887-a7e6-9dbb65c8ed2d"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("b2caff82-b89e-4ca8-8ad2-433c10b9eeda"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("b33ff055-5c7d-426a-b4fe-7a08290ee78d"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("b34a95a5-f4bf-4189-8f1c-feb6bd7caaf7"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("b34a95a5-f4bf-4189-8f1c-feb6bd7caaf7"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("b5a610d7-6592-4be5-a4a7-ce9e945d7328"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("b72baf1d-38e6-48da-8525-d575005f0165"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("bb8a59c2-b165-45b3-801b-7592666c224a"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("bd300cf8-5d59-46ff-b501-6e9e71bcadca"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("bdbbaea4-fa7d-46b8-bb70-f548322fc3a2"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("bdbbaea4-fa7d-46b8-bb70-f548322fc3a2"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("bdbbaea4-fa7d-46b8-bb70-f548322fc3a2"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("c32d3ba9-95d7-4cd5-a8f7-4dd10e024da0"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("c5cd02d6-2c9b-41f0-ba81-4c25097833be"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("c63835ee-bcce-4d8b-85f8-e14678b530ce"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("c91d0f29-0944-4534-9430-dc577bccdd3f"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("c91d0f29-0944-4534-9430-dc577bccdd3f"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("cf2b71ae-c67f-43e3-9df9-b42573c09488"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("cf2b71ae-c67f-43e3-9df9-b42573c09488"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("d40f2c4c-bbd3-4989-8e2c-43661c8a7b28"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("d5ecc812-3dd2-4809-a5f4-11cb3c0d3595"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("d6462b84-45a9-4d69-8e96-a2b4d0454ced"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("d6c89b99-409b-48ec-8fc0-a2e34c73ee1c"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("d9e044bb-aa3f-44d4-ae9e-436d80905958"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("e800ce83-2289-497d-80fe-f5f713890ef6"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("e800ce83-2289-497d-80fe-f5f713890ef6"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("e800ce83-2289-497d-80fe-f5f713890ef6"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("e9d16909-dfe9-49f2-81ca-44e8504fb304"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("eb11099c-8c02-433c-a522-62d6898dd64e"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("eb8928a3-bfb8-4f87-a24d-2fc2fb7943fc"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("ec2ba326-c452-4a5a-94a2-6a46f0e81282"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("ec2ba326-c452-4a5a-94a2-6a46f0e81282"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("ed1bdc18-20ce-4707-9e49-eefb7475e702"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("f1f72e2b-ef64-4583-8907-5f9c9df5b9fd"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0") },
                    { new Guid("f1f72e2b-ef64-4583-8907-5f9c9df5b9fd"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("f1f72e2b-ef64-4583-8907-5f9c9df5b9fd"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("f2671cf2-fc2e-4374-945f-e63bed491ad2"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("f96ce9ef-0fd1-4549-a853-bc768c0c337e"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("f9d44622-20bb-472b-aac9-02b6c52c3f1e"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") },
                    { new Guid("fba38fed-c1b1-4ccb-85e1-f63553a838f7"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("fc1da7de-b46f-4a61-b3a9-ba1db77a0a44"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d") },
                    { new Guid("ff25e48b-b326-4a63-9bcf-a5cb2c51886b"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6") }
                });

            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "Id", "Address", "AvatarUrl", "CCCD", "DateOfBirth", "FullName", "Gender", "PhoneNumber", "UserId" },
                values: new object[,]
                {
                    { new Guid("d61a23a0-dbfe-429f-9c1f-5e0b955beff9"), null, null, null, null, "Nguỵ Chi Mai", null, null, new Guid("5a57223a-6e7d-401b-a19e-bf9282db69fe") },
                    { new Guid("f9ff20a7-05b3-4dbc-b260-26ef16db8513"), null, null, null, null, "Lê Thuý Hiền", null, null, new Guid("8d8707e4-299d-450b-bc5c-f8ab49504fce") }
                });

            migrationBuilder.InsertData(
                table: "GadgetFilters",
                columns: new[] { "Id", "CategoryId", "Name" },
                values: new object[,]
                {
                    { new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Công suất" },
                    { new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "RAM" },
                    { new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Loại" },
                    { new Guid("2b3cdf91-68aa-491a-8541-8154990f30cc"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Apple" },
                    { new Guid("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Cổng sạc" },
                    { new Guid("3df19307-ee88-4e19-8e6f-820d432caa94"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Dung lượng lưu trữ" },
                    { new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Giá" },
                    { new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Giá" },
                    { new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Kích cỡ màn hình" },
                    { new Guid("6228626e-b8c6-4edf-8b42-9ae0e1526771"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "RAM" },
                    { new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Giá" },
                    { new Guid("8d20ec78-aecf-4fb3-8df1-d976f86558fe"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Hệ điều hành" },
                    { new Guid("903f0975-b33f-4e16-9243-94f2530d26dd"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Jack cắm" },
                    { new Guid("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "AMD" },
                    { new Guid("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Độ phân giải" },
                    { new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Incel Core i/Core/Celeron/Pentium" },
                    { new Guid("af177595-3999-4055-8021-23fe477ac074"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Intel Core Ultra (mới nhất)" },
                    { new Guid("b2a18dba-2e74-4918-a263-b2555fb8faeb"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Tính năng sạc" },
                    { new Guid("bea42814-f615-48e0-9937-ca1916266ff1"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Tính năng đặc biệt" },
                    { new Guid("c12b48ec-306d-4797-bdef-92503483fc18"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Tần sồ quét" },
                    { new Guid("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Tiện ích" },
                    { new Guid("d4925542-1eea-4e2b-b237-ac0249ad9044"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Tần sồ quét" },
                    { new Guid("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), new Guid("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), "Thời lượng pin" },
                    { new Guid("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), new Guid("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), "Ổ cứng" },
                    { new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), new Guid("ea4183e8-5a94-401c-865d-e000b5d2b72d"), "Độ phân giải" }
                });

            migrationBuilder.InsertData(
                table: "Managers",
                columns: new[] { "Id", "FullName", "UserId" },
                values: new object[,]
                {
                    { new Guid("0ca3bd29-b37e-49f2-8fb6-7c44efca1745"), "Hình Trọng Hùng", new Guid("638eadf4-a17f-4f16-a9dd-6d12a5b5a80a") },
                    { new Guid("2723dacf-59e1-4d66-bf90-5813432c79a8"), "Mã Duy Hình", new Guid("27a15668-0d9e-4276-a0df-791b7dfeed9e") }
                });

            migrationBuilder.InsertData(
                table: "Sellers",
                columns: new[] { "Id", "BusinessModel", "BusinessRegistrationCertificateUrl", "CompanyName", "PhoneNumber", "ShopAddress", "ShopName", "TaxCode", "UserId" },
                values: new object[,]
                {
                    { new Guid("9488d26a-de33-4bf6-b038-be5d1d641940"), "Personal", null, null, "0877094491", "37 Đ. Lê Quý Đôn, P. 7, Q3, TP. HCM", "Cửa hàng Thuỳ Uyên", "1779231738", new Guid("f56cc7e6-725c-4090-83b8-77f5ce6a53c8") },
                    { new Guid("cd83c20c-dc5c-4115-87b2-a218e6584301"), "Company", "https://storage.googleapis.com/fbdemo-f9d5f.appspot.com/BusinessRegistrationUrl/Seller2.jpg", "Công Ty Nhật Hạ", "0362961803", "128 Đ. Nguyễn Phong Sắc, Q. Cầu Giấy, TP. Hà Nội", "Cửa hàng Nhật Hạ", "4067001394", new Guid("0a4590ef-a843-4489-94ef-762259b78688") }
                });

            migrationBuilder.InsertData(
                table: "GadgetFilterOptions",
                columns: new[] { "Id", "GadgetFilterId", "Value" },
                values: new object[,]
                {
                    { new Guid("029f909e-94c1-41fe-a6ab-8787fc5a083b"), new Guid("d4925542-1eea-4e2b-b237-ac0249ad9044"), "120 Hz" },
                    { new Guid("068e0212-560d-468a-a920-77adac41ce97"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "8 GB" },
                    { new Guid("08b32383-ad68-4666-be35-00feb1bec3d7"), new Guid("bea42814-f615-48e0-9937-ca1916266ff1"), "Công nghệ NFC" },
                    { new Guid("08c506a2-d27a-4f23-834d-f53864001062"), new Guid("b2a18dba-2e74-4918-a263-b2555fb8faeb"), "Sạc không dây" },
                    { new Guid("09d0883c-1f45-4441-b9fa-0db1b29ae258"), new Guid("af177595-3999-4055-8021-23fe477ac074"), "Ultra 9" },
                    { new Guid("0afcdda7-44c7-424e-ac0e-82b8ada87b53"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "16.2 inch" },
                    { new Guid("0c5ba122-46e0-491d-9bb5-384e5897d6c1"), new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), "Từ 10W trở xuống" },
                    { new Guid("0d4fbfb3-f1b7-478a-b047-018d9726df64"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Core 7" },
                    { new Guid("106cd227-51e8-4978-b113-107aa9808af5"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "4 GB" },
                    { new Guid("10a6f572-916a-40b1-b53f-d74f3ec8c836"), new Guid("2b3cdf91-68aa-491a-8541-8154990f30cc"), "Apple M1" },
                    { new Guid("10ffad03-d8de-4cdb-8717-6d195a01b2e8"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "36 GB" },
                    { new Guid("14588df7-ff2d-4e67-a71f-e484fab0a376"), new Guid("3df19307-ee88-4e19-8e6f-820d432caa94"), "512 GB" },
                    { new Guid("181dc2ef-9bec-4fad-b06a-75c67f27ef12"), new Guid("d4925542-1eea-4e2b-b237-ac0249ad9044"), "144 Hz" },
                    { new Guid("1bb4507a-8d94-4bc6-ba27-e1cc8e50de97"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Từ 500.000đ - 1 triệu" },
                    { new Guid("202da262-bd50-446a-973d-83f71e038344"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "17 inch" },
                    { new Guid("210c0319-0fc1-4d44-af52-deda8ce8018f"), new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), "Từ 7 - 13 triệu" },
                    { new Guid("25899e6c-b7db-44a3-bc08-ac73404281ea"), new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), "40W - 100W" },
                    { new Guid("27c9f3a9-137b-4b45-96c1-d238726da6a9"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "QQVGA" },
                    { new Guid("2824fd1b-77dd-46a5-867f-159bfba9e4a9"), new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), "500W - 1000W" },
                    { new Guid("29123eac-0e54-4ff7-b10b-ab5462b5ba4f"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "14.2 inch" },
                    { new Guid("2b603398-f28f-4707-bd02-f766776e0691"), new Guid("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), "Ryzen 4" },
                    { new Guid("2cbe7bdf-a1f1-444f-bb95-d0a31437f70d"), new Guid("903f0975-b33f-4e16-9243-94f2530d26dd"), "Type-C" },
                    { new Guid("2d74f71e-2ffd-48c2-a456-c90ccc3bc1b8"), new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), "100W - 500W" },
                    { new Guid("2e3d901f-13b6-47e4-8815-cedad93393a6"), new Guid("3df19307-ee88-4e19-8e6f-820d432caa94"), "1TB" },
                    { new Guid("2e65afad-61c1-4edb-8d74-ea66d3527ea0"), new Guid("c12b48ec-306d-4797-bdef-92503483fc18"), "144 Hz" },
                    { new Guid("3069802c-126c-4ff1-8944-4f4a25f397d7"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "16.1 inch" },
                    { new Guid("320cd81f-81cd-4462-bdf1-679117c35bca"), new Guid("6228626e-b8c6-4edf-8b42-9ae0e1526771"), "8 GB" },
                    { new Guid("32348105-592c-45b9-8fb0-43b0934f34b6"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "1.5K+" },
                    { new Guid("35c8d5b9-5d0b-4066-84ab-0af695248382"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "Retina (iPhone)" },
                    { new Guid("368bb62e-b40e-4745-b87e-0fc9543736fb"), new Guid("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), "HD" },
                    { new Guid("37f2f496-2c1f-4b1c-aeb7-9d0f9cead239"), new Guid("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), "Full HD" },
                    { new Guid("38593320-d491-4dff-b7d7-5bcaed43fa9b"), new Guid("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), "Có mic đàm thoại" },
                    { new Guid("3cd1bdd1-f7e8-46af-a33d-838cac68ba7f"), new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), "Từ 15 - 20 triệu" },
                    { new Guid("3f29e574-3ccb-45f2-a392-e79915ca5951"), new Guid("af177595-3999-4055-8021-23fe477ac074"), "Ultra 7" },
                    { new Guid("44aa2cee-a6ea-4924-9432-ed64c16f3144"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Loa kéo" },
                    { new Guid("4645b0bd-9c72-4c50-83a9-c05378dca2ba"), new Guid("bea42814-f615-48e0-9937-ca1916266ff1"), "Hỗ trợ 5G" },
                    { new Guid("470598e2-1d97-4465-a70a-83a7b5f29165"), new Guid("8d20ec78-aecf-4fb3-8df1-d976f86558fe"), "Android" },
                    { new Guid("4b2253e8-a6e3-4bcb-88f6-080465e806c9"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "48 GB" },
                    { new Guid("4e69bb48-5dc8-4bad-a7bc-fcbdec870bfb"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "QVGA" },
                    { new Guid("4fcbfbd4-70b9-4090-bfd7-39141301d389"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Chụp tai" },
                    { new Guid("566df58d-3684-4159-95d9-5c5b5097bf7d"), new Guid("6228626e-b8c6-4edf-8b42-9ae0e1526771"), "6 GB" },
                    { new Guid("579f7ae2-73d9-4b99-a85f-9e0e40b83808"), new Guid("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), "Ryzen 7" },
                    { new Guid("58d42732-d950-48a2-9836-370bc0e4595e"), new Guid("2b3cdf91-68aa-491a-8541-8154990f30cc"), "Apple M3 Pro" },
                    { new Guid("59629a6e-cd79-4a59-88e0-316c2db0082a"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "18 GB" },
                    { new Guid("5accff40-716c-4781-ac44-2cce671a4b2e"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Loa thanh, soundbar" },
                    { new Guid("5b69c6ac-81e6-489f-b3d8-1591abac8315"), new Guid("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), "6 - 8 tiếng" },
                    { new Guid("5beea968-565d-4ef5-b0d9-eb1aaedaf308"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "64 GB" },
                    { new Guid("5c1b14aa-50f7-4914-b42c-cb620251f619"), new Guid("3df19307-ee88-4e19-8e6f-820d432caa94"), "128 GB" },
                    { new Guid("5fbfd5d4-679a-486f-b1e6-e50daea79620"), new Guid("2b3cdf91-68aa-491a-8541-8154990f30cc"), "Apple M3 Max" },
                    { new Guid("60ed57b9-c305-4435-b061-00e149456393"), new Guid("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), "Micro USB" },
                    { new Guid("61333095-8e95-488c-adc7-d145b95cd1e0"), new Guid("af177595-3999-4055-8021-23fe477ac074"), "Ultra 5" },
                    { new Guid("61cb4f58-9e58-4feb-b05d-b97e8b705560"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "18 inch" },
                    { new Guid("63a3c408-919c-412e-93bb-8f5494cb5b87"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Core i5" },
                    { new Guid("6432220b-50f9-4008-a9d7-50895959cb3f"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Từ 4 - 7 triệu" },
                    { new Guid("6575d15b-29b2-4e0a-a362-b4408e064e71"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "16 inch" },
                    { new Guid("68e68a4c-72a7-4e5b-bfd1-e5f43a1faf2f"), new Guid("903f0975-b33f-4e16-9243-94f2530d26dd"), "3.5 mm" },
                    { new Guid("69d68f1e-0c3f-43b6-a10c-59a324245152"), new Guid("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), "4 - 6 tiếng" },
                    { new Guid("69e11515-d9d1-43ff-b3df-0e7444424f2b"), new Guid("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), "2K" },
                    { new Guid("6a1dc6f4-dae1-4524-b63d-a50adcb24ced"), new Guid("bea42814-f615-48e0-9937-ca1916266ff1"), "Kháng nước, bụi" },
                    { new Guid("6be9b8a7-eef6-4bf4-b83f-c042ba837072"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "13.6 inch" },
                    { new Guid("6d97d84d-b1b3-4dbb-af75-b21f160b3463"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Trên 10 triệu" },
                    { new Guid("6d9d5111-ce2c-4d3a-8e8a-d0333cb24b6b"), new Guid("2b3cdf91-68aa-491a-8541-8154990f30cc"), "Apple M3" },
                    { new Guid("6f6c9fbd-3d2d-40c2-b49c-7b3a4d9331b3"), new Guid("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), "Chống nước" },
                    { new Guid("71b2beba-a326-42a5-9391-30ccb3aab053"), new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), "Từ 10 - 15 triệu" },
                    { new Guid("72d245f3-4bee-4237-bb31-924fdcc83e78"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "13.3 inch" },
                    { new Guid("73046e00-9602-4ecf-bec8-ae9e33c6a19d"), new Guid("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), "Nhỏ gọn" },
                    { new Guid("762077ac-5eda-4080-8b0d-90e64c8ca9bd"), new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), "10W - 40W" },
                    { new Guid("766d2310-25c7-4f4f-88d6-8046c96a8c3c"), new Guid("6228626e-b8c6-4edf-8b42-9ae0e1526771"), "4 GB" },
                    { new Guid("772e3e58-b3de-4487-98cd-644059d01736"), new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), "Từ 20 - 25 triệu" },
                    { new Guid("78d883b3-a3e7-43f1-8033-e23ee5140bfa"), new Guid("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), "Ryzen 9" },
                    { new Guid("7b2270f4-7527-465a-86f0-f9809ec09add"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "32 GB" },
                    { new Guid("7b492fc0-59f3-4624-b637-bca6acbbb02c"), new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), "Từ 13 - 20 triệu" },
                    { new Guid("7c36a492-87da-486f-a11a-b665049670a5"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "11.6 inch" },
                    { new Guid("7d1b98bd-bacc-4159-bb88-196ee3ebf3eb"), new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), "Từ 4 - 7 triệu" },
                    { new Guid("7e5e22e2-9e65-4a24-9661-efe8bcb016e3"), new Guid("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), "SSD 256 GB" },
                    { new Guid("7f2df584-d8a6-49a1-b4c6-0ed25511b198"), new Guid("c12b48ec-306d-4797-bdef-92503483fc18"), "90 Hz" },
                    { new Guid("7f94026b-cf8c-4c4a-9f92-b89dba5a9f95"), new Guid("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), "Chống ồn" },
                    { new Guid("809d88e8-a293-44ec-b784-28772da2342c"), new Guid("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), "4K" },
                    { new Guid("82d5d8df-7a63-4485-8eb0-581c91c9e8ce"), new Guid("d4925542-1eea-4e2b-b237-ac0249ad9044"), "90 Hz" },
                    { new Guid("85978333-2087-402c-aea9-f8cee1d08d54"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Từ 200.000 - 500.000đ" },
                    { new Guid("8790fec7-6042-4122-9889-a3f160b36a0e"), new Guid("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), "SSD 1 TB" },
                    { new Guid("8a395ad3-9138-4d8a-a398-a3c0f0c96f20"), new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), "Từ 2 - 4 triệu" },
                    { new Guid("8c64ed8a-97dc-4139-93eb-cbf70dc255d2"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Core i9" },
                    { new Guid("8f1acc4d-74b1-4789-910e-e5273b34c8d8"), new Guid("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), "SSD 512 GB" },
                    { new Guid("923a4677-0a7b-4884-8603-0d1bfef720d0"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Celebron/Pentium" },
                    { new Guid("945848f1-6c6c-4412-83b0-288f94801350"), new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), "Trên 20 triệu" },
                    { new Guid("9b9c427d-7551-4132-9333-b4d9e884d842"), new Guid("c12b48ec-306d-4797-bdef-92503483fc18"), "120 Hz" },
                    { new Guid("9c548fd8-0ad6-4d54-b5c1-86c4819dd87b"), new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), "Trên 30 triệu" },
                    { new Guid("a0212238-7541-45b6-8aac-e1b68cb3f824"), new Guid("b2a18dba-2e74-4918-a263-b2555fb8faeb"), "Sạc siêu nhanh (từ 60W)" },
                    { new Guid("a048df56-f9ee-4c9f-bdaa-79fafa9f2a1f"), new Guid("6228626e-b8c6-4edf-8b42-9ae0e1526771"), "3 GB" },
                    { new Guid("a0b1881e-506f-464d-b038-436dc880f0cc"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Từ 7 - 10 triệu" },
                    { new Guid("a1f15e84-adb1-4052-afcd-b32f926cfe13"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "Full HD+" },
                    { new Guid("a2161226-ddff-45d5-9aae-2a9bfde4bc66"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Loa karaoke" },
                    { new Guid("a304ef96-8212-4ee0-a583-ff5a9c7f3ced"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Bluetooth" },
                    { new Guid("ad6e5bdc-5f39-4677-a089-90aefe0b574d"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Core 5" },
                    { new Guid("addd15ad-d9b9-40fb-b096-c7e92eb922df"), new Guid("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), "SSD 2 TB" },
                    { new Guid("b4e08f8b-137e-442d-a028-5d28d923ad43"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Từ 2 - 4 triệu" },
                    { new Guid("b67c51af-d012-47de-adef-a0f3c2a1efea"), new Guid("6228626e-b8c6-4edf-8b42-9ae0e1526771"), "12 GB" },
                    { new Guid("b6b8dece-ad89-4d3a-b002-141393d81e1a"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "14 inch" },
                    { new Guid("b8308b69-b24a-4a9f-9c61-2c0bb661983d"), new Guid("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), "Retina" },
                    { new Guid("ba47ebb4-65a0-4441-9a89-688186841cd6"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Loa bluetooth" },
                    { new Guid("bb11a6ed-7d34-49d6-b27a-207c8d26ce5f"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "16 GB" },
                    { new Guid("bc7f1428-1071-4121-95fa-c1ef84e690b9"), new Guid("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), "Sạc không dây" },
                    { new Guid("bf71797f-89e0-4b16-9b57-b3a5335a1c07"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "1.5K" },
                    { new Guid("c45295de-1fe6-4427-9712-b6ea7b949adb"), new Guid("3df19307-ee88-4e19-8e6f-820d432caa94"), "64 GB" },
                    { new Guid("c50f06dd-0491-4e34-ab74-938f80d9061d"), new Guid("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), "Type-C" },
                    { new Guid("c71cf45b-90ef-42ad-8695-a74ce9d5e1da"), new Guid("d4925542-1eea-4e2b-b237-ac0249ad9044"), "240 Hz" },
                    { new Guid("cad7e715-e630-4692-81b8-5aa20218acbd"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "True Wireless" },
                    { new Guid("cb6a450e-5bca-4984-aefc-dcc7d64435fd"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Có dây" },
                    { new Guid("cb801046-31b0-4ac3-8450-65f55daeb61b"), new Guid("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), "Lightning" },
                    { new Guid("cdadb24a-a954-493e-965d-e19d011fde8d"), new Guid("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), "Dưới 4 tiếng" },
                    { new Guid("cdd998fc-5087-49d9-9376-9c2ce05ed669"), new Guid("021a23f0-fd04-4bb6-89e8-991c2e879a85"), "1000W trở lên" },
                    { new Guid("ce434348-1d46-460a-90d2-69fb6ae1cd8c"), new Guid("8d20ec78-aecf-4fb3-8df1-d976f86558fe"), "iOS" },
                    { new Guid("ceb39c71-d6c1-4c01-9c44-33821766c286"), new Guid("d4925542-1eea-4e2b-b237-ac0249ad9044"), "165 Hz" },
                    { new Guid("d1a68594-a93f-4a0f-be24-6458c059a964"), new Guid("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), "Dưới 2 triệu" },
                    { new Guid("d6cb799e-477d-4baf-9b1a-8c434a9bbeb2"), new Guid("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), "AMD Ryzen AI 9 300 series" },
                    { new Guid("d99d4d1e-dd49-4e4a-a509-875a7bf935fd"), new Guid("bea42814-f615-48e0-9937-ca1916266ff1"), "Bảo mật khuôn mặt 3D" },
                    { new Guid("d9c2295b-367a-4a65-b2b8-640dacd81668"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Core i7" },
                    { new Guid("da084a90-47ad-49c8-bf2c-018774fd0730"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Gaming" },
                    { new Guid("da698064-fcae-4232-9942-30926d49fb85"), new Guid("903f0975-b33f-4e16-9243-94f2530d26dd"), "Lightning" },
                    { new Guid("db53867b-5e54-4eda-a43a-85ebf4730ba8"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "2K+" },
                    { new Guid("dbbcb1f9-fbcd-4261-afcb-3e9bdffd570d"), new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), "Dưới 10 triệu" },
                    { new Guid("df63181e-76e4-4e2b-b285-8590b57cf174"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Dưới 200.000đ" },
                    { new Guid("df7b8b9e-af36-4bb7-8f83-d72155d97f14"), new Guid("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), "Từ 1 - 2 triệu" },
                    { new Guid("e1b1a228-b28a-46f7-8bb7-6bd7b623bc3b"), new Guid("2b3cdf91-68aa-491a-8541-8154990f30cc"), "Apple M2" },
                    { new Guid("e9db14f6-9105-4d17-99d1-4e8a0cfbb1f4"), new Guid("3df19307-ee88-4e19-8e6f-820d432caa94"), "256 GB" },
                    { new Guid("eab4caa4-fb76-439b-9b62-d71f837e3f95"), new Guid("aa34ffb4-077a-480f-b681-970f6144cef4"), "Core i3" },
                    { new Guid("ebae64b2-6561-4089-8b7d-e2fdb35baa54"), new Guid("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), "8 tiếng trở lên" },
                    { new Guid("ec2aada0-d82a-4e9d-9e8e-fe8f067690d2"), new Guid("1d0feb96-608d-46ca-a937-4cd428adbf42"), "24 GB" },
                    { new Guid("ed1cfca8-27e8-46d7-8d6a-e9fb685bee7c"), new Guid("b2a18dba-2e74-4918-a263-b2555fb8faeb"), "Sạc nhanh (từ 20W)" },
                    { new Guid("f590e5f9-4d63-4915-85cf-23a52195c218"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "15.6 inch" },
                    { new Guid("f6d9e0c1-3ee3-4c51-b483-980e2293dd83"), new Guid("491c0d5b-6101-4780-967c-854bd24f5024"), "Từ 25 - 30 triệu" },
                    { new Guid("f8f91ff1-d1ca-4da6-b90c-d2d653e6f5c3"), new Guid("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), "HD+" },
                    { new Guid("f9b34928-45a7-415e-8963-958e62f1b448"), new Guid("24e56e95-f6ad-49f3-96be-7ee5c1556980"), "Loa vi tính" },
                    { new Guid("ff43f32e-ee7f-4d22-9511-f9a036b9b8f5"), new Guid("c12b48ec-306d-4797-bdef-92503483fc18"), "60 Hz" },
                    { new Guid("ffd4babe-fa66-4303-b9f0-426480fb1b98"), new Guid("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), "13.4 inch" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BannerRequests_BannerRequestPriceId",
                table: "BannerRequests",
                column: "BannerRequestPriceId");

            migrationBuilder.CreateIndex(
                name: "IX_BannerRequests_SellerId",
                table: "BannerRequests",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Banners_BannerRequestId",
                table: "Banners",
                column: "BannerRequestId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_BillingMailApplications_SellerApplicationId",
                table: "BillingMailApplications",
                column: "SellerApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_BillingMails_SellerId",
                table: "BillingMails",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_CategoryBrands_CategoryId",
                table: "CategoryBrands",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_UserId",
                table: "Customers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSubscriptionTrackers_CustomerId",
                table: "CustomerSubscriptionTrackers",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerSubscriptionTrackers_CustomerSubscriptionId",
                table: "CustomerSubscriptionTrackers",
                column: "CustomerSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_FavoriteGadgets_GadgetId",
                table: "FavoriteGadgets",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetDescriptions_GadgetId",
                table: "GadgetDescriptions",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetFilterOptions_GadgetFilterId",
                table: "GadgetFilterOptions",
                column: "GadgetFilterId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetFilters_CategoryId",
                table: "GadgetFilters",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetHistories_CustomerId",
                table: "GadgetHistories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetHistories_GadgetId",
                table: "GadgetHistories",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_GadgetImages_GadgetId",
                table: "GadgetImages",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Gadgets_BrandId",
                table: "Gadgets",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Gadgets_CategoryId",
                table: "Gadgets",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Gadgets_SellerId",
                table: "Gadgets",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_Gadgets_ShopId",
                table: "Gadgets",
                column: "ShopId");

            migrationBuilder.CreateIndex(
                name: "IX_KeywordHistories_CustomerId",
                table: "KeywordHistories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_UserId",
                table: "Managers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchGadgetResponses_GadgetId",
                table: "SearchGadgetResponses",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchGadgetResponses_SearchHistoryResponseId",
                table: "SearchGadgetResponses",
                column: "SearchHistoryResponseId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistories_CustomerId",
                table: "SearchHistories",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchHistoryResponses_SearchHistoryId",
                table: "SearchHistoryResponses",
                column: "SearchHistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerApplications_UserId",
                table: "SellerApplications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserId",
                table: "Sellers",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SellerSubscriptionTrackers_SellerId",
                table: "SellerSubscriptionTrackers",
                column: "SellerId");

            migrationBuilder.CreateIndex(
                name: "IX_SellerSubscriptionTrackers_SellerSubscriptionId",
                table: "SellerSubscriptionTrackers",
                column: "SellerSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationKeys_GadgetId",
                table: "SpecificationKeys",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationKeys_SpecificationId",
                table: "SpecificationKeys",
                column: "SpecificationId");

            migrationBuilder.CreateIndex(
                name: "IX_Specifications_GadgetId",
                table: "Specifications",
                column: "GadgetId");

            migrationBuilder.CreateIndex(
                name: "IX_SpecificationValues_SpecificationKeyId",
                table: "SpecificationValues",
                column: "SpecificationKeyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserVerifies_UserId",
                table: "UserVerifies",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "BannerConfigurations");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "BillingMailApplications");

            migrationBuilder.DropTable(
                name: "BillingMails");

            migrationBuilder.DropTable(
                name: "CategoryBrands");

            migrationBuilder.DropTable(
                name: "CustomerSubscriptionTrackers");

            migrationBuilder.DropTable(
                name: "FavoriteGadgets");

            migrationBuilder.DropTable(
                name: "GadgetDescriptions");

            migrationBuilder.DropTable(
                name: "GadgetFilterOptions");

            migrationBuilder.DropTable(
                name: "GadgetHistories");

            migrationBuilder.DropTable(
                name: "GadgetImages");

            migrationBuilder.DropTable(
                name: "KeywordHistories");

            migrationBuilder.DropTable(
                name: "Managers");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "SearchGadgetResponses");

            migrationBuilder.DropTable(
                name: "SellerSubscriptionTrackers");

            migrationBuilder.DropTable(
                name: "SpecificationValues");

            migrationBuilder.DropTable(
                name: "UserVerifies");

            migrationBuilder.DropTable(
                name: "BannerRequests");

            migrationBuilder.DropTable(
                name: "SellerApplications");

            migrationBuilder.DropTable(
                name: "CustomerSubscriptions");

            migrationBuilder.DropTable(
                name: "GadgetFilters");

            migrationBuilder.DropTable(
                name: "SearchHistoryResponses");

            migrationBuilder.DropTable(
                name: "SellerSubscriptions");

            migrationBuilder.DropTable(
                name: "SpecificationKeys");

            migrationBuilder.DropTable(
                name: "BannerRequestPrices");

            migrationBuilder.DropTable(
                name: "SearchHistories");

            migrationBuilder.DropTable(
                name: "Specifications");

            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "Gadgets");

            migrationBuilder.DropTable(
                name: "Brands");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Sellers");

            migrationBuilder.DropTable(
                name: "Shops");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
