using System.Windows;
using System.Windows.Controls;
using PKA.PAKCore;

namespace PKA
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            RsaParametersPanel = new StackPanel();
            EccParametersPanel = new StackPanel();
            ElGamalParametersPanel = new StackPanel();
            RabinParametersPanel = new StackPanel();
            
            InitializeComponent();
            ViewModel viewModel = new ViewModel();
            DataContext = viewModel;
            
        }

        private void GeneratePublicKey_Click(object sender, RoutedEventArgs e)
        {
            // 获取选择的算法和参数值
            var algorithm = (AlgorithmComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string output;

            // 根据选择的算法执行相应的公钥生成逻辑
            switch (algorithm)
            {
                case "RSA":
                    // 执行RSA算法的公钥生成逻辑
                    var rsaKeyLength = int.Parse((RsaKeyLengthComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty);
                    var iterations = int.Parse((RsaIterationsComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty);
                    var rsaKey = new RsaKey(rsaKeyLength, iterations);
                    rsaKey.GenerateRsaKey();
                    output = "KeyLength:\n" + rsaKey.KeyLength.ToString() + "\n";
                    output += "Iterations:\n" + rsaKey.Iterations.ToString() + "\n";
                    output += "Modulus:\n" + rsaKey.Modulus.ToString() + "\n";
                    output += "PublicExponent:\n" + rsaKey.PublicExponent.ToString() + "\n";
                    output += "PrivateExponent:\n" + rsaKey.PrivateExponent.ToString() + "\n";
                    OutputBox.Text = output;
                    break;
                case "ECC":
                    // 执行ECC算法的公钥生成逻辑
                    var eccKeyLength = int.Parse((EccKeyLengthComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? string.Empty);
                    var eccKey = new EccKey(eccKeyLength);
                    eccKey.GenerateEccKey();
                    output = "KeyLenth:\n" + eccKey.EccAlgorithm.KeySize.ToString() + "\n";
                    output = "PrivateKey:\n" + eccKey.PrivateKey.ToString() + "\n";
                    output = "PublicKey:\n" + eccKey.PublicKey.ToString() + "\n";
                    OutputBox.Text = output;
                    break;
                case "ElGamal":
                    // 执行ElGamal算法的公钥生成逻辑

                    break;
                case "Rabin":
                    // 执行Rabin算法的公钥生成逻辑

                    break;
                default:
                    // 默认情况下不执行任何操作
                    OutputBox.Text = "Error!";
                    break;
            }
        }

        private void AlgorithmComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // 根据选择的选项，设置相应的控件元素可见性
            string? selectedOption = (AlgorithmComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            
            RsaParametersPanel.Visibility = Visibility.Collapsed;
            EccParametersPanel.Visibility = Visibility.Collapsed;
            ElGamalParametersPanel.Visibility = Visibility.Collapsed;
            RabinParametersPanel.Visibility = Visibility.Collapsed;

            if (selectedOption != null)
            {
                if (selectedOption == "RSA")
                {
                    RsaParametersPanel.Visibility = Visibility.Visible;
                }
                else if (selectedOption == "ECC")
                {
                    EccParametersPanel.Visibility = Visibility.Visible;
                }
                else if (selectedOption == "ElGamal")
                {
                    ElGamalParametersPanel.Visibility = Visibility.Visible;
                }
                else if (selectedOption == "Rabin")
                {
                    RabinParametersPanel.Visibility = Visibility.Visible;
                }
            }
        }
    }
}