using Chat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chat.DAL {
    public class TestData : System.Data.Entity.DropCreateDatabaseIfModelChanges<ProjectContext> {
        protected override void Seed(ProjectContext context) {
            var user = new List<Users> {
                new Users { Username="johan", Password="johantest", Mail="johan@gmail.com" },
                new Users { Username="bengt", Password="bengttest", Mail="bengt@gmail.com" },
                new Users { Username="nisse", Password="nissetest", Mail="nissetest@gmail.com" },
                new Users { Username="karl", Password="karltest", Mail="karl@gmail.com" }
            };

            user.ForEach(i => context.UsersSet.Add(i));
            context.SaveChanges();

            var project = new List<Projects> {
                new Projects{ ID=0, Name="projectBengt" },
                new Projects{ ID=1, Name="projectKarl" },
                new Projects{ ID=2, Name="projectNisse" },
                new Projects{ ID=3, Name="ProjectJohan" }
            };

            project.ForEach(c => context.ProjecstSet.Add(c));
            context.SaveChanges();

            var message = new List<ProjectMessages> {
                new ProjectMessages{ ID=0, Sender="bengt", Message="hej på er", Timestamp="10/9/2015 9:45:06 PM" },
                new ProjectMessages{ ID=1, Sender="nisse", Message="wassup dudes?", Timestamp="10/9/2015 10:00:00 PM" },
                new ProjectMessages{ ID=2, Sender="karl", Message="allt är bra", Timestamp="10/9/2015 11:05:50 PM" },
                new ProjectMessages{ ID=3, Sender="johan", Message="stol :D", Timestamp="12/10/2015 14:10:50 PM" }
            };

            message.ForEach(r => context.ProjectMessagesSet.Add(r));
            context.SaveChanges();
        }
    }
}