using Chat.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace Chat.DAL {
    public class ProjectContext : DbContext {
        public DbSet<Users> UsersSet { get; set; }
        public DbSet<Groups> GroupsSet { get; set; }
        public DbSet<GroupMessages> ProjectMessagesSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}