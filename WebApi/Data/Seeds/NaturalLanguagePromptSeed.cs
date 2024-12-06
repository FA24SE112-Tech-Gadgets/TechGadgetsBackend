using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class NaturalLanguagePromptSeed
{
    public static readonly DateTime now = DateTime.UtcNow;

    public readonly static List<NaturalLanguagePrompt> Default =
    [
        new NaturalLanguagePrompt {
            Id = Guid.Parse("ca69eeb5-3ac5-422c-be79-1dd7376ee8a2"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Điện thoại màu xanh pin trâu giá sinh viên ra mắt cuối năm ngoái"
        },
        new NaturalLanguagePrompt {
            Id = Guid.Parse("6d52e831-f9ef-4f49-805f-8b9325a12206"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Tai nghe Bluetooth kháng nước mức giá tầm trung đang giảm giá"
        },
        new NaturalLanguagePrompt {
            Id = Guid.Parse("5a12289a-9e86-48f0-bb6e-a4f0069a0424"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Laptop MSI cao cấp ram 32GB, 1TB giá trên 35 triệu"
        },
        new NaturalLanguagePrompt {
            Id = Guid.Parse("34317d64-6bc5-4f95-b63e-880199de2ddb"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Tai nghe có dây xuất xứ ở Việt Nam giá dưới 1 triệu"
        },
        new NaturalLanguagePrompt {
            Id = Guid.Parse("60dbdbc7-cced-4a7a-a38c-95ec6d54c3e1"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Điện thoại bán chạy, có đánh giá tích cực và ra mắt năm nay"
        },
        new NaturalLanguagePrompt {
            Id = Guid.Parse("1a2abee0-91a6-4f1c-9a80-f95e43fc8e86"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Cửa hàng bán loa bluetooth giá rẻ ở hải phòng"
        },
        new NaturalLanguagePrompt {
            Id = Guid.Parse("208eb4eb-5ca8-428c-81de-c0557a9c5b0b"),
            CreatedAt = now,
            UpdatedAt = now,
            Prompt = "Cửa hàng điện thoại bán chạy đang giảm giá"
        },
    ];
}
