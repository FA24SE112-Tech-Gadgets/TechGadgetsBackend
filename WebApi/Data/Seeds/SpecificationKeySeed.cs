using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SpecificationKeySeed
{
    public readonly static List<SpecificationKey> Default =
    [
        new SpecificationKey { Id = Guid.Parse("943d0fe1-e24b-4c35-a6cd-b20489f8dcaf"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Công nghệ màn hình" },
        new SpecificationKey { Id = Guid.Parse("fd72dc15-4453-4006-b979-9b9888a16445"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Mặt kính cảm ứng" },
        new SpecificationKey { Id = Guid.Parse("c3557df7-96ce-4ea8-8ee5-30a69b4c036f"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Hệ điều hành" },
        new SpecificationKey { Id = Guid.Parse("f581dbd7-6bc8-4fba-b8d4-deb026c1db16"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Chip xử lí (CPU)" },
        new SpecificationKey { Id = Guid.Parse("29d662da-39ef-445b-bcb2-499c7fe4922e"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Chip đồ hoạ (GPU)" },
        new SpecificationKey { Id = Guid.Parse("7e557e25-dc89-4566-957d-33b19e001d3c"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Dung lượng pin" },
        new SpecificationKey { Id = Guid.Parse("838dcb1c-1508-45b6-8248-c9d2a4f69acf"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "RAM" },
        new SpecificationKey { Id = Guid.Parse("b8662e16-a30a-4ae4-835d-95e2c9d96673"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Dung lượng lưu trữ" },
        new SpecificationKey { Id = Guid.Parse("89afd25f-610b-4073-a0de-75c1a3b3df98"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "RAM" },
        new SpecificationKey { Id = Guid.Parse("52c31c39-1a89-4982-92ba-8d12557c05f6"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Màn hình" },
        new SpecificationKey { Id = Guid.Parse("66a99ffa-34b8-4043-a98f-1dbdfd5a65dd"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Tần số quét" },
        new SpecificationKey { Id = Guid.Parse("0daefd1c-9f60-404d-a296-8beca95d6be1"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Ổ cứng" },
    ];
}
