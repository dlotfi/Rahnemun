using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Edreamer.Framework.Data;
using Edreamer.Framework.Data.EntityFramework;
using Edreamer.Framework.Helpers;
using Edreamer.Framework.Validation;

namespace Rahnemun.Domain
{
    public class RahnemunDataContext : DataContext, IRahnemunDataContext
    {
        public IRepository<BlogPost> BlogPosts { get { return Repository<BlogPost>(); } }
        public IRepository<Category> Categories { get { return Repository<Category>(); } }
        public IRepository<CategoryGroup> CategoryGroups { get { return Repository<CategoryGroup>(); } }
        public IRepository<Comment> Comments { get { return Repository<Comment>(); } }
        public IRepository<Consultant> Consultants { get { return Repository<Consultant>(); } }
        public IRepository<CustomerMessage> CustomerMessages { get { return Repository<CustomerMessage>(); } }
        public IRepository<Guest> Guests { get { return Repository<Guest>(); } }
        public IRepository<Message> Messages { get { return Repository<Message>(); } }
        public IRepository<Payment> Payments { get { return Repository<Payment>(); } }
        public IRepository<Session> Sessions { get { return Repository<Session>(); } }
        public IRepository<User> Users { get { return Repository<User>(); } }
        
        public RahnemunDataContext()
            : base("Rahnemun")
        {
        }

        //public RahnemunDataContext(IValidationService validationService)
        //    : base("Rahnemun")
        //{
        //    Throw.IfArgumentNull(validationService, "validationService");
        //    ValidationService = validationService; 
        //}

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // ******************** Tables ********************
            modelBuilder.Entity<BlogPost>().ToTable("Rahnemun_BlogPosts");
            modelBuilder.Entity<Category>().ToTable("Rahnemun_Categories");
            modelBuilder.Entity<CategoryGroup>().ToTable("Rahnemun_CategoryGroups");
            modelBuilder.Entity<Comment>().ToTable("Rahnemun_Comments");
            modelBuilder.Entity<Consultant>().ToTable("Rahnemun_Consultants");
            modelBuilder.Entity<CustomerMessage>().ToTable("Rahnemun_CustomerMessages");
            modelBuilder.Entity<Guest>().ToTable("Rahnemun_Guests");
            modelBuilder.Entity<Message>().ToTable("Rahnemun_Messages");
            modelBuilder.Entity<Payment>().ToTable("Rahnemun_Payments");
            modelBuilder.Entity<Session>().ToTable("Rahnemun_Sessions");
            modelBuilder.Entity<User>().ToTable("Rahnemun_Users");

            // ******************** One to Many Cascade Deletes and Foreign Keys ********************
            modelBuilder.Entity<Category>()
                .HasRequired(c => c.CategoryGroup)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Message>()
                .HasRequired(m => m.Session)
                .WithMany()
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Comment>()
                .HasRequired(c => c.BlogPost)
                .WithMany()
                .WillCascadeOnDelete(true);

            // Should not turn cascade delete on, because it may cause cycles or multiple cascade paths.
            //modelBuilder.Entity<Comment>()
            //    .HasOptional(c => c.RepliedComment)
            //    .WithMany()
            //    .WillCascadeOnDelete(true);

            // ******************** Many to Many Relations ********************
            modelBuilder.Entity<Consultant>()
                .HasMany(c => c.Categories)
                .WithMany()
                .Map(m =>
                {
                    m.ToTable("Rahnemun_ConsultantsCategories");
                    m.MapLeftKey("ConsultantId");
                    m.MapRightKey("CategoryId");
                });

            // ******************** Primary Keys ********************
            modelBuilder.Entity<User>()
                .Property(u => u.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            
            // ******************** Conventions ********************
            modelBuilder.Properties()
                .Where(p => p.PropertyType == typeof(byte[]) && p.Name.EqualsIgnoreCase("Timestamp"))
                .Configure(p => p.IsRowVersion());
            
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
