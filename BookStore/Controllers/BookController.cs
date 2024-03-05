using BookStore.Models.Domain;
using BookStore.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net.WebSockets;

namespace BookStore.Controllers
{
	public class BookController  : Controller
	{
		private readonly IBookService service;
		private readonly IAuthorService authorService;
		private readonly IGenreService genreService;
		private readonly IPublisherService publisherService;

		public BookController(IBookService service, IAuthorService authorService,IGenreService genreService,IPublisherService publisherService)
		{
			this.service = service;
			this.authorService = authorService;
			this.genreService = genreService;
			this.publisherService = publisherService;
		}
		public IActionResult Add()
		{
			var model = new Book();
			model.AuthorList = authorService.GetAll().Select(a=> new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString() }).ToList();
			model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString() }).ToList();
			model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString() }).ToList();
			return View(model);
		}
		[HttpPost]
		public IActionResult Add(Book model)
		{
			model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString(),Selected=a.Id==model.AuthorId }).ToList();
			model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString(), Selected=a.Id==model.PublisherId }).ToList();
			model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString(), Selected=a.Id==model.GenreId }).ToList();
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = service.Add(model);
			if(result)
			{
				TempData["msg"] = "Added Successfully";
				return RedirectToAction(nameof(Add));
			}
			TempData["msg"] = "Error has occured on server side";

			return View();
		}

		public IActionResult Update(int id)
		{
			var model = service.FindById(id);
			model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString(), Selected = a.Id == model.AuthorId }).ToList();
			model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString(), Selected = a.Id == model.PublisherId }).ToList();
			model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString(), Selected = a.Id == model.GenreId }).ToList();
			return View(model);
		}
		[HttpPost]
		public IActionResult Update(Book model)
		{
			model.AuthorList = authorService.GetAll().Select(a => new SelectListItem { Text = a.AuthorName, Value = a.Id.ToString(), Selected = a.Id == model.AuthorId }).ToList();
			model.PublisherList = publisherService.GetAll().Select(a => new SelectListItem { Text = a.PublisherName, Value = a.Id.ToString(), Selected = a.Id == model.PublisherId }).ToList();
			model.GenreList = genreService.GetAll().Select(a => new SelectListItem { Text = a.Name, Value = a.Id.ToString(), Selected = a.Id == model.GenreId }).ToList();
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = service.Update(model);
			if (result)
			{
		
				return RedirectToAction("GetAll");
			}
			TempData["msg"] = "Error has occured on server side";

			return View(model);
		}


		public IActionResult Delete(int id)
		{

			var result = service.Delete(id);
			return RedirectToAction("GetAll");
		
		}


		public IActionResult GetAll(int id)
		{

			var data = service.GetAll();
			return View(data);

		}
	}
}

