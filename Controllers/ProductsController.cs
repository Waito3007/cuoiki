using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cuoiki.Data;
using cuoiki.Models;

namespace cuoiki.Controllers
{
    // Controller quản lý các thao tác liên quan đến sản phẩm
    public class ProductsController : Controller
    {
        // Inject DbContext để thao tác với database
        private readonly SportProductContext _context;

        // Constructor nhận DbContext thông qua Dependency Injection
        public ProductsController(SportProductContext context)
        {
            _context = context;
        }
         // Hiển thị danh sách sản phẩm
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.OrderBy(p => p.ProductId).ToListAsync();
            return View(products);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewBag.Categories = new[] { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
            return View();
        }

        // POST: Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            var allowedCategories = new[] { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
            if (!allowedCategories.Contains(product.Category))
            {
                ModelState.AddModelError("Category", "Category phải là một trong các giá trị: Vợt, Bóng, Cầu, Đệm, Quần áo.");
            }
            if (product.ManufacturingDate >= DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("ManufacturingDate", "ManufacturingDate phải nhỏ hơn ngày hiện tại.");
            }
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = allowedCategories;
            return View(product);
        }
        // Action trả về view hiển thị chi tiết sản phẩm đầu tiên (theo ProductId tăng dần)
        public async Task<IActionResult> FirstDetail()
        {
            // Lấy sản phẩm đầu tiên trong bảng Products
            var product = await _context.Products.OrderBy(p => p.ProductId).FirstOrDefaultAsync();
            if (product == null)
            {
                // Nếu không có sản phẩm nào thì trả về NotFound
                return NotFound();
            }
            // Trả về view kèm theo model là sản phẩm vừa lấy được
            return View(product);
        }

        // GET: Products/Edit/5
        // Hiển thị form chỉnh sửa sản phẩm
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            // Truyền danh sách category cho view
            ViewBag.Categories = new[] { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
            return View(product);
        }

        // POST: Products/Edit/5
        // Xử lý lưu thông tin chỉnh sửa sản phẩm với các ràng buộc
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            // Kiểm tra Category hợp lệ
            var allowedCategories = new[] { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
            if (!allowedCategories.Contains(product.Category))
            {
                ModelState.AddModelError("Category", "Category phải là một trong các giá trị: Vợt, Bóng, Cầu, Đệm, Quần áo.");
            }

            // Kiểm tra ManufacturingDate hợp lệ
            if (product.ManufacturingDate >= DateOnly.FromDateTime(DateTime.Now))
            {
                ModelState.AddModelError("ManufacturingDate", "ManufacturingDate phải nhỏ hơn ngày hiện tại.");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Products.Any(e => e.ProductId == product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(FirstDetail));
            }
            ViewBag.Categories = allowedCategories; // allowedCategories là List<string>
            return View(product);
        }
    }
}
