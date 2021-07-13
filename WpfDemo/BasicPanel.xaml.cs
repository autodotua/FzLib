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

namespace FzLib.WpfDemo
{
    public class BasicPanelViewModel : INotifyPropertyChanged
    {
        public BasicPanelViewModel()
        {
            MathButtonCommand = new BasicPanelMathButtonCommand(this);
            CryptographyButtonCommand = new BasicPanelCryptographyButtonCommand(this);
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

        public CipherMode AesMode { get; set; } = CipherMode.CBC;
        public PaddingMode AesPadding { get; set; } = PaddingMode.PKCS7;
        public IEnumerable AesModes { get; } = Enum.GetValues(typeof(CipherMode));
        public IEnumerable AesPaddings { get; } = Enum.GetValues(typeof(PaddingMode));

        public event PropertyChangedEventHandler PropertyChanged;

        public BasicPanelMathButtonCommand MathButtonCommand { get; }
        public BasicPanelCryptographyButtonCommand CryptographyButtonCommand { get; }
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
                                await CommonDialog.ShowOkDialogAsync("该密钥只有公钥","只能进行加密，不可用于解密");
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
                        ViewModel.RsaR = rsa.Encrypt(ViewModel.RsaText.ToUTF8Bytes(), false).ToBase64String();
                        break;

                    case "rsaDe":
                        ViewModel.RsaR = rsa.Decrypt(ViewModel.RsaText.ToBase64Bytes(), false).ToUTF8String();
                        break;
                    case "rsaEnLong":
                        ViewModel.RsaR = rsa.EncryptLong(ViewModel.RsaText.ToUTF8Bytes(), false).ToBase64String();
                        break;

                    case "rsaDeLong":
                        ViewModel.RsaR = rsa.DecryptLong(ViewModel.RsaText.ToBase64Bytes(), false).ToUTF8String();
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