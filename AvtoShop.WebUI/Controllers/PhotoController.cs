using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AvtoShop.DataLayer.DbLayer;
using EFCoreGenericRepository.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AvtoShop.WebUI.Controllers
{
    public class PhotoController : Controller
    {
		IGenericRepository<Photo> repPhoto;
		public PhotoController(IGenericRepository<Photo> repPhoto)
		{
			this.repPhoto = repPhoto;
		}
		public IActionResult Index()
        {
            return View();
        }
		[Authorize]
		public IActionResult AddPhotos(int id)
		{
			IQueryable<Photo> model = repPhoto.FindBy(ph=> ph.AvtoId==id);

			return View(model);
		}

		
	}

}