using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Repository;
using Repository.Entity;

namespace Eyeglasses_TranNgocNam.Pages;

public class Login : PageModel
{
    [BindProperty] public string Email { get; set; } = null!;
    [BindProperty] public string Password { get; set; } = null!;
    public string ErrorMessage { get; set; } = string.Empty;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericRepository<StoreAccount> _storeAccountRepository;
    
    public Login(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
        _storeAccountRepository = _unitOfWork.GetRepository<StoreAccount>();
    }
    public void OnGet()
    {
        
    }
    
    public async Task<IActionResult> OnPost()
    {
        var user = await _storeAccountRepository
            .FindByCondition(x => x.EmailAddress == Email && x.AccountPassword == Password 
                                                          && (x.Role == 1||x.Role == 2) ) 
            .FirstOrDefaultAsync();

        if (user is null)
        {
            ErrorMessage = "You do not have permission to do this function!";
            return Page();
        }
        
        // Set session variable upon successful login
        HttpContext.Session.SetString("Email", user.EmailAddress!);
        HttpContext.Session.SetInt32("Role", user.Role!);

        // Redirect to a different page upon successful login
        return RedirectToPage("/Index");
    }
}