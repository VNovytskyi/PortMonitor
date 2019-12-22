using System;
using System.IO.Ports;
using System.Threading;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

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
        private static Thread TransiverData = null;
        private static Thread ReceiverData = null;
        
        private static SerialPort serialPort = null;

        readonly private static string[] baudRateValues = { "9600", "115200"};
        readonly private static string[] dataBitsValues = { "5", "6", "7", "8", "9"};
        readonly private static string[] parityValues = { "None", "Odd", "Even", "Mark", "Space"};
        readonly private static string[] stopBitsValues = { "1", "2", "1.5"};
        readonly private static string[] EndOfLineValues = {"None", "NL", "CR", "NL&CR"};
        
        private static int oldComPortsAmount = -1;

        private static bool firstDisplayPortList = false;

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

        private void Write(string s, Color color, TextAlignment ta)
        {
            var text = new Run(s) { Foreground = new SolidColorBrush(color) };
            var p = new Paragraph(text);
            p.LineHeight = 2;
            p.TextAlignment = ta;
            outputField.Document.Blocks.Add(p);
            outputField.ScrollToEnd();
        }

        private void WriteLog(string str)
        {
            if (CommandTimeCheckBox.IsChecked == true)
                str = "[ " + DateTime.UtcNow.ToString("HH:mm:ss") + " ] " + str;

            Write(str, Colors.Black, TextAlignment.Left);
        }

        private void WriteSystemLog(string str)
        {
            Dispatcher.Invoke(() =>
            {
                string temp = "";
                if (CommandTimeCheckBox.IsChecked == true)
                    temp += "[ " + DateTime.UtcNow.ToString("HH:mm:ss") + " ] ";

                temp += str;

                if (SystemLogsCheckBox.IsChecked == true)
                    Write(temp, Colors.Black, TextAlignment.Left);
            });
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
                    WriteSystemLog("Connection is broken");
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

                    if(firstDisplayPortList)
                        WriteSystemLog("Port list updated: " + string.Join(" ", portNames));
                    firstDisplayPortList = true;

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

                    if (currentComPortsAmount == 1 && getAutoConnectStatus())
                    {
                        InitConnection();
                    }
                }

                oldComPortsAmount = currentComPortsAmount;

                Thread.Sleep(100);
            }
        }

        private bool getAutoConnectStatus()
        {
            bool a = false;
            Dispatcher.Invoke(() => a = AutoConnectToPortCheckBox.IsChecked == true);
            return a;
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
            InitConnection();
        }

        private void InitConnection()
        {
            Dispatcher.Invoke(()=> WriteSystemLog("Try connect to " + PortsComboBox.SelectedItem + " (" +
                              BaudRateValuesComboBox.SelectedItem + ", " +
                              DataBitsValuesComboBox.SelectedItem + ", " +
                              ParityValuesComboBox.SelectedItem + ", " +
                              StopBitsValuesComboBox.SelectedItem + ")"));
            

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
                serialPort.StopBits = sb;
                serialPort.ReadTimeout = rt;
                serialPort.WriteTimeout = wt;

                serialPort.Open();
            }
            catch (Exception e)
            {
                WriteSystemLog(e.Message);
                StopConnection();
                return;
            }

            allowRefreshCurrentComPortState.Set();

            Dispatcher.Invoke(() =>
            {
                DisconnectToComPortButton.IsEnabled = true;
                SendDataButton.IsEnabled = true;
                SendDataTextBox.IsEnabled = true;
            });
            
            WriteSystemLog("Connection successful");

            ReceiverData = new Thread(ReceiverDataFunc);
            ReceiverData.IsBackground = true;
            ReceiverData.Start();
        }

        private void DisconnectToComPortButton_Click(object sender, RoutedEventArgs e)
        {
            StopConnection();
            WriteSystemLog("Connection is closed");
        }

        private void StopConnection()
        {
            allowRefreshCurrentComPortState.Reset();
            CloseComPort();
            EnableSettings();
            allowRefreshComPorts.Set();

            Dispatcher.Invoke(()=>
            {
                SendDataTextBox.IsEnabled = false;
                SendDataButton.IsEnabled = false;
            });

        }

        private void CloseComPort()
        {
            try
            {
                serialPort.Close();
            }
            catch (Exception e)
            {
                WriteSystemLog(e.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TransiverData = new Thread(TransiverDataFunc);
            TransiverData.Start();
        }

        private void TransiverDataFunc()
        {
            Dispatcher.Invoke(() =>
            {
                string strToSend = SendDataTextBox.Text;
                string strToDisplay = SendDataTextBox.Text;

                switch (EndOfLineComboBox.SelectedItem)
                {
                    case "None":
                        break;

                    case "NL":
                        strToSend += "\n";
                        break;

                    case "CR":
                        strToSend += "\r";
                        break;

                    case "NL&CR":
                        strToSend += "\r\n";
                        break;
                }

                try
                {
                    serialPort.Write(strToSend);
                }
                catch(Exception e)
                {
                    WriteSystemLog("TransiverDataFunc" + e.Message);
                    return;
                }


                if (DisplaySenderCheckBox.IsChecked == true)
                    strToDisplay = "[ PC ] " + strToDisplay;

                if (DisplayTransiverDataCheckBox.IsChecked == true)
                    WriteLog(strToDisplay);

                if (ClearInputFieldAfterSendCheckBox.IsChecked == true)
                    SendDataTextBox.Clear();
            });
        }

        private void ReceiverDataFunc()
        {
            string str = String.Empty;

            while (true)
            {
                str = String.Empty;

                try
                {
                    str += serialPort.ReadExisting();
                }
                catch (Exception e)
                {
                    //WriteSystemLog("ReceiverDataFunc" + e.Message);
                    return;
                }

                Dispatcher.Invoke(() =>
                {
                    if (!String.IsNullOrEmpty(str) && DisplayReceiverDataCheckBox.IsChecked == true)
                    {
                        if (DisplaySenderCheckBox.IsChecked == true)
                            str = "[ Port ] " + str;
                        
                        str = str.Trim(new Char[] { '\n', '\r' });

                        WriteLog(str);
                    }     
                });

                Thread.Sleep(100);
            }   
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            outputField.Document.Blocks.Clear();
        }
    }
}
