using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Microsoft.AspNet.Identity.EntityFramework;

namespace CarRental.Models
{
    public class Diagnostics: BaseEntity
    {
        public Diagnostics()
        {
            Id = Guid.NewGuid();
            DateOfCreation = DateTime.Now;
        }
        [Key]
        [Display(Name = "Id", ResourceType = typeof(Resources.Diagnostics))]
        public Guid Id { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "DateOfCreation", ResourceType = typeof(Resources.Diagnostics))]
        public DateTime? DateOfCreation { get; set; }
        [MaxLength(128)]
        [Display(Name = "Creator", ResourceType = typeof(Resources.Car))]
        public string CreatorId { get; set; }

        [Display(Name = "Vehicle", ResourceType = typeof(Resources.Diagnostics))]
        public Guid? CarId { get; set; }
        public virtual Car Car { get; set; }
                

        [Required(ErrorMessageResourceName = "EnterNumberOfOrder",ErrorMessageResourceType =typeof(Resources.Diagnostics))]
        [Display(Name = "NumberOfOrder", ResourceType = typeof(Resources.Diagnostics))]
        public Guid? OrderId { get; set; }
        public virtual Order Order { get; set; }
        
        [Display(Name = "DetectedFailure", ResourceType = typeof(Resources.Diagnostics))]
        public bool DetectedFailure { get; set; }
        [Display(Name = "RepairCost", ResourceType = typeof(Resources.Diagnostics))]
        public int RepairCost { get; set; }
        [MaxLength(500, ErrorMessageResourceName = "EnterDescriptionLength", ErrorMessageResourceType = typeof(Resources.Diagnostics))]
        [Display(Name = "Description", ResourceType = typeof(Resources.Diagnostics))]
        public string Description { get; set; }
    }
}