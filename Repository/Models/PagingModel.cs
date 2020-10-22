using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmediCodesWebApplication.Repository.Models
{
    public class PagingModel
    {
        public int pageSize { get; set; } = 200;
        public int page { get; set; } = 1;
    }
}