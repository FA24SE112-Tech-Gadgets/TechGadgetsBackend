using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
    public void Configure(EntityTypeBuilder<Customer> builder)
    {
        builder.HasData(
            new Customer
            {
                Id = Guid.Parse("d61a23a0-dbfe-429f-9c1f-5e0b955beff9"),
                UserId = Guid.Parse("5a57223a-6e7d-401b-a19e-bf9282db69fe"),
                FullName = "Nguỵ Chi Mai"
            },
            new Customer
            {
                Id = Guid.Parse("f9ff20a7-05b3-4dbc-b260-26ef16db8513"),
                UserId = Guid.Parse("8d8707e4-299d-450b-bc5c-f8ab49504fce"),
                FullName = "Lê Thuý Hiền"
            }
        );
    }
}
