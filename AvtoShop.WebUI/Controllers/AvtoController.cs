using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EFCoreGenericRepository.Common;
using AvtoShop.DataLayer.DbLayer;
using AvtoShop.WebUI.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace AvtoShop.WebUI.Controllers
{
    public class AvtoController : Controller
    {
        IGenericRepository<Avto> repAvto;
        IGenericRepository<Brand> repBrand;
        IGenericRepository<ModelAvto> repModelAvto;
        IGenericRepository<DriveUnit> repDriveUnit;
        IGenericRepository<AutoBody> repAutoBody;
        IGenericRepository<Fuel> repFuel;
        IGenericRepository<KPP> repKPP;
        public AvtoController(IGenericRepository<Avto> repAvto,
                                IGenericRepository<Brand> repBrand,
                                IGenericRepository<ModelAvto> repModelAvto,
                                IGenericRepository<DriveUnit> repDriveUnit,
                                IGenericRepository<AutoBody> repAutoBody,
                                IGenericRepository<Fuel> repFuel,
                                IGenericRepository<KPP> repKPP)
        {
            this.repAvto = repAvto;
            this.repBrand = repBrand;
            this.repModelAvto = repModelAvto;
            this.repDriveUnit = repDriveUnit;
            this.repAutoBody = repAutoBody;
            this.repFuel = repFuel;
            this.repKPP = repKPP;
        }

        public IActionResult Index()
        {
            var model = repAvto.GetAll()
                .Select(a => new ViewModelAvto
                {
                    AvtoId = a.AvtoId,
                    BrandName = a.ModelAvto.Brand.Name,
                    ModelAvtoName = a.ModelAvto.ModelName,
                    UserName = a.UserName,
                    YearAvto = a.YearAvto,
                    Price = a.Price
                });

            return View(model);
        }
		[Authorize]
		public IActionResult Edit(int id=0)
        {
            var model = (id == 0) ? new Avto() { ModelAvtoId=0, YearAvto=DateTime.Now.Year, Engine=2000} : repAvto.Get(id);
            InitViewBag(model, false);
            return View(model);
        }
        [HttpPost]
        public IActionResult Edit(Avto model)
        {
            if (ModelState.IsValid)
            {
                var ModelAvtoId = model.ModelAvtoId;
                if(model.AvtoId==0) repAvto.Add(model);
                else repAvto.Update(model, model.AvtoId);
                repAvto.Save();
                return RedirectToAction("Index");

            }
            InitViewBag(model, true);//если неудача - проинициализировать ViewBag еще раз!
            return View(model);
        }

        public IActionResult GetModelAvtoPartView(int? BrandId)
        {
            var model = GetModelAvto(BrandId);
            ViewBag.ModelAvtoId = new SelectList(GetModelAvtoNull.Union(model), "ModelAvtoId", "ModelName");
            return PartialView();

        }

        #region private method for DropDownInit
        void InitViewBag(Avto model, bool IsPostMethod)
        {
            if (IsPostMethod)
            {
                var modelAvto = repModelAvto.Get(model.ModelAvtoId);
                ViewBag.BrandId = new SelectList(GetBrand.Union(repBrand.GetAll()), "Id", "Name", modelAvto.BrandId);
                ViewBag.ModelAvtoId = new SelectList(GetModelAvto(modelAvto.BrandId), "ModelAvtoId", "ModelName", model.ModelAvtoId);

            }
            else
            {
                if(model.ModelAvtoId == 0)
                {
                    ViewBag.BrandId = new SelectList(GetBrand.Union(repBrand.GetAll()), "Id", "Name");
                    ViewBag.ModelAvtoId = new SelectList(GetModelAvtoNull, "ModelAvtoId", "ModelName");
                }
                else
                {
                    var modelAvto = repModelAvto.Get(model.ModelAvtoId);
                    ViewBag.BrandId = new SelectList(GetBrand.Union(repBrand.GetAll()), "Id", "Name", modelAvto.BrandId);
                    ViewBag.ModelAvtoId = new SelectList(GetModelAvto(modelAvto.BrandId), "ModelAvtoId", "ModelName", model.ModelAvtoId);

                }

            }
            ViewBag.DriveUnitId = new SelectList(GetDriveUnit.Union(repDriveUnit.GetAll()), "Id", "Name", model.DriveUnitId);
            ViewBag.AutoBodyId = new SelectList(GetAutoBody.Union(repAutoBody.GetAll()), "Id", "Name", model.AutoBodyId);
            ViewBag.FuelId = new SelectList(GetFuel.Union(repFuel.GetAll()), "Id", "Name", model.FuelId);
            ViewBag.KPPId = new SelectList(GetKPP.Union(repKPP.GetAll()), "Id", "Name", model.KPPId);
        }


        IEnumerable<KPP> GetKPP
        {
            get
            {
                yield return new KPP { Id = 0, Name = "Выберите коробку" };
            }
        }
        IEnumerable<Fuel> GetFuel
        {
            get
            {
                yield return new Fuel { Id = 0, Name = "Выберите тип топлива" };
            }
        }
        IEnumerable<AutoBody> GetAutoBody
        {
            get
            {
                yield return new AutoBody { Id = 0, Name = "Выберите кузов" };
            }
        }
        IEnumerable<ModelAvto> GetModelAvto(int? BrandId)
        {
            return repModelAvto.FindBy(p => p.BrandId == BrandId);
        }
        IEnumerable<ModelAvto> GetModelAvtoNull
        {
            get
            {
                yield return new ModelAvto { ModelAvtoId = 0, ModelName = "Выберите модель авто" };
            }
        }


        IEnumerable<DriveUnit> GetDriveUnit
        {
            get
            {
                yield return new DriveUnit { Id = 0, Name = "Выберите привод" };
            }
        }
        IEnumerable<Brand> GetBrand
        {
            get
            {
                yield return new Brand { Id = 0, Name = "Выберите марку авто" };
            }
        }
        #endregion private method for DropDownInit
    }
}