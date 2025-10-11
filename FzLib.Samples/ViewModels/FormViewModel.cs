using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia.Layout;
using CommunityToolkit.Mvvm.ComponentModel;

namespace FzLib.Samples.ViewModels;

public partial class FormViewModel : ObservableObject
{
    [ObservableProperty]
    private bool formHeaderIsChecked = true;

    [ObservableProperty]
    private string[] items2 = ["左边", "右边"];

    [ObservableProperty]
    private bool selected2;

    [ObservableProperty]
    private int selectedIndex1;

    [ObservableProperty]
    private HorizontalAlignment selectedItem1;
    
    [ObservableProperty]
    private string selectedItem2;
    
    [ObservableProperty]
    private ObservableStringList stringList = new ObservableStringList(["001", "two", "第三"]);
}