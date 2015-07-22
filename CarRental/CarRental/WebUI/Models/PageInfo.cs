using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRental.WebUI.Models
{
    public class PageInfo: IPageInfo
    {
        // Кол-во товаров
        public int TotalItems { get; set; }

        // Кол-во товаров на одной странице
        public int ItemsPerPage { get; set; }

        // Номер текущей страницы
        public int CurrentPage { get; set; }

        // Общее кол-во страниц
        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

        public string SortBy { get; set; }
        public int sortOrder { get; set; }
    }
}