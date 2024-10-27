using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class CustomerInformationSeed
{
    public readonly static List<CustomerInformation> Default =
    [
        new CustomerInformation {
            Id = Guid.Parse("cdd7c66c-49d9-4bcc-8668-7a7aa231f8ec"),
            CustomerId = Guid.Parse("d61a23a0-dbfe-429f-9c1f-5e0b955beff9"),
            FullName = "Nguỵ Chi Mai",
            Address = "561A Điện Biên Phủ, Quận Bình Thạnh, TP.HCM",
            PhoneNumber = "0967310804",
            CreatedAt = DateTime.UtcNow,
        },
        new CustomerInformation {
            Id = Guid.Parse("d59307b8-aab8-4129-8033-2ac22f2d5e21"),
            CustomerId = Guid.Parse("f9ff20a7-05b3-4dbc-b260-26ef16db8513"),
            FullName = "Lê Thuý Hiền",
            Address = "88 Song Hành, P. An Phú , Quận 2 , TP. HCM",
            PhoneNumber = "0907110400",
            CreatedAt = DateTime.UtcNow,
        },
    ];
}
