using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SpecificationUnitSeed
{
    public readonly static List<SpecificationUnit> Default =
    [
        new SpecificationUnit { Id = Guid.Parse("2f93b0b3-cf99-4362-a82a-8c1c4aadbbd0"), SpecificationKeyId = Guid.Parse("7e557e25-dc89-4566-957d-33b19e001d3c"), Name = "mAh" },
        new SpecificationUnit { Id = Guid.Parse("ca715bd2-f83f-4a20-aa90-fa6947b8b415"), SpecificationKeyId = Guid.Parse("838dcb1c-1508-45b6-8248-c9d2a4f69acf"), Name = "GB" },
        new SpecificationUnit { Id = Guid.Parse("94924519-f69e-40b6-a9d3-b8fcebc3ceb6"), SpecificationKeyId = Guid.Parse("b8662e16-a30a-4ae4-835d-95e2c9d96673"), Name = "GB" },
        new SpecificationUnit { Id = Guid.Parse("8e4f7bba-c230-4d5a-8c29-af2f2f1bf97e"), SpecificationKeyId = Guid.Parse("89afd25f-610b-4073-a0de-75c1a3b3df98"), Name = "GB" },
        new SpecificationUnit { Id = Guid.Parse("e9fb4ff8-2d73-451d-a214-ae1c2d85ec60"), SpecificationKeyId = Guid.Parse("52c31c39-1a89-4982-92ba-8d12557c05f6"), Name = "Inch" },
        new SpecificationUnit { Id = Guid.Parse("63026603-12e5-41dd-b5fb-2ccb11e97959"), SpecificationKeyId = Guid.Parse("66a99ffa-34b8-4043-a98f-1dbdfd5a65dd"), Name = "Hz" },
        new SpecificationUnit { Id = Guid.Parse("8d9d79fd-66e5-4164-9932-23e565255d5e"), SpecificationKeyId = Guid.Parse("0daefd1c-9f60-404d-a296-8beca95d6be1"), Name = "GB" },
        new SpecificationUnit { Id = Guid.Parse("60a87591-6ed6-4088-b0ed-8eefab7844e1"), SpecificationKeyId = Guid.Parse("0daefd1c-9f60-404d-a296-8beca95d6be1"), Name = "TB" },
    ];
}
