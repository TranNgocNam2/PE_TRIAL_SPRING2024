using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.Entity;

namespace Eyeglasses_TranNgocNam.Pages;

public class Delete : PageModel
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<Eyeglass> _eyeglassRepository;

    public Delete(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _eyeglassRepository = _unitOfWork.GetRepository<Eyeglass>();
    }

    [BindProperty]
    public Eyeglass Eyeglass { get; set; }
    public string LensTypeName { get; set; }
    public IActionResult OnGet(int id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
        {
            return RedirectToPage("/Index");
        }
        var eyeGlass = _eyeglassRepository.FindByCondition(x => x.EyeglassesId == id, 
                x => x.LensType!)
            .FirstOrDefault();
        if (eyeGlass is null)
        {
            return NotFound();
        }

        Eyeglass = eyeGlass;
        LensTypeName = Eyeglass.LensType!.LensTypeName;
        return Page();
    }
    public IActionResult OnPost(int id)
    {
        var role = HttpContext.Session.GetInt32("Role");
        if (role != 1)
        {
            return RedirectToPage("/Index");
        }
        
        _eyeglassRepository.Delete(Eyeglass);
        _unitOfWork.Save();

        return RedirectToPage("/Index");
    }
    
}