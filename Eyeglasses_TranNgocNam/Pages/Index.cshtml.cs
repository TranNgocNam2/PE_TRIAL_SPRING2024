using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Entity;

namespace Eyeglasses_TranNgocNam.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGenericRepository<Eyeglass> _eyeglassRepository;

        public IndexModel(ILogger<IndexModel> logger, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _eyeglassRepository = _unitOfWork.GetRepository<Eyeglass>();
        }

        public List<Eyeglass> Eyeglasses { get; set; } = new List<Eyeglass>();

        public IActionResult OnGet(int minPrice, int maxPrice, string description)
        {
            var role = HttpContext.Session.GetInt32("Role");
            Eyeglasses = _eyeglassRepository.Get(1, 4, x => x.LensType!).OrderByDescending(x => x.CreatedDate).ToList();

            if (role == 1 && (minPrice != 0 || maxPrice != 0 || !string.IsNullOrEmpty(description)))
            {
                if (minPrice != 0 && maxPrice != 0 && !string.IsNullOrEmpty(description))
                {
                    Eyeglasses = _eyeglassRepository
                        .FindByCondition(x => (x.Price <= maxPrice && x.Price >= minPrice)
                                              && (!string.IsNullOrEmpty(x.EyeglassesDescription)
                                                  && x.EyeglassesDescription.Contains(description)), 1, 4 , x => x.LensType!)
                        .OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (minPrice != 0 && maxPrice != 0)
                {
                    Eyeglasses = _eyeglassRepository
                        .FindByCondition(x => x.Price <= maxPrice && x.Price >= minPrice, 1, 4 , x => x.LensType!)
                        .OrderByDescending(x => x.CreatedDate).ToList();
                }
                else if (!string.IsNullOrEmpty(description))
                {
                    Eyeglasses = _eyeglassRepository
                        .FindByCondition(x => !string.IsNullOrEmpty(x.EyeglassesDescription)
                                              && x.EyeglassesDescription.Contains(description), 1, 4 , x => x.LensType!)
                        .OrderByDescending(x => x.CreatedDate).ToList();
                }
            }
            return Page();
        }
    }
}
