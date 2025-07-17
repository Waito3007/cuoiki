using Microsoft.AspNetCore.Mvc;

namespace cuoiki.ViewComponents
{
    public class CategorySelectorViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string selectedCategory)
        {
            IEnumerable<string> categories = new[] { "Vợt", "Bóng", "Cầu", "Đệm", "Quần áo" };
            return View((Categories: categories, SelectedCategory: selectedCategory));
        }
    }
}
