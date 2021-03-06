﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models.ViewModel;
using GraniteHouse.Utitlity;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GraniteHouse.Controllers
{
	[Area("Admin")]
    public class ProductsController : Controller
    {
		private readonly ApplicationDbContext _db;
		private readonly HostingEnvironment _hostingEnviroment;

		[BindProperty]
		public ProductsViewModel ProductsVM { get; set; }

		public ProductsController( ApplicationDbContext db, HostingEnvironment hostingEnvironment)
		{
			_db = db;
			_hostingEnviroment = hostingEnvironment;
			
			ProductsVM = new ProductsViewModel() 
			{ 
				ProductTypes = _db.ProductTypes.ToList(), 
				SpecialTags = _db.SpecialTags.ToList(), 
				Products = new Models.Products()
			};
		}

		//GET : Products Index
		public async Task<IActionResult> Index()
        {
			var products = _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags);

            return View(await products.ToListAsync());
        }

		//GET : Products  Create
		public IActionResult Create()
		{
			return View(ProductsVM);
		}

		//POST : Products Create
		[HttpPost, ActionName("Create")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreatePOST()
		{
			if (!ModelState.IsValid)
			{ return View(ProductsVM); }

			_db.Products.Add(ProductsVM.Products);
			await _db.SaveChangesAsync();

			// Image beeing saved
			string webRootPath = _hostingEnviroment.WebRootPath;
			var files = HttpContext.Request.Form.Files;

			var productsFromDb = _db.Products.Find(ProductsVM.Products.Id);


			if (files.Count() != 0)
			{
				// image has been uploaded
				var uploads = Path.Combine(webRootPath, SD.ImageFolder);
				var extension = Path.GetExtension(files[0].FileName);

				using (var filestream = new FileStream(Path.Combine(uploads, ProductsVM.Products.Id + extension), FileMode.Create))
				{
					files[0].CopyTo(filestream);
				}

				productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + extension;
			}
			else
			{
				// fallback image in case user didnt upload product image
				var uploads = Path.Combine(webRootPath, SD.ImageFolder + @"\" + SD.DefaultProductsImage);
				System.IO.File.Copy(uploads, webRootPath + @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png");
				productsFromDb.Image = @"\" + SD.ImageFolder + @"\" + ProductsVM.Products.Id + ".png";
			}

			await _db.SaveChangesAsync();


			return RedirectToAction(nameof(Index));
		}

		//GET : Edit
		public async Task<IActionResult> Edit(int? id) 
		{ 
			if(id == null)
			{ return NotFound(); }

			ProductsVM.Products = await _db.Products.Include(m => m.SpecialTags).Include(m => m.ProductTypes).SingleOrDefaultAsync(m => m.Id == id);

			if(ProductsVM.Products == null)
			{ return NotFound(); }

			return View(ProductsVM);
		}

		//POST : Edit
    }
}