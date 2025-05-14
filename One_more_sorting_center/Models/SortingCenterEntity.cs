using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace One_more_sorting_center.Models
{
    public class SortingCenterEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        // Навигационные свойства
        public ICollection<Order> InboundOrders { get; set; }
        public ICollection<Order> OutboundOrders { get; set; }
        public ICollection<Order> CreatedOrders { get; set; }
    }
}