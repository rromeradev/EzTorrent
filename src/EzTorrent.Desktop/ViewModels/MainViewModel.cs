using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
namespace EzTorrent.Desktop.ViewModels;
public sealed record DownloadItem(string Name, double Progress, string Status, string DownloadSpeed, string UploadSpeed);
public sealed class MainViewModel : ObservableObject
{
    public ReadOnlyObservableCollection<DownloadItem> Downloads { get; } = new(new ObservableCollection<DownloadItem>());
    public bool IsEmpty => Downloads.Count == 0;
}
