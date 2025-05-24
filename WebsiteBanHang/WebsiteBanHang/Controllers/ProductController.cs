using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebBanHang.Repository_Pattern;
using WebBanHang.Models;


public class ProductController : Controller
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    [HttpGet] 
    public IActionResult Add()
    {
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    // [HttpPost]
    // public IActionResult Add(Product product)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         _productRepository.Add(product);
    //         return RedirectToAction("Index");
    //     }
    //     return View(product);
    // }

    
    [HttpPost]
    [ValidateAntiForgeryToken] 
    public async Task<IActionResult> Add(Product product, IFormFile imageUrl, List<IFormFile> imageUrls)
    {
        
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");

        if (ModelState.IsValid)
        {
            if (imageUrl != null)
            {
                product.ImageUrl = await SaveImage(imageUrl);
            }
            if (imageUrls != null && imageUrls.Any()) 
            {
                product.ImageUrls = new List<string>();
                foreach (var file in imageUrls)
                {
                    product.ImageUrls.Add(await SaveImage(file));
                }
            }
            _productRepository.Add(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }

    public IActionResult Index()
    {
        var products = _productRepository.GetAll();
        return View(products);
    }

    public IActionResult Display(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    [HttpGet] 
    public IActionResult Update(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId); 
        return View(product);
    }

    [HttpPost]
    [ValidateAntiForgeryToken] 
    public IActionResult Update(Product product)
    {
        
        var categories = _categoryRepository.GetAllCategories();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", product.CategoryId); 

        if (ModelState.IsValid)
        {
            _productRepository.Update(product);
            return RedirectToAction("Index");
        }
        return View(product);
    }

    [HttpGet] 
    public IActionResult Delete(int id)
    {
        var product = _productRepository.GetById(id);
        if (product == null)
        {
            return NotFound();
        }
        return View(product);
    }

    
    [HttpPost, ActionName("Delete")] 
    [ValidateAntiForgeryToken] 
    public IActionResult DeleteConfirmed(int id)
    {
        _productRepository.Delete(id);
        return RedirectToAction("Index");
    }

    private async Task<string> SaveImage(IFormFile image)
    {
        
        var imagesFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
        if (!Directory.Exists(imagesFolder))
        {
            Directory.CreateDirectory(imagesFolder);
        }

        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName); 
        var savePath = Path.Combine(imagesFolder, fileName);

        using (var fileStream = new FileStream(savePath, FileMode.Create))
        {
            await image.CopyToAsync(fileStream);
        }
        return "/images/" + fileName; 
    }
}