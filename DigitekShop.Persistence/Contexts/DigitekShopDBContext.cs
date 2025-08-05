using DigitekShop.Domain.Entities;
using DigitekShop.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DigitekShop.Persistence.Contexts
{
    public class DigitekShopDBContext(DbContextOptions<DigitekShopDBContext> options) : DbContext(options)
    {
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<ProductSpecification> ProductSpecifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Wishlist> Wishlists { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure Value Objects
            ConfigureValueObjects(modelBuilder);
            
            // Apply configurations from assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(DigitekShopDBContext).Assembly);
        }

        private void ConfigureValueObjects(ModelBuilder modelBuilder)
        {
            // Configure Customer Value Objects
            modelBuilder.Entity<Customer>(entity =>
            {
                // Configure Address as owned entity type
                entity.OwnsOne(c => c.Address, address =>
                {
                    address.Property(a => a.Province).HasColumnName("Province").IsRequired();
                    address.Property(a => a.City).HasColumnName("City").IsRequired();
                    address.Property(a => a.District).HasColumnName("District");
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.PostalCode).HasColumnName("PostalCode");
                    address.Property(a => a.Details).HasColumnName("Details");
                });

                // Configure Email as owned entity type
                entity.OwnsOne(c => c.Email, email =>
                {
                    email.Property(e => e.Value).HasColumnName("Email").IsRequired();
                });

                // Configure Phone as owned entity type
                entity.OwnsOne(c => c.Phone, phone =>
                {
                    phone.Property(p => p.Value).HasColumnName("Phone").IsRequired();
                });
            });

            // Configure Product Value Objects
            modelBuilder.Entity<Product>(entity =>
            {
                // Configure ProductName as owned entity type
                entity.OwnsOne(p => p.Name, name =>
                {
                    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
                });

                // Configure SKU as owned entity type
                entity.OwnsOne(p => p.SKU, sku =>
                {
                    sku.Property(s => s.Value).HasColumnName("SKU").IsRequired();
                });

                // Configure Money as owned entity type
                entity.OwnsOne(p => p.Price, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("Price").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("Currency").IsRequired();
                });
            });

            // Configure Brand Value Objects
            modelBuilder.Entity<Brand>(entity =>
            {
                // Configure ProductName as owned entity type
                entity.OwnsOne(b => b.Name, name =>
                {
                    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
                });
            });

            // Configure Category Value Objects
            modelBuilder.Entity<Category>(entity =>
            {
                // Configure ProductName as owned entity type
                entity.OwnsOne(c => c.Name, name =>
                {
                    name.Property(n => n.Value).HasColumnName("Name").IsRequired();
                });
            });

            // Configure Order Value Objects
            modelBuilder.Entity<Order>(entity =>
            {
                // Configure OrderNumber as owned entity type
                entity.OwnsOne(o => o.OrderNumber, orderNumber =>
                {
                    orderNumber.Property(on => on.Value).HasColumnName("OrderNumber").IsRequired();
                });

                // Configure Shipping Address as owned entity type
                entity.OwnsOne(o => o.ShippingAddress, address =>
                {
                    address.Property(a => a.Province).HasColumnName("ShippingProvince").IsRequired();
                    address.Property(a => a.City).HasColumnName("ShippingCity").IsRequired();
                    address.Property(a => a.District).HasColumnName("ShippingDistrict");
                    address.Property(a => a.Street).HasColumnName("ShippingStreet");
                    address.Property(a => a.PostalCode).HasColumnName("ShippingPostalCode");
                    address.Property(a => a.Details).HasColumnName("ShippingDetails");
                });

                // Configure Billing Address as owned entity type
                entity.OwnsOne(o => o.BillingAddress, address =>
                {
                    address.Property(a => a.Province).HasColumnName("BillingProvince").IsRequired();
                    address.Property(a => a.City).HasColumnName("BillingCity").IsRequired();
                    address.Property(a => a.District).HasColumnName("BillingDistrict");
                    address.Property(a => a.Street).HasColumnName("BillingStreet");
                    address.Property(a => a.PostalCode).HasColumnName("BillingPostalCode");
                    address.Property(a => a.Details).HasColumnName("BillingDetails");
                });

                // Configure TotalAmount as owned entity type
                entity.OwnsOne(o => o.TotalAmount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("TotalAmount").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("TotalCurrency").IsRequired();
                });

                // Configure ShippingCost as owned entity type
                entity.OwnsOne(o => o.ShippingCost, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("ShippingCost").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("ShippingCurrency").IsRequired();
                });

                // Configure TaxAmount as owned entity type
                entity.OwnsOne(o => o.TaxAmount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("TaxAmount").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("TaxCurrency").IsRequired();
                });

                // Configure DiscountAmount as owned entity type
                entity.OwnsOne(o => o.DiscountAmount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("DiscountAmount").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("DiscountCurrency").IsRequired();
                });

                // Configure FinalAmount as owned entity type
                entity.OwnsOne(o => o.FinalAmount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("FinalAmount").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("FinalCurrency").IsRequired();
                });
            });

            // Configure OrderItem Value Objects
            modelBuilder.Entity<OrderItem>(entity =>
            {
                // Configure UnitPrice as owned entity type
                entity.OwnsOne(oi => oi.UnitPrice, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("UnitPrice").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("UnitCurrency").IsRequired();
                });

                // Configure TotalPrice as owned entity type
                entity.OwnsOne(oi => oi.TotalPrice, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("TotalPrice").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("TotalCurrency").IsRequired();
                });

                // Configure DiscountAmount as owned entity type
                entity.OwnsOne(oi => oi.DiscountAmount, money =>
                {
                    money.Property(m => m.Amount).HasColumnName("DiscountAmount").IsRequired();
                    money.Property(m => m.Currency).HasColumnName("DiscountCurrency").IsRequired();
                });
            });
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            UpdateAuditFields();
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {
            UpdateAuditFields();
            return base.SaveChanges();
        }

        private void UpdateAuditFields()
        {
            var entries = ChangeTracker.Entries<BaseEntity>();

            foreach (var entry in entries)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }
            }
        }
    }
}
