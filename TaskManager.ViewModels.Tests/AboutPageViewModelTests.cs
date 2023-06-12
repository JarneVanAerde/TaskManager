using TaskManager.Services;
using Microsoft.Maui.ApplicationModel;
using System;
using Microsoft.Maui.Graphics;

namespace TaskManager.ViewModels.Tests;

public class AboutPageViewModelTests
{
    private readonly AboutPageViewModel _sut;
    private readonly IAlertService _alertServiceMock;
    private readonly IAppInfo _appInfoMock;
    private readonly IVersionTracking _versionTrackingMock;
    private readonly IMap _mapMock;
    private readonly IBrowser _browserMock;

    public AboutPageViewModelTests()
    {
        _alertServiceMock = Substitute.For<IAlertService>();
        _appInfoMock = Substitute.For<IAppInfo>();
        _versionTrackingMock = Substitute.For<IVersionTracking>();
        _mapMock = Substitute.For<IMap>();
        _browserMock = Substitute.For<IBrowser>();

        _sut = new AboutPageViewModel(
            _alertServiceMock,
            _appInfoMock,
            _versionTrackingMock,
            _mapMock,
            _browserMock);
    }

    [Fact]
    public void Ctor_InitializesWithCorrectTitle()
    {
        Assert.Equal("About", _sut.Title);
    }

    [Fact]
    public void Ctor_InitializesWithCorrectAppInfoProperties()
    {
        _appInfoMock.Name.Returns("Test App");
        _appInfoMock.PackageName.Returns("com.example.app");
        _appInfoMock.VersionString.Returns("1.0");
        _appInfoMock.BuildString.Returns("12345");

        var sut = new AboutPageViewModel(
            _alertServiceMock,
            _appInfoMock,
            _versionTrackingMock,
            _mapMock,
            _browserMock);

        Assert.Equal("Test App", sut.Name);
        Assert.Equal("com.example.app", sut.Package);
        Assert.Equal("1.0", sut.Version);
        Assert.Equal("12345", sut.Build);
    }

    [Fact]
    public void Ctor_InitializesWithYESForFirstLaunch()
    {
        _versionTrackingMock.IsFirstLaunchEver.Returns(true);

        var sut = new AboutPageViewModel(
            _alertServiceMock,
            _appInfoMock,
            _versionTrackingMock,
            _mapMock,
            _browserMock);

        Assert.Equal("YES!", sut.FirstLaunch);
    }

    [Fact]
    public void Ctor_InitializesWithNewForFirstLaunch()
    {
        _versionTrackingMock.IsFirstLaunchEver.Returns(false);

        var sut = new AboutPageViewModel(
            _alertServiceMock,
            _appInfoMock,
            _versionTrackingMock,
            _mapMock,
            _browserMock);

        Assert.Equal("New...", sut.FirstLaunch);
    }

    [Fact]
    public void ShowAppSettings_CallsAppInfoCorrectly()
    {
        _sut.ShowAppSettings();

        _appInfoMock.Received(1).ShowSettingsUI();
    }

    //[Fact]
    //public async Task GoToMaps_CallsMapCorrectly()
    //{
    //    var location = new Location(51.3321142, 4.38072190733679);
    //    var options = new MapLaunchOptions { Name = "Home base" };

    //    await _sut.GoToMaps();

    //    await _mapMock.Received(1).OpenAsync(location, Arg.Is<MapLaunchOptions>(x => x.Name == options.));
    //}

    //[Fact]
    //public async Task GoToMaps_OpeningMapFails_DisplaysError()
    //{
    //    _mapMock.OpenAsync(Arg.Any<Location>(), Arg.Any<MapLaunchOptions>())
    //        .Throws(new Exception("Failed to open maps"));

    //    await _sut.GoToMaps();

    //    await _alertServiceMock.Received(1).DisplayError("Unable to open the maps app");
    //}

    [Fact]
    public async Task GoToPlayStore_CallsBrowserCorrectly()
    {
        var uri = new Uri("https://play.google.com/store/apps/details?id=com.google.android.apps.tasks&hl=nl&gl=US");
        
        await _sut.GoToPlayStore();

        await _browserMock.Received(1).OpenAsync(
            uri,
            Arg.Is<BrowserLaunchOptions>(
                x => x.LaunchMode == BrowserLaunchMode.SystemPreferred &&
                x.TitleMode == BrowserTitleMode.Show &&
                x.PreferredToolbarColor == Colors.Orange));
    }

    //[Fact]
    //public async Task GoToPlayStore_CommandExecuted_DisplaysErrorIfOpeningBrowserFails()
    //{
    //    Arrange
    //   var uri = new Uri(AboutPageViewModel.PlayStoreURL);
    //    var options = new BrowserLaunchOptions
    //    {
    //        LaunchMode = BrowserLaunchMode.SystemPreferred,
    //        TitleMode = BrowserTitleMode.Show,
    //        PreferredToolbarColor = Colors.Orange
    //    };
    //    Browser.Default.OpenAsync(uri, options).Throws(new Exception("Failed to open browser"));

    //    Act
    //   await _sut.GoToPlayStoreCommand.ExecuteAsync(null);

    //    Assert
    //   await _alertServiceMock.Received(1).DisplayError("Unable to open the browser");
    //}
}
