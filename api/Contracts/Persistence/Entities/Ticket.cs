using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nettolicious.Tickets.Contracts.Persistence.Entities
{
[Table("Ticket")]
    public partial class Ticket
    {
        [Key]
        public int TicketId { get; set; }
        public DateTimeOffset SysCreated { get; set; }
        [Required]
        [StringLength(128)]
        public string SysCreatedBy { get; set; }
        public DateTimeOffset SysLastModified { get; set; }
        [Required]
        [StringLength(128)]
        public string SysLastModifiedBy { get; set; }
        public int OrderId { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(100)]
        public string LastName { get; set; }
        [Required]
        [StringLength(25)]
        public string TicketNumber { get; set; }
        public DateTimeOffset EventDate { get; set; }

        [ForeignKey("OrderId")]
        [InverseProperty("Tickets")]
          public virtual Order Order { get; set; }
    }
}
