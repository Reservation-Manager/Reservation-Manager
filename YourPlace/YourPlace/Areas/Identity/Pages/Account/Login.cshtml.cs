// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using YourPlace.Infrastructure.Data.Entities;
using YourPlace.Core.Services;
using YourPlace.Infrastructure.Data.Enums;

namespace YourPlace.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserServices _userServices;

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, ILogger<LoginModel> logger, UserServices userServices)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
            _userServices = userServices;
        }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string ErrorMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [EmailAddress]
            public string Email { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                Tuple<IdentityResult, User> resultTupple = await _userServices.LogInUserAsync(Input.Email, Input.Password);
                User user = resultTupple.Item2;
                if (user == null || !(await _userManager.CheckPasswordAsync(user, Input.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Невалиден имейл или парола.");
                    return Page();
                }


                if (resultTupple.Item1.Succeeded)
                {
                    // Use the following code if you are calling userManager.PasswordValidators[0].ValidateAsync(..)
                    await _signInManager.SignInAsync(user, new AuthenticationProperties());

                    // Else if you want to validate credentials here, with signInManager:
                    await _signInManager.PasswordSignInAsync(user, Input.Password, Input.RememberMe, lockoutOnFailure: true);
                    if(user != null)
                    {
                        var roles = await _signInManager.UserManager.GetRolesAsync(user);
                        
                        if (roles.Contains(Roles.Manager.ToString()))
                        {
                            return RedirectToAction("Index", "ManagerMenu", new { firstName = user.FirstName, lastName = user.Surname, managerID = user.Id });
                        }
                        if (roles.Contains(Roles.Traveller.ToString()))
                        {
                            return RedirectToAction("ToMainBg", "Home");
                        }
                        if(roles.Contains(Roles.Admin.ToString()))
                        {
                            return RedirectToAction("Index", "Admin");
                        }
                        if(roles.Contains(Roles.Receptionist.ToString()))
                        {
                            return RedirectToAction("Index", "Receptionist");
                        }

                    }
                    _logger.LogInformation("User logged successfully!");
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    string code = resultTupple.Item1.Errors.First().Code;
                    string description = resultTupple.Item1.Errors.First().Description;

                    ModelState.AddModelError(code, description);

                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
