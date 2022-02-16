namespace TTCMS.Data.Migrations
{
    using TTCMS.Domain;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity.EntityFramework;
    using Microsoft.AspNet.Identity;
    using System.Collections.Generic;

    internal sealed class Configuration : DbMigrationsConfiguration<TTCMS.Data.TTCMSDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }
        //enter the following command while having TTCMS.Data as the default project.
        //PM update-database -verbose
        protected override void Seed(TTCMS.Data.TTCMSDbContext context)
        {
            ////  This method will be called after migrating to the latest version.
            //var roleStore = new RoleStore<ApplicationRole>(context);
            //var roleManager = new RoleManager<ApplicationRole>(roleStore);
            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            ////You can use the DbSet<T>.AddOrUpdate() helper extension method 
            ////to avoid creating duplicate seed data. E.g.
            //const string Language_vi = "vi-VN";
            //const string Language_en = "en-US";
            //const string name = "thanhtong";
            //const string email = "thanhtong.max@gmail.com";
            //const string password = "123qwe";
            //const string roleName = "Administrator";
            ////khởi tạo tài khoản
            //   var user = new ApplicationUser { UserName = name, Email = email, PhoneNumberConfirmed = true, FirstName = "Thanh", LastName = "Tòng", PhoneNumber = "0908308398", Address = "HCM", Sex = true, CreatedBy = name, DateCreated = System.DateTime.Now, IsDeleted = false, IsLocked = false, Activated = true };
            //    var result = userManager.Create(user, password);
            //    result = userManager.SetLockoutEnabled(user.Id, true);
            
            ////khởi tạo ngôn ngữ
            //context.Languages.AddOrUpdate(
            //     new Language { Id = Language_vi, Name = "Tiếng Việt", Description = "Ngôn ngữ tiếng việt", CreatedDate = System.DateTime.Now, IsActived = true, IsDeleted = false, IsDefault = true, CreatedBy = name, Img_Icon = "flag-vi" },
            //        new Language { Id = Language_en, Name = "English", Description = "Languages ​​English", CreatedDate = System.DateTime.Now, IsActived = true, IsDeleted = false, IsDefault = false, CreatedBy = name, Img_Icon = "flag-us" }
            //    );
            ////khởi tạo roles
            //    var role = new ApplicationRole()
            //    {
            //        Name = "Administrator",
            //        CreatedDate = System.DateTime.Now,
            //        CreatedBy = name,
            //        IsActived = true,
            //        IsDeleted = false
            //    };
            //    var roleresult = roleManager.Create(role);
            //    //khởi tạo ngon ngữ role
            //    new List<Language_Role>
            //             {
            //                 new Language_Role { Description ="Nhóm quản tri cao nhất",Language = context.Languages.Find(Language_vi),Role = roleManager.FindByName(roleName)},

            //                 new Language_Role { Description ="Super Group Administrator",Language = context.Languages.Find(Language_en),Role = roleManager.FindByName(roleName)},
    
            //             }.ForEach(m => context.Language_Roles.Add(m));

            //    // Add user admin to Role Admin if not already added
            //    var rolesForUser = userManager.GetRoles(user.Id);
            //    if (!rolesForUser.Contains(role.Name))
            //    {
            //        var resultRole = userManager.AddToRole(user.Id, role.Name);
            //    }
            
            ////khởi tạo function
            //context.Functions.AddOrUpdate(
            //    new Function { Id ="DASHBOARD",Link ="/TT_Admin",Target="_self",Order=1,CssClass="fa fa-tachometer",IsLocked=true,IsDeleted=false,IsFavorited=false,CreatedDate = System.DateTime.Now,CreatedBy=name},
            //         new Function { Id ="SYSTEM",Link ="/TT_Admin",Target="_self",Order=2,CssClass="fa fa-cogs",IsLocked=true,IsDeleted=false,IsFavorited=false,CreatedDate = System.DateTime.Now,CreatedBy=name},
            //        new Function { Id ="USER",Link ="/TT_Admin/User/Index",Target="_self",Order=1,CssClass="fa fa-users",IsLocked=true,IsDeleted=false,IsFavorited=false,ParentID="SYSTEM",CreatedDate = System.DateTime.Now,CreatedBy=name},
            //         new Function { Id ="FUNCTION",Link ="/TT_Admin/Function/Index",Target="_self",Order=2,CssClass="fa fa-bar-chart-o fa-fw",IsLocked=true,IsDeleted=false,IsFavorited=false,ParentID="SYSTEM",CreatedDate = System.DateTime.Now,CreatedBy=name},
            //          new Function { Id ="ROLE",Link ="/TT_Admin/Role/Index",Target="_self",Order=3,CssClass="fa fa-table fa-fw",IsLocked=true,IsDeleted=false,IsFavorited=false,ParentID="SYSTEM",CreatedDate = System.DateTime.Now,CreatedBy=name},
            //          new Function { Id ="GACTION",Link ="/TT_Admin/Account/Index",Target="_self",Order=4,CssClass="fa fa-edit fa-fw",IsLocked=true,IsDeleted=false,IsFavorited=false,ParentID="SYSTEM",CreatedDate = System.DateTime.Now,CreatedBy=name}
            //    );
            //context.Language_Functions.AddOrUpdate(
            //    new Language_Function{Name ="Trang chủ",Description="Trang chủ",Text="Trang chủ", Language=context.Languages.Find(Language_vi),Function=context.Functions.Find("DASHBOARD")},
            //      new Language_Function{Name ="Home",Description="Home",Text="Home", Language=context.Languages.Find(Language_en),Function=context.Functions.Find("DASHBOARD")},
            //       new Language_Function{Name ="Quản lý hệ thống",Description="Quản lý hệ thống",Text="Quản lý hệ thống", Language=context.Languages.Find(Language_vi),Function=context.Functions.Find("SYSTEM")},
            //      new Language_Function{Name ="System Management",Description="System Management",Text="System Management", Language=context.Languages.Find(Language_en),Function=context.Functions.Find("SYSTEM")},
            //      new Language_Function{Name ="Quản lý người dùng",Description="Quản lý người dùng",Text="Quản lý người dùng", Language=context.Languages.Find(Language_vi),Function=context.Functions.Find("USER")},
            //      new Language_Function{Name ="User Management",Description="User Management",Text="User Management", Language=context.Languages.Find(Language_en),Function=context.Functions.Find("USER")},
            //      new Language_Function{Name ="Quản lý chức năng",Description="Quản lý chức năng",Text="Quản lý chức năng", Language=context.Languages.Find(Language_vi),Function=context.Functions.Find("FUNCTION")},
            //      new Language_Function{Name ="Function Management",Description="Function Management",Text="Function Management", Language=context.Languages.Find(Language_en),Function=context.Functions.Find("FUNCTION")},
            //      new Language_Function{Name ="Nhóm người dùng",Description="Nhóm người dùng",Text="Nhóm người dùng", Language=context.Languages.Find(Language_vi),Function=context.Functions.Find("ROLE")},
            //      new Language_Function{Name ="Group Management",Description="Group Management",Text="Group Management", Language=context.Languages.Find(Language_en),Function=context.Functions.Find("ROLE")},
            //      new Language_Function{Name ="Quản lý quyền",Description="Quản lý quyền",Text="Quản lý quyền", Language=context.Languages.Find(Language_vi),Function=context.Functions.Find("GACTION")},
            //      new Language_Function{Name ="Role Management",Description="Role Management",Text="Role Management", Language=context.Languages.Find(Language_en),Function=context.Functions.Find("GACTION")}
            //    );
            ////khởi tạo action
            //context.GActions.AddOrUpdate(
            //    new GAction{ Id= "CREATE",CreatedDate = System.DateTime.Now, CreatedBy = name, IsActived = true,IsDeleted = false},
            //    new GAction{ Id= "DELETE",CreatedDate = System.DateTime.Now, CreatedBy = name, IsActived = true,IsDeleted = false},
            //    new GAction{ Id= "EDIT",CreatedDate = System.DateTime.Now, CreatedBy = name, IsActived = true,IsDeleted = false},
            //    new GAction{ Id= "VIEW",CreatedDate = System.DateTime.Now, CreatedBy = name, IsActived = true,IsDeleted = false}
            //    );
            //context.Language_GAction.AddOrUpdate(
            //    new Language_GAction{Name ="Thêm mới",Description="Thêm mới 1 bản ghi",Language = context.Languages.Find(Language_vi), GAction = context.GActions.Find("CREATE")},
            //        new Language_GAction{Name ="Add new record",Description="Add new 1 record",Language = context.Languages.Find(Language_en), GAction = context.GActions.Find("CREATE")},
            //        new Language_GAction{Name ="Xóa bản ghi",Description="Xóa bản ghi",Language = context.Languages.Find(Language_vi), GAction = context.GActions.Find("DELETE")},
            //        new Language_GAction { Name = "Delete record", Description = "Delete record", Language = context.Languages.Find(Language_en), GAction = context.GActions.Find("DELETE") },
            //        new Language_GAction{Name ="Sửa bản ghi",Description="Sửa bản ghi",Language = context.Languages.Find(Language_vi), GAction = context.GActions.Find("EDIT")},
            //        new Language_GAction { Name = "Edit record", Description = "Edit record", Language = context.Languages.Find(Language_en), GAction = context.GActions.Find("EDIT") },
            //        new Language_GAction{Name ="Xem danh sách",Description="Xem danh sách",Language = context.Languages.Find(Language_vi), GAction = context.GActions.Find("VIEW")},
            //        new Language_GAction { Name = "View the list", Description = "View the list", Language = context.Languages.Find(Language_en), GAction = context.GActions.Find("VIEW") }
            //    );
            ////khởi tạo quyền cho user
            // var roleid = roleManager.FindByName(roleName);
            //    var action_Create = context.GActions.Find("CREATE");
            //    var action_Delete = context.GActions.Find("DELETE");
            //    var action_Edit = context.GActions.Find("EDIT");
            //    var action_View = context.GActions.Find("VIEW");
            //context.Permissions.AddOrUpdate(
            //    new Permission{Role = roleid,Function = context.Functions.Find("DASHBOARD"),GAction = action_Create},
            //        new Permission{Role = roleid,Function = context.Functions.Find("DASHBOARD"),GAction = action_Delete},
            //         new Permission{Role = roleid,Function = context.Functions.Find("DASHBOARD"),GAction = action_Edit},
            //        new Permission{Role = roleid,Function = context.Functions.Find("DASHBOARD"),GAction = action_View},
            //        new Permission{Role = roleid,Function = context.Functions.Find("SYSTEM"),GAction = action_Create},
            //        new Permission{Role = roleid,Function = context.Functions.Find("SYSTEM"),GAction = action_Delete},
            //         new Permission{Role = roleid,Function = context.Functions.Find("SYSTEM"),GAction = action_Edit},
            //        new Permission{Role = roleid,Function = context.Functions.Find("SYSTEM"),GAction = action_View},
            //        new Permission{Role = roleid,Function = context.Functions.Find("USER"),GAction = action_Create},
            //        new Permission{Role = roleid,Function = context.Functions.Find("USER"),GAction = action_Delete},
            //         new Permission{Role = roleid,Function = context.Functions.Find("USER"),GAction = action_Edit},
            //        new Permission{Role = roleid,Function = context.Functions.Find("USER"),GAction = action_View},
            //         new Permission{Role = roleid,Function = context.Functions.Find("FUNCTION"),GAction = action_Create},
            //        new Permission{Role = roleid,Function = context.Functions.Find("FUNCTION"),GAction = action_Delete},
            //         new Permission{Role = roleid,Function = context.Functions.Find("FUNCTION"),GAction = action_Edit},
            //        new Permission{Role = roleid,Function = context.Functions.Find("FUNCTION"),GAction = action_View},
            //         new Permission{Role = roleid,Function = context.Functions.Find("ROLE"),GAction = action_Create},
            //        new Permission{Role = roleid,Function = context.Functions.Find("ROLE"),GAction = action_Delete},
            //         new Permission{Role = roleid,Function = context.Functions.Find("ROLE"),GAction = action_Edit},
            //        new Permission{Role = roleid,Function = context.Functions.Find("ROLE"),GAction = action_View},
            //        new Permission{Role = roleid,Function = context.Functions.Find("GACTION"),GAction = action_Create},
            //        new Permission{Role = roleid,Function = context.Functions.Find("GACTION"),GAction = action_Delete},
            //         new Permission{Role = roleid,Function = context.Functions.Find("GACTION"),GAction = action_Edit},
            //        new Permission{Role = roleid,Function = context.Functions.Find("GACTION"),GAction = action_View}
            //    );
            ////log khởi tạo
            //context.ActivityLogs.AddOrUpdate(
            //    new ActivityLog {ActivityLogId=Guid.NewGuid(), ServiceName = "Install", MethodName = "Setup", ExecutionTime = System.DateTime.Now, ExecutionDuration = 10, User = userManager.FindByName(name) }
            //    );
            
            ////
            context.Commit();

        }

    }
}
