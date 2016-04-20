using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC5Skeleton;

namespace MVC5Skeleton.Controllers
{
    [Authorize]
    public class VirtualRoomsController : Controller
    {
        private EntityDataModelContainer db = new EntityDataModelContainer();

        // GET: VirtualRooms
        public ActionResult Index()
        {
            return View(db.VirtualRoomSet.ToList());
        }

        // GET: VirtualRooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirtualRoom virtualRoom = db.VirtualRoomSet.Find(id);
            if (virtualRoom == null)
            {
                return HttpNotFound();
            }
            return View(virtualRoom);
        }

        // GET: VirtualRooms/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VirtualRooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name")] VirtualRoom virtualRoom)
        {
            if (ModelState.IsValid)
            {
                db.VirtualRoomSet.Add(virtualRoom);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(virtualRoom);
        }

        // GET: VirtualRooms/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirtualRoom virtualRoom = db.VirtualRoomSet.Find(id);
            if (virtualRoom == null)
            {
                return HttpNotFound();
            }
            return View(virtualRoom);
        }

        // POST: VirtualRooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name")] VirtualRoom virtualRoom)
        {
            if (ModelState.IsValid)
            {
                db.Entry(virtualRoom).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(virtualRoom);
        }

        // GET: VirtualRooms/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            VirtualRoom virtualRoom = db.VirtualRoomSet.Find(id);
            if (virtualRoom == null)
            {
                return HttpNotFound();
            }
            return View(virtualRoom);
        }

        // POST: VirtualRooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VirtualRoom virtualRoom = db.VirtualRoomSet.Find(id);
            db.VirtualRoomSet.Remove(virtualRoom);
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
    }
}
