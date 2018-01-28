using System;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;
using AspNetCoreTodo.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreTodo.Controllers
{
    [Authorize]
    public class TodoController : Controller
    {
        private readonly ITodoItemService _todoItemService;
        private readonly UserManager<ApplicationUser> _userManager;

        public TodoController(ITodoItemService todoItemService, 
        UserManager<ApplicationUser> userManager) 
        {
            _todoItemService = todoItemService;
            _userManager = userManager;
        }

        //public async Task<IActionResult> Index()
        //{
        //    var currentUser = await _userManager.GetUserAsync(User);
        //    if (currentUser == null) return Challenge();

        //    var todoItems = await _todoItemService.GetIncompleteItemsAsync(currentUser);

        //    var model = new TodoViewModel() {
        //        Items = todoItems
        //    };

        //    return View(model);
        //}

        public async Task<IActionResult> Index(string searchString)
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            System.Collections.Generic.IEnumerable<TodoItem> todoSearchItems = null;

            if (!String.IsNullOrEmpty(searchString))
            {
                todoSearchItems = await _todoItemService.GetSearchItemsAsync(currentUser, searchString);
            }
            else
            {
                todoSearchItems = await _todoItemService.GetIncompleteItemsAsync(currentUser);
            }

            var model = new TodoViewModel()
            {
                Items = todoSearchItems
            };

            return View(model);
        }


        public async Task<IActionResult> Done()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Challenge();

            var doneItems = await _todoItemService.GetCompleteItemsAsync(currentUser);

            var model = new TodoViewModel()
            {
                ItemsDone = doneItems
            };

            return View(model);
        }

        public async Task<IActionResult> MarkDone(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var successful = await _todoItemService.MarkDoneAsync(id, currentUser);

            if(!successful) return BadRequest();
            
            return Ok();
        }
        public async Task<IActionResult> AddItem(NewTodoItem newItem)
        {
            if (!ModelState.IsValid)
            {
            return BadRequest(ModelState);
            }
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var successful = await _todoItemService.AddItemAsync(newItem, currentUser);
            if (!successful)
            {
            return BadRequest(new { error = "Could not add item" });
            }
            return Ok();
        }

        //[LayoutInjecter("_mojLayout")]  - trzeba utworzyc nowa klase
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return BadRequest();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null) return Unauthorized();

            var model = new TodoViewModel()
            {
                Item = await _todoItemService.GetIncompleteItemAsync(id, currentUser)
            };

            //return View("~/Views/Shared/_mojLayout.cshtml", model);
            return View(model);
        }
    }
}