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
using System.Text.RegularExpressions;

namespace Chat.Controllers
{
    public class LoginController : Controller
    {
        private ProjectContext db = new ProjectContext();

        // GET: Login
        public ActionResult Index()
        {
            return View(db.UsersSet.ToList());
        }

        // GET: Login/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.UsersSet.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Login/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Login/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Username,Password,Mail")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.UsersSet.Add(users);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(users);
        }

        // GET: Login/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.UsersSet.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Login/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Username,Password,Mail")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        // GET: Login/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.UsersSet.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Login/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Users users = db.UsersSet.Find(id);
            db.UsersSet.Remove(users);
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

        public ActionResult EvalLoginRequest(string requestedUsername, string requestedPassword) {
            //Check if password and username exist.
            Users user = GetUser(requestedUsername);
            if (user == null || user.Password != requestedPassword) {
                // user does not exist!
                ModelState.AddModelError("errorUserNotExist", "User does not exist");
                return View("../Login/Index");
            }

            //Save user for future use.
            Session["currentUser"] = user;

            return RedirectToAction("Index", "Group");
        }

        public ActionResult CreateNewUser(string createUsername, string createPassword, string createMail) {
            Users user = GetUser(createUsername);
            //if user exist.
            if (user != null) {
                ModelState.AddModelError("errorCreateNewUser", "User already exist!");
                return View("../Login/Create");

            } else if (!EvalUsername(createUsername) || !EvalPassword(createPassword) || !EvalMail(createMail)) {
                ModelState.AddModelError("errorCreateNewUser", "Wrong input");
                return View("../Login/Create");
            }

            //Add new user to database.
            user = new Users();
            user.Mail = createMail;
            user.Password = createPassword;
            user.Username = createUsername;
            db.UsersSet.Add(user);
            db.SaveChanges();

            //Save user for future use.
            Session["currentUser"] = user;

            //Redirect to next controller and view.
            return RedirectToAction("Index", "Group");
        }

        private Users GetUser(string username) {
            return db.UsersSet.Find(username);
        }

        private bool EvalPassword(string value) {
            if (value.Length >= 6) {
                bool containsDigit = Regex.IsMatch(value, @"\d");
                bool containsLowLetter = Regex.IsMatch(value, @"[a-z]");
                bool containsUpperLetter = Regex.IsMatch(value, @"[A-Z]");
                if (containsDigit && containsLowLetter && containsUpperLetter) {
                    return true;
                }
            }
            return false;
        }

        private bool EvalUsername(string value) {
            return (value.Length >= 6 && Regex.IsMatch(value, @"[a-zA-Z]"));
        }

        private bool EvalMail(string value) {
            if (value.Length >= 6) {
                 int at = value.IndexOf('@');
                 int dot = value.IndexOf('.');
                 if (at != 0 && dot != 0 && (at < dot)) {
                     string lhsAt = value.Substring(0, at);
                     string rhsAt = value.Substring(at + 1, (dot - at - 1));
                     string rhsDot = value.Substring(dot + 1, value.Length - (dot + 1));
                     if (lhsAt.Length > 0 && rhsAt.Length > 0 && rhsDot.Length > 0) {
                         return true;
                     }
                 }
            }
            return false;
        }
    }
}
