using	System; 
using	System.Collections.Generic; 
using	System.Linq; 
using	System.Threading.Tasks; 
using	Microsoft.AspNetCore.Mvc;
using   AspNetCoreTodo.Services;
using   AspNetCoreTodo.Models;

namespace	AspNetCoreTodo.Controllers 
{				
	public	class	TodoController	:	Controller	
    {
        private readonly ITodoItemService _todoItemService;

        public TodoController(ITodoItemService todoItemService)
        {
            _todoItemService = todoItemService;
        }

		public async Task<IActionResult> Index()
		{
			// Get todo items from database
            var items = await _todoItemService.GetIncompleteItemsAsync();
			
			// Put items into a model
            var model = new TodoViewModel(){
                Items = items
            };

			// Render view using the model
            return View(model);
        }
			
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddItem(TodoItem newItem)
        {
            if(!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            var succcessful = await _todoItemService.AddItemAsync(newItem);
            if(!succcessful)
            {
                return BadRequest("Could not add item.");
            }

            return RedirectToAction("Index");

        }
	} 
}
