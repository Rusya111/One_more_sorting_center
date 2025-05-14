using Microsoft.EntityFrameworkCore;
using One_more_sorting_center.Data;

var builder = WebApplication.CreateBuilder(args);

// 1. ������������ ��������
builder.Services.AddControllersWithViews();

// ��������� DbContext � ������������
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

// 3. ������������� �� (����� Build, �� �� Run)
using (var scope = app.Services.CreateScope())
{
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        // ��������� ��������
        db.Database.Migrate();

        // �������������� �������� ������
        DbInitializer.Initialize(db);

        // �������� ��������
        var orderCount = db.Orders.Count();
        Console.WriteLine($"� �� ������� �������: {orderCount}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"������ ������������� ��: {ex.Message}");
    }
}

// 4. �������������
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Orders}/{action=Index}/{currentUser?}/{id?}");

app.Run();