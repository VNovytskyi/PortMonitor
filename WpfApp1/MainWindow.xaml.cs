using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using System.IO.Ports;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static EventWaitHandle allowRefreshComPorts = new ManualResetEvent(initialState: true);

        private static Thread RefreshComPortsList = null;
        private static Thread ConnectToComPort = null;

        private static SerialPort serialPort = null;

        private static string[] baudRateValues = { "9600", "115200"};
        private static string[] dataBitsValues = { "5", "6", "7", "8", "9"};

        
        private static string[] parityValues = { "none", "odd", "even", "mark", "space"};
        private static string[] stopBitsValues = { "1", "2", "1.5"};

        private static int oldComPortsAmount = -1;

        public MainWindow()
        {
            InitializeComponent();

            InitBaudRateValuesComboBox();
            InitDataBitsValuesComboBox();
            InitParityValuesComboBox();
            InitStopBitsValuesComboBox();

            serialPort = new SerialPort();

            RefreshComPortsList = new Thread(RefreshComPortsListFunc);
            RefreshComPortsList.Start();
        }

        private void RefreshComPortsListFunc()
        {
            while(true)
            {
                allowRefreshComPorts.WaitOne();

                string[] portNames = SerialPort.GetPortNames();
                int currentComPortsAmount = portNames.Length;

                if (oldComPortsAmount != currentComPortsAmount)
                {
                    Dispatcher.Invoke(() =>
                    {
                        PortsComboBox.Items.Clear();
                        foreach (string port in portNames)
                            PortsComboBox.Items.Add(port);
                    });

                    if (currentComPortsAmount > 0)
                    {
                        EnableSettings();
                        setDefaultSettings();
                    }
                    else
                    {
                        DisableSettings();
                        setEmptySettings();
                    }
                }
                
                oldComPortsAmount = currentComPortsAmount;

                Thread.Sleep(100);
            }
        }

        private void EnableSettings()
        {
            Dispatcher.Invoke(() =>
            {
                PortsComboBox.IsEnabled = true;
                BaudRateValuesComboBox.IsEnabled = true;
                DataBitsValuesComboBox.IsEnabled = true;
                ParityValuesComboBox.IsEnabled = true;
                StopBitsValuesComboBox.IsEnabled = true;
                ConnectToComPortButton.IsEnabled = true;
                DisconnectToComPortButton.IsEnabled = false;
                WriteTimeoutTextBox.IsEnabled = true;
                ReadTimeoutTextBox.IsEnabled = true;
            });
        }

        private void setDefaultSettings()
        {
            Dispatcher.Invoke(() =>
            {
                PortsComboBox.SelectedIndex = 0;
                BaudRateValuesComboBox.SelectedItem = "115200";
                DataBitsValuesComboBox.SelectedItem = "8";
                ParityValuesComboBox.SelectedItem = "none";
                StopBitsValuesComboBox.SelectedItem = "1";
            });
        }

        private void DisableSettings()
        {
            Dispatcher.Invoke(() =>
            {
                PortsComboBox.IsEnabled = false;
                BaudRateValuesComboBox.IsEnabled = false;
                DataBitsValuesComboBox.IsEnabled = false;
                ParityValuesComboBox.IsEnabled = false;
                StopBitsValuesComboBox.IsEnabled = false;
                ConnectToComPortButton.IsEnabled = false;
                DisconnectToComPortButton.IsEnabled = true;
                WriteTimeoutTextBox.IsEnabled = false;
                ReadTimeoutTextBox.IsEnabled = false;
            });
        }

        private void setEmptySettings()
        {
            Dispatcher.Invoke(() =>
            {
                BaudRateValuesComboBox.SelectedIndex = -1;
                DataBitsValuesComboBox.SelectedIndex = -1;
                ParityValuesComboBox.SelectedIndex = -1;
                StopBitsValuesComboBox.SelectedIndex = -1;
                ConnectToComPortButton.IsEnabled = false;
                DisconnectToComPortButton.IsEnabled = false;
                WriteTimeoutTextBox.IsEnabled = false;
                ReadTimeoutTextBox.IsEnabled = false;

            });
        }

        private void InitStopBitsValuesComboBox()
        {
            foreach (string stopBitsValue in stopBitsValues)
                StopBitsValuesComboBox.Items.Add(stopBitsValue);

            StopBitsValuesComboBox.SelectedItem = "1";
        }

        private void InitParityValuesComboBox()
        {
            foreach (string parityValue in parityValues)
                ParityValuesComboBox.Items.Add(parityValue);

            ParityValuesComboBox.SelectedItem = "none";
        }

        private void InitDataBitsValuesComboBox()
        {
            foreach (string dataBitsValue in dataBitsValues)
                DataBitsValuesComboBox.Items.Add(dataBitsValue);

            DataBitsValuesComboBox.SelectedItem = "8";
        }

        private void InitBaudRateValuesComboBox()
        { 
             foreach (string baudRateValue in baudRateValues)
                BaudRateValuesComboBox.Items.Add(baudRateValue);

            BaudRateValuesComboBox.SelectedItem = "115200";
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            RefreshComPortsList.Abort();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            allowRefreshComPorts.Reset();

            DisableSettings();

            ConnectToComPort = new Thread(ConnectToComPortAction);
            ConnectToComPort.Start();
        }

        private void ConnectToComPortAction()
        {
            string pn = "";
            int br = 0, db = 0, rt = 0, wt = 0;
            Parity p = Parity.None;
            StopBits sb = StopBits.One;

            Dispatcher.Invoke(()=> 
            {
                pn = PortsComboBox.SelectedItem.ToString();
                br = Convert.ToInt32(BaudRateValuesComboBox.SelectedItem);
                db = Convert.ToInt32(DataBitsValuesComboBox.SelectedItem);
                p = (Parity)Convert.ToInt32(ParityValuesComboBox.SelectedIndex);
                sb = (StopBits)Convert.ToInt32(StopBitsValuesComboBox.SelectedIndex + 1);
                rt = Convert.ToInt32(ReadTimeoutTextBox.Text);
                wt = Convert.ToInt32(WriteTimeoutTextBox.Text);
            });

           
            try
            {
                serialPort.PortName = pn;
                serialPort.BaudRate = br;
                serialPort.DataBits = db;
                serialPort.Parity = p;
                Dispatcher.Invoke(() => l.Content = sb);
                serialPort.StopBits = sb;
                serialPort.ReadTimeout = rt;
                serialPort.WriteTimeout = wt;

                serialPort.Open();
            }
            catch (NullReferenceException)
            {

            }
            //Dispatcher.Invoke(() => l.Content = serialPort.Parity);
        }

        private void DisconnectToComPortButton_Click(object sender, RoutedEventArgs e)
        {
            CloseComPort();
        }

        private void CloseComPort()
        {
            EnableSettings();

            try
            {
                serialPort.Close();
            }
            catch (NullReferenceException)
            {

            }

            allowRefreshComPorts.Set();
        }
    }
}
