using TTCMS.Data.Configurations;
using TTCMS.Domain;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace TTCMS.Data
{
    public class TTCMSDbContext : IdentityDbContext<ApplicationUser>
    {
        public class ApplicationRole : IdentityRole
        {

        }
        public TTCMSDbContext() : base("TTCMSEntities") { }

        static TTCMSDbContext()
        {
            //Database.SetInitializer<TTCMSDbContext>(new TTCMSDbInitializer());
        }
        public static TTCMSDbContext Create()
        {
            return new TTCMSDbContext();
        }
        //core
        public DbSet<Function> Functions { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<GAction> GActions { get; set; }
        public DbSet<Language_Function> Language_Functions { get; set; }
        public DbSet<Language_Role> Language_Roles { get; set; }
        public DbSet<Language_GAction> Language_GAction { get; set; }
        public DbSet<AccessStatistic> AccessStatistics { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        //cms
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SinglePage> SinglePages { get; set; }
        public DbSet<Language_SinglePage> Language_SinglePages { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Language_Setting> Language_Settings { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProductColor> ProductColors { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Slide> Slides { get; set; }
        public DbSet<Advertisements> Advertisements { get; set; }
        public DbSet<Contacts> Contacts { get; set; }
        public DbSet<ProductSize> ProductSizes { get; set; }
        public DbSet<News> News { get; set; }
        //

        public virtual void Commit()
        {
            base.SaveChanges();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Configurations.Add(new FunctionConfiguration());
            modelBuilder.Configurations.Add(new LanguageConfiguration());
            modelBuilder.Configurations.Add(new GActionConfiguration());
            modelBuilder.Configurations.Add(new Language_RoleConfiguration());
            modelBuilder.Configurations.Add(new Language_FunctionConfiguration());
            modelBuilder.Configurations.Add(new Language_GActionConfiguration());
            modelBuilder.Configurations.Add(new AccessStatisticConfiguration());
            modelBuilder.Configurations.Add(new ActivityLogConfiguration());
            modelBuilder.Configurations.Add(new MenuConfiguration());
            modelBuilder.Configurations.Add(new CategoryConfiguration());
            modelBuilder.Configurations.Add(new SinglePageConfiguration());
            modelBuilder.Configurations.Add(new Language_SinglePageConfiguration());
            modelBuilder.Configurations.Add(new SettingConfiguration());
            modelBuilder.Configurations.Add(new Language_SettingConfiguration());
            modelBuilder.Configurations.Add(new ProductConfiguration());
            modelBuilder.Configurations.Add(new OrderConfiguration());
            modelBuilder.Configurations.Add(new OrderDetailConfiguration());
            modelBuilder.Configurations.Add(new UserConfiguration());
            modelBuilder.Configurations.Add(new ProductColorConfiguration());
            modelBuilder.Configurations.Add(new ProductImageConfiguration());
            modelBuilder.Configurations.Add(new SlideConfiguration());
            modelBuilder.Configurations.Add(new QuangCaoConfiguration());
            modelBuilder.Configurations.Add(new LienHeConfiguration());
            modelBuilder.Configurations.Add(new ProductSizeConfiguration());
            modelBuilder.Configurations.Add(new NewsConfiguration());
            //

        }
    }
}
