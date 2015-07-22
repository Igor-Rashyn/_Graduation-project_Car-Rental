using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Models;

namespace CarRental.Repositories
{
    public interface IRepository<T> where T: BaseEntity //IEntity//, IDisposable
    {
        IEnumerable<T> GetAll(); // получение всех объектов IQueryable
        T Find(object id); // получение одного объекта по id
        void Create(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Delete(T item); // удаление объекта по id
        void Save();  // сохранение изменений

    }
}
