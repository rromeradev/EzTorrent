using EzTorrent.Core.Settings;

namespace EzTorrent.Desktop.Services;

public sealed class SettingsSession(ISettingsStore store)
{
    private readonly SemaphoreSlim gate = new(1, 1);
    public AppSettings Current { get; private set; } = new();
    public event EventHandler? Changed;

    public async Task InitializeAsync() => Current = await store.LoadAsync();

    public async Task UpdateAsync(Func<AppSettings, AppSettings> update)
    {
        await gate.WaitAsync();
        try
        {
            var next = update(Current);
            await store.SaveAsync(next);
            Current = next;
            Changed?.Invoke(this, EventArgs.Empty);
        }
        finally { gate.Release(); }
    }
}
