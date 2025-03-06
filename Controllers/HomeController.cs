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
        string extension = "";

        if (imageFile != null)
        {
            string[] allowedExtensions = [".jpg", ".jpeg", ".png"];
            extension = Path.GetExtension(imageFile.FileName).ToLower();
            if (!allowedExtensions.Contains(extension))
            {
                ModelState.AddModelError("", "Sadece jpg, jpeg ve png uzantılı doslayaları seçebilirsiniz.");
            }
        }

        if (ModelState.IsValid)
        {
            if (imageFile != null)
            {
                string randomFileName = $"{Guid.NewGuid()}{extension}";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
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
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        Product entity = Repository.Products.FirstOrDefault(prd => prd.ProductId == id)!;
        if (entity == null)
        {
            return NotFound();
        }
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(entity);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int? id, Product model, IFormFile? imageFile)
    {
        if (id != model.ProductId)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            if (imageFile != null)
            {
                string extension = Path.GetExtension(imageFile.FileName);
                string randomFileName = string.Format($"{Guid.NewGuid()}{extension}");
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", randomFileName);
                using (FileStream stream = new(path, FileMode.Create))
                {
                    await imageFile.CopyToAsync(stream);
                }
                model.Image = randomFileName;
            }
            Repository.Editproduct(model);
            return RedirectToAction("Index");
        }
        ViewBag.Categories = new SelectList(Repository.Categories, "CategoryId", "Name");
        return View(model);
    }
}
