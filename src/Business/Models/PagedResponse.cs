using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Business.Models
{
    [ExcludeFromCodeCoverage]
    public class PagedResponse<T>
    {
        public IEnumerable<T> Data { get; set; }
        public int TotalPages { get; set; }
        public int PageNumber { get; set; }
        public int TotalItens { get; set; }
    }
}