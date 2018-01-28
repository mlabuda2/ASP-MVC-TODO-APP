using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AspNetCoreTodo.Models
{
    public class TodoItem
    {
        //generowany na podstawie czasu i liczb pseudolosowych
        public Guid Id { get; set; }
        public bool IsDone { get; set; }

        [DisplayName("TREŒÆ TODO (atrybut dot. widoku)")]
        public string Title { get; set; }

        [DisplayName("DATA ZROBIENIA (atrybut dot. widoku)")]
        public DateTime MadeAt {get;set;}
        public DateTimeOffset? DueAt { get; set; }

        [DisplayName("ID W£ASCICIELA TODO (atrybut dot. widoku)")]
        public string OwnerId { get; set; }
    }
}