namespace FzLib.Avalonia.Controls;

public interface ILayoutChangeable
{
    int Columns { get; set; }
    ItemsControlLayout Layout { get; set; }
    int Rows { get; set; }
    double Spacing { get; set; }
}
