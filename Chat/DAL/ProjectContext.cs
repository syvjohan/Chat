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
        public DbSet<Projects> ProjecstSet { get; set; }
        public DbSet<ProjectMessages> ProjectMessagesSet { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}