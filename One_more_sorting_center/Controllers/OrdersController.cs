using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using One_more_sorting_center.Data;
using One_more_sorting_center.Models;
using System.Linq;

namespace One_more_sorting_center.Controllers
{
    public class OrdersController : Controller
    {
        private readonly AppDbContext _context;
        private readonly List<string> _sortingCenters = new()
        {
            "Уфа", "Грибки", "Екатеринбург", "Тарный",
            "Дзержинск", "Новгород", "Самара", "Софьино"
        };

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // Главное меню
        public IActionResult Index(string currentUser, string searchString)
        {
            currentUser ??= "Уфа";
            ViewBag.CurrentUser = currentUser;
            ViewData["CurrentFilter"] = searchString;

            var orders = _context.Orders
                .Where(o => o.UserSC == currentUser && !o.IsHidden);

            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o =>
                    o.OrderNumber.Contains(searchString) ||
                    o.CargoNumber.Contains(searchString));
            }

            return View(orders.OrderByDescending(o => o.CreatedDate).ToList());
        }

        // Создание заказа
        public IActionResult Create(string currentUser)
        {
            ViewBag.CurrentUser = currentUser;
            ViewBag.SortingCenters = _sortingCenters;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Order order, string currentUser)
        {
            order.UserSC = currentUser;
            order.CreatedDate = DateTime.SpecifyKind(
                order.CreatedDate == default ? DateTime.UtcNow : order.CreatedDate.ToUniversalTime(),
                DateTimeKind.Utc);

            if (ModelState.IsValid)
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
                return RedirectToAction("Index", new { currentUser });
            }

            ViewBag.CurrentUser = currentUser;
            ViewBag.SortingCenters = _sortingCenters;
            return View(order);
        }

        // Скрытые заказы
        public IActionResult HiddenOrders(string currentUser, string sortOrder, string searchString)
        {
            currentUser ??= "Уфа";
            ViewBag.CurrentUser = currentUser;

            // Сохраняем параметры для View
            ViewData["DateSort"] = sortOrder == "date" ? "date_desc" : "date";
            ViewData["CurrentFilter"] = searchString;

            var orders = _context.Orders
                .Where(o => o.UserSC == currentUser && o.IsHidden);

            // Применяем поиск
            if (!string.IsNullOrEmpty(searchString))
            {
                orders = orders.Where(o =>
                    o.OrderNumber.Contains(searchString) ||
                    o.CargoNumber.Contains(searchString));
            }

            // Применяем сортировку
            orders = sortOrder switch
            {
                "date" => orders.OrderBy(o => o.CreatedDate),
                "date_desc" => orders.OrderByDescending(o => o.CreatedDate),
                _ => orders.OrderByDescending(o => o.CreatedDate)
            };

            return View(orders.ToList());
        }

        // Удаление заказа
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, string currentUser)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            _context.SaveChanges();

            TempData["Success"] = $"Заказ {order.OrderNumber} успешно удален";
            return RedirectToAction("Index", new { currentUser });
        }

        // Скрытие/восстановление заказа
        [HttpPost]
        public IActionResult ToggleVisibility(int id, string currentUser)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            order.IsHidden = !order.IsHidden;
            _context.SaveChanges();

            TempData["Success"] = $"Заказ {order.OrderNumber} успешно " +
                                 (order.IsHidden ? "скрыт" : "восстановлен");

            return RedirectToAction(order.IsHidden ? "HiddenOrders" : "Index",
                                 new { currentUser });
        }

        public IActionResult Edit(int id, string currentUser)
        {
            var order = _context.Orders.Find(id);
            if (order == null)
            {
                return NotFound();
            }

            ViewBag.CurrentUser = currentUser;
            ViewBag.SortingCenters = _sortingCenters;
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, string currentUser, [Bind("Id,OrderNumber,CargoNumber,OutboundSC,InboundSC,Status,ProblemType")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingOrder = _context.Orders.Find(id);
                    if (existingOrder == null)
                    {
                        return NotFound();
                    }

                    existingOrder.OrderNumber = order.OrderNumber;
                    existingOrder.CargoNumber = order.CargoNumber;
                    existingOrder.OutboundSC = order.OutboundSC;
                    existingOrder.InboundSC = order.InboundSC;
                    existingOrder.Status = order.Status;
                    existingOrder.ProblemType = order.ProblemType;

                    _context.Update(existingOrder);
                    _context.SaveChanges();

                    TempData["Success"] = "Заказ успешно обновлен!";
                    return RedirectToAction("Index", new { currentUser });
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", $"Ошибка сохранения: {ex.Message}");
                }
            }

            ViewBag.CurrentUser = currentUser;
            ViewBag.SortingCenters = _sortingCenters;
            return View(order);
        }
    }
}