﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApi.Data.Entities;

namespace WebApi.Data.Configurations;

public class FavoriteGadgetConfiguration : IEntityTypeConfiguration<FavoriteGadget>
{
    public void Configure(EntityTypeBuilder<FavoriteGadget> builder)
    {
        builder.HasKey(x => new { x.CustomerId, x.GadgetId });
    }
}
