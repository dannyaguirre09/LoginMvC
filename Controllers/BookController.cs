using LibraryTemplate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryTemplate.Controllers
{
    public class BookController : Controller
    {
        // GET: Book
        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public JsonResult GetBooks()
		{
			using (DBModel db = new DBModel())
			{
				var bookList = db.Book.Select(b => new {
					IdBook = b.IdBook,
					NameBook = b.NameBook,
					IdCategory = b.IdCategory,
					DescriptionBook = b.DescriptionBook,
					NameCategory = b.Category.NameCategory
				}).ToList();

				return Json(new { data = bookList }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public ActionResult AddOrEdit(int id = 0)
		{
			DBModel db = new DBModel();
			ViewBag.Categories = new SelectList(db.Category, "IdCategory", "NameCategory");

			if (id == 0)
				return View(new Book());
			else
			{
				return View(db.Book.Where(x => x.IdBook.Equals(id)).FirstOrDefault<Book>());
			}
		}

		[HttpPost]
		public JsonResult AddOrEdit(Book objBook)
		{
			bool success = false;
			string message = "";
			int code = 400;

			try
			{
				using (DBModel db = new DBModel())
				{
					if (objBook.IdBook == 0)
					{
						db.Book.Add(objBook);
						db.SaveChanges();
						message = "Saved Successfully";
					}
					else
					{
						db.Entry(objBook).State = EntityState.Modified;
						db.SaveChanges();
						message = "Updated Successfully";
					}
					success = true;
					code = 200;
				}
			}
			catch (Exception ex)
			{
				message = ex.Message;
			}

			return new JsonResult { Data = new { success = success, message = message, code = code } };

		}

		[HttpPost]
		public ActionResult Delete(int id)
		{
			bool success = false;
			string message = "";
			int code = 400;

			try
			{
				using (DBModel db = new DBModel())
				{
					Book objBook = db.Book.Where(x => x.IdBook == id).FirstOrDefault<Book>();
					db.Book.Remove(objBook);
					db.SaveChanges();
					message = "Deleted Successfully";
					success = true;
					code = 200;
				}
			}
			catch (Exception ex)
			{
				message = ex.Message;
			}

			return new JsonResult { Data = new { success = success, message = message, code = code } };
		}

	}
}