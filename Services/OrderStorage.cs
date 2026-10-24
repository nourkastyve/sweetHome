using System.Text.Json;
using SweetHome.Models;
namespace SweetHome.Services;
public static class OrderStorage
{
    private static readonly string FilePath = Path.Combine(FileSystem.AppDataDirectory, "sweethome-orders.json");
    public static async Task<List<CakeOrder>> LoadAsync()
    {
        try { if (!File.Exists(FilePath)) return new(); var json = await File.ReadAllTextAsync(FilePath); return JsonSerializer.Deserialize<List<CakeOrder>>(json) ?? new(); }
        catch { return new(); }
    }
    public static async Task SaveAsync(List<CakeOrder> orders)
    {
        var json = JsonSerializer.Serialize(orders, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, json);
    }
}
