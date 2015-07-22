using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CarRental.Models;

namespace CarRental.WebUI.Models
{
    public class ListViewModel <Tmodel>: IListViewModel<Tmodel> where Tmodel : class
    {
        public IEnumerable<Tmodel> Model { get; set; }
        public PageInfo PagingInfo { get; set; }
    }
}