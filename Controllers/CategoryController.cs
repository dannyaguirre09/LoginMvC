using LibraryTemplate.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryTemplate.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category
        public ActionResult Index()
        {
            return View();
        }

		[HttpGet]
		public JsonResult GetCategories()
		{
			using (DBModel db = new DBModel())
			{
				var categoryList = db.Category.Select(b => new {
					IdCategory = b.IdCategory,
					NameCategory = b.NameCategory,
					DescriptionCategory = b.DescriptionCategory
				}).ToList();

				return Json(new { data = categoryList }, JsonRequestBehavior.AllowGet);
			}
		}

		[HttpGet]
		public ActionResult AddOrEdit(int id = 0)
		{
			if (id == 0)
				return View(new Category());
			else
			{
				using (DBModel db = new DBModel())
				{
					return View(db.Category.Where(x => x.IdCategory.Equals(id)).FirstOrDefault<Category>());
				}
			}
		}

		[HttpPost]
		public JsonResult AddOrEdit(Category objCategory)
		{
			bool success = false;
			string message = "";
			int code = 400;

			try
			{
				using (DBModel db = new DBModel())
				{
					if (objCategory.IdCategory == 0)
					{
						db.Category.Add(objCategory);
						db.SaveChanges();
						message = "Saved Successfully";
					}
					else
					{
						db.Entry(objCategory).State = EntityState.Modified;
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
					Category objCategory = db.Category.Where(x => x.IdCategory.Equals(id)).FirstOrDefault<Category>();
					db.Category.Remove(objCategory);
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