using Microsoft.EntityFrameworkCore;
using One_more_sorting_center.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. Конфигурация сервисов
builder.Services.AddControllersWithViews();

// Настройка DbContext с логированием
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
           .EnableSensitiveDataLogging()
           .LogTo(Console.WriteLine, LogLevel.Information);
});

var app = builder.Build();

// 2. Middleware pipeline
app.UseStaticFiles();
app.UseRouting();

// 3. Инициализация БД (после Build, но до Run)
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // Применяем миграции
        db.Database.Migrate();

        // Инициализируем тестовые данные
        DbInitializer.Initialize(db);

        // Тестовая проверка
        var orderCount = db.Orders.Count();
        Console.WriteLine($"В БД найдено заказов: {orderCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка инициализации БД: {ex.Message}");
    }
}

// 4. Маршрутизация
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{currentUser?}/{id?}");

app.Run();