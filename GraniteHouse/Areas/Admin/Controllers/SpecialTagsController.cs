using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraniteHouse.Data;
using GraniteHouse.Models;
using Microsoft.AspNetCore.Mvc;

namespace GraniteHouse.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class SpecialTagsController : Controller
    {
		private readonly ApplicationDbContext _db;

		public SpecialTagsController( ApplicationDbContext db)
		{
			_db = db;
		}

		public IActionResult Index()
        {
            return View(_db.SpecialTags.ToList());
        }

		//GET Create Action
		public IActionResult Create()
		{
			return View();
		}

		//POST Create Action
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(SpecialTags specialTag)
		{
			if(ModelState.IsValid)
			{
				_db.Add(specialTag);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			
			return View(specialTag);
		}

		//GET Edit Action
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null)
			{ return NotFound(); }

			var specialTag = await _db.SpecialTags.FindAsync(id);

			if (specialTag == null)
			{ return NotFound(); }

			return View(specialTag);
		}

		//POST Edit Action
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, SpecialTags specialTag)
		{
			if (id != specialTag.Id)
			{ return NotFound(); }

			if (ModelState.IsValid)
			{
				_db.Update(specialTag);
				await _db.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}

			return View(nameof(Index));
		}

		//GET Details Action
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{ return NotFound(); }

			var specialTag = await _db.SpecialTags.FindAsync(id);

			if (specialTag == null)
			{ return NotFound(); }

			return View(specialTag);
		}

		//GET Delete Action
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null) 
				return NotFound(); 

			var specialTag = await _db.SpecialTags.FindAsync(id);

			if(specialTag == null)
				return NotFound(); 

			return View(specialTag);
		}

		//POST Delete Action
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Delete(int id)
		{
			var special = await _db.SpecialTags.FindAsync(id);
			_db.SpecialTags.Remove(special);
			await _db.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
			
		}

	}
}