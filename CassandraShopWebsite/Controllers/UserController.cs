using Cassandra;
using CassandraShopWebsite.Models.AccountModels;
using CassandraShopWebsite.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CassandraShopWebsite.Controllers
{
    public class UserController : Controller
    {
        private IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["UserNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            ViewData["FullNameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "fullname_desc" : "fullname";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            var users = _userRepository.GetAll();
            if (!String.IsNullOrEmpty(searchString))
            {
                users = users.Where(user => (user.UserName.Contains(searchString)
                                            || user.FullName.Contains(searchString))).ToList();
            }
            switch (sortOrder)
            {
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName).ToList();
                    break;
                case "Date":
                    users = users.OrderBy(s => s.Birthday).ToList();
                    break;
                case "date_desc":
                    users = users.OrderByDescending(s => s.Birthday).ToList();
                    break;
                case "fullname_desc":
                    users = users.OrderByDescending(s => s.FullName).ToList();
                    break;
                case "fullname":
                    users = users.OrderBy(s => s.FullName).ToList();
                    break;
                default:
                    users = users.OrderBy(s => s.UserName).ToList();
                    break;
            }
            int pageSize = 3;
            return View(PaginatedList<User>.Created(users, page ?? 1, pageSize));
        }

        // GET: User/Details/5
        public IActionResult Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.Find(id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // GET: User/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        [HttpPost]
        public IActionResult Create([Bind("UserName,FullName,BirthdayTemp,Password")] User user)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    user.Birthday = LocalDate.Parse(user.BirthdayTemp.ToString("yyyy-MM-dd"));
                    _userRepository.Add(user);
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
            return View(user);
        }

        // GET: User/Edit/5
        public IActionResult Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.Find(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("Edit")]
        // Create and attach
        // You can use this approach when the web page UI includes all of the fields in the entity and can update any of them.
        public IActionResult Edit(string id, [Bind("Id,UserName,FullName,BirthdayTemp,Password")] User user)
        {
            if (Guid.Parse(id) != user.Id)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    user.Birthday = LocalDate.Parse(user.BirthdayTemp.ToString("yyyy-MM-dd"));
                    _userRepository.Update(user);
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
            return View(user);
        }

        public IActionResult Delete(string id, bool? saveChangeError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = _userRepository.Find(id);
            if (user == null)
            {
                return NotFound();
            }

            if (saveChangeError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] = "Delete failed. Try again, and if the problem persists " +
                                            "see your system administrator.";
            }

            return View(user);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            var user = _userRepository.Find(id);
            if (user == null)
            {
                return RedirectToAction("Index");
            }

            try
            {
                _userRepository.Remove(id);
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
