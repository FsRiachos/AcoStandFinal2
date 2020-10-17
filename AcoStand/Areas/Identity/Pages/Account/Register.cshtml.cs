using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using AcoStand.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace AcoStand.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext context;

        public RegisterModel(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _roleManager = roleManager;
            this.context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Display(Name = "Nome do Utilizador")]
            [Required(ErrorMessage = "Preenchimento obrigatório.")]
            [StringLength(75, ErrorMessage = "O {0} deverá conter {1} caracteres no máximo.")]
            [RegularExpression("[A-ZÁÉÍÓÚ][a-záéíóúàèÌòùãõîôûâç]+(( | e | de | do | das | da | dos |-|')[A-ZÁÉÍÓÚ][a-zzáéíóúàèÌòùãõîôûâç]+)*",
                ErrorMessage = "P.f verifique o {0} introduzido. Este deve ser composto por primeiro e ultimo nome. Cada palavra deve começar com Maiúscula. Não são aceites caracteres numéricos.")]
            public string Nome { get; set; }

            /// <summary>
            /// Username do utilizador
            /// </summary>
            [Display(Name = "Username")]
            public string Username { get; set; }

            /// <summary>
            /// Localidade do Utilizador 
            /// </summary>
            [Required(ErrorMessage = "Preenchimento obrigatório.")]
            [StringLength(30, ErrorMessage = "A {0} deverá conter {1} caracteres no máximo.")]
            [RegularExpression("[A-ZÁÉÍÓÚ][a-záéíóúàèÌòùãõîôûâç]+(( | e | de | do | das | da | dos |-|')[A-ZÁÉÍÓÚ][a-zzáéíóúàèÌòùãõîôûâç]+)*",
                ErrorMessage = "A {0} só pode conter letras. Cada palavra deve começar com Maiúscula.")]
            public string Localidade { get; set; }

            /// <summary>
            /// Sexo do Utilizador
            /// </summary>
            [Required]
            [StringLength(9)]
            [RegularExpression("Masculino|Feminino", ErrorMessage = "Introduzir: Masculino ou Feminino")]
            [Display(Name = "Sexo")]
            public string Sexo { get; set; }


            /// <summary>
            /// Data Nascimento
            /// </summary>
            [Required(ErrorMessage = "Preenchimento obrigatório.")]
            [DataType(DataType.Date)]
            [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
            [Display(Name = "Data de Nascimento")]
            public DateTime DataNasc { get; set; }

            public string Name { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            ViewData["roles"] = _roleManager.Roles.ToList();
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            var role = _roleManager.FindByIdAsync(Input.Name).Result;
            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.Email, Email = Input.Email };
                var result = await _userManager.CreateAsync(user, Input.Password);
                var roles = await _userManager.AddToRoleAsync(user, "User");
                if (result.Succeeded && roles.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    
                    
                    context.Utilizadores.Add(new Models.Utilizadores { Nome = Input.Nome, Username = Input.Username, Sexo = Input.Sexo, Localidade = Input.Localidade, DataNasc = Input.DataNasc, UserFK = user.Id });
                    await context.SaveChangesAsync();

                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
