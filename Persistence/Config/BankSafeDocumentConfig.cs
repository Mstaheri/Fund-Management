﻿using Domain.Entity;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Convertors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Config
{
    public class BankSafeDocumentConfig : IEntityTypeConfiguration<BankSafeDocument>
    {
        public void Configure(EntityTypeBuilder<BankSafeDocument> builder)
        {
            builder.HasQueryFilter(m => EF.Property<bool>(m, "IsRemoved") == false);

            builder.HasKey(p => p.Code);

            builder.Property(p => p.NameBankSafe)
                .HasConversion<NameConverter>()
                .HasMaxLength(50)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(p => p.AccountNumber)
                .HasConversion<AccountNumberConverter>()
                .HasMaxLength(16)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(p => p.RegistrationDate)
                .HasConversion<PersianDateConverter>()
                .HasMaxLength(10)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(p => p.DueDate)
                .HasConversion<PersianDateConverter>()
                .HasMaxLength(10)
                .IsUnicode(true)
                .IsRequired(true);

            builder.Property(p => p.Deposit)
                .HasConversion<MoneyConverter>()
                .HasMaxLength(12)
                .IsRequired(true);

            builder.Property(p => p.Withdrawal)
                .HasConversion<MoneyConverter>()
                .HasMaxLength(12)
                .IsRequired(true);
        }
    }
}
