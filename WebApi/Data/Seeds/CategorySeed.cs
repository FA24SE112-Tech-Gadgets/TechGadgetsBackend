using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class CategorySeed
{
    public readonly static List<Category> Default =
    [
        new Category { Id = Guid.Parse("e17cfaae-4f34-43bc-85da-056f4cba13e5"), Name = "Điện thoại" },
        new Category { Id = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Laptop" },
        new Category { Id = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Tai nghe" },
        new Category { Id = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Loa" },
    ];
}
