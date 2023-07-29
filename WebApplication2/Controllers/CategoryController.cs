using Microsoft.AspNetCore.Mvc;
using WebApplication2.Data;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
    
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories.ToList();
            return View(objCategoryList);
        }

        // Кнопка Create New Category
        //GET
        public IActionResult Create()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)//Здесь obj - просто название экземпляра модели Category
        {
            if (obj.Name == obj.DisplayOrder.ToString()) //если оба поля совпали, то выдаем ошибку
            {
                // Изменяем текст вывода ошибки в поле Name
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            }    
            if (ModelState.IsValid) //проверяем чтобы не было пустых ячеек
            {
                _db.Categories.Add(obj);
                _db.SaveChanges();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Кнопка Edit
        //GET
        public IActionResult Edit(int? id)
        {
            if (id==null || id==0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id); // Выделяем строку, соответствующую данному id
            
            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString()) //если оба поля совпали, то выдаем ошибку
            {
                // Изменяем текст вывода ошибки в поле Name
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            }
            if (ModelState.IsValid) //проверяем чтобы не было пустых ячеек
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View(obj);
        }

        // Кнопка Delete
        //GET
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id); // Выделяем строку, соответствующую данному id

            if (categoryFromDb == null)
            {
                return NotFound();
            }

            return View(categoryFromDb);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var categoryFromDb = _db.Categories.FirstOrDefault(c => c.Id == id); // Выделяем строку, соответствующую данному id
            if (categoryFromDb == null)
            {
                return NotFound();
            }
            _db.Categories.Remove(categoryFromDb);
            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
