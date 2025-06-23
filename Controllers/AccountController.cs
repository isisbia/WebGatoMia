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
            return View("Register"); // Exibe o formulário de registro
        }

        // --- POST: /Account/Register ---
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            // 1. Valida o modelo recebido com base nas Data Annotations do RegisterViewModel
            if (!ModelState.IsValid)
            {
                return View(model); // Se a validação falhar, retorna a View com os erros de validação
            }

            try
            {
                // 2. Chama o serviço para lidar com a lógica de registro do usuário
                var registrationSuccessful = await _userService.RegisterUserAsync(model);

                // 3. Lida com resultados específicos do registro (ex: e-mail já cadastrado)
                if (!registrationSuccessful)
                {
                    ModelState.AddModelError("", "O e-mail fornecido já está cadastrado. Por favor, utilize outro.");
                    return View(model);
                }

                // 4. Registro bem-sucedido: Opcionalmente, pode-se fazer login automático ou redirecionar para a página de login
                // Para simplicidade e fluxo comum, vamos redirecionar para a página de login.
                TempData["SuccessMessage"] = "Cadastro realizado com sucesso! Agora você pode fazer login.";
                return RedirectToAction("Login", "Home");
            }
            catch (Exception ex)
            {
                // 5. Captura quaisquer erros inesperados durante o registro
                ModelState.AddModelError("", $"Ocorreu um erro inesperado ao tentar registrar. Por favor, tente novamente. Detalhes: {ex.Message}");
                // Em uma aplicação real, você também logaria esta exceção: _logger.LogError(ex, "Erro durante o registro de usuário.");
                return View(model);
            }
        }

        // --- GET: /Account/Login ---
        // Permite redirecionar de volta para a URL original após o login bem-sucedido (ex: se o usuário tentou acessar uma página [Authorize])
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Passa a URL de retorno para a view
            return View(); // Exibe o formulário de login
        }

        // --- POST: /Account/Login ---
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl; // Mantém a URL de retorno se a validação falhar

            // 1. Valida o modelo recebido
            if (!ModelState.IsValid)
            {
                return View(model); // Se a validação falhar, retorna a view com os erros
            }

            // 2. Autentica as credenciais do usuário através do serviço
            var user = await _userService.AuthenticateUserAsync(model.Email, model.Password);

            // 3. Lida com credenciais inválidas
            if (user == null)
            {
                ModelState.AddModelError("", "Usuário ou senha inválidos. Por favor, verifique suas credenciais.");
                return View(model);
            }

            // --- 4. Usuário autenticado: Cria o cookie de autenticação (sessão) ---
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Identificador único para o usuário
                new Claim(ClaimTypes.Name, user.Name),                   // Nome de exibição do usuário
                new Claim(ClaimTypes.Email, user.Email)                  // E-mail do usuário
                // Adicione a claim de role com base no UserType, garantindo que UserType seja carregado (ex: via Include no serviço)
                // Se UserType puder ser nulo, forneça um valor padrão ou trate isso.
                // Exemplo: new Claim(ClaimTypes.Role, user.UserType?.Name ?? "UsuarioPadrao")
            };

            // Configura o ClaimsIdentity com o esquema de autenticação
            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Define as propriedades de autenticação (ex: persistência, expiração)
            var authProperties = new AuthenticationProperties
            {
                IsPersistent = model.RememberMe, // Se 'Lembrar-me' estiver marcado, torna o cookie persistente
                ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddDays(14) : DateTimeOffset.UtcNow.AddMinutes(30) // Expiração do cookie
            };

            // Realiza o login do usuário, criando o cookie de autenticação
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // 5. Redireciona o usuário após o login bem-sucedido
            // Redireciona para a URL original, se fornecida e for uma URL local (verificação de segurança)
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            // Caso contrário, redireciona para a página inicial padrão
            return RedirectToAction("Index", "Home");
        }

        // --- POST: /Account/Logout ---
        // Usar POST para logout é uma boa prática de segurança (proteção CSRF)
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public async Task<IActionResult> Logout()
        {
            // Realiza o logout do usuário, excluindo o cookie de autenticação
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            TempData["InfoMessage"] = "Você foi desconectado com sucesso."; // Mensagem informativa
            return RedirectToAction("Login", "Home"); // Redireciona para a página de login após o logout
        }
    }
}
