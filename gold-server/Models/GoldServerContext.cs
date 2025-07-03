using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace gold_server.Models;

public partial class GoldServerContext : DbContext
{
    public GoldServerContext()
    {
    }

    public GoldServerContext(DbContextOptions<GoldServerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<BANNER> BANNERs { get; set; }

    public virtual DbSet<CART_DETAIL> CART_DETAILs { get; set; }

    public virtual DbSet<CATEGORY> CATEGORIEs { get; set; }

    public virtual DbSet<IMAGE> IMAGEs { get; set; }

    public virtual DbSet<PRODUCT_IMAGE> PRODUCT_IMAGEs { get; set; }

    public virtual DbSet<BANNER_IMAGE> BANNER_IMAGEs { get; set; }

    public virtual DbSet<CATEGORY_IMAGE> CATEGORY_IMAGEs { get; set; }

    public virtual DbSet<INVOICE> INVOICEs { get; set; }

    public virtual DbSet<INVOICE_DETAIL> INVOICE_DETAILs { get; set; }

    public virtual DbSet<PAYMENT_METHOD> PAYMENT_METHODs { get; set; }

    public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }

    public virtual DbSet<PRODUCT> PRODUCTs { get; set; }

    public virtual DbSet<ROLE> ROLEs { get; set; }

    public virtual DbSet<STATUS> STATUSs { get; set; }

    public virtual DbSet<USER> USERs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
                if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder
                .UseSqlServer("Server=localhost,1433;Database=DB_YENSAOGOLD;User Id=sa;Password=hniVeL@0355;TrustServerCertificate=True;")
                .LogTo(Console.WriteLine, LogLevel.Information); // 👈 Ghi log ra console
        }
    }
// #warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
//     => optionsBuilder.UseSqlServer("");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BANNER>(entity =>
        {
            entity.ToTable("BANNERS");

            entity.HasKey(e => e.ID_Banner).HasName("PK_BANNERS_TBL");

            entity.Property(e => e.ID_Banner).ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.CreateDate)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(e => e.CreateBy)
                .IsRequired();

            entity.HasOne(b => b.CreateByNavigation)
                .WithMany(u => u.CREATED_BANNERs)
                .HasForeignKey(b => b.CreateBy)
                .HasPrincipalKey(u => u.ID_User)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BANNERS_CreateBy");
        });

        modelBuilder.Entity<CART_DETAIL>(entity =>
        {
            entity.ToTable("CART_DETAIL");

            entity.HasKey(e => e.ID_CartDetail).HasName("PK_CART_DETAIL_TBL");

            entity.Property(e => e.ID_CartDetail).ValueGeneratedOnAdd();

            entity.Property(e => e.ID_User)
                .IsRequired();

            entity.Property(e => e.ID_Product)
                .IsRequired();
            
            entity.Property(e => e.Count)
                .IsRequired();

            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime");

            entity.Property(e => e.CreateDate)
                .IsRequired()
                .HasColumnType("datetime");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.CART_DETAILs)
                .HasForeignKey(d => d.ID_User)
                .HasPrincipalKey(u => u.ID_User)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CART_DETAIL_ID_User");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.CART_DETAILs)
                .HasForeignKey(d => d.ID_Product)
                .HasPrincipalKey(p => p.ID_Product)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CART_DETAIL_ID_Product");
        });

        modelBuilder.Entity<CATEGORY>(entity =>
        {
            entity.ToTable("CATEGORIES");

            entity.HasKey(e => e.ID_Category).HasName("PK_CATEGORIES_TBL");

            entity.Property(e => e.ID_Category).ValueGeneratedOnAdd();

            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(e => e.CreateDate)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(e => e.UpdateDate)
                .HasColumnType("datetime");

            entity.Property(e => e.CreateBy)
                .IsRequired();
            
            entity.HasOne(c => c.CreateByNavigation).WithMany(u => u.CREATED_CATEGORIEs)
                .HasForeignKey(c => c.CreateBy)
                .HasPrincipalKey(u => u.ID_User)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CATEGORIES_CreateBy");
            entity.HasOne(c => c.UpdateByNavigation).WithMany(u => u.UPDATED_CATEGORIEs)
                .HasForeignKey(c => c.UpdateBy)
                .HasPrincipalKey(u => u.ID_User)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CATEGORIES_UpdateBy");
        });

        modelBuilder.Entity<IMAGE>(entity =>
        {
            entity.ToTable("IMAGES");

            entity.HasKey(e => e.ID_Image).HasName("PK_IMAGES_TBL");

            entity.Property(e => e.ID_Image).ValueGeneratedOnAdd();

            entity.Property(e => e.URL).HasMaxLength(1000);
        });

        modelBuilder.Entity<PRODUCT_IMAGE>(entity =>
        {
            entity.ToTable("PRODUCT_IMAGE");

            entity.HasKey(e => new { e.ID_Image, e.ID_Product }).HasName("PK_PRODUCT_IMAGE_TBL");

            entity.Property(e => e.SortOrder).IsRequired();

            entity.Property(e => e.AltText).HasMaxLength(100);

            entity.HasOne(e => e.ImageNavigation)
                .WithMany(i => i.PRODUCT_IMAGEs)
                .HasForeignKey(e => e.ID_Image)
                .HasPrincipalKey(i => i.ID_Image)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PRODUCT_IMAGE_ID_Image");

            entity.HasOne(e => e.ProductNavigation)
                .WithMany(p => p.PRODUCT_IMAGEs)
                .HasForeignKey(e => e.ID_Product)
                .HasPrincipalKey(p => p.ID_Product)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_PRODUCT_IMAGE_ID_Product");
        });

        modelBuilder.Entity<BANNER_IMAGE>(entity =>
        {
            entity.ToTable("BANNER_IMAGE");

            entity.HasKey(e => new { e.ID_Image, e.ID_Banner }).HasName("PK_BANNER_IMAGE_TBL");

            entity.Property(e => e.SortOrder).IsRequired();

            entity.Property(e => e.AltText).HasMaxLength(100);

            entity.HasOne(e => e.ImageNavigation)
                .WithMany(i => i.BANNER_IMAGEs)
                .HasForeignKey(e => e.ID_Image)
                .HasPrincipalKey(i => i.ID_Image)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BANNER_IMAGE_ID_Image");

            entity.HasOne(e => e.BannerNavigation)
                .WithMany(b => b.BANNER_IMAGEs)
                .HasForeignKey(e => e.ID_Banner)
                .HasPrincipalKey(b => b.ID_Banner)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_BANNER_IMAGE_ID_Banner");
        });

        modelBuilder.Entity<CATEGORY_IMAGE>(entity =>
        {
            entity.ToTable("CATEGORY_IMAGE");

            entity.HasKey(e => new { e.ID_Image, e.ID_Category }).HasName("PK_CATEGORY_IMAGE_TBL");

            entity.Property(e => e.SortOrder).IsRequired();

            entity.Property(e => e.AltText).HasMaxLength(100);

            entity.HasOne(e => e.ImageNavigation)
                .WithMany(i => i.CATEGORY_IMAGEs)
                .HasForeignKey(e => e.ID_Image)
                .HasPrincipalKey(i => i.ID_Image)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CATEGORY_IMAGE_ID_Image");

            entity.HasOne(e => e.CategoryNavigation)
                .WithMany(c => c.CATEGORY_IMAGEs)
                .HasForeignKey(e => e.ID_Category)
                .HasPrincipalKey(c => c.ID_Category)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_CATEGORY_IMAGE_ID_Category");
        });

        modelBuilder.Entity<INVOICE>(entity =>
        {
            entity.ToTable("INVOICES");

            entity.HasKey(e => e.ID_Invoice).HasName("PK_INVOICES_TBL");

            entity.Property(e => e.ID_Invoice).ValueGeneratedOnAdd();

            entity.Property(e => e.ID_Payment)
                .IsRequired();

            entity.Property(e => e.Total)
                .IsRequired()
                .HasDefaultValue(0)
                .HasColumnType("decimal(19, 0)");

            entity.Property(e => e.CreateDate)
                .IsRequired()
                .HasColumnType("datetime");

            entity.Property(e => e.ID_Status)
                .IsRequired();

            entity.Property(e => e.FullName_Receiver)
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(true);

            entity.Property(e => e.Phone_Receiver)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(true);

            entity.Property(e => e.Note)
                .HasMaxLength(255)
                .IsUnicode(true);

            entity.HasOne(d => d.PaymentNavigation).WithMany(p => p.INVOICEs)
                .HasForeignKey(d => d.ID_Payment)
                .HasPrincipalKey(p => p.ID_Payment)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_INVOICES_ID_Payment");

            entity.HasOne(d => d.StatusNavigation).WithMany(p => p.INVOICEs)
                .HasForeignKey(d => d.ID_Status)
                .HasPrincipalKey(s => s.ID_Status)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_INVOICES_ID_Status");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.INVOICEs)
                .HasForeignKey(d => d.ID_User)
                .HasPrincipalKey(u => u.ID_User)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_INVOICES_ID_User");
        });

        modelBuilder.Entity<INVOICE_DETAIL>(entity =>
        {
            entity.ToTable("INVOICE_DETAIL");

            entity.HasKey(e => e.ID_InvoiceDetail).HasName("PK_INVOICE_DETAIL_TBL");

            entity.Property(e => e.ID_InvoiceDetail).ValueGeneratedOnAdd();

            entity.Property(e => e.ID_Invoice)
                .IsRequired();

            entity.Property(e => e.ID_Product)
                .IsRequired();

            entity.Property(e => e.Count)
                .IsRequired();

            entity.Property(e => e.Price)
                .HasColumnType("decimal(19, 0)");

            entity.HasOne(d => d.InvoiceNavigation).WithMany(p => p.INVOICE_DETAILs)
                .HasForeignKey(d => d.ID_Invoice)
                .HasPrincipalKey(i => i.ID_Invoice)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_INVOICE_DETAIL_ID_Invoice");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.INVOICE_DETAILs)
                .HasForeignKey(d => d.ID_Product)
                .HasPrincipalKey(p => p.ID_Product)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_INVOICE_DETAIL_ID_Product");
        });

        modelBuilder.Entity<PAYMENT_METHOD>(entity =>
        {
            entity.ToTable("PAYMENT_METHODS");

            entity.HasKey(e => e.ID_Payment).HasName("PK_PAYMENT_METHODS_TBL");

            entity.Property(e => e.ID_Payment).ValueGeneratedOnAdd();

            entity.Property(e => e.PaymentName)
                .IsRequired()
                .HasMaxLength(100);
        });

        modelBuilder.Entity<PERMISSION>(entity =>
        {
            entity.ToTable("PERMISSIONS");

            entity.HasKey(e => e.ID_Permission).HasName("PK_PERMISSIONS_TBL");

            entity.Property(e => e.ID_Permission).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<PRODUCT>(entity =>
        {
            entity.ToTable("PRODUCTS");

            entity.HasKey(e => e.ID_Product).HasName("PK_PRODUCTS_TBL");

            entity.Property(e => e.ID_Product).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.Property(e => e.Price).IsRequired().HasColumnType("decimal(19, 0)");

            entity.Property(e => e.CreateBy)
                .IsRequired();

            entity.Property(e => e.CreateDate).IsRequired().HasColumnType("datetime");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.Property(e => e.InStock).IsRequired().HasDefaultValue(0);

            entity.Property(e => e.ID_Status).IsRequired();

            entity.Property(e => e.SoldQuantity).IsRequired().HasDefaultValue(0);

            entity.HasOne(u => u.CreateByNavigation)
            .WithMany(p => p.CREATED_PRODUCTs)
            .HasForeignKey(u => u.CreateBy)
            .HasPrincipalKey(u => u.ID_User)
            .HasConstraintName("FK_PRODUCTS_CreateBy");

            entity.HasOne(c => c.CategoryNavigation)
                .WithMany(p => p.PRODUCTs)
                .HasForeignKey(c => c.ID_Category)
                .HasPrincipalKey(c => c.ID_Category)
                .HasConstraintName("FK_PRODUCTS_ID_Category");

            entity.HasOne(d => d.StatusNavigation)
                .WithMany(p => p.PRODUCTs)
                .HasForeignKey(d => d.ID_Status)
                .HasPrincipalKey(s => s.ID_Status)
                .HasConstraintName("FK_PRODUCTS_ID_Status");

            entity.HasOne(u => u.UpdateByNavigation)
                .WithMany(p => p.UPDATED_PRODUCTs)
                .HasForeignKey(u => u.UpdateBy)
                .HasPrincipalKey(u => u.ID_User)
                .HasConstraintName("FK_PRODUCTS_UpdateBy");
        });

        modelBuilder.Entity<ROLE>(entity =>
        {
            entity.ToTable("ROLES");

            entity.HasKey(e => e.ID_Role).HasName("PK_ROLES_TBL");

            entity.Property(e => e.ID_Role).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.Property(e => e.Description).HasMaxLength(255);

            entity.HasMany(d => d.PERMISSIONs)
                .WithMany(p => p.ROLEs)
                .UsingEntity<Dictionary<string, object>>(
                    "ROLE_PERMISSION",
                    r => r.HasOne<PERMISSION>().WithMany()
                        .HasForeignKey("ID_Permission")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_RolePermission_ID_Permission"),
                    l => l.HasOne<ROLE>().WithMany()
                        .HasForeignKey("ID_Role")
                        .OnDelete(DeleteBehavior.Restrict)
                        .HasConstraintName("FK_RolePermission_ID_Role"),
                    j =>
                    {
                        j.HasKey("ID_Role", "ID_Permission");
                        j.ToTable("ROLE_PERMISSION");
                    });
        });

        modelBuilder.Entity<STATUS>(entity =>
        {
            entity.ToTable("STATUS");

            entity.HasKey(e => e.ID_Status).HasName("PK_STATUS_TBL");

            entity.Property(e => e.ID_Status).ValueGeneratedOnAdd();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);

            entity.Property(e => e.Description).HasMaxLength(255);
        });

        modelBuilder.Entity<USER>(entity =>
        {
            entity.ToTable("USERS");

            entity.HasKey(e => e.ID_User).HasName("PK_USERS_TBL");
            entity.HasIndex(e => e.Email, "UQ_EMAIL_USER").IsUnique();
            entity.HasIndex(e => e.Phone, "UQ_PHONE_USER").IsUnique();

            entity.Property(e => e.ID_User).ValueGeneratedOnAdd();
            
            entity.Property(e => e.Email).IsRequired().HasMaxLength(255);

            entity.Property(e => e.FullName).IsRequired().HasMaxLength(100).IsUnicode(true);

            entity.Property(e => e.Password_Hash).IsRequired().HasMaxLength(255);

            entity.Property(e => e.Phone)
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            entity.Property(e => e.ID_Role).IsRequired();

            entity.Property(e => e.CreateDate).IsRequired().HasColumnType("datetime");

            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.RoleNavigation)
                .WithMany(p => p.USERs)
                .HasForeignKey(d => d.ID_Role)
                .HasPrincipalKey(r => r.ID_Role)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Users_ID_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
