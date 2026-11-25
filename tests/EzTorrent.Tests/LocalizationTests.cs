using System.Globalization;
using EzTorrent.Desktop.Localization;
using Xunit;

namespace EzTorrent.Tests;

public sealed class LocalizationTests
{
    [Fact]
    public async Task DialogTextUsesConfiguredLanguageAcrossExecutionContexts()
    {
        Texts.UseCulture(CultureInfo.GetCultureInfo("en-US"));
        var text = await Task.Run(() =>
        {
            CultureInfo.CurrentUICulture = CultureInfo.GetCultureInfo("pt-BR");
            return Texts.Get("Preferences");
        }, TestContext.Current.CancellationToken);
        Assert.Equal("Preferences", text);
    }
}
