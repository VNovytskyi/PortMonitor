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
        private static EventWaitHandle allowRefreshCurrentComPortState = new ManualResetEvent(initialState: false);

        private static Thread RefreshCurrentComPortState = null;
        private static Thread RefreshComPortsList = null;
        private static Thread ConnectToComPort = null;

        private static SerialPort serialPort = null;

        private static string[] baudRateValues = { "9600", "115200"};
        private static string[] dataBitsValues = { "5", "6", "7", "8", "9"};
        private static string[] parityValues = { "None", "Odd", "Even", "Mark", "Space"};
        private static string[] stopBitsValues = { "1", "2", "1.5"};
        private static string[] EndOfLineValues = {"None", "NL", "CR", "NL&CR"};
        private static int oldComPortsAmount = -1;

        public MainWindow()
        {
            InitializeComponent();

            InitBaudRateValuesComboBox();
            InitDataBitsValuesComboBox();
            InitParityValuesComboBox();
            InitStopBitsValuesComboBox();
            InitEndOfLineComboBox();

            SendDataButton.IsEnabled = false;

            serialPort = new SerialPort();

            RefreshCurrentComPortState = new Thread(RefreshCurrentComPortStateFunc);
            RefreshCurrentComPortState.Start();

            RefreshComPortsList = new Thread(RefreshComPortsListFunc);
            RefreshComPortsList.Start();
        }

        void Write(string s, Color color, TextAlignment ta)
        {
            var text = new Run(s) { Foreground = new SolidColorBrush(color) };
            var p = new Paragraph(text);
            p.LineHeight = 2;
            p.TextAlignment = ta;
            outputField.Document.Blocks.Add(p);
        }

        private void RefreshCurrentComPortStateFunc()
        {
            while(true)
            {
                allowRefreshCurrentComPortState.WaitOne();

                bool portState = true;
                Dispatcher.Invoke(() => portState = serialPort.IsOpen);

                if (!portState)
                {
                    StopConnection();

                    Dispatcher.Invoke(() =>
                    {
                        if (SystemLogsCheckBox.IsChecked == true)
                            Write("Connection is broken", Colors.Black, TextAlignment.Left);
                    });
                }
                Thread.Sleep(100);
            }
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
                EndOfLineComboBox.IsEnabled = true;
            });
        }

        private void setDefaultSettings()
        {
            Dispatcher.Invoke(() =>
            {
                PortsComboBox.SelectedIndex = 0;
                BaudRateValuesComboBox.SelectedItem = "115200";
                DataBitsValuesComboBox.SelectedItem = "8";
                ParityValuesComboBox.SelectedItem = "None";
                StopBitsValuesComboBox.SelectedItem = "1";
                EndOfLineComboBox.SelectedItem = "NL&CR";
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
                ConnectToComPortButton.IsEnabled = false;
                DisconnectToComPortButton.IsEnabled = false;
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
                EndOfLineComboBox.SelectedIndex = -1;
                EndOfLineComboBox.IsEnabled = false;
            });
        }

        private void InitEndOfLineComboBox()
        {
            foreach (string EndOfLineValue in EndOfLineValues)
                EndOfLineComboBox.Items.Add(EndOfLineValue);

            EndOfLineComboBox.SelectedItem = "NL&CR";
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

            ParityValuesComboBox.SelectedItem = "None";
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
            try
            {
                RefreshComPortsList.Abort();
                RefreshCurrentComPortState.Abort();
                CloseComPort();
            }
            catch
            {

            }       
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string temp = "";

            if (CommandTimeCheckBox.IsChecked == true)
                temp += "[ " + DateTime.UtcNow.ToString("HH:mm:ss") + " ] ";

            temp += "Try connect to " + PortsComboBox.SelectedItem + " (" +
                          BaudRateValuesComboBox.SelectedItem + ", " +
                          DataBitsValuesComboBox.SelectedItem + ", " +
                          ParityValuesComboBox.SelectedItem + ", " +
                          StopBitsValuesComboBox.SelectedItem + ")";

            if (SystemLogsCheckBox.IsChecked == true)
                Write(temp, Colors.Black, TextAlignment.Left);
            
            allowRefreshComPorts.Reset();
            DisableSettings();
            ConnectToComPort = new Thread(ConnectToComPortFunc);
            ConnectToComPort.Start();
        }

        private void ConnectToComPortFunc()
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
            catch (Exception e)
            {
                Dispatcher.Invoke(() =>
                {
                    if (SystemLogsCheckBox.IsChecked == true)
                        Write(e.Message, Colors.Black, TextAlignment.Left);
                });
                StopConnection();
                return;
            }

            allowRefreshCurrentComPortState.Set();

            Dispatcher.Invoke(() =>
            {
                DisconnectToComPortButton.IsEnabled = true;
                SendDataButton.IsEnabled = true;

                if (SystemLogsCheckBox.IsChecked == true)
                    Write("Connection successful", Colors.Black, TextAlignment.Left);
            });
        }

        private void DisconnectToComPortButton_Click(object sender, RoutedEventArgs e)
        {
            StopConnection();

            string temp = "";
            if (CommandTimeCheckBox.IsChecked == true)
                temp += "[ " + DateTime.UtcNow.ToString("HH:mm:ss") + " ] ";
            temp += "Connection is closed";

            if (SystemLogsCheckBox.IsChecked == true)
                Write(temp, Colors.Black, TextAlignment.Left);
        }

        private void StopConnection()
        {
            allowRefreshCurrentComPortState.Reset();
            CloseComPort();
            EnableSettings();
            allowRefreshComPorts.Set();

            Dispatcher.Invoke(()=> SendDataButton.IsEnabled = false);
        }

        private void CloseComPort()
        {
            try
            {
                serialPort.Close();
            }
            catch (NullReferenceException)
            {

            } 
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            outputField.Document.Blocks.Clear();
        }
    }
}
