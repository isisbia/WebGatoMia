using Microsoft.AspNetCore.Mvc;
using WebGatoMia.Models.ViewModels;
using WebGatoMia.Services.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
namespace WebGatoMia.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        // --- GET: /Account/Register ---
        [HttpGet]
        public IActionResult Register()
        {
            return View("Register"); // Exibe o formul�rio de registro
        }

        // --- POST: /Account/Register ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // 1. Valida o modelo recebido com base nas Data Annotations do RegisterViewModel
            if (!ModelState.IsValid)
            {
                return View(model); // Se a valida��o falhar, retorna a View com os erros de valida��o
            }

            try
            {
                // 2. Chama o servi�o para lidar com a l�gica de registro do usu�rio
                var registrationSuccessful = await _userService.RegisterUserAsync(model);

                // 3. Lida com resultados espec�ficos do registro (ex: e-mail j� cadastrado)
                if (!registrationSuccessful)
                {
                    ModelState.AddModelError("", "O e-mail fornecido j� est� cadastrado. Por favor, utilize outro.");
                    return View(model);
                }

                // 4. Registro bem-sucedido: Opcionalmente, pode-se fazer login autom�tico ou redirecionar para a p�gina de login
                // Para simplicidade e fluxo comum, vamos redirecionar para a p�gina de login.
                TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Agora voc� pode fazer login.";
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                // 5. Captura quaisquer erros inesperados durante o registro
                ModelState.AddModelError("", $"Ocorreu um erro inesperado ao tentar registrar. Por favor, tente novamente. Detalhes: {ex.Message}");
                // Em uma aplica��o real, voc� tamb�m logaria esta exce��o: _logger.LogError(ex, "Erro durante o registro de usu�rio.");
                return View(model);
            }
        }

        // --- GET: /Account/Login ---
        // Permite redirecionar de volta para a URL original ap�s o login bem-sucedido (ex: se o usu�rio tentou acessar uma p�gina [Authorize])
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Passa a URL de retorno para a view
            return View(); // Exibe o formul�rio de login
        }

        // --- POST: /Account/Login ---
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Mant�m a URL de retorno se a valida��o falhar

            // 1. Valida o modelo recebido
            if (!ModelState.IsValid)
            {
                return View(model); // Se a valida��o falhar, retorna a view com os erros
            }

            // 2. Autentica as credenciais do usu�rio atrav�s do servi�o
            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);

            // 3. Lida com credenciais inv�lidas
            if (user == null)
            {
                ModelState.AddModelError("", "Usu�rio ou senha inv�lidos. Por favor, verifique suas credenciais.");
                return View(model);
            }

            // --- 4. Usu�rio autenticado: Cria o cookie de autentica��o (sess�o) ---
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Identificador �nico para o usu�rio
                new Claim(ClaimTypes.Name, user.Name),                   // Nome de exibi��o do usu�rio
                new Claim(ClaimTypes.Email, user.Email)                  // E-mail do usu�rio
                // Adicione a claim de role com base no UserType, garantindo que UserType seja carregado (ex: via Include no servi�o)
                // Se UserType puder ser nulo, forne�a um valor padr�o ou trate isso.
                // Exemplo: new Claim(ClaimTypes.Role, user.UserType?.Name ?? "UsuarioPadrao")
            };

            // Configura o ClaimsIdentity com o esquema de autentica��o
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Define as propriedades de autentica��o (ex: persist�ncia, expira��o)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe, // Se 'Lembrar-me' estiver marcado, torna o cookie persistente
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddMinutes(30) // Expira��o do cookie
            };

            // Realiza o login do usu�rio, criando o cookie de autentica��o
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // 5. Redireciona o usu�rio ap�s o login bem-sucedido
            // Redireciona para a URL original, se fornecida e for uma URL local (verifica��o de seguran�a)
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            // Caso contr�rio, redireciona para a p�gina inicial padr�o
            return RedirectToAction("Index", "Home");
        }

        // --- POST: /Account/Logout ---
        // Usar POST para logout � uma boa pr�tica de seguran�a (prote��o CSRF)
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Logout()
        {
            // Realiza o logout do usu�rio, excluindo o cookie de autentica��o
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["InfoMessage"] = "Voc� foi desconectado com sucesso."; // Mensagem informativa
            return RedirectToAction("Login", "Home"); // Redireciona para a p�gina de login ap�s o logout
        }
    }
}
