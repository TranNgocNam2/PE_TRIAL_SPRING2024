using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Entity;

namespace Eyeglasses_TranNgocNam.Pages;

public class Update : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Eyeglass> _eyeglassRepository;
    private readonly IGenericRepository<LensType> _lenTypeRepository;

    public Update(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _eyeglassRepository = _unitOfWork.GetRepository<Eyeglass>();
        _lenTypeRepository = _unitOfWork.GetRepository<LensType>();
    }

    [BindProperty]
    public Eyeglass Eyeglass { get; set; }
    
    public List<LensType> LensTypes { get; set; }
    
    public IActionResult OnGet(int id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
        {
            return RedirectToPage("/Index");
        }
        
        var eyeGlass = _eyeglassRepository.GetById(id);
        if (eyeGlass is null)
        {
            return NotFound();
        }

        Eyeglass = eyeGlass;
        LensTypes = _lenTypeRepository.Get().ToList();
        return Page();
    }
    
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }
        
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
        {
            ModelState.AddModelError("Role", "Unauthorized");
            LensTypes = _lenTypeRepository.Get().ToList();
            return Page();
        }
        
        if (Eyeglass.Quantity is < 0 or > 999)
        {
            ModelState.AddModelError("Eyeglass.Quantity", "Quantity must be between 0 and 999.");
            LensTypes = _lenTypeRepository.Get().ToList();
            return Page();
        }

        if (Eyeglass.EyeglassesName.Length <= 10)
        {
            ModelState.AddModelError("Eyeglass.EyeglassesName", "Eyeglasses Name must be greater than 10 characters.");
            LensTypes = _lenTypeRepository.Get().ToList();
            return Page();
        }

        var words = Eyeglass.EyeglassesName.Split(' ');
        if (words.Any(word => char.IsLower(word[0])))
        {
            ModelState.AddModelError("Eyeglass.EyeglassesName", "Each word of the Eyeglasses Name must begin with a capital letter.");
            LensTypes = _lenTypeRepository.Get().ToList();
            return Page();
        }

        _eyeglassRepository.Update(Eyeglass);
        _unitOfWork.Save();

        return RedirectToPage("/Index");
    }
}