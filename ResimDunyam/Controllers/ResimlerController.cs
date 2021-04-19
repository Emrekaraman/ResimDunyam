using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using ResimDunyam.Data;
using ResimDunyam.Models;
using ResimDunyam.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ResimDunyam.Controllers
{
    // https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-5.0
    // https://www.c-sharpcorner.com/article/upload-and-display-image-in-asp-net-core-3-1/
    [Authorize]
    public class ResimlerController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ResimlerController(ApplicationDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Yeni()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public IActionResult Yeni(YeniResimViewModel model)
        {
            //https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (ModelState.IsValid)
            {
                var ext = Path.GetExtension(model.Resim.FileName);
                var dosyaAd = Guid.NewGuid() + ext;
                var dosyaYolu = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", dosyaAd);

                using (var stream = new FileStream(dosyaYolu, FileMode.Create))
                {
                    model.Resim.CopyTo(stream);
                }
                Resim resim = new Resim()
                {
                    IdentityUserId = userId,
                    ResimYolu = dosyaAd,
                    YuklenmeZamani = DateTime.Now
                };
                _db.Resimler.Add(resim);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Sil(int id)
        {
            //https://stackoverflow.com/questions/30701006/how-to-get-the-current-logged-in-user-id-in-asp-net-core
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var resim = _db.Resimler.Find(id);

            // resim bulunamadıysa ya da bulunan resim login olmuş kullanıcıya ait değilse
            if (resim == null || resim.IdentityUserId != userId)
                return NotFound();

            var dosyaYolu = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", resim.ResimYolu);

            if (System.IO.File.Exists(dosyaYolu))
                System.IO.File.Delete(dosyaYolu);

            _db.Remove(resim);
            _db.SaveChanges();
            return new EmptyResult();
        }
    }
}
