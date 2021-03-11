using System.Collections.Generic;

namespace Calendar.Models
{
    public class SelectionModel<T>
    {
        public int TotalCount {get; set;}
        public List<T> Result {get; set;}
    }
}