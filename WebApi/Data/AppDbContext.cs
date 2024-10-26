using Microsoft.EntityFrameworkCore;
using System.Reflection;
using WebApi.Data.Entities;

namespace WebApi.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Admin> Admins { get; set; }
    public DbSet<BillingMail> BillingMails { get; set; }
    public DbSet<BillingMailApplication> BillingMailApplications { get; set; }
    public DbSet<Brand> Brands { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartGadget> CartGadgets { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryBrand> CategoryBrands { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<CustomerInformation> CustomerInformation { get; set; }
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
    public DbSet<Order> Orders { get; set; }
    public DbSet<Review> Reviews { get; set; }
    public DbSet<SearchAI> SearchAIs { get; set; }
    public DbSet<SearchAIVector> SearchAIVectors { get; set; }
    public DbSet<Seller> Sellers { get; set; }
    public DbSet<SellerApplication> SellerApplications { get; set; }
    public DbSet<SellerInformation> SellerInformation { get; set; }
    public DbSet<SellerOrder> SellerOrders { get; set; }
    public DbSet<SellerOrderItem> SellerOrderItems { get; set; }
    public DbSet<SellerReply> SellerReplies { get; set; }
    public DbSet<SpecificationKey> SpecificationKeys { get; set; }
    public DbSet<SpecificationUnit> SpecificationUnits { get; set; }
    public DbSet<SpecificationValue> SpecificationValues { get; set; }
    public DbSet<SystemOrderDetailTracking> SystemOrderDetailTrackings { get; set; }
    public DbSet<SystemWallet> SystemWallets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserVerify> UserVerifies { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTracking> WalletTrackings { get; set; }

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
