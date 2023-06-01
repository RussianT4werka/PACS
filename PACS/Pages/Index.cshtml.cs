using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using PACS.DB;
using PACS.Tools;

namespace PACS.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly pacsContext _pacsContext;

        public string Login { get; set; }
        public string Password { get; set; }
        public string Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger, pacsContext pacsContext)
        {
            _logger = logger;
            _pacsContext = pacsContext;
        }

        public IActionResult OnPost(string login, string password)
        {
            

            if(string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                Message = "Не все поля заполнены";
                return null;
            }
            else
            {
                var admin = _pacsContext.Admins.FirstOrDefault(s => s.Login == login && s.Password == Hash.HashPass(password));

                if (admin != null)
                {
                    return RedirectToPage("ListPass", Session.CreateSession(admin));
                }
                else
                {
                    Message = "Неверный логин или пароль";
                    return null;
                }
                //Для создания хешированного пароля
                /*var User = _pacsContext.Admins.FirstOrDefault(s => s.Id == 1);
                if (User != null)
                {
                    User.Password = Hash.HashPass(password);
                    _pacsContext.Admins.Update(User);
                    _pacsContext.SaveChanges();
                    return null;
                }
                else
                {
                    return null;
                }*/
            }
        }
    }
}