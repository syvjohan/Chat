using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Chat.DAL;
using Chat.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Chat.Controllers
{
    public class GroupController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Groups
        public ActionResult Index()
        {
            return View(db.GroupsSet.ToList());
        }

        // GET: Groups/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.GroupsSet.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // GET: Groups/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Groups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.GroupsSet.Add(groups);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(groups);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateNewGroup([Bind(Include = "Name")] string groupName) {
            if (GroupNameExist(groupName)) {
                ModelState.AddModelError("errorCreateNewUser", "Username already exist");
                return View("../Group/Create");
            } else if (!CheckInput(groupName)) {
                ModelState.AddModelError("errorCreateNewUser", "invalid input");
                return View("../Group/Create");
            }
            Users user = (Users)Session["currentUser"];
            Groups group = new Groups();
            group.Owner = user.Username;
            group.Name = groupName;

            db.GroupsSet.Add(group);
            db.SaveChanges();

            return RedirectToAction("Index", "Group");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditGroup([Bind(Include = "Name")] string newGroupName) {
            if (GroupNameExist(newGroupName)) {
                ModelState.AddModelError("errorChangeGroupName", "Username already exist");
                return View("../Group/Edit");
            } else if (!CheckInput(newGroupName)) {
                ModelState.AddModelError("errorChangeGroupName", "invalid input");
                return View("../Group/Edit");
            }
            
            bool found = false;
            Users user = (Users)Session["currentUser"];
            foreach (Groups g in db.GroupsSet.ToList()) {
                if (g.Owner == user.Username) {
                    g.Name = newGroupName;
                    db.Entry(g).State = EntityState.Modified;
                    db.SaveChanges();

                    found = true;
                    break;
                }
            }
            Debug.Assert(found != false, "Error occured in EditGroup (GroupController) username was not found!");
            return RedirectToAction("Index", "Group");
        }

        // GET: Groups/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups group = db.GroupsSet.Find(id);
            if (group == null)
            {
                return HttpNotFound();
            }

            Session["currentGroup"] = group;
            return View(group);
        }

        // POST: Groups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Name,Owner")] Groups groups)
        {
            if (ModelState.IsValid)
            {
                db.Entry(groups).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(groups);
        }

        // GET: Groups/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Groups groups = db.GroupsSet.Find(id);
            if (groups == null)
            {
                return HttpNotFound();
            }
            return View(groups);
        }

        // POST: Groups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            Groups groups = db.GroupsSet.Find(id);
            db.GroupsSet.Remove(groups);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public bool EvalGroupOwner(Groups group) 
        {
            Users g = (Users)Session["currentUser"];
            return group.Owner == g.Username;
        }

        private bool GroupNameExist(string groupName) {
            if (GetGroup(groupName) == null) {
                return false;
            }
            return true;
        }

        private Groups GetGroup(string groupName) {
            Groups group = null;
            foreach (var g in db.GroupsSet) {
                if (g.Name == groupName) {
                    group = g;
                    break;
                }
            }
            return group;
        }

        private bool CheckInput(string value) {
            return (value.Length >= 3 && Regex.IsMatch(value, @"[a-zA-Z]"));
        }
    }
}
