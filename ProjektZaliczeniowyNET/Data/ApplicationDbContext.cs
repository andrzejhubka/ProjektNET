using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ProjektZaliczeniowyNET.Models;

namespace ProjektZaliczeniowyNET.Data;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers { get; set; }
    public DbSet<Vehicle> Vehicles { get; set; }
    public DbSet<ServiceOrder> ServiceOrders { get; set; }
    public DbSet<ServiceTask> ServiceTasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Part> Parts { get; set; }
    public DbSet<ServiceOrderPart> ServiceOrderParts { get; set; }
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Konfiguracja dla Customer
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Notes).HasMaxLength(500);

            // Index dla unikalnego emaila
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Konfiguracja dla Vehicle
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.VIN).IsRequired().HasMaxLength(17);
            entity.Property(e => e.LicensePlate).IsRequired().HasMaxLength(15);
            entity.Property(e => e.Make).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Model).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Year).IsRequired();
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.EngineNumber).HasMaxLength(20);
            entity.Property(e => e.FuelType).HasMaxLength(20).HasDefaultValue("Benzyna");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(1000);
            entity.Property(e => e.CustomerId).IsRequired();

            // Index dla unikalnego VIN
            entity.HasIndex(e => e.VIN).IsUnique();
            
            // Właściwość obliczana - ignorowana w mapowaniu bazy danych
            entity.Ignore(e => e.DisplayName);
        });

        // Konfiguracja dla ServiceOrder
        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderNumber).IsRequired().HasMaxLength(20);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Status).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.EstimatedCost).HasColumnType("decimal(10,2)");
            entity.Property(e => e.FinalCost).HasColumnType("decimal(10,2)");
            entity.Property(e => e.CustomerComplaints).HasMaxLength(1000);
            entity.Property(e => e.InternalNotes).HasMaxLength(1000);
            entity.Property(e => e.CustomerId).IsRequired();
            entity.Property(e => e.VehicleId).IsRequired();
            entity.Property(e => e.CreatedByUserId).IsRequired();

            // Index dla unikalnego numeru zlecenia
            entity.HasIndex(e => e.OrderNumber).IsUnique();
            
            // Właściwości obliczane - ignorowane w mapowaniu bazy danych
            entity.Ignore(e => e.TotalLaborCost);
            entity.Ignore(e => e.TotalPartsCost);
            entity.Ignore(e => e.TotalCost);
        });

        modelBuilder.Entity<Part>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(e => e.Description)
                .HasMaxLength(500);

            // Precyzja i skala dla ceny
            entity.Property(e => e.Price)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            entity.Property(e => e.QuantityInStock)
                .IsRequired();

            entity.Property(e => e.CreatedAt)
                .IsRequired();

            entity.Property(e => e.UpdatedAt)
                .IsRequired(false);
        });

        // Konfiguracja dla ServiceTask
        modelBuilder.Entity<ServiceTask>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.DetailedDescription).HasMaxLength(1000);
            entity.Property(e => e.LaborHours).IsRequired().HasColumnType("decimal(8,2)");
            entity.Property(e => e.HourlyRate).IsRequired().HasColumnType("decimal(8,2)");
            entity.Property(e => e.IsCompleted).IsRequired().HasDefaultValue(false);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.ServiceOrderId).IsRequired();

            // Właściwość obliczana - ignorowana w mapowaniu bazy danych
            entity.Ignore(e => e.TotalTaskCost);
        });

        // Konfiguracja dla Comment
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Content).IsRequired().HasMaxLength(2000);
            entity.Property(e => e.Type)
                .IsRequired()
                .HasConversion<int>(); // Konwersja enum na int
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.UpdatedAt).IsRequired(false);
            entity.Property(e => e.AuthorId).IsRequired().HasMaxLength(450); // Dodaj maksymalną długość
            entity.Property(e => e.ServiceOrderId).IsRequired();
    
            // Ignoruj właściwość obliczaną
            entity.Ignore(e => e.IsEdited);
        });

        // Konfiguracja dla ServiceOrderPart
        modelBuilder.Entity<ServiceOrderPart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cost).HasColumnType("decimal(10,2)");
            entity.Property(e => e.Quantity).IsRequired();
        });

        // Relacje
        modelBuilder.Entity<Vehicle>()
            .HasOne(v => v.Customer)
            .WithMany(c => c.Vehicles)
            .HasForeignKey(v => v.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Customer)
            .WithMany(c => c.ServiceOrders)
            .HasForeignKey(so => so.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.Vehicle)
            .WithMany(v => v.ServiceOrders)
            .HasForeignKey(so => so.VehicleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.AssignedMechanic)
            .WithMany()
            .HasForeignKey(so => so.AssignedMechanicId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ServiceOrder>()
            .HasOne(so => so.CreatedByUser)
            .WithMany()
            .HasForeignKey(so => so.CreatedByUserId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<ServiceTask>()
            .HasOne(st => st.ServiceOrder)
            .WithMany(so => so.ServiceTasks)
            .HasForeignKey(st => st.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Comment>()
            .HasOne(c => c.ServiceOrder)
            .WithMany(so => so.Comments)
            .HasForeignKey(c => c.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceOrderPart>()
            .HasOne(sop => sop.ServiceOrder)
            .WithMany(so => so.ServiceOrderParts)
            .HasForeignKey(sop => sop.ServiceOrderId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<ServiceOrderPart>()
            .HasOne(sop => sop.Part)
            .WithMany()
            .HasForeignKey(sop => sop.PartId)
            .OnDelete(DeleteBehavior.Restrict);

        // Dodatkowe konfiguracje indeksów dla wydajności
        modelBuilder.Entity<ServiceTask>()
            .HasIndex(st => st.ServiceOrderId)
            .HasDatabaseName("IX_ServiceTasks_ServiceOrderId");

        modelBuilder.Entity<ServiceTask>()
            .HasIndex(st => st.IsCompleted)
            .HasDatabaseName("IX_ServiceTasks_IsCompleted");

        modelBuilder.Entity<ServiceOrder>()
            .HasIndex(so => so.Status)
            .HasDatabaseName("IX_ServiceOrders_Status");

        modelBuilder.Entity<ServiceOrder>()
            .HasIndex(so => so.CreatedAt)
            .HasDatabaseName("IX_ServiceOrders_CreatedAt");

        modelBuilder.Entity<ServiceOrder>()
            .HasIndex(so => so.VehicleId)
            .HasDatabaseName("IX_ServiceOrders_VehicleId");

        modelBuilder.Entity<ServiceOrderPart>()
            .HasIndex(sop => sop.ServiceOrderId)
            .HasDatabaseName("IX_ServiceOrderParts_ServiceOrderId");

        modelBuilder.Entity<ServiceOrderPart>()
            .HasIndex(sop => sop.PartId)
            .HasDatabaseName("IX_ServiceOrderParts_PartId");
    }
}