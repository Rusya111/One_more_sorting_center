using One_more_sorting_center.Models;

namespace One_more_sorting_center.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AppDbContext context)
        {
            context.Database.EnsureCreated();

            if (!context.Orders.Any())
            {
                var orders = new[]
                {
                    // Заказы для Уфы
                    new Order {
                        OrderNumber = "UF-1001",
                        CargoNumber = "CN-12345",
                        OutboundSC = "Уфа",
                        InboundSC = "Екатеринбург",
                        Status = OrderStatus.InProgress,
                        ProblemType = IssueType.Duplicate,
                        UserSC = "Уфа",
                        CreatedDate = DateTime.UtcNow,
                        IsHidden = true
                    },
                    new Order {
                        OrderNumber = "UF-1002",
                        CargoNumber = "CN-12346",
                        OutboundSC = "Уфа",
                        InboundSC = "Тарный",
                        Status = OrderStatus.Completed,
                        ProblemType = IssueType.NoBarcode,
                        UserSC = "Уфа",
                        CreatedDate = DateTime.UtcNow,
                        IsHidden = true
                    },
                    
                    // Заказы для Грибков
                    new Order {
                        OrderNumber = "GR-2001",
                        CargoNumber = "CN-22345",
                        OutboundSC = "Грибки",
                        InboundSC = "Софьино",
                        Status = OrderStatus.InProgress,
                        ProblemType = IssueType.StuckInStorage,
                        UserSC = "Грибки",
                        CreatedDate = DateTime.UtcNow,
                        IsHidden = true
                    },
                    new Order {
                        OrderNumber = "GR-2002",
                        CargoNumber = "CN-22346",
                        OutboundSC = "Грибки",
                        InboundSC = "Новгород",
                        Status = OrderStatus.Cancelled,
                        ProblemType = IssueType.Damaged,
                        UserSC = "Грибки",
                        CreatedDate = DateTime.UtcNow,
                        IsHidden = true
                    }
                };

                context.Orders.AddRange(orders);
                context.SaveChanges();
            }
        }
    }
}