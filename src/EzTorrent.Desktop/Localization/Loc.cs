using System.Globalization;
using System.Resources;
using System.Windows.Markup;

namespace EzTorrent.Desktop.Localization;

public static class Texts
{
    private static readonly ResourceManager Resources = new("EzTorrent.Desktop.Resources.Strings", typeof(Texts).Assembly);
    private static CultureInfo uiCulture = CultureInfo.CurrentUICulture;
    public static void UseCulture(CultureInfo culture)
    {
        ArgumentNullException.ThrowIfNull(culture);
        Volatile.Write(ref uiCulture, culture);
    }
    // Dispatcher callbacks can restore a different thread culture after async startup.
    public static string Get(string key) => Resources.GetString(key, Volatile.Read(ref uiCulture)) ?? key;
}

public sealed class LocExtension(string key) : MarkupExtension
{
    public override object ProvideValue(IServiceProvider serviceProvider) => Texts.Get(key);
}
