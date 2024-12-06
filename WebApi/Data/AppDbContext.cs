using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.Data.Entities;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; } = default!;
    public DbSet<BillingMail> BillingMails { get; set; } = default!;
    public DbSet<BillingMailApplication> BillingMailApplications { get; set; } = default!;
    public DbSet<Brand> Brands { get; set; } = default!;
    public DbSet<Cart> Carts { get; set; } = default!;
    public DbSet<CartGadget> CartGadgets { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<CategoryBrand> CategoryBrands { get; set; } = default!;
    public DbSet<Criteria> Criteria { get; set; } = default!;
    public DbSet<CriteriaCategory> CriteriaCategories { get; set; } = default!;
    public DbSet<Customer> Customers { get; set; } = default!;
    public DbSet<CustomerInformation> CustomerInformation { get; set; } = default!;
    public DbSet<Device> Devices { get; set; } = default!;
    public DbSet<FavoriteGadget> FavoriteGadgets { get; set; } = default!;
    public DbSet<Gadget> Gadgets { get; set; } = default!;
    public DbSet<GadgetDescription> GadgetDescriptions { get; set; } = default!;
    public DbSet<GadgetDiscount> GadgetDiscounts { get; set; } = default!;
    public DbSet<GadgetFilter> GadgetFilters { get; set; } = default!;
    public DbSet<GadgetHistory> GadgetHistories { get; set; } = default!;
    public DbSet<GadgetImage> GadgetImages { get; set; } = default!;
    public DbSet<KeywordHistory> KeywordHistories { get; set; } = default!;
    public DbSet<Manager> Managers { get; set; } = default!;
    public DbSet<NaturalLanguageKeyword> NaturalLanguageKeywords { get; set; } = default!;
    public DbSet<NaturalLanguageKeywordGroup> NaturalLanguageKeywordGroups { get; set; } = default!;
    public DbSet<NaturalLanguagePrompt> NaturalLanguagePrompts { get; set; } = default!;
    public DbSet<Notification> Notifications { get; set; } = default!;
    public DbSet<Order> Orders { get; set; } = default!;
    public DbSet<Review> Reviews { get; set; } = default!;
    public DbSet<Seller> Sellers { get; set; } = default!;
    public DbSet<SellerApplication> SellerApplications { get; set; } = default!;
    public DbSet<SellerInformation> SellerInformation { get; set; } = default!;
    public DbSet<SellerOrder> SellerOrders { get; set; } = default!;
    public DbSet<SellerOrderItem> SellerOrderItems { get; set; } = default!;
    public DbSet<SellerReply> SellerReplies { get; set; } = default!;
    public DbSet<SpecificationKey> SpecificationKeys { get; set; } = default!;
    public DbSet<SpecificationUnit> SpecificationUnits { get; set; } = default!;
    public DbSet<SpecificationValue> SpecificationValues { get; set; } = default!;
    public DbSet<SystemSellerOrderTracking> SystemSellerOrderTrackings { get; set; } = default!;
    public DbSet<SystemWallet> SystemWallets { get; set; } = default!;
    public DbSet<User> Users { get; set; } = default!;
    public DbSet<UserVerify> UserVerifies { get; set; } = default!;
    public DbSet<Wallet> Wallets { get; set; } = default!;
    public DbSet<WalletTracking> WalletTrackings { get; set; } = default!;

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
