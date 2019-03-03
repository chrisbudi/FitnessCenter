using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel.Config;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace IdentityModel.Model
{
    // You will not likely need to customize there, but it is necessary/easier to create our own 
    // project-specific implementations, so here they are:
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {

    }

    public class ApplicationUserClaim : IdentityUserClaim<string>
    {

    }

    public class ApplicationUserRole : IdentityUserRole<string>
    {

    }

    // Must be expressed in terms of our custom Role and other types:
    public class ApplicationUser
        : IdentityUser<string, ApplicationUserLogin,
            ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationUser()
        {
            this.Id = Guid.NewGuid().ToString();

            // Add any custom User properties/code here
        }
        public string Password { get; set; }


        public async Task<ClaimsIdentity>
            GenerateUserIdentityAsync(ApplicationUserManager manager)
        {
            var userIdentity = await manager
                .CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }


    // Must be expressed in terms of our custom UserRole:
    public class ApplicationRole : IdentityRole<string, ApplicationUserRole>
    {
        public ApplicationRole()
        {
            this.Id = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// ini hanya di gunakan untuk input role ketika membuat forms
        /// </summary>
        /// <param name="name">_C_R_U_D</param>
        public ApplicationRole(string name)
            : this()
        {
            this.Name = name;
            this.Description = "Form";
            this.Active = true;
        }
        /// <summary>
        /// ini baru dapat di gunakan dengan bebas wkwkwkkw...
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <param name="isActive"></param>
        public ApplicationRole(string name, string description, bool isActive)
            : this()
        {
            this.Name = name;
            this.Description = description;
            this.Active = isActive;
        }

        // Add any custom Role properties/code here
        public string Description { get; set; }
        public bool Active { get; set; }
    }

    // Must be expressed in terms of our custom types:
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser, ApplicationRole,
            string, ApplicationUserLogin, ApplicationUserRole, ApplicationUserClaim>
    {
        public ApplicationDbContext()
            : base("FitnessDbContext")
        {
        }

        //static ApplicationDbContext()
        //{
        //    Database.SetInitializer<ApplicationDbContext>(new ApplicationDbInitializer());
        //}

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        // Add the ApplicationGroups property:
        public virtual IDbSet<ApplicationGroup> ApplicationGroups { get; set; }
        public virtual IDbSet<AspForm> AspForms { get; set; }
        public virtual IDbSet<AspFormAuthorization> AspFormsAuthorizations { get; set; }
        public virtual IDbSet<ApplicationGroupRole> ApplicationGroupRoles { get; set; }
        public virtual IDbSet<ApplicationUserGroup> ApplicationUserGroups { get; set; }

        // Override OnModelsCreating:
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationUserGroup>((ApplicationGroup g) => g.ApplicationUsers)
                .WithRequired().HasForeignKey<string>((ApplicationUserGroup ag) => ag.ApplicationGroupId);
            modelBuilder.Entity<ApplicationUserGroup>()
                .HasKey((ApplicationUserGroup r) =>
                    new
                    {
                        ApplicationUserId = r.ApplicationUserId,
                        ApplicationGroupId = r.ApplicationGroupId
                    }).ToTable("ApplicationUserGroups");

            modelBuilder.Entity<ApplicationGroup>()
                .HasMany<ApplicationGroupRole>((ApplicationGroup g) => g.ApplicationRoles)
                .WithRequired().HasForeignKey<string>((ApplicationGroupRole ap) => ap.ApplicationGroupId);
            modelBuilder.Entity<ApplicationGroupRole>().HasKey((ApplicationGroupRole gr) =>
                new
                {
                    ApplicationRoleId = gr.ApplicationRoleId,
                    ApplicationGroupId = gr.ApplicationGroupId
                }).ToTable("ApplicationGroupRoles");

            modelBuilder.Entity<ApplicationGroup>()
             .HasMany(e => e.AspFormAuthorizations)
             .WithOptional(e => e.ApplicationGroup)
             .HasForeignKey(e => e.GroupId);

            modelBuilder.Entity<AspForm>()
               .Property(e => e.Module)
               .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.desciption)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.controller)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.action)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.url)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.area)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.parameter)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .Property(e => e.statusCode)
                .IsUnicode(false);

            modelBuilder.Entity<AspForm>()
                .HasMany(e => e.AspForm1)
                .WithOptional(e => e.AspForm2)
                .HasForeignKey(e => e.parent_ID);

        }
    }


    // Most likely won't need to customize these either, but they were needed because we implemented
    // custom versions of all the other types:
    public class ApplicationUserStore
        : UserStore<ApplicationUser, ApplicationRole, string,
            ApplicationUserLogin, ApplicationUserRole,
            ApplicationUserClaim>, IUserStore<ApplicationUser, string>,
            IDisposable
    {
        public ApplicationUserStore()
            : this(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationUserStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationRoleStore
        : RoleStore<ApplicationRole, string, ApplicationUserRole>,
            IQueryableRoleStore<ApplicationRole, string>,
            IRoleStore<ApplicationRole, string>, IDisposable
    {
        public ApplicationRoleStore()
            : base(new IdentityDbContext())
        {
            base.DisposeContext = true;
        }

        public ApplicationRoleStore(DbContext context)
            : base(context)
        {
        }
    }


    public class ApplicationGroup
    {
        public ApplicationGroup()
        {
            this.Id = Guid.NewGuid().ToString();
            this.ApplicationRoles = new List<ApplicationGroupRole>();
            this.ApplicationUsers = new List<ApplicationUserGroup>();
            this.AspFormAuthorizations = new List<AspFormAuthorization>();
        }

        public ApplicationGroup(string name)
            : this()
        {
            this.Name = name;
        }

        public ApplicationGroup(string name, string description, bool active)
            : this(name)
        {
            this.Description = description;
            this.Active = active;
        }

        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public virtual ICollection<ApplicationGroupRole> ApplicationRoles { get; set; }
        public virtual ICollection<ApplicationUserGroup> ApplicationUsers { get; set; }
        public virtual ICollection<AspFormAuthorization> AspFormAuthorizations { get; set; }


    }

    [Table("AspFormAuthorization")]
    public partial class AspFormAuthorization
    {
        [Key]
        public int authID { get; set; }

        public int FormID { get; set; }

        [StringLength(128)]
        public string GroupId { get; set; }

        public virtual ApplicationGroup ApplicationGroup { get; set; }

        public virtual AspForm AspForm { get; set; }
    }

    [Table("AspForm")]
    public partial class AspForm
    {
        public AspForm()
        {
            AspForm1 = new HashSet<AspForm>();
            AspFormAuthorizations = new HashSet<AspFormAuthorization>();
        }

        [Key]
        public int FormId { get; set; }

        public int? parent_ID { get; set; }

        [StringLength(20)]
        public string MasterModule { get; set; }

        [Required]
        [StringLength(20)]
        public string Module { get; set; }

        [Required]
        [StringLength(50)]
        public string title { get; set; }

        [StringLength(255)]
        public string desciption { get; set; }

        [StringLength(50)]
        public string controller { get; set; }

        [StringLength(50)]
        public string action { get; set; }

        [StringLength(255)]
        public string url { get; set; }

        [StringLength(50)]
        public string area { get; set; }

        public bool? visible { get; set; }

        [StringLength(50)]
        public string parameter { get; set; }

        public string iconclass { get; set; }

        public bool? clickable { get; set; }

        [StringLength(10)]
        public string statusCode { get; set; }

        public int? displayOrder { get; set; }

        public virtual ICollection<AspForm> AspForm1 { get; set; }

        public virtual AspForm AspForm2 { get; set; }

        public virtual ICollection<AspFormAuthorization> AspFormAuthorizations { get; set; }
    }

    public class ApplicationUserGroup
    {
        public string ApplicationUserId { get; set; }
        public string ApplicationGroupId { get; set; }
    }

    public class ApplicationGroupRole
    {
        public string ApplicationGroupId { get; set; }
        public string ApplicationRoleId { get; set; }
    }
}