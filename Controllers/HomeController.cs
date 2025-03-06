using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using FormsApp.Models;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace FormsApp.Controllers;

public class HomeController : Controller
{
    [HttpGet]
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

    [HttpGet]
    public IActionResult Create()
    {
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Product model, IFormFile imageFile)
    {
        if (imageFile != null)

        {
            var extension = Path.GetExtension(imageFile.FileName).ToLower();
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Sadece jpg, jpeg ve png uzantılı doslayaları seçebilirsiniz.");
            }
            else
            {
                var randomFileName = $"{Guid.NewGuid()}{extension}";
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                if (ModelState.IsValid)
                {
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    model.Image = randomFileName;
                    model.ProductId = Repository.Products.Count + 1;
                    Repository.CreateProduct(model);
                    return RedirectToAction("Index");
                }
            }
        }
        else
        {
            ModelState.AddModelError("", "Lütfen bir resim seçiniz.");
        }

        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }
}
