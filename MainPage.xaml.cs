using SweetHome.Models;
using SweetHome.Services;
namespace SweetHome;
public partial class MainPage : ContentPage
{
    private readonly List<CakeProduct> _products = new()
    {
        new() { Name = "Gâteau au chocolat", Description = "Gâteau moelleux, ganache chocolat.", Price = 15000, Emoji = "🍫" },
        new() { Name = "Gâteau vanille & fraise", Description = "Crème légère et fraises.", Price = 18000, Emoji = "🍓" },
        new() { Name = "Gâteau d'anniversaire", Description = "Décoration personnalisée.", Price = 25000, Emoji = "🎂" },
        new() { Name = "Cupcakes assortis", Description = "Boîte de 6 cupcakes décorés.", Price = 8000, Emoji = "🧁" }
    };
    private List<CakeOrder> _orders = new();
    private Entry _nameEntry = new(), _phoneEntry = new(), _messageEntry = new(), _totalEntry = new(), _depositEntry = new();
    private Picker _cakePicker = new(), _sizePicker = new();
    private DatePicker _datePicker = new();

    public MainPage() { InitializeComponent(); _ = InitializeAsync(); }
    private async Task InitializeAsync() { _orders = await OrderStorage.LoadAsync(); ShowCatalog(); }
    private void OnCatalogClicked(object sender, EventArgs e) => ShowCatalog();
    private void OnNewOrderClicked(object sender, EventArgs e) => ShowOrderForm();
    private void OnOrdersClicked(object sender, EventArgs e) => ShowOrders();

    private void ShowCatalog()
    {
        SectionTitle.Text = "Notre catalogue"; StatusMessage.Text = "Choisissez un gâteau pour préparer votre commande."; ContentArea.Children.Clear();
        foreach (var product in _products)
        {
            var card = CreateCard();
            card.Children.Add(new Label { Text = $"{product.Emoji}  {product.Name}", FontSize = 18, FontAttributes = FontAttributes.Bold });
            card.Children.Add(new Label { Text = product.Description, FontSize = 14 });
            card.Children.Add(new Label { Text = $"{product.Price:N0} FCFA", FontSize = 16, TextColor = Color.FromArgb("#8D4B55"), FontAttributes = FontAttributes.Bold });
            var button = new Button { Text = "Commander ce gâteau", FontSize = 13 };
            button.Clicked += (_, _) => ShowOrderForm(product.Name); card.Children.Add(button); ContentArea.Children.Add(card);
        }
    }

    private void ShowOrderForm(string? selectedCake = null)
    {
        SectionTitle.Text = "Nouvelle commande"; StatusMessage.Text = "Renseignez les détails et l'acompte versé."; ContentArea.Children.Clear();
        _nameEntry = NewEntry("Nom du client"); _phoneEntry = NewEntry("Téléphone");
        _cakePicker = new Picker { Title = "Choisir un gâteau", ItemsSource = _products.Select(p => p.Name).ToList() };
        var index = selectedCake == null ? 0 : _products.FindIndex(p => p.Name == selectedCake);
        _cakePicker.SelectedIndex = Math.Max(0, index);
        _sizePicker = new Picker { Title = "Taille", ItemsSource = new List<string> { "Petit", "Moyen", "Grand" }, SelectedIndex = 1 };
        _messageEntry = NewEntry("Message sur le gâteau (facultatif)");
        _datePicker = new DatePicker { MinimumDate = DateTime.Today, Date = DateTime.Today.AddDays(2) };
        _totalEntry = NewEntry("Prix total en FCFA"); _depositEntry = NewEntry("Acompte versé en FCFA");
        var form = CreateCard();
        AddField(form, "Nom du client", _nameEntry); AddField(form, "Téléphone", _phoneEntry); AddField(form, "Gâteau", _cakePicker);
        AddField(form, "Taille", _sizePicker); AddField(form, "Message personnalisé", _messageEntry); AddField(form, "Date de livraison", _datePicker);
        AddField(form, "Prix total (FCFA)", _totalEntry); AddField(form, "Acompte (FCFA)", _depositEntry);
        var selected = _products.FirstOrDefault(p => p.Name == selectedCake) ?? _products[0]; _totalEntry.Text = selected.Price.ToString("0");
        _cakePicker.SelectedIndexChanged += (_, _) => { var p = _products.FirstOrDefault(x => x.Name == _cakePicker.SelectedItem?.ToString()); if (p != null) _totalEntry.Text = p.Price.ToString("0"); };
        var save = new Button { Text = "Enregistrer la commande" }; save.Clicked += async (_, _) => await SaveOrderAsync(); form.Children.Add(save); ContentArea.Children.Add(form);
    }

    private async Task SaveOrderAsync()
    {
        if (string.IsNullOrWhiteSpace(_nameEntry.Text) || string.IsNullOrWhiteSpace(_phoneEntry.Text) || _cakePicker.SelectedItem == null)
        { await DisplayAlert("Informations manquantes", "Saisissez le nom, le téléphone et choisissez un gâteau.", "OK"); return; }
        if (!decimal.TryParse(_totalEntry.Text, out var total) || total <= 0 ||
            !decimal.TryParse(string.IsNullOrWhiteSpace(_depositEntry.Text) ? "0" : _depositEntry.Text, out var deposit) || deposit < 0 || deposit > total)
        { await DisplayAlert("Montants incorrects", "Le prix doit être positif et l'acompte compris entre 0 et le prix total.", "OK"); return; }
        var order = new CakeOrder { CustomerName = _nameEntry.Text.Trim(), Phone = _phoneEntry.Text.Trim(),
            CakeName = _cakePicker.SelectedItem.ToString()!, Size = _sizePicker.SelectedItem?.ToString() ?? "Moyen",
            MessageOnCake = _messageEntry.Text?.Trim() ?? "", DeliveryDate = _datePicker.Date, TotalPrice = total,
            Deposit = deposit, Status = "Nouvelle commande" };
        _orders.Insert(0, order); await OrderStorage.SaveAsync(_orders);
        await DisplayAlert("Commande enregistrée", $"Commande de {order.CustomerName} enregistrée. Reste à payer : {order.Remaining:N0} FCFA.", "OK");
        ShowOrders();
    }

    private void ShowOrders()
    {
        SectionTitle.Text = "Commandes et suivi"; StatusMessage.Text = $"{_orders.Count} commande(s) enregistrée(s) sur cet appareil."; ContentArea.Children.Clear();
        if (_orders.Count == 0) { ContentArea.Children.Add(new Label { Text = "Aucune commande pour le moment. Appuyez sur « Commander » pour en créer une." }); return; }
        foreach (var order in _orders)
        {
            var card = CreateCard();
            card.Children.Add(new Label { Text = $"{order.CakeName} — {order.CustomerName}", FontSize = 17, FontAttributes = FontAttributes.Bold });
            card.Children.Add(new Label { Text = $"Téléphone : {order.Phone}" });
            card.Children.Add(new Label { Text = $"Taille : {order.Size} | Livraison : {order.DeliveryDate:dd/MM/yyyy}" });
            if (!string.IsNullOrWhiteSpace(order.MessageOnCake)) card.Children.Add(new Label { Text = $"Message : {order.MessageOnCake}" });
            card.Children.Add(new Label { Text = $"Total : {order.TotalPrice:N0} FCFA | Acompte : {order.Deposit:N0} FCFA" });
            card.Children.Add(new Label { Text = $"Reste à payer : {order.Remaining:N0} FCFA", FontAttributes = FontAttributes.Bold, TextColor = Color.FromArgb("#8D4B55") });
            var picker = new Picker { Title = "État de préparation", ItemsSource = new List<string> { "Nouvelle commande", "En préparation", "Prête", "Livrée" }, SelectedItem = order.Status };
            picker.SelectedIndexChanged += async (_, _) => { if (picker.SelectedItem is string status) { order.Status = status; await OrderStorage.SaveAsync(_orders); StatusMessage.Text = $"Statut mis à jour : {status}"; } };
            card.Children.Add(new Label { Text = "Statut", FontAttributes = FontAttributes.Bold }); card.Children.Add(picker); ContentArea.Children.Add(card);
        }
    }

    private static Entry NewEntry(string placeholder) => new() { Placeholder = placeholder, BackgroundColor = Colors.White, TextColor = Color.FromArgb("#35292A"), PlaceholderColor = Color.FromArgb("#8B7C7D") };
    private static VerticalStackLayout CreateCard() => new() { Padding = 16, Spacing = 10, BackgroundColor = Colors.White, Margin = new Thickness(0, 0, 0, 4) };
    private static void AddField(VerticalStackLayout parent, string label, View input) { parent.Children.Add(new Label { Text = label, FontAttributes = FontAttributes.Bold, FontSize = 13 }); parent.Children.Add(input); }
}
