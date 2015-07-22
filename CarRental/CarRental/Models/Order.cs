using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Models
{
    [Table("Orders")]
    public class Order: BaseEntity
    {
        public Order()
        {
            Id = Guid.NewGuid();
            DateOfCreation = DateTime.Now;
            Status = "New";
            StatusRu="Новый";
        }
        [Key]
        [Display(Name = "Id", ResourceType = typeof(Resources.Order))]
        public Guid Id { get; set; }
        [Timestamp] // Timestamp -защищает от одновременного редактирования
        public byte[] TimeStamp { get; set; }

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]     ВАЖНЫЙ МОМЕНТ ДЛЯ ДАТЫ
        [Display(Name = "DateOfCreation", ResourceType = typeof(Resources.Order))]
        public DateTime? DateOfCreation { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "DateOfChange", ResourceType = typeof(Resources.Order))]
        public DateTime? DateOfChange { get; set; }
        [Display(Name = "Status", ResourceType = typeof(Resources.Order))]
        public string Status { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Resources.Order))]
        public string StatusRu { get; set; }

        [MaxLength(128)]
        [Display(Name = "Creator", ResourceType = typeof(Resources.Car))]
        public string CreatorId { get; set; }



        // автоматически генерит новое значение
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ApplicationNumber", ResourceType = typeof(Resources.Order))]
        public int ApplicationNumber { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterPickupLocation")]
        [Display(Name = "PickupLocation", ResourceType = typeof(Resources.Order))]
        public string PickupLocation { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterPickupDateTime")]
        [Display(Name = "PickupDateTime", ResourceType = typeof(Resources.Order))]
        public DateTime PickupDateTime { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterReturnLocation")]
        [Display(Name = "ReturnLocation", ResourceType = typeof(Resources.Order))]
        public string ReturnLocation { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterReturnDateTime")]
        [Display(Name = "ReturnDateTime", ResourceType = typeof(Resources.Order))]
        public DateTime ReturnDateTime { get; set; }
        [Display(Name = "Vehicle", ResourceType = typeof(Resources.Order))]
        public Guid? CarId { get; set; }
        public virtual Car Car { get; set; }
        

        [Display(Name = "TotalPrice", ResourceType = typeof(Resources.Order))]
        public int TotalPrice { get; set; }
       
        #region Info about Customer

        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterFirstName"), MaxLength(50, ErrorMessageResourceName = "EnterFirstNameLength", ErrorMessageResourceType = typeof(Resources.Order))]
        [Display(Name = "FirstName", ResourceType = typeof(Resources.Order))]
        public string FirstName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterLastName"), MaxLength(50, ErrorMessageResourceName = "EnterLastNameLength", ErrorMessageResourceType = typeof(Resources.Order))]
        [Display(Name = "LastName", ResourceType = typeof(Resources.Order))]
        public string LastName { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterEmail"), MaxLength(50, ErrorMessageResourceName = "EnterEmailLength", ErrorMessageResourceType = typeof(Resources.Order))]
        [Display(Name = "Email", ResourceType = typeof(Resources.Order))]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterPhone"), MaxLength(20, ErrorMessageResourceName = "EnterPhoneLength", ErrorMessageResourceType = typeof(Resources.Order))]
        [Display(Name = "Phone", ResourceType = typeof(Resources.Order))]
        public string Phone { get; set; }

        [DataType(DataType.Date)]
        [Required(ErrorMessageResourceType = typeof(Resources.Order), ErrorMessageResourceName = "EnterDateOfBirthday")]
        [Display(Name = "DateOfBirthday", ResourceType = typeof(Resources.Order))]
        public DateTime DateOfBirthday { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Resources.Order))]
        public string Address { get; set; }
        [Display(Name = "City", ResourceType = typeof(Resources.Order))]
        public string City { get; set; }
        [Display(Name = "StateOrProvince", ResourceType = typeof(Resources.Order))]
        public string StateOrProvince { get; set; }
        [Display(Name = "PostcodeOrZip", ResourceType = typeof(Resources.Order))]
        public string PostcodeOrZip { get; set; }
        [Display(Name = "CountryOfResidence", ResourceType = typeof(Resources.Order))]
        public string CountryOfResidence { get; set; }
       
        #endregion

        #region Info about License

        [Display(Name = "LicenseNo", ResourceType = typeof(Resources.Order))]
        public string LicenseNo { get; set; }
        [Display(Name = "CountryOfIssue", ResourceType = typeof(Resources.Order))]
        public string CountryOfIssue { get; set; }
        [Display(Name = "ExpiryDate", ResourceType = typeof(Resources.Order))]
        public DateTime? ExpiryDate { get; set; }

        #endregion
    }
}