using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Models
{
    /// <summary>
    /// Нужно добавить поле PictureContentType в которое записывать контент тайп, это необходимо. Также даты поставить nullable. Может быть добавить поле для для хранения маленькой версии фотки
    /// </summary>
    [Table("Cars")]
    public class Car: BaseEntity
    {
        public Car()
        {
            Id = Guid.NewGuid();
            DateOfCreation = DateTime.Now;
            Status = "Available";
            StatusRu = "Доступный";
        }
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "DateOfCreation", ResourceType = typeof(Resources.Car))]
        public DateTime? DateOfCreation { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "DateOfChange", ResourceType = typeof(Resources.Car))]
        public DateTime? DateOfChange { get; set; }

        [MaxLength(128)]
        [Display(Name = "Creator", ResourceType = typeof(Resources.Car))]
        public string CreatorId { get; set; }
        
        [Display(Name = "Status", ResourceType = typeof(Resources.Car))]
        public string Status { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Car))]
        public string StatusRu { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "PickupDateTime", ResourceType = typeof(Resources.Car))]
        public DateTime? PickupDateTime { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "ReturnDateTime", ResourceType = typeof(Resources.Car))]
        public DateTime? ReturnDateTime { get; set; }
        [Display(Name = "Location", ResourceType = typeof(Resources.Car))]
        public string Location { get; set; }
        [Required]
        [Display(Name = "Passengers", ResourceType = typeof(Resources.Car))]
        public int Passengers { get; set; }
        [MaxLength(50, ErrorMessageResourceType =typeof(Resources.Car),ErrorMessageResourceName = "EnterAmountOfLuggageLength")]
        [Display(Name = "AmountOfLuggage", ResourceType = typeof(Resources.Car))]
        public string AmountOfLuggage { get; set; }

        [MaxLength(50, ErrorMessageResourceType = typeof(Resources.Car), ErrorMessageResourceName = "EnterAmountOfLuggageLength")]
        [Display(Name = "AmountOfLuggage", ResourceType = typeof(Resources.Car))]
        public string AmountOfLuggageRu { get; set; }


        [Display(Name = "Transmission", ResourceType = typeof(Resources.Car))]
        public bool Transmission { get; set; }
        [Required(ErrorMessageResourceName = "EnterFuelConsumption", ErrorMessageResourceType =typeof(Resources.Car))]
        [Display(Name = "FuelConsumption", ResourceType = typeof(Resources.Car))]
        public int FuelConsumption { get; set; }
        [MaxLength(50,ErrorMessageResourceType =typeof(Resources.Car),ErrorMessageResourceName = "EnterBrandLength"), Required(ErrorMessageResourceName ="EnterBrand", ErrorMessageResourceType =typeof(Resources.Car))]
        [Display(Name = "Brand", ResourceType = typeof(Resources.Car))]
        public string Brand { get; set; }
        [MaxLength(50, ErrorMessageResourceName ="EnterModelLength", ErrorMessageResourceType =typeof(Resources.Car)), Required(ErrorMessageResourceName = "EnterModel", ErrorMessageResourceType = typeof(Resources.Car))]
        [Display(Name = "Model", ResourceType = typeof(Resources.Car))]
        public string Model { get; set; }
        [Required(ErrorMessageResourceName = "EnterPrice", ErrorMessageResourceType = typeof(Resources.Car))]
        [Display(Name = "Price", ResourceType = typeof(Resources.Car))]
        public int Price { get; set; }
        [MaxLength(500, ErrorMessageResourceName = "EnterAdditionInformationLength", ErrorMessageResourceType = typeof(Resources.Car))]
        [Display(Name = "AdditionInformation", ResourceType = typeof(Resources.Car))]
        public string AdditionInformation { get; set; }
        [MaxLength(500, ErrorMessageResourceName = "EnterAdditionInformationLength", ErrorMessageResourceType = typeof(Resources.Car))]
        [Display(Name = "AdditionInformation", ResourceType = typeof(Resources.Car))]
        public string AdditionInformationRu { get; set; }
        [Display(Name = "AirConditioning", ResourceType = typeof(Resources.Car))]
        public bool AirConditioning { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "CarNumber", ResourceType = typeof(Resources.Car))]
        public int CarNumber { get; set; }

        [Display(Name = "Picture", ResourceType = typeof(Resources.Car))]
        public Guid? PictureId { get; set; }
        public virtual Picture Picture { get; set; }



    }
}