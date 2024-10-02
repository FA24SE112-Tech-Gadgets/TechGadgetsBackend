using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class GadgetFilterConfiguration : IEntityTypeConfiguration<GadgetFilter>
{
    public void Configure(EntityTypeBuilder<GadgetFilter> builder)
    {
        builder.HasData(
            new GadgetFilter { Id = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Giá" },
            new GadgetFilter { Id = Guid.Parse("6228626e-b8c6-4edf-8b42-9ae0e1526771"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "RAM" },
            new GadgetFilter { Id = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Độ phân giải" },
            new GadgetFilter { Id = Guid.Parse("c12b48ec-306d-4797-bdef-92503483fc18"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Tần sồ quét" },
            new GadgetFilter { Id = Guid.Parse("bea42814-f615-48e0-9937-ca1916266ff1"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Tính năng đặc biệt" },
            new GadgetFilter { Id = Guid.Parse("3df19307-ee88-4e19-8e6f-820d432caa94"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Dung lượng lưu trữ" },
            new GadgetFilter { Id = Guid.Parse("b2a18dba-2e74-4918-a263-b2555fb8faeb"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Tính năng sạc" },
            new GadgetFilter { Id = Guid.Parse("8d20ec78-aecf-4fb3-8df1-d976f86558fe"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Hệ điều hành" },
            new GadgetFilter { Id = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Giá" },
            new GadgetFilter { Id = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Kích cỡ màn hình" },
            new GadgetFilter { Id = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "RAM" },
            new GadgetFilter { Id = Guid.Parse("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Độ phân giải" },
            new GadgetFilter { Id = Guid.Parse("d4925542-1eea-4e2b-b237-ac0249ad9044"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Tần sồ quét" },
            new GadgetFilter { Id = Guid.Parse("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Ổ cứng" },
            new GadgetFilter { Id = Guid.Parse("af177595-3999-4055-8021-23fe477ac074"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Intel Core Ultra (mới nhất)" },
            new GadgetFilter { Id = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Incel Core i/Core/Celeron/Pentium" },
            new GadgetFilter { Id = Guid.Parse("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "AMD" },
            new GadgetFilter { Id = Guid.Parse("2b3cdf91-68aa-491a-8541-8154990f30cc"), CategoryId = Guid.Parse("47827ce7-8c57-4ee8-9d3b-712396e2a4a0"), Name = "Apple" },
            new GadgetFilter { Id = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Loại" },
            new GadgetFilter { Id = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Giá" },
            new GadgetFilter { Id = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Công suất" },
            new GadgetFilter { Id = Guid.Parse("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Thời lượng pin" },
            new GadgetFilter { Id = Guid.Parse("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Cổng sạc" },
            new GadgetFilter { Id = Guid.Parse("903f0975-b33f-4e16-9243-94f2530d26dd"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Jack cắm" },
            new GadgetFilter { Id = Guid.Parse("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), CategoryId = Guid.Parse("f2e254ee-d0e7-47b2-99d8-779f3d29b7d6"), Name = "Tiện ích" }
        );
    }
}
