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

namespace FzLib.WpfDemo
{
    public class BasicPanelViewModel : INotifyPropertyChanged
    {
        public BasicPanelViewModel()
        {
            ButtonCommand = new BasicPanelButtonCommand(this);
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

        public event PropertyChangedEventHandler PropertyChanged;

        public BasicPanelButtonCommand ButtonCommand { get; }
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