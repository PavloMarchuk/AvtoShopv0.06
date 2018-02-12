using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AvtoShop.WebUI.Controllers.Generic;
using AvtoShop.DataLayer.DbLayer;
using EFCoreGenericRepository.Common;

namespace AvtoShop.WebUI.Controllers
{
    public class AutoBodyController : GenericController<AutoBody>
    {
        public AutoBodyController(IGenericRepository<AutoBody> repFuel): base(repFuel)
        {
            Path = "AutoBody";
        }
    }
}