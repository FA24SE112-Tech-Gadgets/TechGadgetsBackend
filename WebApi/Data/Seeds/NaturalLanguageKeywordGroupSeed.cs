using WebApi.Data.Entities;

namespace WebApi.Data.Seeds;

public static class NaturalLanguageKeywordGroupSeed
{
    private static readonly DateTime now = DateTime.UtcNow;

    public readonly static List<NaturalLanguageKeywordGroup> Default =
    [
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("a8edb927-7cb7-43d0-a285-55f8cd654700"),
            Name = "Học tập",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "học tập",
                    Id = Guid.Parse("060a0006-75d4-4459-aada-60e2296bf43d")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "học tập",
                     Id = Guid.Parse("c1a4a276-d074-477e-8126-1db6a7090cc4"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "sinh viên",
                     Id = Guid.Parse("b43334be-d18d-4775-809e-342ca848f6cd"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "học sinh",
                     Id = Guid.Parse("8df25a72-ad22-4739-8c4d-521fcc80a30f"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "học tập",
                     Id = Guid.Parse("c61e150f-52d0-4ed6-8909-75afabe7698e"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "sinh viên",
                     Id = Guid.Parse("8783578e-7f5f-48ac-834b-52ea20ec7b36"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "học sinh",
                     Id = Guid.Parse("d4b62946-6927-4db7-84ba-bb98ceda5a8d"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("ad891f8f-f652-4305-8967-4d4049dffcf6"),
            Name = "Văn phòng",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "làm việc",
                    Id = Guid.Parse("247f5697-76d2-4b35-a253-5fda211be879")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "văn phòng",
                    Id = Guid.Parse("49b657ca-24fc-4f84-a083-098457b62632")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "văn phòng",
                     Id = Guid.Parse("5a68fe49-99b3-4874-9a70-4a3ec007e820"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "văn phòng",
                     Id = Guid.Parse("7d482db9-9015-4cd1-b098-f59f0e85d229"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "công sở",
                     Id = Guid.Parse("284dbbf6-7367-47f6-8cec-4c90bf6ff18f"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ],
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("e1f25c09-2c66-420e-8725-ccb4d2ffb126"),
            Name = "Giải trí",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "gaming",
                    Id = Guid.Parse("4940b765-d9e2-47d0-a749-cff2ef211564")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "giải trí",
                    Id = Guid.Parse("42efc01b-4fba-4b05-80bb-0f0bbe67e959")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "gaming",
                     Id = Guid.Parse("a0796429-5b27-4e98-9aad-a2beb301f8c1"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "gaming",
                     Id = Guid.Parse("4d42ca3b-d282-44f1-a88c-f1d81c88bcea"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "giải trí",
                     Id = Guid.Parse("1dd981d4-ba14-40c4-a0f5-8781e1074736"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "chơi game",
                     Id = Guid.Parse("2c13dceb-58ff-4f08-9ba9-e12655e9f899"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 }
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("3db4cc51-e79b-4964-81a6-fd71c386ba70"),
            Name = "Thể thao",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "vận động",
                    Id = Guid.Parse("1caf41cf-35b4-421c-a054-996f943c5add")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "ngoài trời",
                    Id = Guid.Parse("56e6b90c-778e-4431-8b1b-f2cd414f9431")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "thể thao",
                    Id = Guid.Parse("f1b2eab2-766b-46ce-8bd8-2fbe04700c58")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "sport",
                     Id = Guid.Parse("ff466e87-6154-48a6-8ec0-1f41c0d0e644"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "chạy bộ",
                     Id = Guid.Parse("e19d842c-18ed-446a-89d6-cf39274a1973"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "thể dục",
                     Id = Guid.Parse("04fc2fa0-635b-4c07-ace6-3b4a7fd80e58"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "thể thao",
                     Id = Guid.Parse("8f34b26e-ea4a-4baf-8ab1-46f1a2fccc2a"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "vận động",
                     Id = Guid.Parse("1ddec9d0-e356-4938-928e-42ee56ae5a5e"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "ngoài trời",
                     Id = Guid.Parse("965779b6-7a66-4ddd-835b-419bfbaac085"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "đi dạo",
                     Id = Guid.Parse("b40d5bb2-70d2-49e9-a7fc-39b045cb8903"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "chạy bộ",
                     Id = Guid.Parse("2f3e8c4a-7680-4d80-a602-a94a15cfc38b"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "sport",
                     Id = Guid.Parse("8168eeaf-1350-4b62-9fbc-7f42f445c1d8"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("6998ee77-b9e8-4663-b7f6-611f6ea9f01b"),
            Name = "Kháng nước",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "kháng nước",
                    Id = Guid.Parse("eac4c4b0-32d7-48c7-9d4f-0357e2de7a38")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "chống nước",
                    Id = Guid.Parse("a905bc05-9944-440a-9805-9c6fb1605f7e")
                },
            ],
            Criteria = [
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "chống nước",
                     Id = Guid.Parse("0dbecba9-977e-41e9-a66f-86f803005be4"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "kháng nước",
                     Id = Guid.Parse("a01748a5-31a0-4527-ba66-a7335ed68ad4"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("d1252056-db34-4c68-a88d-8e46c472df4a"),
            Name = "Điện thoại gập",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "gập",
                    Id = Guid.Parse("cad12bac-8449-4a12-a5d5-eb759b56f4ba")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "flip",
                    Id = Guid.Parse("65540ee3-7cc6-4575-9464-e4b3aca6aa04")
                },
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "gập",
                     Id = Guid.Parse("eed2af76-92b3-4970-87e0-315edbd2db97"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "flip",
                     Id = Guid.Parse("b2464c12-9e08-4011-bda3-07a85711b22f"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "công nghệ gập",
                     Id = Guid.Parse("ad8c68b1-eb9a-4d38-8f65-1a803ead7f56"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "thiết kế gập",
                     Id = Guid.Parse("0cada94b-4a19-4c7d-9c2d-2c00624035ae"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "màn hình gập",
                     Id = Guid.Parse("a604208b-6657-436f-8339-1952a40669e6"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "điện thoại gập",
                     Id = Guid.Parse("86b97f41-f83a-4ee2-bcee-971c695fc513"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("c367df25-8159-491d-b14e-7472bce7f049"),
            Name = "Phân khúc thấp",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "giá rẻ",
                    Id = Guid.Parse("03b433c1-d49c-42cf-b326-c43a11560184")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "giá tốt",
                    Id = Guid.Parse("c9cfe0b1-d146-4b92-9866-2903ef5d25ec")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "giá sinh viên",
                    Id = Guid.Parse("aff4631c-d3f4-441e-8e43-06cf06a15ec4")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 0,
                     MaxPrice = 4_000_000,
                     Id = Guid.Parse("272f0891-129c-48cb-a68e-02863d5eaa91"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 0,
                     MaxPrice = 10_000_000,
                     Id = Guid.Parse("f4b75224-5eda-48ad-941c-8fead13da101"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 0,
                     MaxPrice = 500_000,
                     Id = Guid.Parse("a1da3086-863e-42ae-9b18-67295a04b4fb"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 0,
                     MaxPrice = 2_000_000,
                     Id = Guid.Parse("d667440f-c622-4182-afe3-ff705070e0c3"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("60bf7432-c2a4-4a6e-b648-fa8c9ffcd399"),
            Name = "Phân khúc tầm trung",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "giá tầm trung",
                    Id = Guid.Parse("f77cd1d9-b10d-4fc3-9ed2-064aae671216")
                },
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 4_000_000,
                     MaxPrice = 13_000_000,
                     Id = Guid.Parse("db847360-ff23-4908-8ace-942a6a45e4b4"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 10_000_000,
                     MaxPrice = 25_000_000,
                     Id = Guid.Parse("d83d07a0-7190-49e6-b23f-23657c2fcc9c"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 500_000,
                     MaxPrice = 2_000_000,
                     Id = Guid.Parse("8babef93-b7a5-4889-a50b-28d643b59401"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 2_000_000,
                     MaxPrice = 7_000_000,
                     Id = Guid.Parse("c1af60dc-42c6-45f6-9de9-645d133caf89"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("675510be-b092-46d7-8f21-16994bcd00c5"),
            Name = "Phân khúc cao cấp",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "cao cấp",
                    Id = Guid.Parse("8d988ebd-28b3-4855-a4bd-7d4bf79e8c5d")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "hiện đại",
                    Id = Guid.Parse("58464932-3920-48f0-8415-1bab33311117")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 13_000_000,
                     MaxPrice = 150_000_000,
                     Id = Guid.Parse("cc57bf26-a45f-4446-9308-acc74615435d"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 25_000_000,
                     MaxPrice = 150_000_000,
                     Id = Guid.Parse("fbe6d49e-d1a3-438c-8f4d-d69490d74ccf"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 2_000_000,
                     MaxPrice = 150_000_000,
                     Id = Guid.Parse("98181eb5-e1c8-4139-a525-70715f796273"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Price,
                     MinPrice = 7_000_000,
                     MaxPrice = 150_000_000,
                     Id = Guid.Parse("b9eb62b5-ed17-4703-af31-5b508081542c"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("dfd343e2-da9f-4fbf-8db1-c0694a5b7f7f"),
            Name = "Tiết kiệm điện",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "tiết kiệm điện",
                    Id = Guid.Parse("1e8c7ee2-0db9-4072-b204-3ae511bcb54a")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "tiêu thụ điện thấp",
                    Id = Guid.Parse("18be012d-43eb-47ea-bbe5-b270a74d9574")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "tiết kiệm điện",
                     Id = Guid.Parse("997203be-5c01-492f-9d9e-54adae04a2a8"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "tiết kiệm điện",
                     Id = Guid.Parse("75da58df-b6be-432d-bc18-a3574337b6c9"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "tiêu thụ điện thấp",
                     Id = Guid.Parse("6eee487e-0261-4fee-b7f5-b063e99fb62a"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "điện năng thấp",
                     Id = Guid.Parse("ce6956b3-9a38-4d62-82d5-fbd1dac18c9f"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("cb26cc00-1a1f-46ef-a1ff-690b63de3f4d"),
            Name = "Tai nghe Bluetooth",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "tai nghe bluetooth",
                    Id = Guid.Parse("9f5fdbcf-08fe-4486-9722-74e4d78f1f21")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "bluetooth",
                     Id = Guid.Parse("1658f650-c217-41ac-abec-909072529013"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "bluetooth",
                     Id = Guid.Parse("29ff5cbc-4722-4559-90b2-0aa3f1d60618"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("4d437461-e1db-4950-9da7-947d52bdc001"),
            Name = "Tai nghe có dây",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "tai nghe có dây",
                    Id = Guid.Parse("77d00bab-8c6a-41ad-ac60-717ec58126a0")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "có dây",
                     Id = Guid.Parse("188b5405-0239-413e-bd31-a43e4d63c797"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "có dây",
                     Id = Guid.Parse("512eba59-d1b1-4a30-bd8d-6ecb99ce35b4"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("cb249245-d8e7-4ba0-a1b5-3f2d67a614eb"),
            Name = "Tai nghe chụp tai",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "tai nghe chụp tai",
                    Id = Guid.Parse("b58650de-19b1-4fbd-a3ea-0271c055a3e2")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "chụp tai",
                     Id = Guid.Parse("6afb6ee6-bea9-4175-9070-1c686d8c035e"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "chụp tai",
                     Id = Guid.Parse("8136fb57-021f-4559-8d6f-f0f76cd61ad9"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("ffe5d43a-a36b-4dbf-b96c-ee24a70439d4"),
            Name = "Tai nghe gaming",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "tai nghe gaming",
                    Id = Guid.Parse("8b4ab273-bb3b-450c-a420-14540361ddde")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "gaming",
                     Id = Guid.Parse("eeb39bac-88e8-4d77-ba81-40a430ede85f"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "gaming",
                     Id = Guid.Parse("67d94142-5efd-4cb8-9b3e-6e45487697f1"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("29b00f13-fc0e-468e-9eb0-8fb64c001024"),
            Name = "Loa Bluetooth",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "loa bluetooth",
                    Id = Guid.Parse("3219559e-e529-4221-8394-8e592c564315")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "bluetooth",
                     Id = Guid.Parse("2edd5e7d-b53b-4975-9f81-02f1eeb53337"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "bluetooth",
                     Id = Guid.Parse("439cd7bc-564b-471e-bc0e-9964535ea435"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("a4f128eb-8335-4f91-aa96-4fab3172d5ba"),
            Name = "Loa kéo",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "loa kéo",
                    Id = Guid.Parse("b530a6fb-0ef2-4a87-8e5a-cf7796d54bd4")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "kéo",
                     Id = Guid.Parse("7b023cce-3b07-4a3a-b89a-523ae627963a"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "kéo",
                     Id = Guid.Parse("90fb873d-3efd-47e6-a5d7-702b0203eeeb"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("2ea9e59d-ad12-46ee-999f-03ab21a4d7b8"),
            Name = "Loa karaoke",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "loa karaoke",
                    Id = Guid.Parse("0b7c8867-d270-48f2-b393-f11c614e56f9")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "karaoke",
                     Id = Guid.Parse("f4c77e4a-d71f-4f26-9953-49df79e9834c"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "karaoke",
                     Id = Guid.Parse("8c708bda-1223-4b4c-b5eb-9698fd87925a"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("f23e695e-83cf-4b72-97aa-1709e03ba5b1"),
            Name = "Loa điện",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "loa điện",
                    Id = Guid.Parse("6077d4ef-e316-4d07-b464-ce142d20245f")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "loa điện",
                     Id = Guid.Parse("b777e392-acf8-45b8-9792-e6d760b3fa1a"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "loa điện",
                     Id = Guid.Parse("48cfc0b3-7495-4475-afd4-8e76bed609ef"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("064b9aac-a425-4f91-8eaa-02c2dd2d0597"),
            Name = "Loa vi tính",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "loa vi tính",
                    Id = Guid.Parse("56dc970a-ae54-4d4b-ac5f-423861ea7b5a")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "loa vi tính",
                     Id = Guid.Parse("1a9d86b0-c360-4a5d-83d4-a7df6c5f2512"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "loa vi tính",
                     Id = Guid.Parse("e1942de3-eee7-4a4f-a1b0-157ce7fd4660"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("9bba0f5a-b1d9-4619-a312-ed1fee9c0f2b"),
            Name = "Loa thanh",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "loa thanh",
                    Id = Guid.Parse("9f54ffd0-d560-48b5-b482-394dcd998fa2")
                }
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Name,
                     Contains = "loa thanh",
                     Id = Guid.Parse("753847ad-0dbc-4607-ba2b-88784c78229e"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "loa thanh",
                     Id = Guid.Parse("4f978bd4-b943-49b6-9446-80cba51cf009"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        }
                     ]
                 },
            ]
        },
        new NaturalLanguageKeywordGroup {
            Id = Guid.Parse("029f8544-ca97-4139-866a-3da8afba058f"),
            Name = "Pin trâu",
            CreatedAt = now,
            UpdatedAt = now,
            Status = NaturalLanguageKeywordGroupStatus.Active,
            NaturalLanguageKeywords = [
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "pin trâu",
                    Id = Guid.Parse("6940602c-9d94-45ae-9425-cf4a90e0690b")
                },
                new NaturalLanguageKeyword {
                    Status = NaturalLanguageKeywordStatus.Active,
                    CreatedAt = now,
                    UpdatedAt = now,
                    Keyword = "pin khoẻ",
                    Id = Guid.Parse("8d769ec5-142e-40ca-ba99-ce691cc1bd23")
                },
            ],
            Criteria = [
                 new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "pin trâu",
                     Id = Guid.Parse("6ef188c5-e603-43b3-801e-13f99760dd4a"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 },
                new Criteria {
                     CreatedAt = now,
                     UpdatedAt = now,
                     Type = CriteriaType.Description,
                     Contains = "pin khoẻ",
                     Id = Guid.Parse("0cdfaf90-e9cc-4e04-8a94-1de8816ee04d"),
                     CriteriaCategories = [
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("ea4183e8-5a94-401c-865d-e000b5d2b72d")
                        },
                        new CriteriaCategory {
                            CategoryId = Guid.Parse("458d7752-e45e-444a-adf9-f7201c07acd1")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("9f6ac480-e673-49ec-9bc0-7566cca80b86")
                        },
                         new CriteriaCategory {
                            CategoryId = Guid.Parse("bb65a310-e28e-4226-a562-0b6ea27f4faa")
                        },
                     ]
                 }
            ]
        },
    ];
}
