using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MovieWeb.Models;

namespace MovieWeb.Controllers
{
    public class MovieController : Controller
    {
        private MovieDBContext db = new MovieDBContext();
        static int page_now = 1;                // 当前页
        static int page_total = 0;              // 总页数
        static int page_pre = 0;                // 上一页
        static int page_next = 2;               // 下一页
        static int page_show = 5;               // 一页展示数量
        

        public ActionResult Show(int? page_num)
        {
            if (page_num == null)
                page_num = 1;

            page_pre = (int)page_num - 1;
            page_next = (int)page_num + 1;
            page_total = db.Movies.Count() / page_show + 1;
            if (page_pre < 1) page_pre = 1;
            if (page_next > page_total) page_next = page_total;
            ViewBag.page_pre = "/Movie/Show?page_num=" + page_pre;
            ViewBag.page_next = "/Movie/Show?page_num=" + page_next;
            ViewBag.page_tail = "/Movie/Show?page_num=" + page_total;

            var movies = db.Movies.OrderBy(o => o.MovieId).Skip(((int)page_num - 1) * page_show).Take(page_show);
            return View("Show", movies);


            /*
            var GenreQry = from d in db.Movie
                           orderby d.Genre
                           select d.Genre;
            GenreLst.AddRange(GenreQry.Distinct());
            ViewBag.movieGenre = new SelectList(GenreLst);
            var movies = from m in db.Movie
                         select m;
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(s =>; s.Title.Contains(searchString));
            }
            if (!string.IsNullOrEmpty(movieGenre))
            {
                movies = movies.Where(x =>; x.Genre == movieGenre);
            }
            */
        }
        public ActionResult FindMovie()
        {
            return View();
        }
        public ActionResult FindMovie(string title)
        {
            if (String.IsNullOrEmpty(title))
            {
                return View();
            }

            var movies = db.Movies.Where(s =>s.Title.Contains(title));

            return View("FindMovie", movies);
        }


        #region 模板部分
        // GET: Movie
        public ActionResult Index()
        {
            return View(db.Movies.ToList());
        }

        // GET: Movie/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // GET: Movie/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Movie/Create
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MovieId,Type,Actors,Info,Url,Source,StartTime,Address")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Movies.Add(movie);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(movie);
        }

        // GET: Movie/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Edit/5
        // 为了防止“过多发布”攻击，请启用要绑定到的特定属性，有关 
        // 详细信息，请参阅 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MovieId,Type,Actors,Info,Url,Source,StartTime,Address")] Movie movie)
        {
            if (ModelState.IsValid)
            {
                db.Entry(movie).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(movie);
        }

        // GET: Movie/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Movie movie = db.Movies.Find(id);
            if (movie == null)
            {
                return HttpNotFound();
            }
            return View(movie);
        }

        // POST: Movie/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Movie movie = db.Movies.Find(id);
            db.Movies.Remove(movie);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion

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
