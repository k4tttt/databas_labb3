using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Labb3.Models;
using System.Data.SqlClient;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Labb3.Controllers
{
    public static class StringExtensions
    {
        public static bool Contains(this string source, string toCheck, StringComparison comp)
        {
            return source?.IndexOf(toCheck, comp) >= 0;
        }
    }

    public class PersonController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult InsertPersonForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult InsertPersonForm(PersonDetail pd)
        {
            PersonMethods pm = new PersonMethods();
            int i = 0;
            string error = "";

            i = pm.InsertPerson(pd, out error);
            ViewBag.error = error;
            ViewBag.count = i;

            if (i == 1) { return RedirectToAction("SelectWithDataSet"); }
            else { return View("InsertPerson"); }
        }

        public IActionResult SelectWithDataSet()
        {
            List<PersonDetail> PersonList = new List<PersonDetail>();
            PersonMethods pm = new PersonMethods();
            string error = "";
            PersonList = pm.GetPersonWithDataSet(out error);
            ViewBag.error = error;
            return View(PersonList);
        }

        [HttpGet]
        public IActionResult EditPerson(int Id)
        {
            List<PersonDetail> PersonList = new List<PersonDetail>();
            PersonMethods pm = new PersonMethods();
            string error = "";
            PersonList = pm.GetPersonWithDataSet(out error);
            ViewBag.error = error;
            var person = PersonList.Where(p => p.Id == Id).FirstOrDefault();
            return View(person);
        }

        [HttpPost]
        public IActionResult EditPerson(PersonDetail pd)
        {
            PersonMethods pm = new PersonMethods();
            int i = 0;
            string error = "";

            i = pm.EditPerson(pd, out error);
            ViewBag.error = error;
            ViewBag.count = i;

            if (i == 1) { return RedirectToAction("Filter"); }
            else { return View("EditPerson"); }
        }

        [HttpGet]
        public IActionResult DeletePerson(int Id)
        {
            PersonDetail person = new PersonDetail();
            PersonMethods pm = new PersonMethods();
            person = pm.GetPerson(Id, out string error);
            ViewBag.error = error;
            return View(person);
        }

        [HttpPost]
        public IActionResult DeletePerson(PersonDetail pd)
        {
            PersonMethods pm = new PersonMethods();
            int i = 0;
            string error = "";

            i = pm.DeletePerson(pd, out error);
            ViewBag.error = error;
            ViewBag.count = i;

            if (i == 1) { return RedirectToAction("Filter"); }
            else { return View("DeletePerson"); }
        }

        public IActionResult SelectGenres()
        {
            List<PersonGenres> GenreList = new List<PersonGenres>();
            PersonMethods pm = new PersonMethods();
            string error = "";
            GenreList = pm.GetGenresWithDataSet(out error);
            ViewBag.error = error;
            return View(GenreList);
        }

        [HttpGet]
        public IActionResult Filter()
        {
            PersonMethods pm = new PersonMethods();
            GenreMethods gm = new GenreMethods();

            ViewModelPG viewModel = new ViewModelPG
            {
                PGList = pm.GetGenresWithDataSet(out string errormsg1),
                GDList = gm.GetGenreList(out string errormsg2)
            };
            ViewBag.error = "1: " + errormsg1 + " 2: " + errormsg2;
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Filter(String Genre)
        {
            int i = Convert.ToInt32(Genre);
            ViewData["Genre"] = i;

            PersonMethods pm = new PersonMethods();
            GenreMethods gm = new GenreMethods();

            ViewModelPG viewModel = new ViewModelPG
            {
                PGList = pm.GetGenresWithDataSet(out string errormsg1, i),
                GDList = gm.GetGenreList(out string errormsg2)
            };
            ViewBag.error = "1: " + errormsg1 + " 2: " + errormsg2;
            return View(viewModel);
        }

        public IActionResult Sort(string sortBy)
        {
            PersonMethods pm = new PersonMethods();
            GenreMethods gm = new GenreMethods();

            ViewModelPG viewModel = new ViewModelPG
            {
                PGList = pm.GetGenresWithDataSet(out string errormsg1),
                GDList = gm.GetGenreList(out string errormsg2)
            };

            ViewData["NameSort"] = sortBy == "name" ? "name_desc" : "name";
            ViewData["AgeSort"] = sortBy == "age" ? "age_desc" : "age";
            ViewData["GenreSort"] = sortBy == "genre" ? "genre_desc" : "genre";

            switch (sortBy)
            {
                case "name":
                    viewModel.PGList = viewModel.PGList.OrderBy(p => p.Name);
                    break;
                case "name_desc":
                    viewModel.PGList = viewModel.PGList.OrderByDescending(p => p.Name);
                    break;
                case "date":
                    viewModel.PGList = viewModel.PGList.OrderBy(p => Convert.ToInt16(p.Age));
                    break;
                case "date_desc":
                    viewModel.PGList = viewModel.PGList.OrderByDescending(p => Convert.ToInt16(p.Age));
                    break;
                case "genre":
                    viewModel.PGList = viewModel.PGList.OrderBy(p => p.Genre);
                    break;
                case "genre_desc":
                    viewModel.PGList = viewModel.PGList.OrderByDescending(p => p.Genre);
                    break;
            }

            return View(viewModel);
        }

        public IActionResult Search(String SearchString)
        {
            PersonMethods pm = new PersonMethods();
            GenreMethods gm = new GenreMethods();

            ViewModelPG viewModel = new ViewModelPG
            {
                PGList = pm.GetGenresWithDataSet(out string errormsg1),
                GDList = gm.GetGenreList(out string errormsg2)
            };

            if (!String.IsNullOrEmpty(SearchString))
            {
                viewModel.PGList = viewModel.PGList.Where(p => (p.Genre.Contains(SearchString, StringComparison.OrdinalIgnoreCase)) || (p.Name.Contains(SearchString, StringComparison.OrdinalIgnoreCase)));
            }

            return View(viewModel);
        }

        public IActionResult Details(int Id)
        {
            PersonDetail person = new PersonDetail();
            PersonMethods pm = new PersonMethods();

            person = pm.GetPerson(Id, out string error);
            ViewBag.error = error;

            return View(person);
        }
    }
}

