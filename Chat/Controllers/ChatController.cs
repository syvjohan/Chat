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

namespace Chat.Controllers
{
    public class ChatController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Chat
        public ActionResult Index(long id)
        {
            return View(UpdateMessagesShown(id));
        }

        // GET: Chat/Details/5
        public ActionResult Details(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMessages projectMessages = db.ProjectMessagesSet.Find(id);
            if (projectMessages == null)
            {
                return HttpNotFound();
            }
            return View(projectMessages);
        }

        // GET: Chat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Chat/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Sender,Message,Timestamp")] GroupMessages projectMessages)
        {
            if (ModelState.IsValid)
            {
                db.ProjectMessagesSet.Add(projectMessages);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(projectMessages);
        }

        // GET: Chat/Edit/5
        public ActionResult Edit(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMessages projectMessages = db.ProjectMessagesSet.Find(id);
            if (projectMessages == null)
            {
                return HttpNotFound();
            }
            return View(projectMessages);
        }

        // POST: Chat/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Sender,Message,Timestamp")] GroupMessages projectMessages)
        {
            if (ModelState.IsValid)
            {
                db.Entry(projectMessages).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(projectMessages);
        }

        // GET: Chat/Delete/5
        public ActionResult Delete(long? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupMessages projectMessages = db.ProjectMessagesSet.Find(id);
            if (projectMessages == null)
            {
                return HttpNotFound();
            }
            return View(projectMessages);
        }

        // POST: Chat/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(long id)
        {
            GroupMessages projectMessages = db.ProjectMessagesSet.Find(id);
            db.ProjectMessagesSet.Remove(projectMessages);
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

        private string GetGroupName(long id) {
            return db.GroupsSet.Find(id).Owner;
        }

        public List<GroupMessages> UpdateMessagesShown(long id) {
            string sender = GetGroupName(id);
            List<GroupMessages> lstMsg = new List<GroupMessages>();
            foreach (GroupMessages msg in db.ProjectMessagesSet.ToList()) {
                if (msg.Sender == sender) {
                    lstMsg.Add(msg);
                }
            }
            return lstMsg;
        }
    }
}
