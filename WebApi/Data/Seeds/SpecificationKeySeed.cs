﻿using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class SpecificationKeySeed
{
    public readonly static List<SpecificationKey> Default =
    [
new SpecificationKey { Id = Guid.Parse("99ea7edb-f4cd-4caf-b9e0-696e89f6770e"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Công nghệ màn hình" },
new SpecificationKey { Id = Guid.Parse("30e4fee5-3e42-482a-992d-dd9c0da4e6ec"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Độ phân giải màn hình" },
new SpecificationKey { Id = Guid.Parse("f4f704bd-0c6f-4ce7-99b1-e6fb2e35895a"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Màn hình rộng" },
new SpecificationKey { Id = Guid.Parse("52b8f532-614e-42b3-80b0-4c89ff7e9fac"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Tần số quét" },
new SpecificationKey { Id = Guid.Parse("fb9f6a00-6b12-4cd9-a45c-0b838e28a7e0"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Độ sáng tối đa" },
new SpecificationKey { Id = Guid.Parse("69106728-4022-45ed-9d1a-a75a363c0996"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Mặt kính cảm ứng" },
new SpecificationKey { Id = Guid.Parse("ca9c44f5-b8f0-46c2-9bba-220c40de22a6"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Độ phân giải camera sau" },
new SpecificationKey { Id = Guid.Parse("e0b78b05-5c7d-4949-809b-eb9bffb9a29b"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Tính năng" },
new SpecificationKey { Id = Guid.Parse("a0ac5508-d10c-4a18-adf7-d2e4d3a1f4d2"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Độ phân giải camera trước" },
new SpecificationKey { Id = Guid.Parse("967a0690-044e-49a5-924e-842f0cd8a649"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Hệ điều hành" },
new SpecificationKey { Id = Guid.Parse("61794f75-a41f-41b8-8120-8545a47557e0"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Chíp xử lý (CPU)" },
new SpecificationKey { Id = Guid.Parse("d77ec635-b418-49db-a797-745d9324a322"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Tốc độ CPU" },
new SpecificationKey { Id = Guid.Parse("8c04a83a-706b-4d26-8806-337421e0d9bf"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Chíp đồ họa (GPU)" },
new SpecificationKey { Id = Guid.Parse("910e7f97-b139-4645-a765-d33ce93ec676"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "RAM" },
new SpecificationKey { Id = Guid.Parse("5e6541d2-6cd0-4964-a190-2573f9c6f755"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Dung lượng lưu trữ" },
new SpecificationKey { Id = Guid.Parse("4c1d6d1a-12c5-4950-ad33-65e836fc145f"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Cổng kết nối/sạc" },
new SpecificationKey { Id = Guid.Parse("ff78acd2-730d-4fb3-9124-8f12b408ba5e"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Jack tai nghe" },
new SpecificationKey { Id = Guid.Parse("c9c77a06-79d2-42f1-9560-f7232ce9b0dc"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Dung lượng pin" },
new SpecificationKey { Id = Guid.Parse("d4f0c7c8-ec32-4562-968e-6f0f22008c21"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Loại pin" },
new SpecificationKey { Id = Guid.Parse("c9374f68-55dc-4ffa-886a-9a4c73c3f679"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Hỗ trợ sạc tối đa" },
new SpecificationKey { Id = Guid.Parse("809b7149-cae8-4b55-9121-576d433358f0"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Kích thước, khối lượng" },
new SpecificationKey { Id = Guid.Parse("410b264e-e7cd-459c-986c-e42671973452"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Thời điểm ra mắt" },
new SpecificationKey { Id = Guid.Parse("78000692-8165-4e5f-9f3a-b0610c375241"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Xuất xứ" },
new SpecificationKey { Id = Guid.Parse("c2fe3588-40f9-40b7-9203-ff7cfe68c788"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Màu sắc" },
new SpecificationKey { Id = Guid.Parse("b5990fca-7c86-4341-9d2e-8bbd7eacc10a"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Ghi âm" },
new SpecificationKey { Id = Guid.Parse("e4fb075d-af4f-4f58-ba4e-398116484afe"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Radio" },
new SpecificationKey { Id = Guid.Parse("5b400fe2-2aa7-4451-b392-f42d967474c9"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Đèn pin" },
new SpecificationKey { Id = Guid.Parse("84759bbf-c7c4-4390-b913-e01263cca8d4"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Xem phim" },
new SpecificationKey { Id = Guid.Parse("9961e26b-db85-4eac-803f-1fba29f15b98"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "Nghe nhạc" },
new SpecificationKey { Id = Guid.Parse("a9408729-0168-49f4-bd26-4fc1bfadfb5d"), CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d"), Name = "SIM" },
new SpecificationKey { Id = Guid.Parse("6f72e174-3dd5-4ef4-8326-e6bc5283c0cc"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Công nghệ CPU" },
new SpecificationKey { Id = Guid.Parse("4da38515-5131-4295-a0bd-e3bc123cc52f"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Số nhân" },
new SpecificationKey { Id = Guid.Parse("c83668bd-255c-4a91-9156-d01131fbf2e5"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Số luồng" },
new SpecificationKey { Id = Guid.Parse("6932f8d1-c1e8-44f4-9c33-58e11a9c2f5f"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Tốc độ CPU" },
new SpecificationKey { Id = Guid.Parse("7e58f41b-7374-439c-976c-0c567472e0ae"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Tốc độ tối đa" },
new SpecificationKey { Id = Guid.Parse("d9226089-223c-42d7-82dd-93724b97ba2d"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Bộ nhớ đệm" },
new SpecificationKey { Id = Guid.Parse("3af9c08d-ba0f-4a09-8f7f-b0a33bbc82c8"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "RAM" },
new SpecificationKey { Id = Guid.Parse("f92a18c7-e378-4a43-b0e6-0ddc7eeebbd1"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Loại RAM" },
new SpecificationKey { Id = Guid.Parse("8ce25e52-6b53-4ded-95c2-849d21f32322"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Tốc độ Bus RAM" },
new SpecificationKey { Id = Guid.Parse("d3060947-5db8-48a8-b502-a0528029101b"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Hỗ trợ RAM tối đa" },
new SpecificationKey { Id = Guid.Parse("01016216-acc7-4043-abb4-e040ff2bb6c8"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Ổ cứng SSD" },
new SpecificationKey { Id = Guid.Parse("adb455ec-87d4-4381-983b-20c9e775f6d3"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Ổ cứng HDD" },
new SpecificationKey { Id = Guid.Parse("a17c69da-6867-4178-aef5-019ebea7a6b9"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Ổ cứng eMMC" },
new SpecificationKey { Id = Guid.Parse("755d4959-0926-4255-8a55-b922df2619be"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Màn hình" },
new SpecificationKey { Id = Guid.Parse("d69c7754-df12-47b6-936c-280d1736786a"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Độ phân giải" },
new SpecificationKey { Id = Guid.Parse("08cf30be-1e80-4dd9-a6c5-d51a0a761f59"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Tần số quét" },
new SpecificationKey { Id = Guid.Parse("9e6fdb90-2021-4406-aab7-5ccbfa67d4f6"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Card màn hình" },
new SpecificationKey { Id = Guid.Parse("1df8de8a-228b-4a98-a0d1-936838028db0"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Công nghệ âm thanh" },
new SpecificationKey { Id = Guid.Parse("1b7e727a-a8fb-497b-a4fe-9af5c3745ea2"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Cổng giao tiếp" },
new SpecificationKey { Id = Guid.Parse("f3e8e67e-1193-4910-9066-0c0efc83dcc0"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Tính năng" },
new SpecificationKey { Id = Guid.Parse("88f83206-ce93-4bbd-96b3-f2b6d05b80f3"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Kích thước" },
new SpecificationKey { Id = Guid.Parse("af3b2b82-62cc-43f1-85ab-285148925e48"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Khối lượng" },
new SpecificationKey { Id = Guid.Parse("d09743af-b791-4816-9ae8-e1e34d31a129"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Thông tin pin" },
new SpecificationKey { Id = Guid.Parse("ccf78a72-b90e-447f-bb38-d3d5104afd8a"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Hệ điều hành" },
new SpecificationKey { Id = Guid.Parse("a639cde8-caf0-4114-95fb-4767fe8c726b"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Thời điểm ra mắt" },
new SpecificationKey { Id = Guid.Parse("3a20e69a-6381-44af-958a-46d6c68cedc0"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Xuất xứ" },
new SpecificationKey { Id = Guid.Parse("eac78b9f-695b-41b9-9e7a-e098025447d3"), CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1"), Name = "Màu sắc" },
new SpecificationKey { Id = Guid.Parse("b249d655-35a6-43f2-98b7-4e3ab7bf500b"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Thời lượng pin tai nghe" },
new SpecificationKey { Id = Guid.Parse("ea0217da-65f7-4444-8785-b34b0043b6e5"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Thời lượng pin hộp sạc" },
new SpecificationKey { Id = Guid.Parse("412942fb-065d-4550-9ac7-74e235cee397"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Cổng sạc" },
new SpecificationKey { Id = Guid.Parse("80da98e6-893f-4433-bb4d-f899c9683f32"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Công nghệ âm thanh" },
new SpecificationKey { Id = Guid.Parse("3e786cf3-d4b5-4473-95d6-debf34ab4dbc"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Tương thích" },
new SpecificationKey { Id = Guid.Parse("27300406-720d-4503-ab89-d06b51693e9f"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Tính năng" },
new SpecificationKey { Id = Guid.Parse("708229b0-32d6-444d-8b96-8aa7e2f5a003"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Kết nối cùng lúc" },
new SpecificationKey { Id = Guid.Parse("1711f0c2-3dcd-4c8f-b737-cfc4474b5d9b"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Công nghệ kết nối" },
new SpecificationKey { Id = Guid.Parse("2d82c2cb-6268-4223-9259-02e7461b992b"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Điều khiển" },
new SpecificationKey { Id = Guid.Parse("8eb28982-54e4-4ac7-b568-559f7ab46dcd"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Kích thước" },
new SpecificationKey { Id = Guid.Parse("d915b5d7-d685-4bf7-9fad-ba48ab374ac7"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Khối lượng" },
new SpecificationKey { Id = Guid.Parse("d42b220e-f988-4866-989d-a6777e34345d"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Xuất xứ" },
new SpecificationKey { Id = Guid.Parse("31822044-08be-4017-a734-d25d1fb587ff"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Jack cấm" },
new SpecificationKey { Id = Guid.Parse("fb072d57-ec6f-4f9e-b61c-2336522acb48"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Độ dài dây" },
new SpecificationKey { Id = Guid.Parse("6798f1bd-b60a-40eb-b18c-8da698e3773b"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Thương hiệu của" },
new SpecificationKey { Id = Guid.Parse("e5e99e21-e6b8-4a82-8ea0-7f2d93494f19"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Thời điểm ra mắt" },
new SpecificationKey { Id = Guid.Parse("05e763be-85d1-4e03-b121-89b82ea71a97"), CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86"), Name = "Màu sắc" },
new SpecificationKey { Id = Guid.Parse("7409ef43-3578-4127-a02d-24f2870a7aa7"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Loại sản phẩm" },
new SpecificationKey { Id = Guid.Parse("2ad8f132-794a-4299-86ae-c02af9493c2f"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tổng công suất" },
new SpecificationKey { Id = Guid.Parse("e72da76a-149e-43ab-a284-2dc67b0be625"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Nguồn" },
new SpecificationKey { Id = Guid.Parse("c69f3652-fa3a-48b6-a3e8-267c4ca60305"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Thời gian sử dụng" },
new SpecificationKey { Id = Guid.Parse("78e761b1-687c-42da-8a0a-3e9d668e951e"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Thời gian sạc" },
new SpecificationKey { Id = Guid.Parse("beef6002-ce1b-41ed-9165-5db0cf3a358c"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Công nghệ âm thanh" },
new SpecificationKey { Id = Guid.Parse("6905e227-4e79-407f-91c0-4d7ac7f55f49"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tính năng" },
new SpecificationKey { Id = Guid.Parse("6829acc6-3dcb-4140-b6a6-56e3dbbdcdeb"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kết nối" },
new SpecificationKey { Id = Guid.Parse("0379a2a5-f61d-4498-a8cc-f41ebd4880fd"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Cổng sạc" },
new SpecificationKey { Id = Guid.Parse("33383362-8985-41c4-8155-44cdf29e3ccb"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Khoảng cách kết nối tối đa" },
new SpecificationKey { Id = Guid.Parse("f2a7d556-fb1d-4cf9-917e-c3ec99171595"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước" },
new SpecificationKey { Id = Guid.Parse("1ea93190-11f0-456a-960f-b0a04bf8a279"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Thương hiệu của" },
new SpecificationKey { Id = Guid.Parse("dd40c8d9-8242-4a0a-838c-05dfa7412d59"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Xuất xứ" },
new SpecificationKey { Id = Guid.Parse("87987fb2-2b91-4392-b019-f1fc2bcfac49"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Thời điểm ra mắt" },
new SpecificationKey { Id = Guid.Parse("9c6e8c16-a152-4bda-91ce-74a5fa170274"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Số lượng kênh" },
new SpecificationKey { Id = Guid.Parse("821d2d3f-5e8a-43ee-bb65-61c3e038f899"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Số đường tiếng của loa" },
new SpecificationKey { Id = Guid.Parse("7e8d1f8f-c111-4cb3-a715-7ec6cbd23e88"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tổng số lượng loa Bass" },
new SpecificationKey { Id = Guid.Parse("de57dd71-2c5f-4944-aa2a-e409e3cc6967"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa Bass" },
new SpecificationKey { Id = Guid.Parse("cfec43f4-8787-4174-9f1a-752d40497eee"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tổng số lượng loa Treble" },
new SpecificationKey { Id = Guid.Parse("882c82b3-07e9-4ca2-9550-e57ac1cc106a"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa Treble" },
new SpecificationKey { Id = Guid.Parse("48ddd9d1-e503-4e9c-94c2-798d1755cbe9"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Công suất loa thanh" },
new SpecificationKey { Id = Guid.Parse("356b96f5-f3ed-49d8-a070-80ce12651300"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Công suất loa Sub" },
new SpecificationKey { Id = Guid.Parse("3d4dfddb-71dd-4029-a8bd-3bd5ee75426c"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kich thước loa Sub" },
new SpecificationKey { Id = Guid.Parse("47a6308d-d895-4860-8cf5-3fa430b58e69"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa trước (loa Front)" },
new SpecificationKey { Id = Guid.Parse("89629dab-8222-4870-83f3-bbe923ffeda3"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa vệ tinh" },
new SpecificationKey { Id = Guid.Parse("52e7256c-08a1-426d-9eb7-8172c692bd3e"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa thanh" },
new SpecificationKey { Id = Guid.Parse("35dc40b2-d71e-4501-8e2d-178ddb6e5b29"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tổng số lượng loa Sub" },
new SpecificationKey { Id = Guid.Parse("663b7679-17dd-4cff-8f03-a2784d9f1a6f"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa sau (loa Surround)" },
new SpecificationKey { Id = Guid.Parse("b2113e58-ed39-4b49-8de9-933d397ed220"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tổng số lượng loa Woofer" },
new SpecificationKey { Id = Guid.Parse("34540138-4492-4b54-9e50-b7d9699332f8"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa Woofer" },
new SpecificationKey { Id = Guid.Parse("581b4e78-774d-4387-b4e7-66e0da679492"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Tổng số lượng loa đánh trần (Up-Firing)" },
new SpecificationKey { Id = Guid.Parse("910ec043-7107-433d-9e58-51db22bfafb3"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Kích thước loa đánh trần" },
new SpecificationKey { Id = Guid.Parse("c5813bff-6124-4d39-98f9-7b71d6b92ee7"), CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa"), Name = "Màu sắc" },
    ];
}
