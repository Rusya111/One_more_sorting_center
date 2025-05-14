using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace One_more_sorting_center.Models
{
    public enum IssueType
    {
        [Display(Name = "Дубль")]
        Duplicate,
        [Display(Name = "Заказ без ШК")]
        NoBarcode,
        [Display(Name = "Засыл")]
        WrongAddress,
        [Display(Name = "Зависшее хранение")]
        StuckInStorage,
        [Display(Name = "Поврежденный")]
        Damaged,
        [Display(Name = "Другая проблема")]
        Other
    }

    public enum OrderStatus
    {
        [Display(Name = "Отменен")]
        Cancelled,
        [Display(Name = "В процессе")]
        InProgress,
        [Display(Name = "Завершен")]
        Completed,
        [Display(Name = "Другое")]
        Other
    }

    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "Номер заказа обязателен")]
        [Display(Name = "Номер заказа")]
        public string OrderNumber { get; set; }

        [Required(ErrorMessage = "Номер грузоместа обязателен")]
        [Display(Name = "Номер грузоместа")]
        public string CargoNumber { get; set; }

        [Required(ErrorMessage = "Выберите СЦ отгрузки")]
        [Display(Name = "СЦ отгрузки")]
        public string OutboundSC { get; set; }

        [Required(ErrorMessage = "Выберите СЦ поставки")]
        [Display(Name = "СЦ поставки")]
        public string InboundSC { get; set; }

        [Display(Name = "Дата создания")]
        [Column(TypeName = "timestamp with time zone")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Выберите статус")]
        [Display(Name = "Статус")]
        public OrderStatus Status { get; set; }

        [Required(ErrorMessage = "Выберите тип проблемы")]
        [Display(Name = "Тип проблемы")]
        public IssueType ProblemType { get; set; }

        [Display(Name = "Скрытый")]
        public bool IsHidden { get; set; } = false;

        [Display(Name = "Пользователь")]
        public string UserSC { get; set; }
    }
}