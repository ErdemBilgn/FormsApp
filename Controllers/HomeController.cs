using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FormsApp.Controllers;

public class HomeController : Controller
{

    public IActionResult Index(string searchTerm, string category)
    {
        List<Product> products = Repository.Products;
        if (!String.IsNullOrEmpty(searchTerm))
        {
            ViewBag.SearchTerm = searchTerm;
            products = products.Where(prd => prd.Name.ToLowerInvariant().Contains(searchTerm)).ToList();
        }


        if (!String.IsNullOrEmpty(category) && category != "0")
        {

            products = products.Where(prd => prd.CategoryId == int.Parse(category)).ToList();
        }
        // ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name", category);

        ProductViewModel model = new ProductViewModel
        {
            Products = products,
            Categories = Repository.Categories,
            SelectedCategory = category
        };
        return View(model);
    }

    public IActionResult Privacy()
    {
        return View();
    }
}
