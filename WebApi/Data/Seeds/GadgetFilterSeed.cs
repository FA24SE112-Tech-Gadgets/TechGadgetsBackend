﻿using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class GadgetFilterSeed
{
    public readonly static List<GadgetFilter> Default =
    [
new GadgetFilter { Id = Guid.Parse("cd222d35-d3ab-442a-a360-b00ad793f9b8"), SpecificationKeyId = Guid.Parse("967a0690-044e-49a5-924e-842f0cd8a649"), SpecificationUnitId = null, Value = "Android" },
new GadgetFilter { Id = Guid.Parse("3040e673-c1a5-4f84-b65c-4a8c95b7726d"), SpecificationKeyId = Guid.Parse("967a0690-044e-49a5-924e-842f0cd8a649"), SpecificationUnitId = null, Value = "iOS" },
new GadgetFilter { Id = Guid.Parse("ef6fa620-c5af-4b94-a728-1d2a6c7908da"), SpecificationKeyId = Guid.Parse("967a0690-044e-49a5-924e-842f0cd8a649"), SpecificationUnitId = null, Value = "Khác" },
new GadgetFilter { Id = Guid.Parse("806918fc-f80b-4cb2-9bd3-0b272d483bb6"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), SpecificationUnitId = Guid.Parse("aa407b94-f43d-494c-b95c-b8b5cf5d54b0"), Value = "3" },
new GadgetFilter { Id = Guid.Parse("135bcf76-96fc-4731-90d6-12df724f5bc5"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), SpecificationUnitId = Guid.Parse("aa407b94-f43d-494c-b95c-b8b5cf5d54b0"), Value = "4" },
new GadgetFilter { Id = Guid.Parse("5ec234da-2c54-4fbc-819e-a2f3f48b7ab0"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), SpecificationUnitId = Guid.Parse("aa407b94-f43d-494c-b95c-b8b5cf5d54b0"), Value = "6" },
new GadgetFilter { Id = Guid.Parse("8513043a-e9ec-4a8e-a13b-46c46f53cc7e"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), SpecificationUnitId = Guid.Parse("aa407b94-f43d-494c-b95c-b8b5cf5d54b0"), Value = "8" },
new GadgetFilter { Id = Guid.Parse("25fab047-dc45-4d3a-8f7e-e5aeda8e569e"), SpecificationKeyId = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), SpecificationUnitId = Guid.Parse("aa407b94-f43d-494c-b95c-b8b5cf5d54b0"), Value = "12" },
new GadgetFilter { Id = Guid.Parse("e0f9674f-15e4-49c2-bf79-cbb86c39e622"), SpecificationKeyId = Guid.Parse("52b8f532-614e-42b3-80b0-4c89ff7e9fac"), SpecificationUnitId = Guid.Parse("cb75f311-fd4d-488c-9131-ae44c3d53825"), Value = "60" },
new GadgetFilter { Id = Guid.Parse("bd4a874b-f623-4747-bd21-1a08ea9b6550"), SpecificationKeyId = Guid.Parse("52b8f532-614e-42b3-80b0-4c89ff7e9fac"), SpecificationUnitId = Guid.Parse("cb75f311-fd4d-488c-9131-ae44c3d53825"), Value = "90" },
new GadgetFilter { Id = Guid.Parse("b1395d73-f77d-4b5f-be0a-034c35e3db90"), SpecificationKeyId = Guid.Parse("52b8f532-614e-42b3-80b0-4c89ff7e9fac"), SpecificationUnitId = Guid.Parse("cb75f311-fd4d-488c-9131-ae44c3d53825"), Value = "120" },
new GadgetFilter { Id = Guid.Parse("d4d1c3af-f7e2-4a60-9b5a-a5fc7b03f818"), SpecificationKeyId = Guid.Parse("52b8f532-614e-42b3-80b0-4c89ff7e9fac"), SpecificationUnitId = Guid.Parse("cb75f311-fd4d-488c-9131-ae44c3d53825"), Value = "144" },
new GadgetFilter { Id = Guid.Parse("f39fef7b-8d75-4c99-b9f8-8c9b93ea5470"), SpecificationKeyId = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), SpecificationUnitId = Guid.Parse("c9202ec7-396c-43d8-b2ea-caac691cb4ae"), Value = "64" },
new GadgetFilter { Id = Guid.Parse("4e1f1e2e-84a4-4017-8b5a-95ab84ba8f50"), SpecificationKeyId = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), SpecificationUnitId = Guid.Parse("c9202ec7-396c-43d8-b2ea-caac691cb4ae"), Value = "128" },
new GadgetFilter { Id = Guid.Parse("fbf8487e-89f7-424a-9a3c-cd5c20b9f7d1"), SpecificationKeyId = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), SpecificationUnitId = Guid.Parse("c9202ec7-396c-43d8-b2ea-caac691cb4ae"), Value = "256" },
new GadgetFilter { Id = Guid.Parse("a2666411-e92c-4a80-9c60-33944e776247"), SpecificationKeyId = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), SpecificationUnitId = Guid.Parse("c9202ec7-396c-43d8-b2ea-caac691cb4ae"), Value = "512" },
new GadgetFilter { Id = Guid.Parse("798bb217-a195-459b-857c-a0ee16d967f8"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "11.6" },
new GadgetFilter { Id = Guid.Parse("78ca9a83-9f10-471c-b9c2-eef0d3bd296b"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "13.3" },
new GadgetFilter { Id = Guid.Parse("16f49bf5-26a1-4e28-baae-29e1fa97a439"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "13.4" },
new GadgetFilter { Id = Guid.Parse("1756a12e-abe6-4000-8e9e-26449694386f"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "13.6" },
new GadgetFilter { Id = Guid.Parse("744b8b50-ba2b-4b5f-ae79-d2e5f284e27a"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "14" },
new GadgetFilter { Id = Guid.Parse("3c320911-d232-4ee2-9c79-3238871a21ce"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "14.2" },
new GadgetFilter { Id = Guid.Parse("9a7d024e-37cf-4207-b7e9-ce6b6f5b0279"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "15.6" },
new GadgetFilter { Id = Guid.Parse("b45d0cfa-6fe7-4c5d-bc6f-ecd557b66766"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "16" },
new GadgetFilter { Id = Guid.Parse("894fb0ac-a0d4-4276-a70d-b3f67919bc74"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "16.1" },
new GadgetFilter { Id = Guid.Parse("ac65f68e-97c6-4686-b726-af24d47371dc"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "16.2" },
new GadgetFilter { Id = Guid.Parse("1b3e42fb-a499-4a30-b96d-4080fc3ded58"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "17" },
new GadgetFilter { Id = Guid.Parse("3229f280-fb8a-491e-a579-568313101b91"), SpecificationKeyId = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), SpecificationUnitId = Guid.Parse("d4db59af-2163-4a97-ba1a-e03dad3cb719"), Value = "18" },
new GadgetFilter { Id = Guid.Parse("3af20c20-c018-41aa-aae0-675da29b3a8f"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), SpecificationUnitId = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), Value = "60" },
new GadgetFilter { Id = Guid.Parse("fe9bc814-b48e-40e0-9867-c0b2bd3414e6"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), SpecificationUnitId = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), Value = "90" },
new GadgetFilter { Id = Guid.Parse("a3f593f9-8bad-4b7a-8757-81038326ec13"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), SpecificationUnitId = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), Value = "120" },
new GadgetFilter { Id = Guid.Parse("382359a1-8073-43bd-9e06-9d75663918de"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), SpecificationUnitId = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), Value = "144" },
new GadgetFilter { Id = Guid.Parse("9d9f8e4c-5026-4be5-b12d-f92cfc5b53cf"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), SpecificationUnitId = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), Value = "165" },
new GadgetFilter { Id = Guid.Parse("da60b8c3-50cb-4fc1-8639-21bfc1124f4b"), SpecificationKeyId = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), SpecificationUnitId = Guid.Parse("90f45e3f-b9e0-476e-85b4-4f5ef0c4f3d8"), Value = "240" },
new GadgetFilter { Id = Guid.Parse("604e72df-c0a3-4166-afd6-30c69163d0e7"), SpecificationKeyId = Guid.Parse("ccf78a72-b90e-447f-bb38-d3d5104afd8a"), SpecificationUnitId = null, Value = "Window" },
new GadgetFilter { Id = Guid.Parse("b7edf30c-ccbf-4d46-a2f0-6bf794bb0e92"), SpecificationKeyId = Guid.Parse("ccf78a72-b90e-447f-bb38-d3d5104afd8a"), SpecificationUnitId = null, Value = "MacOS" },
new GadgetFilter { Id = Guid.Parse("3cfcc8ab-80a0-4aba-918c-92fa8ea86625"), SpecificationKeyId = Guid.Parse("ccf78a72-b90e-447f-bb38-d3d5104afd8a"), SpecificationUnitId = null, Value = "ChromeOS" },
new GadgetFilter { Id = Guid.Parse("09065753-3381-4ff3-abe7-c3390d029e6c"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "4" },
new GadgetFilter { Id = Guid.Parse("d8033ae4-24ea-43c2-88b3-c2f7c0382a36"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "8" },
new GadgetFilter { Id = Guid.Parse("ecb0663c-724a-4323-a480-47cdbd9cd143"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "16" },
new GadgetFilter { Id = Guid.Parse("ea6530be-0d3f-461e-858a-f06b8d5aaf3c"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "18" },
new GadgetFilter { Id = Guid.Parse("1fa9e1e1-92cc-4e42-a2f9-622476832bd3"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "24" },
new GadgetFilter { Id = Guid.Parse("c4e0dd8f-cf69-4b7f-84fe-da3e339f6580"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "32" },
new GadgetFilter { Id = Guid.Parse("4544b022-1335-4202-9011-28492000cf3e"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "36" },
new GadgetFilter { Id = Guid.Parse("2054cab3-83f2-4e45-88d2-0990ce199139"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "48" },
new GadgetFilter { Id = Guid.Parse("cdbc425e-f534-42b9-84c9-c568ee07856b"), SpecificationKeyId = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), SpecificationUnitId = Guid.Parse("ba58b21e-b0e7-4f23-9c8c-c718ab7e91ed"), Value = "64" },
new GadgetFilter { Id = Guid.Parse("4732091e-0079-468c-a870-969bbfa98b7c"), SpecificationKeyId = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), SpecificationUnitId = Guid.Parse("ced12fad-3c7c-4d6e-981f-a9677a720a88"), Value = "256" },
new GadgetFilter { Id = Guid.Parse("f13fa8d4-abbb-4438-9ad9-dff9a4dc6635"), SpecificationKeyId = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), SpecificationUnitId = Guid.Parse("ced12fad-3c7c-4d6e-981f-a9677a720a88"), Value = "512" },
new GadgetFilter { Id = Guid.Parse("01d8abaf-ae08-4f0a-8868-fa5f4e66a495"), SpecificationKeyId = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), SpecificationUnitId = Guid.Parse("df2f9ee3-b97c-4a72-88f6-8d5ed1d19950"), Value = "1" },
new GadgetFilter { Id = Guid.Parse("59cf750a-595e-4bad-87a1-3f3ac4eed44e"), SpecificationKeyId = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), SpecificationUnitId = Guid.Parse("df2f9ee3-b97c-4a72-88f6-8d5ed1d19950"), Value = "2" },
    ];
}