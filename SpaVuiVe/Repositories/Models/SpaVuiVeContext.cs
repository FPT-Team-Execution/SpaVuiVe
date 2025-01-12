using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Repositories.Models;

public partial class SpaVuiVeContext : DbContext
{
    public SpaVuiVeContext()
    {
    }

    public SpaVuiVeContext(DbContextOptions<SpaVuiVeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<CustomerProfile> CustomerProfiles { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<PromotionUsage> PromotionUsages { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<RoutineProduct> RoutineProducts { get; set; }

    public virtual DbSet<SkinCareRoutine> SkinCareRoutines { get; set; }

    public virtual DbSet<SkinTest> SkinTests { get; set; }

    public virtual DbSet<SkinType> SkinTypes { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=.;Database=SpaVuiVe;User ID=sa;Password=12345;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0BB93C5C1F");

            entity.ToTable("Category", tb => tb.HasTrigger("TR_Category_UpdatedAt"));

            entity.HasIndex(e => e.ParentCategoryId, "IX_Category_ParentCategoryId");

            entity.Property(e => e.CategoryId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.ParentCategoryId).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK_Category_ParentCategory");
        });

        modelBuilder.Entity<CustomerProfile>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64D8C61C6B5D");

            entity.ToTable("CustomerProfile");

            entity.HasIndex(e => e.Email, "IX_CustomerProfile_Email");

            entity.HasIndex(e => e.SkinTypeId, "IX_CustomerProfile_SkinTypeId");

            entity.HasIndex(e => e.Email, "UQ_CustomerProfile_Email").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ_CustomerProfile_Phone").IsUnique();

            entity.Property(e => e.CustomerId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.IsSubscribed).HasDefaultValue(false);
            entity.Property(e => e.LoyaltyPoints).HasDefaultValue(0);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.PreferredPaymentMethod).HasMaxLength(50);
            entity.Property(e => e.SkinTypeId).HasMaxLength(100);
            entity.Property(e => e.UserId).HasMaxLength(100);

            entity.HasOne(d => d.SkinType).WithMany(p => p.CustomerProfiles)
                .HasForeignKey(d => d.SkinTypeId)
                .HasConstraintName("FK_CustomerProfile_SkinType");

            entity.HasOne(d => d.User).WithMany(p => p.CustomerProfiles)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK_CustomerProfile_User");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BCFE361C187");

            entity.ToTable("Order", tb => tb.HasTrigger("TR_Order_UpdatedAt"));

            entity.HasIndex(e => e.CustomerId, "IX_Order_CustomerId");

            entity.HasIndex(e => e.OrderDate, "IX_Order_OrderDate");

            entity.HasIndex(e => e.Status, "IX_Order_Status");

            entity.Property(e => e.OrderId).HasMaxLength(100);
            entity.Property(e => e.CustomerId).HasMaxLength(100);
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.ShippingAddress).HasMaxLength(500);
            entity.Property(e => e.ShippingFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TrackingNumber).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Order_CustomerProfile");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D36CF5EA4495");

            entity.ToTable("OrderDetail", tb => tb.HasTrigger("TR_OrderDetail_UpdatedAt"));

            entity.HasIndex(e => e.OrderId, "IX_OrderDetail_OrderId");

            entity.HasIndex(e => e.ProductId, "IX_OrderDetail_ProductId");

            entity.Property(e => e.OrderDetailId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Discount)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsReviewed).HasDefaultValue(false);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OrderId).HasMaxLength(100);
            entity.Property(e => e.ProductId).HasMaxLength(100);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_OrderDetail_Order");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_OrderDetail_Product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Product__B40CC6CDE986D00B");

            entity.ToTable("Product", tb => tb.HasTrigger("TR_Product_UpdatedAt"));

            entity.HasIndex(e => e.CategoryId, "IX_Product_CategoryId");

            entity.HasIndex(e => e.IsActive, "IX_Product_IsActive");

            entity.HasIndex(e => e.Name, "IX_Product_Name");

            entity.Property(e => e.ProductId).HasMaxLength(100);
            entity.Property(e => e.Brand).HasMaxLength(100);
            entity.Property(e => e.CategoryId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.Ingredients).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockQuantity).HasDefaultValue(0);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK_Product_Category");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Promotio__52C42FCFE92794C2");

            entity.ToTable("Promotion");

            entity.HasIndex(e => e.Code, "IX_Promotion_Code");

            entity.HasIndex(e => e.IsActive, "IX_Promotion_IsActive");

            entity.HasIndex(e => e.Code, "UQ_Promotion_Code").IsUnique();

            entity.Property(e => e.PromotionId).HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EndDate).HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MinimumPurchase).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.StartDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PromotionUsage>(entity =>
        {
            entity.HasKey(e => e.UsageId).HasName("PK__Promotio__29B19720AA418F50");

            entity.ToTable("PromotionUsage");

            entity.HasIndex(e => e.OrderId, "IX_PromotionUsage_OrderId");

            entity.HasIndex(e => e.PromotionId, "IX_PromotionUsage_PromotionId");

            entity.Property(e => e.UsageId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsValid).HasDefaultValue(true);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OrderId).HasMaxLength(100);
            entity.Property(e => e.PromotionId).HasMaxLength(100);
            entity.Property(e => e.UsedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Order).WithMany(p => p.PromotionUsages)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK_PromotionUsage_Order");

            entity.HasOne(d => d.Promotion).WithMany(p => p.PromotionUsages)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK_PromotionUsage_Promotion");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Review__74BC79CE4D4438DB");

            entity.ToTable("Review");

            entity.HasIndex(e => e.CustomerId, "IX_Review_CustomerId");

            entity.HasIndex(e => e.IsVisible, "IX_Review_IsVisible");

            entity.HasIndex(e => e.ProductId, "IX_Review_ProductId");

            entity.Property(e => e.ReviewId).HasMaxLength(100);
            entity.Property(e => e.Comment).HasMaxLength(1000);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.CustomerId).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsVerified).HasDefaultValue(false);
            entity.Property(e => e.IsVisible).HasDefaultValue(true);
            entity.Property(e => e.ProductId).HasMaxLength(100);
            entity.Property(e => e.PurchaseDate).HasColumnType("datetime");

            entity.HasOne(d => d.Customer).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.CustomerId)
                .HasConstraintName("FK_Review_CustomerProfile");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_Review_Product");
        });

        modelBuilder.Entity<RoutineProduct>(entity =>
        {
            entity.HasKey(e => e.RoutineProductId).HasName("PK__RoutineP__901DE1BCFAE2146A");

            entity.ToTable("RoutineProduct");

            entity.Property(e => e.RoutineProductId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Frequency).HasMaxLength(50);
            entity.Property(e => e.IsRequired).HasDefaultValue(true);
            entity.Property(e => e.ProductId).HasMaxLength(100);
            entity.Property(e => e.RoutineId).HasMaxLength(100);
            entity.Property(e => e.Step).HasMaxLength(100);
            entity.Property(e => e.TimeOfDay).HasMaxLength(50);
            entity.Property(e => e.UsageInstructions).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.RoutineProducts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_RoutineProduct_Product");

            entity.HasOne(d => d.Routine).WithMany(p => p.RoutineProducts)
                .HasForeignKey(d => d.RoutineId)
                .HasConstraintName("FK_RoutineProduct_Routine");
        });

        modelBuilder.Entity<SkinCareRoutine>(entity =>
        {
            entity.HasKey(e => e.RoutineId).HasName("PK__SkinCare__A6E3E4FAE995F9B4");

            entity.ToTable("SkinCareRoutine", tb => tb.HasTrigger("TR_SkinCareRoutine_UpdatedAt"));

            entity.HasIndex(e => e.SkinTypeId, "IX_SkinCareRoutine_SkinTypeId");

            entity.Property(e => e.RoutineId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Duration).HasMaxLength(50);
            entity.Property(e => e.EveningSteps).HasMaxLength(1000);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MorningSteps).HasMaxLength(1000);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.SkinTypeId).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.WeeklySteps).HasMaxLength(1000);

            entity.HasOne(d => d.SkinType).WithMany(p => p.SkinCareRoutines)
                .HasForeignKey(d => d.SkinTypeId)
                .HasConstraintName("FK_SkinCareRoutine_SkinType");
        });

        modelBuilder.Entity<SkinTest>(entity =>
        {
            entity.HasKey(e => e.TestId).HasName("PK__SkinTest__8CC33160807FDF7D");

            entity.ToTable("SkinTest");

            entity.Property(e => e.TestId).HasMaxLength(100);
            entity.Property(e => e.CorrectSkinTypeId).HasMaxLength(100);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.OptionA).HasMaxLength(200);
            entity.Property(e => e.OptionB).HasMaxLength(200);
            entity.Property(e => e.OptionC).HasMaxLength(200);
            entity.Property(e => e.OptionD).HasMaxLength(200);
            entity.Property(e => e.Question).HasMaxLength(500);

            entity.HasOne(d => d.CorrectSkinType).WithMany(p => p.SkinTests)
                .HasForeignKey(d => d.CorrectSkinTypeId)
                .HasConstraintName("FK_SkinTest_SkinType");
        });

        modelBuilder.Entity<SkinType>(entity =>
        {
            entity.HasKey(e => e.SkinTypeId).HasName("PK__SkinType__D5D2960B0EC5EF50");

            entity.ToTable("SkinType");

            entity.HasIndex(e => e.Name, "UQ_SkinType_Name").IsUnique();

            entity.Property(e => e.SkinTypeId).HasMaxLength(100);
            entity.Property(e => e.AvoidIngredients).HasMaxLength(500);
            entity.Property(e => e.CareInstructions).HasMaxLength(1000);
            entity.Property(e => e.Characteristics).HasMaxLength(500);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.RecommendedIngredients).HasMaxLength(500);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C6A54A0AA");

            entity.ToTable("User", tb => tb.HasTrigger("TR_User_UpdatedAt"));

            entity.HasIndex(e => e.Email, "IX_User_Email");

            entity.HasIndex(e => e.IsActive, "IX_User_IsActive");

            entity.HasIndex(e => e.PhoneNumber, "IX_User_PhoneNumber");

            entity.HasIndex(e => e.RoleName, "IX_User_RoleName");

            entity.HasIndex(e => e.Username, "IX_User_Username");

            entity.HasIndex(e => e.Email, "UQ_User_Email").IsUnique();

            entity.HasIndex(e => e.Username, "UQ_User_Username").IsUnique();

            entity.Property(e => e.UserId).HasMaxLength(100);
            entity.Property(e => e.AccessFailedCount).HasDefaultValue(0);
            entity.Property(e => e.Avatar).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FirstName).HasMaxLength(50);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsEmailVerified).HasDefaultValue(false);
            entity.Property(e => e.IsPhoneVerified).HasDefaultValue(false);
            entity.Property(e => e.LastLoginDate).HasColumnType("datetime");
            entity.Property(e => e.LastName).HasMaxLength(50);
            entity.Property(e => e.LockoutEndDate).HasColumnType("datetime");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.PasswordSalt).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.RefreshToken).HasMaxLength(255);
            entity.Property(e => e.RefreshTokenExpiryDate).HasColumnType("datetime");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .HasDefaultValue("Customer");
            entity.Property(e => e.TwoFactorEnabled).HasDefaultValue(false);
            entity.Property(e => e.TwoFactorKey).HasMaxLength(50);
            entity.Property(e => e.UpdatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Username).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
