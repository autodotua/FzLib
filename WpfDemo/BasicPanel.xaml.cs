using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static FzLib.INotifyPropertyChangedExtension;
using System.Numerics;
using System.Linq;
using FzLib.Cryptography;
using System;
using System.Security.Cryptography;
using System.Collections;
using Microsoft.WindowsAPICodePack.Dialogs;
using Microsoft.WindowsAPICodePack.FzExtension;
using ModernWpf.FzExtension.CommonDialog;
using System.IO;
using FzLib.DataStorage.Serialization;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Globalization;

namespace FzLib.WpfDemo
{
    public class NumberValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string s = value as string;
            double d;
            long l;
            switch (parameter as string)
            {
                case "bytes":
                    if (!long.TryParse(s, out l) || l < 0)
                    {
                        return "请输入一个正整数";
                    }
                    return NumberConverter.ByteToFitString(l);

                case "time":
                    if (!long.TryParse(s, out l) || l < 0)
                    {
                        return "请输入一个正整数";
                    }
                    return NumberConverter.SecondToFitString(l);

                case "length":
                    if (!double.TryParse(s, out d) || d < 0)
                    {
                        return "请输入一个正数";
                    }
                    return NumberConverter.MeterToFitString(d);

                case "area":
                    if (!double.TryParse(s, out d) || d < 0)
                    {
                        return "请输入一个正数";
                    }
                    return NumberConverter.SquareMeterToFitString(d);

                default:
                    return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    [Serializable]
    public class SerializationData : IJsonSerializable, INotifyPropertyChanged
    {
        public SerializationData()
        {
            Path = System.IO.Path.GetTempFileName();
        }

        public string Path { get; }
        public JsonSerializerSettings Settings { get; set; }
        private string text = "文字";

        public string Text
        {
            get => text;
            set => this.SetValueAndNotify(ref text, value, nameof(Text));
        }

        private DateTime date = DateTime.Now;

        public DateTime Date
        {
            get => date;
            set => this.SetValueAndNotify(ref date, value, nameof(Date));
        }

        private bool @bool = true;

        public bool Bool
        {
            get => @bool;
            set => this.SetValueAndNotify(ref @bool, value, nameof(Bool));
        }

        private double num = 12.34;

        public event PropertyChangedEventHandler PropertyChanged;

        public double Num
        {
            get => num;
            set => this.SetValueAndNotify(ref num, value, nameof(Num));
        }

        private Hashs hash = Hashs.SHA1;

        public Hashs Hash
        {
            get => hash;
            set => this.SetValueAndNotify(ref hash, value, nameof(Hash));
        }

        private ObservableCollection<SerializationData> childrens;

        public ObservableCollection<SerializationData> Childrens
        {
            get => childrens;
            set => this.SetValueAndNotify(ref childrens, value, nameof(Childrens));
        }
    }

    public class BasicPanelViewModel : INotifyPropertyChanged
    {
        public BasicPanelViewModel()
        {
            ButtonCommand = new BasicPanelButtonCommand(this);
            MathButtonCommand = new BasicPanelMathButtonCommand(this);
            CryptographyButtonCommand = new BasicPanelCryptographyButtonCommand(this);
            SerializationButtonCommand = new BasicPanelSerializationButtonCommand(this);
            SerializationData = new SerializationData();
            SerializationData.Childrens = new ObservableCollection<SerializationData>()
        { new SerializationData(), new SerializationData() };
            SerializationData.SetDateTimeFormat("yyyy-MM-dd")
                .SetIndented()
                .UseStringEnumValue();
        }

        private string calc;

        public string Calc
        {
            get => calc;
            set => this.SetValueAndNotify(ref calc, value, nameof(Calc));
        }

        private string calcR;

        public string CalcR
        {
            get => calcR;
            set => this.SetValueAndNotify(ref calcR, value, nameof(CalcR));
        }

        private string func;

        public string Func
        {
            get => func;
            set => this.SetValueAndNotify(ref func, value, nameof(Func));
        }

        private double funcX;

        public double FuncX
        {
            get => funcX;
            set => this.SetValueAndNotify(ref funcX, value, nameof(FuncX));
        }

        private string funcR;

        public string FuncR
        {
            get => funcR;
            set => this.SetValueAndNotify(ref funcR, value, nameof(FuncR));
        }

        public long Euclid1 { get; set; }
        public long Euclid2 { get; set; }
        private long euclidR;

        public long EuclidR
        {
            get => euclidR;
            set => this.SetValueAndNotify(ref euclidR, value, nameof(EuclidR));
        }

        public long CommonDivisor1 { get; set; }
        public long CommonDivisor2 { get; set; }
        private long commonDivisorR;

        public long CommonDivisorR
        {
            get => commonDivisorR;
            set => this.SetValueAndNotify(ref commonDivisorR, value, nameof(CommonDivisorR));
        }

        public long Prime { get; set; }
        private bool? primeR;

        public bool? PrimeR
        {
            get => primeR;
            set => this.SetValueAndNotify(ref primeR, value, nameof(PrimeR));
        }

        public long PPrime { get; set; }
        private bool? pPrimeR;

        public bool? PPrimeR
        {
            get => pPrimeR;
            set => this.SetValueAndNotify(ref pPrimeR, value, nameof(PPrimeR));
        }

        public long Facter { get; set; }
        private string facterR;

        public string FacterR
        {
            get => facterR;
            set => this.SetValueAndNotify(ref facterR, value, nameof(FacterR));
        }

        public string AesKey { get; set; } = "1234";
        public string AesIV { get; set; } = "1234";
        public string AesText { get; set; }
        private string aesR;
        public string AesSuffix { get; set; } = ".aes";
        public bool AesVolume { get; set; } = false;

        public string AesR
        {
            get => aesR;
            set => this.SetValueAndNotify(ref aesR, value, nameof(AesR));
        }

        private string aesFileSource;

        public string AesFileSource
        {
            get => aesFileSource;
            set => this.SetValueAndNotify(ref aesFileSource, value, nameof(AesFileSource));
        }

        private string aesFileTarget;

        public string AesFileTarget
        {
            get => aesFileTarget;
            set => this.SetValueAndNotify(ref aesFileTarget, value, nameof(AesFileTarget));
        }

        private string rsaPublicXml;

        public string RsaPublicXml
        {
            get => rsaPublicXml;
            set => this.SetValueAndNotify(ref rsaPublicXml, value, nameof(RsaPublicXml));
        }

        private string rsaPrivateXml;

        public string RsaPrivateXml
        {
            get => rsaPrivateXml;
            set => this.SetValueAndNotify(ref rsaPrivateXml, value, nameof(RsaPrivateXml));
        }

        private string rsaPublicPem;

        public string RsaPublicPem
        {
            get => rsaPublicPem;
            set => this.SetValueAndNotify(ref rsaPublicPem, value, nameof(RsaPublicPem));
        }

        private string rsaText;

        public string RsaText
        {
            get => rsaText;
            set => this.SetValueAndNotify(ref rsaText, value, nameof(RsaText));
        }

        private string rsaR;

        public string RsaR
        {
            get => rsaR;
            set => this.SetValueAndNotify(ref rsaR, value, nameof(RsaR));
        }

        public bool OAEP { get; set; }

        public Hashs Hash { get; set; } = FzLib.Cryptography.Hashs.SHA256;
        public string HashText { get; set; }
        private string hashFile;

        public string HashFile
        {
            get => hashFile;
            set => this.SetValueAndNotify(ref hashFile, value, nameof(HashFile));
        }

        private string hashR;
        public string HashSeparator { get; set; } = "";
        public string HashFormat { get; set; } = "X2";

        public string HashR
        {
            get => hashR;
            set => this.SetValueAndNotify(ref hashR, value, nameof(HashR));
        }

        public CipherMode AesMode { get; set; } = CipherMode.CBC;
        public PaddingMode AesPadding { get; set; } = PaddingMode.PKCS7;
        public IEnumerable AesModes { get; } = Enum.GetValues(typeof(CipherMode));
        public IEnumerable AesPaddings { get; } = Enum.GetValues(typeof(PaddingMode));
        public IEnumerable Hashs { get; } = Enum.GetValues(typeof(Hashs));

        private SerializationData serializationData;

        public SerializationData SerializationData
        {
            get => serializationData;
            set => this.SetValueAndNotify(ref serializationData, value, nameof(SerializationData));
        }

        public DateTime Time1 { get; set; } = DateTime.Now - TimeSpan.FromHours(24);
        public DateTime Time2 { get; set; } = DateTime.Now;
        private DateTime timeR;

        public DateTime TimeR
        {
            get => timeR;
            set => this.SetValueAndNotify(ref timeR, value, nameof(TimeR));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public BasicPanelButtonCommand ButtonCommand { get; }
        public BasicPanelMathButtonCommand MathButtonCommand { get; }
        public BasicPanelCryptographyButtonCommand CryptographyButtonCommand { get; }
        public BasicPanelSerializationButtonCommand SerializationButtonCommand { get; }
    }

    public class BasicPanelButtonCommand : PanelButtonCommandBase
    {
        public BasicPanelButtonCommand(BasicPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public BasicPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            Do(() =>
            {
                switch (parameter as string)
                {
                    case "timeAve":
                        ViewModel.TimeR = DateTimeExtension.GetAverageDateTime(ViewModel.Time1, ViewModel.Time2);
                        break;
                }
            });
        }
    }

    public class BasicPanelSerializationButtonCommand : PanelButtonCommandBase
    {
        public BasicPanelSerializationButtonCommand(BasicPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public BasicPanelViewModel ViewModel { get; }
        private string binPath = System.IO.Path.GetTempFileName();

        public override void Execute(object parameter)
        {
            Do(() =>
           {
               switch (parameter as string)
               {
                   case "saveJSON":
                       ViewModel.SerializationData.Save();
                       break;

                   case "loadJSON":
                       App.Current.Dispatcher.Invoke(() =>
                       {
                           if (!ViewModel.SerializationData.TryLoadFromJsonFile())
                           {
                               CommonDialog.ShowErrorDialogAsync("不存在已保存的配置文件");
                           }
                       });
                       break;

                   case "browseJSON":
                       if (!File.Exists(ViewModel.SerializationData.Path))
                       {
                           CommonDialog.ShowErrorDialogAsync("不存在已保存的配置文件");
                       }
                       else
                       {
                           IO.FileSystem.OpenFileOrFolder("notepad.exe", ViewModel.SerializationData.Path);
                       }
                       break;

                   case "saveBin":
                       var bin = BinarySerialization.Serialize(ViewModel.SerializationData);
                       File.WriteAllBytes(binPath, bin);
                       break;

                   case "openBin":
                       if (!File.Exists(binPath))
                       {
                           CommonDialog.ShowErrorDialogAsync("不存在已保存的配置文件");
                       }
                       else
                       {
                           var obj = BinarySerialization.Deserialize<SerializationData>(File.ReadAllBytes(binPath));
                           ViewModel.SerializationData = obj;
                       }
                       break;
               }
           });
        }
    }

    public class BasicPanelCryptographyButtonCommand : PanelButtonCommandBase
    {
        public BasicPanelCryptographyButtonCommand(BasicPanelViewModel viewModel)
        {
            ViewModel = viewModel;
            rsa.PersistKeyInCsp = false;
            ViewModel.RsaPublicXml = rsa.ToXmlString(false);
            ViewModel.RsaPrivateXml = rsa.ToXmlString(true);
            viewModel.RsaPublicPem = rsa.ToPemPublicKey();
        }

        public BasicPanelViewModel ViewModel { get; }
        private RSACryptoServiceProvider rsa = RsaExtension.Create();

        public override void Execute(object parameter)
        {
            Do(async () =>
            {
                switch (parameter as string)
                {
                    case "aesStr":
                        ViewModel.AesR = AesExtension.CreateManager(ViewModel.AesMode, ViewModel.AesPadding)
                        .SetStringKey(ViewModel.AesKey)
                        .SetStringIV(ViewModel.AesIV)
                       .Encrypt(ViewModel.AesText);
                        break;

                    case "aesBrowseSource":

                        ViewModel.AesFileSource = App.Current.Dispatcher.Invoke(() =>
                        new FileFilterCollection().AddAll().CreateDialog<CommonOpenFileDialog>().GetFilePath());
                        break;

                    case "aesBrowseTarget":
                        ViewModel.AesFileTarget = App.Current.Dispatcher.Invoke(() =>
                        new FileFilterCollection().AddAll().CreateDialog<CommonSaveFileDialog>().GetFilePath());
                        break;

                    case "aesEnFile":
                        AesExtension.CreateManager(ViewModel.AesMode, ViewModel.AesPadding)
                    .SetStringKey(ViewModel.AesKey)
                    .SetStringIV(ViewModel.AesIV)
                    .EncryptFile(ViewModel.AesFileSource,
                                 ViewModel.AesFileTarget,
                                 suffix: ViewModel.AesSuffix,
                                 volumeSize: ViewModel.AesVolume ? 1024 * 1024 * 64 : 0,
                                 refreshFileProgress: (source, target, max, value) =>
                                 {
                                     ViewModel.AesR = $"{source}=>{target}  {1.0 * value / max:0.00%}  {value}/{max}";
                                 });
                        ViewModel.AesR = "完成";
                        break;

                    case "aesDeFile":
                        AesExtension.CreateManager(ViewModel.AesMode, ViewModel.AesPadding)
                    .SetStringKey(ViewModel.AesKey)
                    .SetStringIV(ViewModel.AesIV)
                    .DecryptFile(ViewModel.AesFileSource,
                                 ViewModel.AesFileTarget,
                                 suffix: ViewModel.AesSuffix,
                                 refreshFileProgress: (source, target, max, value) =>
                                 {
                                     ViewModel.AesR = $"{source}=>{target}  {1.0 * value / max:0.00%}  {value}/{max}";
                                 });
                        ViewModel.AesR = "完成";
                        break;

                    case "setRsaXml":
                        string xml = await CommonDialog.ShowInputDialogAsync("请输入XML格式的密钥", multiLines: true, maxLines: 4);
                        if (xml != null)
                        {
                            rsa.ImportXmlKey(xml);
                            ViewModel.RsaPublicXml = rsa.ToXmlString(false);
                            ViewModel.RsaPublicPem = rsa.ToPemPublicKey();
                            try
                            {
                                ViewModel.RsaPrivateXml = rsa.ToXmlString(true);
                                ViewModel.RsaPublicPem = "";
                            }
                            catch (Exception)
                            {
                                await CommonDialog.ShowOkDialogAsync("该密钥只有公钥", "只能进行加密，不可用于解密");
                            }
                        }
                        break;

                    case "setRsaPem":
                        string pem = await CommonDialog.ShowInputDialogAsync("请输入PEM格式的密钥", multiLines: true, maxLines: 4);
                        if (pem != null)
                        {
                            rsa.ImportPemKey(pem);
                            ViewModel.RsaPublicXml = rsa.ToXmlString(false);
                            ViewModel.RsaPublicPem = rsa.ToPemPublicKey();
                            ViewModel.RsaPublicPem = "";
                        }
                        break;

                    case "rsaEn":
                        ViewModel.RsaR = rsa.Encrypt(ViewModel.RsaText.ToUTF8Bytes(), ViewModel.OAEP).ToBase64String();
                        break;

                    case "rsaDe":
                        ViewModel.RsaR = rsa.Decrypt(ViewModel.RsaText.ToBase64Bytes(), ViewModel.OAEP).ToUTF8String();
                        break;

                    case "rsaEnLong":
                        ViewModel.RsaR = rsa.EncryptLong(ViewModel.RsaText.ToUTF8Bytes(), ViewModel.OAEP).ToBase64String();
                        break;

                    case "rsaDeLong":
                        ViewModel.RsaR = rsa.DecryptLong(ViewModel.RsaText.ToBase64Bytes(), ViewModel.OAEP).ToUTF8String();
                        break;

                    case "hashText":
                        ViewModel.HashR = new Hash()
                        {
                            HexStringFormat = ViewModel.HashFormat,
                            HexStringSeparator = ViewModel.HashSeparator
                        }.GetString(ViewModel.Hash, ViewModel.HashText);
                        break;

                    case "hashBrowse":
                        ViewModel.HashFile = App.Current.Dispatcher.Invoke(() =>
                     new FileFilterCollection().AddAll().CreateDialog<CommonOpenFileDialog>().GetFilePath());
                        break;

                    case "hashFile":
                        if (string.IsNullOrEmpty(ViewModel.HashFile))
                        {
                            throw new Exception("需要先设置文件地址");
                        }
                        ViewModel.HashR = new Hash()
                        {
                            HexStringFormat = ViewModel.HashFormat,
                            HexStringSeparator = ViewModel.HashSeparator
                        }.GetStringFromFile(ViewModel.Hash, ViewModel.HashFile);
                        break;

                    case "hashFileAll":
                        using (var stream = File.OpenRead(ViewModel.HashFile))
                        {
                            var hashs = Enum.GetValues(typeof(Hashs)).Cast<Hashs>().ToArray();
                            ViewModel.HashR =
                                  string.Join(Environment.NewLine,
                                  new Hash()
                                  {
                                      HexStringFormat = ViewModel.HashFormat,
                                      HexStringSeparator = ViewModel.HashSeparator
                                  }.GetString(hashs, stream)
                                  .Select(p => p.Key.ToString() + "：" + p.Value));
                        }

                        break;
                }
            });
        }
    }

    public class BasicPanelMathButtonCommand : PanelButtonCommandBase
    {
        public BasicPanelMathButtonCommand(BasicPanelViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public BasicPanelViewModel ViewModel { get; }

        public override void Execute(object parameter)
        {
            Do(() =>
            {
                switch (parameter as string)
                {
                    case "calc":
                        {
                            Calculation c = new Calculation();
                            var result = c.Calc(ViewModel.Calc);
                            ViewModel.CalcR = result.Success ? result.Value.ToString() : result.LastExpression;
                        }
                        break;

                    case "func":
                        {
                            Calculation c = new Calculation();
                            c.InitializeFunction(ViewModel.Func, "x");
                            var result = c.F(ViewModel.FuncX);
                            ViewModel.FuncR = result.Success ? result.Value.ToString() : result.LastExpression;
                        }
                        break;

                    case "euclid":
                        {
                            long big = ViewModel.Euclid1;
                            long small = ViewModel.Euclid2;
                            if (big < small)
                            {
                                long temp = big;
                                big = small;
                                small = temp;
                            }
                            ViewModel.EuclidR = Algebra.ExtendedEuclid(ViewModel.Euclid1, ViewModel.Euclid2);
                        }
                        break;

                    case "divisor":
                        ViewModel.CommonDivisorR = Algebra.GetCommonDivisor(ViewModel.CommonDivisor1, ViewModel.CommonDivisor2);
                        break;

                    case "prime":
                        ViewModel.PrimeR = Algebra.IsPrime(ViewModel.Prime);
                        break;

                    case "pprime":
                        ViewModel.PPrimeR = Algebra.IsProbablePrime(ViewModel.PPrime);
                        break;

                    case "factor":
                        ViewModel.FacterR = string.Join('+',
                            Algebra.DecomposeFacter(ViewModel.Facter)
                            .Select(p => p.Value == 1 ? p.Key.ToString() : $"{p.Key}^{p.Value}"));
                        break;
                }
            });
        }
    }

    /// <summary>
    /// BasicPanel.xaml 的交互逻辑
    /// </summary>
    public partial class BasicPanel : UserControl
    {
        public BasicPanelViewModel ViewModel { get; } = new BasicPanelViewModel();

        public BasicPanel()
        {
            DataContext = ViewModel;
            InitializeComponent();
        }
    }
}