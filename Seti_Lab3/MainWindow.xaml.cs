using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;

namespace Seti_Lab3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private static byte[] fileBytes;

        private void OpenFileButton_Click(object sender, RoutedEventArgs e)
        {
            string fileInputPath;
            var dialog = new Microsoft.Win32.OpenFileDialog();
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                fileInputPath = dialog.FileName;
            }
            else
            {
                return;
            }
            InitialDataTextBox.Clear();
            fileBytes = File.ReadAllBytes(fileInputPath);
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < fileBytes.Length; i++)
            {
                sb.Append(Convert.ToString(fileBytes[i], 2).PadLeft(8, '0'));
                sb.Append("\n");
            }
            sb.Remove(sb.Length - 1, 1);
            InitialDataTextBox.Text = sb.ToString();           
        }

        private void ChecksumCalculationButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ParityControlDataGrid.Items.Clear();
                VertHorParityControlDataGrid.Items.Clear();
                CRCTextBox.Clear();
                string polynomialString = PolynomialTextBox.Text;
                while (true)
                {
                    if (polynomialString[0].ToString() == "0")
                    {
                        polynomialString = polynomialString.Remove(0, 1);
                    }
                    else
                        break;
                }
                string polynomialSubstring = polynomialString.Remove(0, 1);
                while (true)
                {
                    if (polynomialSubstring.Length % 8 != 0 || polynomialSubstring.Length < 8)
                    {
                        polynomialSubstring = polynomialSubstring.Insert(0, "0");
                    }
                    else
                        break;
                }
                byte[] polynomialInBytes = Enumerable.Range(0, int.MaxValue / 8)
                                                     .Select(i => i * 8)
                                                     .TakeWhile(i => i < polynomialSubstring.Length)
                                                     .Select(i => polynomialSubstring.Substring(i, 8))
                                                     .Select(s => Convert.ToByte(s, 2))
                                                     .ToArray();
                byte[] horParityControlChecksum = Algorithms.HorParityControl((byte[])fileBytes.Clone());
                string crcLog = Algorithms.CRC((byte[])fileBytes.Clone(), (byte[])polynomialInBytes.Clone(), polynomialString.Length - 1); 

                for (int i = 0; i < InitialDataTextBox.LineCount; i++)
                {
                    string temp = InitialDataTextBox.GetLineText(i);
                    string[] tempArray = new string[8];
                    for (int j = 0; j < 8; j++)
                    {
                        tempArray[j] = temp[j].ToString();                       
                    }
                    ParityControlDataGrid.Items.Add(new Byte( (i + 1).ToString(), tempArray, horParityControlChecksum[i].ToString()));
                    VertHorParityControlDataGrid.Items.Add(new Byte((i + 1).ToString(), tempArray, horParityControlChecksum[i].ToString()));
                }
                var tempBytes = fileBytes;
                if (tempBytes.Length % 8 != 0)
                {
                    int extraSize = 8 - tempBytes.Length % 8;
                    Array.Resize(ref tempBytes, tempBytes.Length + extraSize);
                    for (int i = tempBytes.Length - extraSize; i < tempBytes.Length; i++)
                    {
                        tempBytes[i] = 0;
                        VertHorParityControlDataGrid.Items.Insert(i, new Byte((i + 1).ToString(), tempBytes[i], "0"));
                    }
                }
                for (int i = 0; i < tempBytes.Length / 8; i++)
                {
                    byte[] eightBytesArray = new byte[8];
                    for (int j = 0; j < 8; j++)
                    {
                        eightBytesArray[j] = tempBytes[i * 8 + j];
                    }
                    byte[] vertParityControlChecksum = Algorithms.VertParityControl((byte[])eightBytesArray.Clone());
                    VertHorParityControlDataGrid.Items.Insert(i * 8 + 8 + i * 2, new Byte("КС", vertParityControlChecksum, ""));
                    VertHorParityControlDataGrid.Items.Insert(i * 8 + 9 + i * 2, new Byte(""));
                }
                VertHorParityControlDataGrid.Items.RemoveAt(VertHorParityControlDataGrid.Items.Count - 1);
                CRCTextBox.Text = crcLog;
            }
            catch (IndexOutOfRangeException)
            {
                MessageBox.Show("Вы не ввели данные или\n" +
                                "данные содержат некорректные данные.\n", "Ошибка!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Исходные данные содержат некорректные значения.\n" + "Данные не должны содержать букв и спец. символов.", "Ошибка!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (OverflowException)
            {
                MessageBox.Show("Значение было недопустимо малым или недопустимо большим.", "Ошибка!",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
