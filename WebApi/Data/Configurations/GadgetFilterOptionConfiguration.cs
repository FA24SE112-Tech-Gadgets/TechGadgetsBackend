﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class GadgetFilterOptionConfiguration : IEntityTypeConfiguration<GadgetFilterOption>
{
    public void Configure(EntityTypeBuilder<GadgetFilterOption> builder)
    {
        builder.HasData(
            new GadgetFilterOption { Id = Guid.Parse("d1a68594-a93f-4a0f-be24-6458c059a964"), GadgetFilterId = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), Value = "Dưới 2 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("8a395ad3-9138-4d8a-a398-a3c0f0c96f20"), GadgetFilterId = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), Value = "Từ 2 - 4 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("7d1b98bd-bacc-4159-bb88-196ee3ebf3eb"), GadgetFilterId = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), Value = "Từ 4 - 7 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("210c0319-0fc1-4d44-af52-deda8ce8018f"), GadgetFilterId = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), Value = "Từ 7 - 13 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("7b492fc0-59f3-4624-b637-bca6acbbb02c"), GadgetFilterId = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), Value = "Từ 13 - 20 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("945848f1-6c6c-4412-83b0-288f94801350"), GadgetFilterId = Guid.Parse("79b9494c-b5c1-419d-8f5d-4ae3ed7b5cd0"), Value = "Trên 20 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("a048df56-f9ee-4c9f-bdaa-79fafa9f2a1f"), GadgetFilterId = Guid.Parse("6228626e-b8c6-4edf-8b42-9ae0e1526771"), Value = "3 GB" },
            new GadgetFilterOption { Id = Guid.Parse("766d2310-25c7-4f4f-88d6-8046c96a8c3c"), GadgetFilterId = Guid.Parse("6228626e-b8c6-4edf-8b42-9ae0e1526771"), Value = "4 GB" },
            new GadgetFilterOption { Id = Guid.Parse("566df58d-3684-4159-95d9-5c5b5097bf7d"), GadgetFilterId = Guid.Parse("6228626e-b8c6-4edf-8b42-9ae0e1526771"), Value = "6 GB" },
            new GadgetFilterOption { Id = Guid.Parse("320cd81f-81cd-4462-bdf1-679117c35bca"), GadgetFilterId = Guid.Parse("6228626e-b8c6-4edf-8b42-9ae0e1526771"), Value = "8 GB" },
            new GadgetFilterOption { Id = Guid.Parse("b67c51af-d012-47de-adef-a0f3c2a1efea"), GadgetFilterId = Guid.Parse("6228626e-b8c6-4edf-8b42-9ae0e1526771"), Value = "12 GB" },
            new GadgetFilterOption { Id = Guid.Parse("27c9f3a9-137b-4b45-96c1-d238726da6a9"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "QQVGA" },
            new GadgetFilterOption { Id = Guid.Parse("4e69bb48-5dc8-4bad-a7bc-fcbdec870bfb"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "QVGA" },
            new GadgetFilterOption { Id = Guid.Parse("f8f91ff1-d1ca-4da6-b90c-d2d653e6f5c3"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "HD+" },
            new GadgetFilterOption { Id = Guid.Parse("a1f15e84-adb1-4052-afcd-b32f926cfe13"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "Full HD+" },
            new GadgetFilterOption { Id = Guid.Parse("bf71797f-89e0-4b16-9b57-b3a5335a1c07"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "1.5K" },
            new GadgetFilterOption { Id = Guid.Parse("32348105-592c-45b9-8fb0-43b0934f34b6"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "1.5K+" },
            new GadgetFilterOption { Id = Guid.Parse("db53867b-5e54-4eda-a43a-85ebf4730ba8"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "2K+" },
            new GadgetFilterOption { Id = Guid.Parse("35c8d5b9-5d0b-4066-84ab-0af695248382"), GadgetFilterId = Guid.Parse("fc5480ca-1c61-4eb4-a858-dc15a51b2992"), Value = "Retina (iPhone)" },
            new GadgetFilterOption { Id = Guid.Parse("ff43f32e-ee7f-4d22-9511-f9a036b9b8f5"), GadgetFilterId = Guid.Parse("c12b48ec-306d-4797-bdef-92503483fc18"), Value = "60 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("2e65afad-61c1-4edb-8d74-ea66d3527ea0"), GadgetFilterId = Guid.Parse("c12b48ec-306d-4797-bdef-92503483fc18"), Value = "144 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("7f2df584-d8a6-49a1-b4c6-0ed25511b198"), GadgetFilterId = Guid.Parse("c12b48ec-306d-4797-bdef-92503483fc18"), Value = "90 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("9b9c427d-7551-4132-9333-b4d9e884d842"), GadgetFilterId = Guid.Parse("c12b48ec-306d-4797-bdef-92503483fc18"), Value = "120 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("6a1dc6f4-dae1-4524-b63d-a50adcb24ced"), GadgetFilterId = Guid.Parse("bea42814-f615-48e0-9937-ca1916266ff1"), Value = "Kháng nước, bụi" },
            new GadgetFilterOption { Id = Guid.Parse("4645b0bd-9c72-4c50-83a9-c05378dca2ba"), GadgetFilterId = Guid.Parse("bea42814-f615-48e0-9937-ca1916266ff1"), Value = "Hỗ trợ 5G" },
            new GadgetFilterOption { Id = Guid.Parse("d99d4d1e-dd49-4e4a-a509-875a7bf935fd"), GadgetFilterId = Guid.Parse("bea42814-f615-48e0-9937-ca1916266ff1"), Value = "Bảo mật khuôn mặt 3D" },
            new GadgetFilterOption { Id = Guid.Parse("08b32383-ad68-4666-be35-00feb1bec3d7"), GadgetFilterId = Guid.Parse("bea42814-f615-48e0-9937-ca1916266ff1"), Value = "Công nghệ NFC" },
            new GadgetFilterOption { Id = Guid.Parse("c45295de-1fe6-4427-9712-b6ea7b949adb"), GadgetFilterId = Guid.Parse("3df19307-ee88-4e19-8e6f-820d432caa94"), Value = "64 GB" },
            new GadgetFilterOption { Id = Guid.Parse("5c1b14aa-50f7-4914-b42c-cb620251f619"), GadgetFilterId = Guid.Parse("3df19307-ee88-4e19-8e6f-820d432caa94"), Value = "128 GB" },
            new GadgetFilterOption { Id = Guid.Parse("e9db14f6-9105-4d17-99d1-4e8a0cfbb1f4"), GadgetFilterId = Guid.Parse("3df19307-ee88-4e19-8e6f-820d432caa94"), Value = "256 GB" },
            new GadgetFilterOption { Id = Guid.Parse("14588df7-ff2d-4e67-a71f-e484fab0a376"), GadgetFilterId = Guid.Parse("3df19307-ee88-4e19-8e6f-820d432caa94"), Value = "512 GB" },
            new GadgetFilterOption { Id = Guid.Parse("2e3d901f-13b6-47e4-8815-cedad93393a6"), GadgetFilterId = Guid.Parse("3df19307-ee88-4e19-8e6f-820d432caa94"), Value = "1TB" },
            new GadgetFilterOption { Id = Guid.Parse("ed1cfca8-27e8-46d7-8d6a-e9fb685bee7c"), GadgetFilterId = Guid.Parse("b2a18dba-2e74-4918-a263-b2555fb8faeb"), Value = "Sạc nhanh (từ 20W)" },
            new GadgetFilterOption { Id = Guid.Parse("a0212238-7541-45b6-8aac-e1b68cb3f824"), GadgetFilterId = Guid.Parse("b2a18dba-2e74-4918-a263-b2555fb8faeb"), Value = "Sạc siêu nhanh (từ 60W)" },
            new GadgetFilterOption { Id = Guid.Parse("08c506a2-d27a-4f23-834d-f53864001062"), GadgetFilterId = Guid.Parse("b2a18dba-2e74-4918-a263-b2555fb8faeb"), Value = "Sạc không dây" },
            new GadgetFilterOption { Id = Guid.Parse("ce434348-1d46-460a-90d2-69fb6ae1cd8c"), GadgetFilterId = Guid.Parse("8d20ec78-aecf-4fb3-8df1-d976f86558fe"), Value = "iOS" },
            new GadgetFilterOption { Id = Guid.Parse("470598e2-1d97-4465-a70a-83a7b5f29165"), GadgetFilterId = Guid.Parse("8d20ec78-aecf-4fb3-8df1-d976f86558fe"), Value = "Android" },
            new GadgetFilterOption { Id = Guid.Parse("dbbcb1f9-fbcd-4261-afcb-3e9bdffd570d"), GadgetFilterId = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), Value = "Dưới 10 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("71b2beba-a326-42a5-9391-30ccb3aab053"), GadgetFilterId = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), Value = "Từ 10 - 15 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("3cd1bdd1-f7e8-46af-a33d-838cac68ba7f"), GadgetFilterId = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), Value = "Từ 15 - 20 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("772e3e58-b3de-4487-98cd-644059d01736"), GadgetFilterId = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), Value = "Từ 20 - 25 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("f6d9e0c1-3ee3-4c51-b483-980e2293dd83"), GadgetFilterId = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), Value = "Từ 25 - 30 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("9c548fd8-0ad6-4d54-b5c1-86c4819dd87b"), GadgetFilterId = Guid.Parse("491c0d5b-6101-4780-967c-854bd24f5024"), Value = "Trên 30 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("7c36a492-87da-486f-a11a-b665049670a5"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "11.6 inch" },
            new GadgetFilterOption { Id = Guid.Parse("72d245f3-4bee-4237-bb31-924fdcc83e78"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "13.3 inch" },
            new GadgetFilterOption { Id = Guid.Parse("ffd4babe-fa66-4303-b9f0-426480fb1b98"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "13.4 inch" },
            new GadgetFilterOption { Id = Guid.Parse("6be9b8a7-eef6-4bf4-b83f-c042ba837072"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "13.6 inch" },
            new GadgetFilterOption { Id = Guid.Parse("b6b8dece-ad89-4d3a-b002-141393d81e1a"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "14 inch" },
            new GadgetFilterOption { Id = Guid.Parse("29123eac-0e54-4ff7-b10b-ab5462b5ba4f"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "14.2 inch" },
            new GadgetFilterOption { Id = Guid.Parse("f590e5f9-4d63-4915-85cf-23a52195c218"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "15.6 inch" },
            new GadgetFilterOption { Id = Guid.Parse("6575d15b-29b2-4e0a-a362-b4408e064e71"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "16 inch" },
            new GadgetFilterOption { Id = Guid.Parse("3069802c-126c-4ff1-8944-4f4a25f397d7"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "16.1 inch" },
            new GadgetFilterOption { Id = Guid.Parse("0afcdda7-44c7-424e-ac0e-82b8ada87b53"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "16.2 inch" },
            new GadgetFilterOption { Id = Guid.Parse("202da262-bd50-446a-973d-83f71e038344"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "17 inch" },
            new GadgetFilterOption { Id = Guid.Parse("61cb4f58-9e58-4feb-b05d-b97e8b705560"), GadgetFilterId = Guid.Parse("4bb160a9-e4e4-49a0-9bc5-44b443d7070e"), Value = "18 inch" },
            new GadgetFilterOption { Id = Guid.Parse("106cd227-51e8-4978-b113-107aa9808af5"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "4 GB" },
            new GadgetFilterOption { Id = Guid.Parse("068e0212-560d-468a-a920-77adac41ce97"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "8 GB" },
            new GadgetFilterOption { Id = Guid.Parse("bb11a6ed-7d34-49d6-b27a-207c8d26ce5f"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "16 GB" },
            new GadgetFilterOption { Id = Guid.Parse("59629a6e-cd79-4a59-88e0-316c2db0082a"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "18 GB" },
            new GadgetFilterOption { Id = Guid.Parse("ec2aada0-d82a-4e9d-9e8e-fe8f067690d2"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "24 GB" },
            new GadgetFilterOption { Id = Guid.Parse("7b2270f4-7527-465a-86f0-f9809ec09add"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "32 GB" },
            new GadgetFilterOption { Id = Guid.Parse("10ffad03-d8de-4cdb-8717-6d195a01b2e8"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "36 GB" },
            new GadgetFilterOption { Id = Guid.Parse("4b2253e8-a6e3-4bcb-88f6-080465e806c9"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "48 GB" },
            new GadgetFilterOption { Id = Guid.Parse("5beea968-565d-4ef5-b0d9-eb1aaedaf308"), GadgetFilterId = Guid.Parse("1d0feb96-608d-46ca-a937-4cd428adbf42"), Value = "64 GB" },
            new GadgetFilterOption { Id = Guid.Parse("809d88e8-a293-44ec-b784-28772da2342c"), GadgetFilterId = Guid.Parse("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), Value = "4K" },
            new GadgetFilterOption { Id = Guid.Parse("69e11515-d9d1-43ff-b3df-0e7444424f2b"), GadgetFilterId = Guid.Parse("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), Value = "2K" },
            new GadgetFilterOption { Id = Guid.Parse("b8308b69-b24a-4a9f-9c61-2c0bb661983d"), GadgetFilterId = Guid.Parse("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), Value = "Retina" },
            new GadgetFilterOption { Id = Guid.Parse("37f2f496-2c1f-4b1c-aeb7-9d0f9cead239"), GadgetFilterId = Guid.Parse("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), Value = "Full HD" },
            new GadgetFilterOption { Id = Guid.Parse("368bb62e-b40e-4745-b87e-0fc9543736fb"), GadgetFilterId = Guid.Parse("a1d2dcbb-5be5-4fb8-a44e-95815bfdd18f"), Value = "HD" },
            new GadgetFilterOption { Id = Guid.Parse("82d5d8df-7a63-4485-8eb0-581c91c9e8ce"), GadgetFilterId = Guid.Parse("d4925542-1eea-4e2b-b237-ac0249ad9044"), Value = "90 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("029f909e-94c1-41fe-a6ab-8787fc5a083b"), GadgetFilterId = Guid.Parse("d4925542-1eea-4e2b-b237-ac0249ad9044"), Value = "120 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("181dc2ef-9bec-4fad-b06a-75c67f27ef12"), GadgetFilterId = Guid.Parse("d4925542-1eea-4e2b-b237-ac0249ad9044"), Value = "144 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("ceb39c71-d6c1-4c01-9c44-33821766c286"), GadgetFilterId = Guid.Parse("d4925542-1eea-4e2b-b237-ac0249ad9044"), Value = "165 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("c71cf45b-90ef-42ad-8695-a74ce9d5e1da"), GadgetFilterId = Guid.Parse("d4925542-1eea-4e2b-b237-ac0249ad9044"), Value = "240 Hz" },
            new GadgetFilterOption { Id = Guid.Parse("addd15ad-d9b9-40fb-b096-c7e92eb922df"), GadgetFilterId = Guid.Parse("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), Value = "SSD 2 TB" },
            new GadgetFilterOption { Id = Guid.Parse("8790fec7-6042-4122-9889-a3f160b36a0e"), GadgetFilterId = Guid.Parse("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), Value = "SSD 1 TB" },
            new GadgetFilterOption { Id = Guid.Parse("8f1acc4d-74b1-4789-910e-e5273b34c8d8"), GadgetFilterId = Guid.Parse("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), Value = "SSD 512 GB" },
            new GadgetFilterOption { Id = Guid.Parse("7e5e22e2-9e65-4a24-9661-efe8bcb016e3"), GadgetFilterId = Guid.Parse("edeb7ed6-d45b-429c-88ae-16ef497b6dcd"), Value = "SSD 256 GB" },
            new GadgetFilterOption { Id = Guid.Parse("09d0883c-1f45-4441-b9fa-0db1b29ae258"), GadgetFilterId = Guid.Parse("af177595-3999-4055-8021-23fe477ac074"), Value = "Ultra 9" },
            new GadgetFilterOption { Id = Guid.Parse("3f29e574-3ccb-45f2-a392-e79915ca5951"), GadgetFilterId = Guid.Parse("af177595-3999-4055-8021-23fe477ac074"), Value = "Ultra 7" },
            new GadgetFilterOption { Id = Guid.Parse("61333095-8e95-488c-adc7-d145b95cd1e0"), GadgetFilterId = Guid.Parse("af177595-3999-4055-8021-23fe477ac074"), Value = "Ultra 5" },
            new GadgetFilterOption { Id = Guid.Parse("8c64ed8a-97dc-4139-93eb-cbf70dc255d2"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Core i9" },
            new GadgetFilterOption { Id = Guid.Parse("d9c2295b-367a-4a65-b2b8-640dacd81668"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Core i7" },
            new GadgetFilterOption { Id = Guid.Parse("63a3c408-919c-412e-93bb-8f5494cb5b87"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Core i5" },
            new GadgetFilterOption { Id = Guid.Parse("eab4caa4-fb76-439b-9b62-d71f837e3f95"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Core i3" },
            new GadgetFilterOption { Id = Guid.Parse("0d4fbfb3-f1b7-478a-b047-018d9726df64"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Core 7" },
            new GadgetFilterOption { Id = Guid.Parse("ad6e5bdc-5f39-4677-a089-90aefe0b574d"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Core 5" },
            new GadgetFilterOption { Id = Guid.Parse("923a4677-0a7b-4884-8603-0d1bfef720d0"), GadgetFilterId = Guid.Parse("aa34ffb4-077a-480f-b681-970f6144cef4"), Value = "Celebron/Pentium" },
            new GadgetFilterOption { Id = Guid.Parse("78d883b3-a3e7-43f1-8033-e23ee5140bfa"), GadgetFilterId = Guid.Parse("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), Value = "Ryzen 9" },
            new GadgetFilterOption { Id = Guid.Parse("579f7ae2-73d9-4b99-a85f-9e0e40b83808"), GadgetFilterId = Guid.Parse("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), Value = "Ryzen 7" },
            new GadgetFilterOption { Id = Guid.Parse("2b603398-f28f-4707-bd02-f766776e0691"), GadgetFilterId = Guid.Parse("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), Value = "Ryzen 4" },
            new GadgetFilterOption { Id = Guid.Parse("d6cb799e-477d-4baf-9b1a-8c434a9bbeb2"), GadgetFilterId = Guid.Parse("9e64a59d-82fe-417b-a16f-66fae0ad5ac4"), Value = "AMD Ryzen AI 9 300 series" },
            new GadgetFilterOption { Id = Guid.Parse("10a6f572-916a-40b1-b53f-d74f3ec8c836"), GadgetFilterId = Guid.Parse("2b3cdf91-68aa-491a-8541-8154990f30cc"), Value = "Apple M1" },
            new GadgetFilterOption { Id = Guid.Parse("e1b1a228-b28a-46f7-8bb7-6bd7b623bc3b"), GadgetFilterId = Guid.Parse("2b3cdf91-68aa-491a-8541-8154990f30cc"), Value = "Apple M2" },
            new GadgetFilterOption { Id = Guid.Parse("6d9d5111-ce2c-4d3a-8e8a-d0333cb24b6b"), GadgetFilterId = Guid.Parse("2b3cdf91-68aa-491a-8541-8154990f30cc"), Value = "Apple M3" },
            new GadgetFilterOption { Id = Guid.Parse("58d42732-d950-48a2-9836-370bc0e4595e"), GadgetFilterId = Guid.Parse("2b3cdf91-68aa-491a-8541-8154990f30cc"), Value = "Apple M3 Pro" },
            new GadgetFilterOption { Id = Guid.Parse("5fbfd5d4-679a-486f-b1e6-e50daea79620"), GadgetFilterId = Guid.Parse("2b3cdf91-68aa-491a-8541-8154990f30cc"), Value = "Apple M3 Max" },
            new GadgetFilterOption { Id = Guid.Parse("a304ef96-8212-4ee0-a583-ff5a9c7f3ced"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Bluetooth" },
            new GadgetFilterOption { Id = Guid.Parse("cad7e715-e630-4692-81b8-5aa20218acbd"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "True Wireless" },
            new GadgetFilterOption { Id = Guid.Parse("4fcbfbd4-70b9-4090-bfd7-39141301d389"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Chụp tai" },
            new GadgetFilterOption { Id = Guid.Parse("da084a90-47ad-49c8-bf2c-018774fd0730"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Gaming" },
            new GadgetFilterOption { Id = Guid.Parse("cb6a450e-5bca-4984-aefc-dcc7d64435fd"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Có dây" },
            new GadgetFilterOption { Id = Guid.Parse("ba47ebb4-65a0-4441-9a89-688186841cd6"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Loa bluetooth" },
            new GadgetFilterOption { Id = Guid.Parse("44aa2cee-a6ea-4924-9432-ed64c16f3144"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Loa kéo" },
            new GadgetFilterOption { Id = Guid.Parse("a2161226-ddff-45d5-9aae-2a9bfde4bc66"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Loa karaoke" },
            new GadgetFilterOption { Id = Guid.Parse("f9b34928-45a7-415e-8963-958e62f1b448"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Loa vi tính" },
            new GadgetFilterOption { Id = Guid.Parse("5accff40-716c-4781-ac44-2cce671a4b2e"), GadgetFilterId = Guid.Parse("24e56e95-f6ad-49f3-96be-7ee5c1556980"), Value = "Loa thanh, soundbar" },
            new GadgetFilterOption { Id = Guid.Parse("df63181e-76e4-4e2b-b285-8590b57cf174"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Dưới 200.000đ" },
            new GadgetFilterOption { Id = Guid.Parse("85978333-2087-402c-aea9-f8cee1d08d54"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Từ 200.000 - 500.000đ" },
            new GadgetFilterOption { Id = Guid.Parse("1bb4507a-8d94-4bc6-ba27-e1cc8e50de97"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Từ 500.000đ - 1 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("df7b8b9e-af36-4bb7-8f83-d72155d97f14"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Từ 1 - 2 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("b4e08f8b-137e-442d-a028-5d28d923ad43"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Từ 2 - 4 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("6432220b-50f9-4008-a9d7-50895959cb3f"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Từ 4 - 7 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("a0b1881e-506f-464d-b038-436dc880f0cc"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Từ 7 - 10 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("6d97d84d-b1b3-4dbb-af75-b21f160b3463"), GadgetFilterId = Guid.Parse("485a61b8-c010-4f7b-b994-d4ee14d0fc87"), Value = "Trên 10 triệu" },
            new GadgetFilterOption { Id = Guid.Parse("0c5ba122-46e0-491d-9bb5-384e5897d6c1"), GadgetFilterId = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), Value = "Từ 10W trở xuống" },
            new GadgetFilterOption { Id = Guid.Parse("762077ac-5eda-4080-8b0d-90e64c8ca9bd"), GadgetFilterId = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), Value = "10W - 40W" },
            new GadgetFilterOption { Id = Guid.Parse("25899e6c-b7db-44a3-bc08-ac73404281ea"), GadgetFilterId = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), Value = "40W - 100W" },
            new GadgetFilterOption { Id = Guid.Parse("2d74f71e-2ffd-48c2-a456-c90ccc3bc1b8"), GadgetFilterId = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), Value = "100W - 500W" },
            new GadgetFilterOption { Id = Guid.Parse("2824fd1b-77dd-46a5-867f-159bfba9e4a9"), GadgetFilterId = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), Value = "500W - 1000W" },
            new GadgetFilterOption { Id = Guid.Parse("cdd998fc-5087-49d9-9376-9c2ce05ed669"), GadgetFilterId = Guid.Parse("021a23f0-fd04-4bb6-89e8-991c2e879a85"), Value = "1000W trở lên" },
            new GadgetFilterOption { Id = Guid.Parse("cdadb24a-a954-493e-965d-e19d011fde8d"), GadgetFilterId = Guid.Parse("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), Value = "Dưới 4 tiếng" },
            new GadgetFilterOption { Id = Guid.Parse("69d68f1e-0c3f-43b6-a10c-59a324245152"), GadgetFilterId = Guid.Parse("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), Value = "4 - 6 tiếng" },
            new GadgetFilterOption { Id = Guid.Parse("5b69c6ac-81e6-489f-b3d8-1591abac8315"), GadgetFilterId = Guid.Parse("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), Value = "6 - 8 tiếng" },
            new GadgetFilterOption { Id = Guid.Parse("ebae64b2-6561-4089-8b7d-e2fdb35baa54"), GadgetFilterId = Guid.Parse("dbac6116-25b2-4a4e-adfd-1d09d0f8bff5"), Value = "8 tiếng trở lên" },
            new GadgetFilterOption { Id = Guid.Parse("c50f06dd-0491-4e34-ab74-938f80d9061d"), GadgetFilterId = Guid.Parse("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), Value = "Type-C" },
            new GadgetFilterOption { Id = Guid.Parse("cb801046-31b0-4ac3-8450-65f55daeb61b"), GadgetFilterId = Guid.Parse("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), Value = "Lightning" },
            new GadgetFilterOption { Id = Guid.Parse("60ed57b9-c305-4435-b061-00e149456393"), GadgetFilterId = Guid.Parse("351c7135-5bf6-46b1-a07d-c1e3ffaa2cdb"), Value = "Micro USB" },
            new GadgetFilterOption { Id = Guid.Parse("68e68a4c-72a7-4e5b-bfd1-e5f43a1faf2f"), GadgetFilterId = Guid.Parse("903f0975-b33f-4e16-9243-94f2530d26dd"), Value = "3.5 mm" },
            new GadgetFilterOption { Id = Guid.Parse("2cbe7bdf-a1f1-444f-bb95-d0a31437f70d"), GadgetFilterId = Guid.Parse("903f0975-b33f-4e16-9243-94f2530d26dd"), Value = "Type-C" },
            new GadgetFilterOption { Id = Guid.Parse("da698064-fcae-4232-9942-30926d49fb85"), GadgetFilterId = Guid.Parse("903f0975-b33f-4e16-9243-94f2530d26dd"), Value = "Lightning" },
            new GadgetFilterOption { Id = Guid.Parse("bc7f1428-1071-4121-95fa-c1ef84e690b9"), GadgetFilterId = Guid.Parse("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), Value = "Sạc không dây" },
            new GadgetFilterOption { Id = Guid.Parse("6f6c9fbd-3d2d-40c2-b49c-7b3a4d9331b3"), GadgetFilterId = Guid.Parse("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), Value = "Chống nước" },
            new GadgetFilterOption { Id = Guid.Parse("7f94026b-cf8c-4c4a-9f92-b89dba5a9f95"), GadgetFilterId = Guid.Parse("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), Value = "Chống ồn" },
            new GadgetFilterOption { Id = Guid.Parse("38593320-d491-4dff-b7d7-5bcaed43fa9b"), GadgetFilterId = Guid.Parse("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), Value = "Có mic đàm thoại" },
            new GadgetFilterOption { Id = Guid.Parse("73046e00-9602-4ecf-bec8-ae9e33c6a19d"), GadgetFilterId = Guid.Parse("d152b425-8b7e-4fea-902a-0c2227a7e0c3"), Value = "Nhỏ gọn" }
        );
    }
}
