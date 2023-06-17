﻿using CommunityToolkit.Mvvm.ComponentModel;

namespace TaskManager.ViewModels;

public partial class BaseViewModel : ObservableObject
{
    [ObservableProperty]
    private string title;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    private bool isBusy;

    public bool IsNotBusy => !IsBusy;
}
