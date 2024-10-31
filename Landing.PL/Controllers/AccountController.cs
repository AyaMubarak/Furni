using Landing.PL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = new IdentityUser { UserName = model.Email, Email = model.Email };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
        return View(model);
    }

    // GET: /Account/Login
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

	// POST: /Account/Login
	[HttpPost]
	[ValidateAntiForgeryToken]
	public async Task<IActionResult> Login(LoginViewModel model)
	{
		if (ModelState.IsValid)
		{
			var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
			if (result.Succeeded)
			{
				// Get the user object to check their role
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user != null)
				{
					// Check user roles
					if (await _userManager.IsInRoleAsync(user, "Admin"))
					{
						// Redirect to Dashboard/Dashboard for Admin
						return RedirectToAction("Dashboard", "Dashboard");
					}
					else if (await _userManager.IsInRoleAsync(user, "User"))
					{
						// Redirect to Home for regular User
						return RedirectToAction("Index", "Home");
					}
					else
					{
						// Optional: Redirect to a default page for users without a specific role
						return RedirectToAction("Index", "Home");
					}
				}
			}
			ModelState.AddModelError(string.Empty, "Invalid login attempt.");
		}

		// If we got this far, something failed; redisplay form
		return View(model);
	}


	// POST: /Account/Logout
	[HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    // Redirects to local URL
    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        return RedirectToAction("Index", "Home");
    }
    public IActionResult ForgetPassword()
    {
        return View();
    }
    public async Task<IActionResult> SendRestPasswordUrl(ForgetPassword model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var resetPasswordUrl = Url.Action("ResetPassword", "Account", new { token = token, email = model.Email }, "https", "localhost:44304");

                var email = new Email
                {
                    Subject = "Reset Password",
                    Recivers = model.Email,
                    Body = $"Please reset your password by clicking the following link: {resetPasswordUrl}"
                };

                EmailSettings.SendEmail(email);
            }
        }

        return View("ForgetPassword", model);
    }
    // GET: /Account/ResetPassword
    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
    {
        if (string.IsNullOrEmpty(token) || string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Error", "Home"); // Handle invalid token or email
        }

        return View(new ResetPasswordViewModel { Token = token, Email = email });
    }

    // POST: /Account/ResetPassword
    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation", "Account");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
        }

        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }
}

