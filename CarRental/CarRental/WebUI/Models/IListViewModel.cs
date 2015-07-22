using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.WebUI.Models
{
    public interface IListViewModel<Tmodel> where Tmodel : class
    {
         IEnumerable<Tmodel> Model { get; set; }
         PageInfo PagingInfo { get; set; }
    }
}
