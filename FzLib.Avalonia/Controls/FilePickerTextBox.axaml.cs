using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Platform.Storage;
using FzLib.Avalonia.Converters;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FzLib.Avalonia.Controls
{
    public partial class FilePickerTextBox : UserControl
    {
        public static readonly StyledProperty<object> ButtonContentProperty =
            AvaloniaProperty.Register<FilePickerTextBox, object>(nameof(ButtonContent), "‰Ø¿¿..");

        public static readonly StyledProperty<string> FileNamesProperty =
            AvaloniaProperty.Register<FilePickerTextBox, string>(nameof(FileNames), defaultBindingMode: BindingMode.TwoWay);

        public static readonly StyledProperty<string> LabelProperty =
            AvaloniaProperty.Register<FilePickerTextBox, string>(nameof(Label));

        public FilePickerTextBox()
        {
            InitializeComponent();
            AddHandler(DragDrop.DragEnterEvent, DragEnter);
            AddHandler(DragDrop.DropEvent, Drop);
        }
        public enum PickerType
        {
            OpenFile,
            OpenFolder,
            SaveFile
        }

        public bool AllowMultiple { get; set; }

        public object ButtonContent
        {
            get => GetValue(ButtonContentProperty);
            set => SetValue(ButtonContentProperty, value);
        }

        public string FileNames
        {
            get => GetValue(FileNamesProperty);
            set => SetValue(FileNamesProperty, value);
        }

        public List<FilePickerFileType> FileTypeFilter { get; set; }

        public string StringFileTypeFilter
        {
            set
            {
                FileTypeFilter= FilePickerFilterConverter.String2FilterList(value);
            }
        }

        public string Label
        {
            get => GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public string MultipleFilesSeparator { get; set; } = "; ";

        public string SaveFileDefaultExtension { get; set; }

        public string SaveFileSuggestedFileName { get; set; }

        public bool? ShowOverwritePrompt { get; set; }

        public string SuggestedStartLocation { get; set; }

        public string Title { get; set; }

        public PickerType Type { get; set; } = PickerType.OpenFile;

        public void DragEnter(object sender, DragEventArgs e)
        {
            if (CanDrop(e))
            {
                e.DragEffects = DragDropEffects.Link;
            }
        }

        public void Drop(object sender, DragEventArgs e)
        {
            if (CanDrop(e))
            {
                var files = e.Data.GetFiles().Select(p => p.TryGetLocalPath());
                FileNames = string.Join(MultipleFilesSeparator, files);
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var storageProvider = TopLevel.GetTopLevel(this).StorageProvider;
            string suggestedStartLocation = SuggestedStartLocation;
            if (suggestedStartLocation == null && !string.IsNullOrWhiteSpace(FileNames))
            {
                var file = FileNames.Split(MultipleFilesSeparator)[0];
                if (Type is PickerType.OpenFile or PickerType.SaveFile && File.Exists(file))
                {
                    suggestedStartLocation = Path.GetDirectoryName(file);
                }
                else if (Type is PickerType.OpenFolder && Directory.Exists(file))
                {
                    suggestedStartLocation = file;
                }
            }
            switch (Type)
            {
                case PickerType.OpenFile:
                    var openFiles = await storageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
                    {
                        AllowMultiple = AllowMultiple,
                        FileTypeFilter = FileTypeFilter,
                        Title = Title,
                        SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(suggestedStartLocation)
                    });
                    if (openFiles != null && openFiles.Count > 0)
                    {
                        FileNames = string.Join(MultipleFilesSeparator, openFiles.Select(p => p.TryGetLocalPath()));
                    }
                    break;
                case PickerType.OpenFolder:
                    var folders = await storageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions()
                    {
                        Title = Title,
                        AllowMultiple = AllowMultiple,
                        SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(suggestedStartLocation),
                    });
                    if (folders != null && folders.Count > 0)
                    {
                        FileNames = string.Join(MultipleFilesSeparator, folders.Select(p => p.TryGetLocalPath()));
                    }
                    break;
                case PickerType.SaveFile:
                    var saveFiles = await storageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
                    {
                        Title = Title,
                        FileTypeChoices = FileTypeFilter,
                        DefaultExtension = SaveFileDefaultExtension,
                        ShowOverwritePrompt = ShowOverwritePrompt,
                        SuggestedFileName = SaveFileSuggestedFileName,
                        SuggestedStartLocation = await storageProvider.TryGetFolderFromPathAsync(suggestedStartLocation),
                    });
                    if (saveFiles != null)
                    {
                        FileNames = saveFiles.TryGetLocalPath();
                    }
                    break;
            }
        }

        private bool CanDrop(DragEventArgs e)
        {
            if (e.Data.GetDataFormats().Contains(DataFormats.Files))
            {
                var fileAttributes = e.Data.GetFiles()
                    .Select(p => p.TryGetLocalPath())
                    .Select(p => File.GetAttributes(p))
                    .ToList();
                if (Type == PickerType.SaveFile && fileAttributes.Count > 1)
                {
                    return false;
                }
                var isAllDir = fileAttributes.All(p => p.HasFlag(FileAttributes.Directory));
                var isAllFile = fileAttributes.All(p => !p.HasFlag(FileAttributes.Directory));
                switch (Type)
                {
                    case PickerType.OpenFile:
                    case PickerType.SaveFile:
                        if (AllowMultiple && isAllFile)
                        {
                            return true;
                        }
                        else if (!AllowMultiple && fileAttributes.Count == 1 && isAllFile)
                        {
                            return true;
                        }
                        break;
                    case PickerType.OpenFolder:
                        if (AllowMultiple && isAllDir)
                        {
                            return true;
                        }
                        else if (!AllowMultiple && fileAttributes.Count == 1 && isAllDir)
                        {
                            return true;
                        }
                        break;
                }
                return false;
            }
            return false;
        }
    }
}
