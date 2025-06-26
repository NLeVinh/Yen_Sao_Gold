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

    public virtual DbSet<CART> CARTs { get; set; }

    public virtual DbSet<CART_DETAIL> CART_DETAILs { get; set; }

    public virtual DbSet<CATEGORy> CATEGORIEs { get; set; }

    public virtual DbSet<IMAGE> IMAGEs { get; set; }

    public virtual DbSet<INVOICE> INVOICEs { get; set; }

    public virtual DbSet<INVOICE_DETAIL> INVOICE_DETAILs { get; set; }

    public virtual DbSet<PAYMENT_METHOD> PAYMENT_METHODs { get; set; }

    public virtual DbSet<PERMISSION> PERMISSIONs { get; set; }

    public virtual DbSet<PRODUCT> PRODUCTs { get; set; }

    public virtual DbSet<REF_IMAGE> REF_IMAGEs { get; set; }

    public virtual DbSet<ROLE> ROLEs { get; set; }

    public virtual DbSet<STATUS> STATUSes { get; set; }

    public virtual DbSet<USER> USERs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=DB_YENSAOGOLD;User Id=sa;Password=Caovupro/6262115;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BANNER>(entity =>
        {
            entity.HasKey(e => e.ID_Banner).HasName("PK__BANNERS__C15351B2BB43C305");

            entity.ToTable("BANNERS");

            entity.Property(e => e.ID_Banner)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(255);
        });

        modelBuilder.Entity<CART>(entity =>
        {
            entity.HasKey(e => e.ID_Cart).HasName("PK__CARTS__72140ECF39245656");

            entity.ToTable("CARTS");

            entity.Property(e => e.ID_User)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<CART_DETAIL>(entity =>
        {
            entity.HasKey(e => e.ID_CartDetail).HasName("PK__CART_DET__19B4E08237204EC0");

            entity.ToTable("CART_DETAIL");

            entity.Property(e => e.ID_Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ID_CartNavigation).WithMany(p => p.CART_DETAILs)
                .HasForeignKey(d => d.ID_Cart)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_ID_Cart");

            entity.HasOne(d => d.ID_ProductNavigation).WithMany(p => p.CART_DETAILs)
                .HasForeignKey(d => d.ID_Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_CartDetail_ID_Product");
        });

        modelBuilder.Entity<CATEGORy>(entity =>
        {
            entity.HasKey(e => e.ID_Category).HasName("PK__CATEGORI__6DB3A68A72A07908");

            entity.ToTable("CATEGORIES");

            entity.Property(e => e.ID_Category)
                .HasMaxLength(20)
                .IsUnicode(false);
            // .HasComputedColumnSql("(CONVERT([varchar](20),'C'+CONVERT([varchar],[IndexAutoCategory])))", true);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            // entity.Property(e => e.IndexAutoCategory).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<IMAGE>(entity =>
        {
            entity.HasKey(e => e.ID_Image).HasName("PK__IMAGES__31E45A2A51B7F0F7");

            entity.ToTable("IMAGES");

            entity.Property(e => e.URL).HasMaxLength(1000);
        });

        modelBuilder.Entity<INVOICE>(entity =>
        {
            entity.HasKey(e => e.ID_Invoice).HasName("PK__INVOICES__0540CA609BF2D64C");

            entity.ToTable("INVOICES");

            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ID_User)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Total).HasColumnType("decimal(19, 0)");

            entity.HasOne(d => d.ID_PaymentNavigation).WithMany(p => p.INVOICEs)
                .HasForeignKey(d => d.ID_Payment)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_INVOICES_ID_Payment");

            entity.HasOne(d => d.ID_StatusNavigation).WithMany(p => p.INVOICEs)
                .HasForeignKey(d => d.ID_Status)
                .HasConstraintName("FK_INVOICES_ID_Status");

        });

        modelBuilder.Entity<INVOICE_DETAIL>(entity =>
        {
            entity.HasKey(e => e.ID_InvoiceDetail).HasName("PK__INVOICE___6879A3C0127B5BFC");

            entity.ToTable("INVOICE_DETAIL");

            entity.Property(e => e.ID_Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(19, 0)");

            entity.HasOne(d => d.ID_InvoiceNavigation).WithMany(p => p.INVOICE_DETAILs)
                .HasForeignKey(d => d.ID_Invoice)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_ID_Invoice");

            entity.HasOne(d => d.ID_ProductNavigation).WithMany(p => p.INVOICE_DETAILs)
                .HasForeignKey(d => d.ID_Product)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_InvoiceDetail_ID_Product");
        });

        modelBuilder.Entity<PAYMENT_METHOD>(entity =>
        {
            entity.HasKey(e => e.ID_Payment).HasName("PK__PAYMENT___C2118ADE1E9E7A8B");

            entity.ToTable("PAYMENT_METHODS");

            entity.Property(e => e.PaymentName).HasMaxLength(100);
        });

        modelBuilder.Entity<PERMISSION>(entity =>
        {
            entity.HasKey(e => e.ID_Permission).HasName("PK__PERMISSI__D832E15CCBD850F9");

            entity.ToTable("PERMISSIONS");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<PRODUCT>(entity =>
        {
            entity.HasKey(e => e.ID_Product).HasName("PK__PRODUCTS__522DE496708DDAF3");

            entity.ToTable("PRODUCTS");

            entity.Property(e => e.ID_Product)
                .HasMaxLength(20)
                .IsUnicode(false);
            // .HasComputedColumnSql("(CONVERT([varchar](20),'P'+CONVERT([varchar],[IndexAutoProduct])))", true);
            entity.Property(e => e.CreateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.ID_Category)
                .HasMaxLength(20)
                .IsUnicode(false);
            // entity.Property(e => e.IndexAutoProduct).ValueGeneratedOnAdd();
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(19, 0)");
            entity.Property(e => e.UpdateBy)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ID_CategoryNavigation).WithMany(p => p.PRODUCTs)
                .HasForeignKey(d => d.ID_Category)
                .HasConstraintName("FK_Products_Categories");

            entity.HasOne(d => d.ID_StatusNavigation).WithMany(p => p.PRODUCTs)
                .HasForeignKey(d => d.ID_Status)
                .HasConstraintName("FK_Products_Status");

        });

        modelBuilder.Entity<REF_IMAGE>(entity =>
        {
            entity.HasKey(e => e.ID_Ref).HasName("PK__REF_IMAG__202AE27BAC34A6ED");

            entity.ToTable("REF_IMAGES", tb => tb.HasTrigger("trg_Validate_RefImages"));

            entity.Property(e => e.ID_ImageRef)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RefType).HasMaxLength(50);

            entity.HasOne(d => d.ID_ImageNavigation).WithMany(p => p.REF_IMAGEs)
                .HasForeignKey(d => d.ID_Image)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_RefImages_Images");
        });

        modelBuilder.Entity<ROLE>(entity =>
        {
            entity.HasKey(e => e.ID_Roles).HasName("PK__ROLES__30F62993308B92A1");

            entity.ToTable("ROLES");

            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasMany(d => d.ID_Permissions).WithMany(p => p.ID_Roles)
                .UsingEntity<Dictionary<string, object>>(
                    "ROLE_PERMISSION",
                    r => r.HasOne<PERMISSION>().WithMany()
                        .HasForeignKey("ID_Permission")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolePermission_Permission"),
                    l => l.HasOne<ROLE>().WithMany()
                        .HasForeignKey("ID_Roles")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_RolePermission_Role"),
                    j =>
                    {
                        j.HasKey("ID_Roles", "ID_Permission");
                        j.ToTable("ROLE_PERMISSION");
                    });
        });

        modelBuilder.Entity<STATUS>(entity =>
        {
            entity.HasKey(e => e.ID_Status).HasName("PK__STATUS__5AC2A7346ED38746");

            entity.ToTable("STATUS");

            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<USER>(entity =>
        {
            entity.HasKey(e => e.IndexAutoUser).HasName("PK__USERS__IndexAutoUser");

            entity.ToTable("USERS");

            entity.HasIndex(e => e.Phone, "UQ__USERS__5C7E359ED1A4ED9A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__USERS__A9D105347F233481").IsUnique();

            entity.Property(e => e.ID_User)
                .HasMaxLength(20)
                .IsUnicode(false)
            .HasComputedColumnSql("(CONVERT([varchar](20),'U'+CONVERT([varchar],[IndexAutoUser])))", true);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IndexAutoUser).ValueGeneratedOnAdd();
            entity.Property(e => e.Password_Hash).HasMaxLength(255);
            entity.Property(e => e.Phone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");

            entity.HasOne(d => d.ID_RoleNavigation).WithMany(p => p.USERs)
                .HasForeignKey(d => d.ID_Role)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_ID_Role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
