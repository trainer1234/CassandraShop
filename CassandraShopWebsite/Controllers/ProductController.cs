using CassandraShopWebsite.Models.ProductModels;
using CassandraShopWebsite.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Controllers
{
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["YearSortParm"] = sortOrder == "Year" ? "year_desc" : "Year";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var products = _productRepository.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(product => (product.Name.Contains(searchString)
                                            || product.Manufacture_Name.Contains(searchString)
                                            || product.Description.Contains(searchString))).ToList();
            }
            switch (sortOrder)
            {
                case "name_desc":
                    products = products.OrderByDescending(s => s.Name).ToList();
                    break;
                case "year":
                    products = products.OrderBy(s => s.Manufacture_Year).ToList();
                    break;
                case "year_desc":
                    products = products.OrderByDescending(s => s.Manufacture_Year).ToList();
                    break;
                default:
                    products = products.OrderBy(s => s.Name).ToList();
                    break;
            }
            int pageSize = 3;
            return View(PaginatedList<Product>.Created(products, page ?? 1, pageSize));
        }

        // GET: Product/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = _productRepository.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public IActionResult Create([Bind("Name,Manufacture_Name,Manufacture_Year,Price,Description")] Product product)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _productRepository.Add(product);
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                // Log the error (uncomment ex variable name and write a log)
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system aministrator.");
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Edit")]
        // Create and attach
        // You can use this approach when the web page UI includes all of the fields in the entity and can update any of them.
        public IActionResult Edit(string id, [Bind("Id,Name,Manufacture_Name,Manufacture_Year,Price,Description")] Product product)
        {
            if (Guid.Parse(id) != product.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    _productRepository.Update(product);
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(product);
        }

        public IActionResult Delete(string id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productRepository.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            if (saveChangeError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again, and if the problem persists " +
                                            "see your system administrator.";
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var product = _productRepository.Find(id);
            if (product == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _productRepository.Remove(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction("Delete", new { id = id, saveChangeError = true });
            }
        }

    }
}
