using System;
using System.Windows;
using System.Windows.Controls;
using PKA.PAKCore;

namespace PKA
{
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
            ViewModel viewModel = new ViewModel();
            DataContext = viewModel;
        }

        private void GeneratePublicKey_Click(object sender, RoutedEventArgs e)
        {
            // 获取选择的算法和参数值
            var algorithm = (AlgorithmComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();


            // 根据选择的算法执行相应的公钥生成逻辑
            switch (algorithm)
            {
                case "RSA":
                    var keyLength = int.Parse((KeyLengthComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                    var iterations = int.Parse((IterationsComboBox.SelectedItem as ComboBoxItem)?.Content.ToString());
                    // 执行RSA算法的公钥生成逻辑
                    var rsaKey = new RsaKey(keyLength, iterations);
                    rsaKey.GenerateRsaKey();
                    var output = "KeyLength:\n" + rsaKey.KeyLength.ToString() + "\n";
                    output += "Iterations:\n" + rsaKey.Iterations.ToString() + "\n";
                    output += "Modulus:\n" + rsaKey.Modulus.ToString() + "\n";
                    output += "PublicExponent:\n" + rsaKey.PublicExponent.ToString() + "\n";
                    output += "PrivateExponent:\n" + rsaKey.PrivateExponent.ToString() + "\n";
                    OutputBox.Text = output;
                    break;
                case "ECC":
                    // 执行ECC算法的公钥生成逻辑
                   
                    break;
                case "ElGamal":
                    // 执行ElGamal算法的公钥生成逻辑
                   
                    break;
                case "Rabin":
                    // 执行Rabin算法的公钥生成逻辑
                   
                    break;
                default:
                    // 默认情况下不执行任何操作
                    break;
            }
        }
    }
}