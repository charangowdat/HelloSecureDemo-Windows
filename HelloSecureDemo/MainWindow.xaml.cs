using System.IO;
using System.Security.Cryptography;
using System.Text.Json;
using System.Windows;
using Windows.Security.Credentials.UI;
namespace HelloSecureDemo;
public partial class MainWindow : Window
{
    private readonly string dataDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "HelloSecureDemo");
    private string AuthFile => Path.Combine(dataDir, "auth.json");
    private AuthRecord? record;
    public MainWindow()
    {
        InitializeComponent(); Directory.CreateDirectory(dataDir); record = LoadRecord();
        if (record is null) { Heading.Text = "Create your password"; Hint.Text = "Set a local demo password. You can then test Windows Hello."; PasswordButton.Content = "Create password"; }
        else { Heading.Text = "Secure login"; Hint.Text = "Enter your demo password, or authenticate with Windows Hello."; PasswordButton.Content = "Login with password"; HelloButton.IsEnabled = true; }
    }
    private AuthRecord? LoadRecord() { try { return File.Exists(AuthFile) ? JsonSerializer.Deserialize<AuthRecord>(File.ReadAllText(AuthFile)) : null; } catch { return null; } }
    private void PasswordButton_Click(object sender, RoutedEventArgs e)
    {
        Status.Text = ""; var password = PasswordInput.Password;
        if (password.Length < 6) { Status.Text = "Use at least 6 characters."; return; }
        if (record is null)
        {
            var salt = RandomNumberGenerator.GetBytes(16);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, 210000, HashAlgorithmName.SHA256, 32);
            record = new AuthRecord(Convert.ToBase64String(salt), Convert.ToBase64String(hash));
            File.WriteAllText(AuthFile, JsonSerializer.Serialize(record)); PasswordInput.Clear(); ShowWelcome(); return;
        }
        var candidate = Rfc2898DeriveBytes.Pbkdf2(password, Convert.FromBase64String(record.Salt), 210000, HashAlgorithmName.SHA256, 32);
        if (CryptographicOperations.FixedTimeEquals(candidate, Convert.FromBase64String(record.Hash))) { PasswordInput.Clear(); ShowWelcome(); }
        else Status.Text = "Incorrect password. Try again.";
    }
    private async void HelloButton_Click(object sender, RoutedEventArgs e)
    {
        Status.Text = "";
        try
        {
            var availability = await UserConsentVerifier.CheckAvailabilityAsync();
            if (availability != UserConsentVerifierAvailability.Available) { Status.Text = "Windows Hello is not available. Configure it in Windows Settings, or use your password."; return; }
            var result = await UserConsentVerifier.RequestVerificationAsync("Verify your identity to open HelloSecure Demo");
            if (result == UserConsentVerificationResult.Verified) ShowWelcome(); else Status.Text = $"Windows Hello verification was not completed ({result}).";
        }
        catch (Exception ex) { Status.Text = "Windows Hello could not start: " + ex.Message; }
    }
    private void ShowWelcome() { new WelcomeWindow().Show(); Close(); }
    private sealed record AuthRecord(string Salt, string Hash);
}
