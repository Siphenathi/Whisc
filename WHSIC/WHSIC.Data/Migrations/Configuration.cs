namespace WHSIC.Data.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<WHSIC.Data.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(WHSIC.Data.ApplicationDbContext context)
        {
            AdminAndRole(context);
        }

        bool AdminAndRole(ApplicationDbContext context)
        {
            string email = "Admin@whsic.org";
            IdentityResult ir;
            var rm = new RoleManager<IdentityRole>
            (new RoleStore<IdentityRole>(context));
            ir = rm.Create(new IdentityRole("Admin"));
            var um = new UserManager<ApplicationUser>
           (new UserStore<ApplicationUser>(context));
            var user = new ApplicationUser()
            {
                UserName = email,
                PhoneNumber = null,
                Email = email,
                EmailConfirmed = true
            };
            ir = um.Create(user, "Password01$");
            if (ir.Succeeded == false)
                return ir.Succeeded;
            ir = um.AddToRole(user.Id, "Admin");
            return ir.Succeeded;
        }
    }
}
