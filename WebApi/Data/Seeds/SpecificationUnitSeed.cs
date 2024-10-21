using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SpecificationUnitSeed
{
    public readonly static List<SpecificationUnit> Default =
    [
new SpecificationUnit { Id = Guid.Parse("f9f79259-3a47-4454-a145-c088136b561d"), SpecificationKeyId = Guid.Parse("f4f704bd-0c6f-4ce7-99b1-e6fb2e35895a"), Name = "Inch" },
new SpecificationUnit { Id = Guid.Parse("cb75f311-fd4d-488c-9131-ae44c3d53825"), SpecificationKeyId = Guid.Parse("52b8f532-614e-42b3-80b0-4c89ff7e9fac"), Name = "Hz" },
new SpecificationUnit { Id = Guid.Parse("6bd675e5-ab49-4d88-9c47-e120bed509e4"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), Name = "MB" },
new SpecificationUnit { Id = Guid.Parse("aa407b94-f43d-494c-b95c-b8b5cf5d54b0"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), Name = "GB" },
new SpecificationUnit { Id = Guid.Parse("63285f18-a27a-41d7-be19-4f1496339445"), SpecificationKeyId = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), Name = "MB" },
new SpecificationUnit { Id = Guid.Parse("c9202ec7-396c-43d8-b2ea-caac691cb4ae"), SpecificationKeyId = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), Name = "GB" },
new SpecificationUnit { Id = Guid.Parse("5ce898f9-496a-4cbf-94f3-f57492f5034d"), SpecificationKeyId = Guid.Parse("c9374f68-55dc-4ffa-886a-9a4c73c3f679"), Name = "W" },
new SpecificationUnit { Id = Guid.Parse("d9f5f13c-de9f-4315-b112-fdfe66bd6b50"), SpecificationKeyId = Guid.Parse("c9c77a06-79d2-42f1-9560-f7232ce9b0dc"), Name = "mAh" },
new SpecificationUnit { Id = Guid.Parse("6a32f4ff-4c03-42f9-b1ac-2fc0c4f45d41"), SpecificationKeyId = Guid.Parse("d9226089-223c-42d7-82dd-93724b97ba2d"), Name = "MB" },
new SpecificationUnit { Id = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), Name = "GB" },
new SpecificationUnit { Id = Guid.Parse("11a2c56c-e44d-4fe1-a939-9590d111d0e9"), SpecificationKeyId = Guid.Parse("8ce25e52-6b53-4ded-95c2-849d21f32322"), Name = "MHz" },
new SpecificationUnit { Id = Guid.Parse("ced12fad-3c7c-4d6e-981f-a9677a720a88"), SpecificationKeyId = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), Name = "GB" },
new SpecificationUnit { Id = Guid.Parse("740cacf6-a36c-4e33-ac45-12cc9c699430"), SpecificationKeyId = Guid.Parse("adb455ec-87d4-4381-983b-20c9e775f6d3"), Name = "GB" },
new SpecificationUnit { Id = Guid.Parse("e6b51261-5edc-4930-a729-b077afaab91b"), SpecificationKeyId = Guid.Parse("adb455ec-87d4-4381-983b-20c9e775f6d3"), Name = "TB" },
new SpecificationUnit { Id = Guid.Parse("df2f9ee3-b97c-4a72-88f6-8d5ed1d19950"), SpecificationKeyId = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), Name = "TB" },
new SpecificationUnit { Id = Guid.Parse("654b9205-7b05-4791-8b64-e91d572a9790"), SpecificationKeyId = Guid.Parse("a17c69da-6867-4178-aef5-019ebea7a6b9"), Name = "GB" },
new SpecificationUnit { Id = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), Name = "Inch" },
new SpecificationUnit { Id = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), Name = "Hz" },
new SpecificationUnit { Id = Guid.Parse("35b3da1b-aef9-44e2-a66d-4dde93ed440d"), SpecificationKeyId = Guid.Parse("af3b2b82-62cc-43f1-85ab-285148925e48"), Name = "kg" },
new SpecificationUnit { Id = Guid.Parse("99dbfeb0-5f87-4943-ac8e-b50f7ab3faa0"), SpecificationKeyId = Guid.Parse("d915b5d7-d685-4bf7-9fad-ba48ab374ac7"), Name = "g" },
new SpecificationUnit { Id = Guid.Parse("ef72e0f4-fd4b-4403-a5c8-28278a1850aa"), SpecificationKeyId = Guid.Parse("708229b0-32d6-444d-8b96-8aa7e2f5a003"), Name = "thiết bị" },
new SpecificationUnit { Id = Guid.Parse("b5d120c2-a8a3-47e5-8014-db79d5debccb"), SpecificationKeyId = Guid.Parse("2ad8f132-794a-4299-86ae-c02af9493c2f"), Name = "W" },
new SpecificationUnit { Id = Guid.Parse("2cf2a453-def9-49e1-b4fe-146bbdc2a370"), SpecificationKeyId = Guid.Parse("33383362-8985-41c4-8155-44cdf29e3ccb"), Name = "m" },
new SpecificationUnit { Id = Guid.Parse("6c083b89-da36-499b-9d86-9c09f9a7ed64"), SpecificationKeyId = Guid.Parse("48ddd9d1-e503-4e9c-94c2-798d1755cbe9"), Name = "W" },
new SpecificationUnit { Id = Guid.Parse("4b965866-1e1b-48de-b9cf-839146da9d06"), SpecificationKeyId = Guid.Parse("356b96f5-f3ed-49d8-a070-80ce12651300"), Name = "W" },
    ];
}
