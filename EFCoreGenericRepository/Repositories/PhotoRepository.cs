using AvtoShop.DataLayer.DbLayer;
using EFCoreGenericRepository.Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace EFCoreGenericRepository.Repositories
{
	public class PhotoRepository : GenericRepository<Photo>
	{
		public PhotoRepository(DbContext context) : base(context)
		{
		}
	}
	
}
