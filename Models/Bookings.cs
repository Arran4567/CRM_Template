using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Bicks.Models
{
    public class Bookings
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Display(Name = "ID")]
        public int ID { get; set; }
        [Required]
        [Display(Name = "Client")]
        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }
        [Required]
        [Display(Name = "Date and time of booking")]
        public DateTime dateTimeOfBooking { get; set; }
    }
}
