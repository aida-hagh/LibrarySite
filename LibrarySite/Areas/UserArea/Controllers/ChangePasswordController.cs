using DataLayer.ViewModels;
using EntityLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace LibrarySite.Areas.UserArea.Controllers
{

    [Area("UserArea")]
 
    public class ChangePasswordController : Controller
    {
        private readonly UserManager<ApplicationUsers> _userManager;

        public ChangePasswordController(UserManager<ApplicationUsers> userManager)
        {
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            //نمایش اطلاعات کاربر
            var query_fullname = (from u in _userManager.Users where u.Id == _userManager.GetUserId(HttpContext.User) select u).SingleOrDefault();
            ViewBag.fullname = query_fullname.FirstName + " " + query_fullname.LastName;
            ViewBag.phone = query_fullname.UserName;
            ViewBag.Wallet = query_fullname.Wallet;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Changing(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByIdAsync(_userManager.GetUserId(HttpContext.User));
                PasswordVerificationResult passResult =
                    _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, model.OldPassword);

                if (passResult == PasswordVerificationResult.Success)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, model.NewPassword);
                    var result = await _userManager.UpdateAsync(user);

                    if (!result.Succeeded)
                    {
                        ViewBag.msg = "در ثبت اطلاعات مشکلی به وجود آمده است.";
                        ViewBag.alt = "alert-danger";
                        return View("Index");
                    }

                    ViewBag.msg = "رمز عبور شما با موفقیت تغییر کرد.";
                    ViewBag.alt = "alert-success";
                    return View("Index");
                }
                else
                {
                    ViewBag.msg = "رمز عبور قدیمی صحیح نیست.";
                    ViewBag.alt = "alert-danger";
                    return View("Index");
                }
            }
            return View("Index", model);
        }
    }
}
