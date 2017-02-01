namespace ORM.EF
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class EntityModel : DbContext
    {
        public EntityModel(string connectionString)
            : base(connectionString)
        {
        }

        public virtual DbSet<Bid> Bids { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<Commentary> Commentaries { get; set; }
        public virtual DbSet<Lot> Lots { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserProfile> UserProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bid>()
                .Property(e => e.Price)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Category>()
                .HasMany(e => e.Lots)
                .WithRequired(e => e.Category)
                .HasForeignKey(e => e.CtegorieId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lot>()
                .Property(e => e.CurrentPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Lot>()
                .HasMany(e => e.Bids)
                .WithRequired(e => e.Lot)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Lot>()
                .HasMany(e => e.Commentaries)
                .WithRequired(e => e.Lot)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Role>()
                .HasMany(e => e.Users)
                .WithMany(e => e.Roles)
                .Map(m => m.ToTable("UserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<User>()
                .HasMany(e => e.Bids)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Commentaries)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.Lots)
                .WithRequired(e => e.User)
                .HasForeignKey(e => e.SellerId)
                .WillCascadeOnDelete(false);
        }
    }
}
