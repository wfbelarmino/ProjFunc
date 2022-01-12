using ProjFunc.Models;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjFunc.Controllers
{
    public class ContaController : Controller
    {
        [AllowAnonymous]
        //Se o usuário passar por uma Url privada, a Url vai constar aqui.
        //vai redirecionar para a tela de login. O paramatro está passando no "login.cshtml".
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginViewModel login, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(login);
            }

            //var achou = (login.Usuario == "wagnersouza" && login.Senha == "123456");
            var achou = UsuarioModel.ValidarUsuario(login.Usuario, login.Senha);

            if (achou)
            {
                FormsAuthentication.SetAuthCookie(login.Usuario, login.LembrarMe);
                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ModelState.AddModelError("", "Login inválido.");
            }

            return View(login);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}