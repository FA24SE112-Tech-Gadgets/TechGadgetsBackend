using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.Data.Entities;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<Banner> Banners { get; set; }
    public DbSet<BannerConfiguration> BannerConfigurations { get; set; }
    public DbSet<BannerRequest> BannerRequests { get; set; }
    public DbSet<BannerRequestPrice> BannerRequestPrices { get; set; }
    public DbSet<BillingMail> BillingMails { get; set; }
    public DbSet<BillingMailApplication> BillingMailApplications { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryBrand> CategoryBrands { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerSubscription> CustomerSubscriptions { get; set; }
    public DbSet<CustomerSubscriptionTracker> CustomerSubscriptionTrackers { get; set; }
    public DbSet<FavoriteGadget> FavoriteGadgets { get; set; }
    public DbSet<Gadget> Gadgets { get; set; }
    public DbSet<GadgetDescription> GadgetDescriptions { get; set; }
    public DbSet<GadgetFilter> GadgetFilters { get; set; }
    public DbSet<GadgetFilterOption> GadgetFilterOptions { get; set; }
    public DbSet<GadgetHistory> GadgetHistories { get; set; }
    public DbSet<GadgetImage> GadgetImages { get; set; }
    public DbSet<KeywordHistory> KeywordHistories { get; set; }
    public DbSet<Manager> Managers { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<SearchGadgetResponse> SearchGadgetResponses { get; set; }
    public DbSet<SearchHistory> SearchHistories { get; set; }
    public DbSet<SearchHistoryResponse> SearchHistoryResponses { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<SellerApplication> SellerApplications { get; set; }
    public DbSet<SellerSubscription> SellerSubscriptions { get; set; }
    public DbSet<SellerSubscriptionTracker> SellerSubscriptionTrackers { get; set; }
    public DbSet<Shop> Shops { get; set; }
    public DbSet<Specification> Specifications { get; set; }
    public DbSet<SpecificationKey> SpecificationKeys { get; set; }
    public DbSet<SpecificationValue> SpecificationValues { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserVerify> UserVerifies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresExtension("vector");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<Enum>().HaveConversion<string>();
    }

}
