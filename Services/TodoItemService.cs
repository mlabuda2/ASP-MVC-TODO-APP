using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreTodo.Data;
using AspNetCoreTodo.Models;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreTodo.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ApplicationDbContext _context;

        public TodoItemService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task<bool> AddItemAsync(NewTodoItem newItem, ApplicationUser user)
        {
            var entity = new TodoItem
            {
                Id = Guid.NewGuid(),
                IsDone = false,
                Title = newItem.Title,
                MadeAt = DateTime.Now,
                DueAt = new DateTimeOffset(newItem.Date),
                OwnerId = user.Id
            };

            _context.Items.Add(entity);

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1;
        }

        public async Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(ApplicationUser user)
        {
            var items = await _context.Items
                        .Where(x => x.IsDone == false && x.OwnerId == user.Id)
                        .ToArrayAsync();

            return items;
        }

        public async Task<TodoItem> GetIncompleteItemAsync(Guid id, ApplicationUser user)
        {
            var item =  await _context.Items
                        .Where(x => x.Id == id && x.OwnerId == user.Id)
                        .SingleOrDefaultAsync();

            return item;
        }

        public async Task<IEnumerable<TodoItem>> GetSearchItemsAsync(ApplicationUser user, string search)
        {
            var searchItems = await _context.Items
                             .Where(x => x.Title.Contains(search) && x.IsDone == false && x.OwnerId == user.Id)
                             .ToArrayAsync();

            return searchItems;
        }

        public async Task<IEnumerable<TodoItem>> GetCompleteItemsAsync(ApplicationUser user)
        {

            var itemsDone = await _context.Items
                           .Where(x => x.IsDone == true && x.OwnerId == user.Id)
                           .ToArrayAsync();

            return itemsDone;
        }

        public async Task<bool> MarkDoneAsync(Guid id, ApplicationUser user)
        {
            var item = await _context.Items
                       .Where(x => x.Id == id && x.OwnerId == user.Id)
                       .SingleOrDefaultAsync();

            if (item == null) return false;

            item.IsDone = true;

            var saveResult = await _context.SaveChangesAsync();
            return saveResult == 1; 
            // One entity should have been updated.
        }
    }
}