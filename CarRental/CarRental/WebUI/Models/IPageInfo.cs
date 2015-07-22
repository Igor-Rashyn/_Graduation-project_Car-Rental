using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.WebUI.Models
{
    public interface IPageInfo
    {
        // Кол-во товаров
        int TotalItems { get; set; }

        // Кол-во товаров на одной странице
        int ItemsPerPage { get; set; }

        // Номер текущей страницы
        int CurrentPage { get; set; }

        // Общее кол-во страниц
        int TotalPages { get; }
    }
}
