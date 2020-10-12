using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nettolicious.Tickets.Contracts.Persistence.Entities
{
[Table("Order")]
    public partial class Order
    {
        public Order()
        {
            Tickets = new HashSet<Ticket>();
          }

        [Key]
        public int OrderId { get; set; }
        public DateTimeOffset SysCreated { get; set; }
        [Required]
        [StringLength(128)]
        public string SysCreatedBy { get; set; }
        public DateTimeOffset SysLastModified { get; set; }
        [Required]
        [StringLength(128)]
        public string SysLastModifiedBy { get; set; }
        public int ImportOrderId { get; set; }

        [InverseProperty("Order")]
        public virtual ICollection<Ticket> Tickets { get; set; }
    }
}
