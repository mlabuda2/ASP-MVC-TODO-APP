using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AspNetCoreTodo.Models;

namespace AspNetCoreTodo.Services
{
    public interface ITodoItemService
    {
        Task<IEnumerable<TodoItem>> GetIncompleteItemsAsync(ApplicationUser user);

        Task<TodoItem> GetIncompleteItemAsync(Guid id, ApplicationUser user);

        Task<IEnumerable<TodoItem>> GetCompleteItemsAsync(ApplicationUser user);

        Task<IEnumerable<TodoItem>> GetSearchItemsAsync(ApplicationUser user, string search);

        Task<bool> MarkDoneAsync(Guid id, ApplicationUser user);
        
        Task<bool> AddItemAsync(NewTodoItem newItem, ApplicationUser user);
    }
}