using System;
using System.Collections.Generic;

namespace StoreClient.Models
{
    public partial class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
    }
}
