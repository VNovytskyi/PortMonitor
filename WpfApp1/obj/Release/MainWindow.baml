   M S B A M L   `   `   `  ���� B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1      WpfApp1.MainWindow   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:WpfApp1     -     � Title$  Port monitor v0.1��$��519.6��$��853.6��.��� $��
0,0,-0.4,3q�.+��� $��Display��$
��Left=�$	��178��$��
672,10,0,0q�$	��Top=�$	��156��$��14(��� -    ��Name$ outputField��$��14(�$��0 3 0 0q�$
��Left=�$	��404��$��
226,63,0,0q�$	��Top=�$	��437��. �/� �� $��Actions��$
��Left=�$	��109��$��
10,358,0,0q�$	��Top=�$	��194��$��14(��� $��Main settings��$
��Left=�$	��190��$��	10,10,0,0q�$	��Top=�$	��194��$��14(��� -    ��Name$ PortsComboBox��$
��Left=�$��
112,45,0,0q�$	��Top=�$��78���� $��	Com port:��$
��Left=�$��	20,43,0,0q�$	��Top=�$��14(��� $��
Baud rate:��$
��Left=�$��	20,72,0,0q�$	��Top=�$��14(��� -   $ BaudRateValuesComboBox��$
��Left=�$��
112,75,0,0q�$	��Top=�$��78���� $��
Data bits:��$
��Left=�$��
20,101,0,0q�$	��Top=�$��14(��� -   $ DataBitsValuesComboBox��$
��Left=�$��112,106,0,0q�$	��Top=�$��78���� $��Parity:��$
��Left=�$��
20,130,0,0q�$	��Top=�$��14(��� -   $ ParityValuesComboBox��$
��Left=�$��112,136,0,0q�$	��Top=�$��78���� $��
Stop bits:��$
��Left=�$��
20,159,0,0q�$	��Top=�$��14(��� -   $ StopBitsValuesComboBox��$
��Left=�$��112,165,0,0q�$	��Top=�$��78���� -    ��Name$ ConnectToComPortButton��$
��Open��$
��Left=�$��
20,393,0,0q�$	��Top=�$��85��$��14(�$��23���� $��Additional settings��$
��Left=�$	��134��$��
10,209,0,0q�$	��Top=�$	��194��$��14(��� $��Write tiomeout:��$
��Left=�$��
20,268,0,0q�$	��Top=�$��14(��� $��Read tiomeout:��$
��Left=�$��
20,234,0,0q�$	��Top=�$��14(��� -	    ��Name$ ReadTimeoutTextBox��$
��Left=�$��23��$��129,237,0,0q� �� TextWrapping$
 Wrap=�$
��1000��$	��Top=�$��61��$��14(��� -
   $ WriteTimeoutTextBox��$
��Left=�$��23��$��129,272,0,0q�$
 Wrap=�$
��1000��$	��Top=�$��61��$��14(��� -   $ DisconnectToComPortButton��$��Close��$
��Left=�$	��Top=�$��85��$��112,393,0,0q�$��14(�$��23���� -   $ SendDataButton��$
��Send��$
��Left=�$��
598,22,0,0q�$	��Top=�$��65��$��23��$��14(��� -   $��Clear output��$
��Left=�$��
20,427,0,0q�$	��Top=�$	��177��$��23��$��14(��� -   $ SendDataTextBox��{�.  $
��Left=�$��23��$��
226,22,0,0q�$
 Wrap=�$	��Top=�$	��367��$��14(��� $��Display mode��$
��Left=�$��91��$	��Top=�$	��156��$��672,195,0,0q�$��14(��� -    ��Name$ CommandTimeCheckBox�� d� 	IsChecked$ FalseY�$��Command time��$
��Left=�$	��Top=�$��
691,72,0,0q�$	��121��$��14(�	�  	� 	GroupName$ DisplayMode��$
 TrueY�$��Standart��$
��Left=�$��691,232,0,0q�$	��Top=�$��14(�	� $ DisplayMode��$��Correspondence��$
��Left=�$��691,258,0,0q�$	��Top=�$��14(��� -   $ SystemLogsCheckBox��$
 TrueY�$��System logs��$
��Left=�$��
691,44,0,0q�$	��Top=�$��14(��� -   $ EndOfLineComboBox��$
��Left=�$��112,308,0,0q�$	��Top=�$��78��$��23���� $��End of line:��$
��Left=�$��
20,302,0,0q�$	��Top=�$��14(��� -   $! DisplayReceiverDataCheckBox��$
 TrueY�$��Receiver data��$
��Left=�$��691,130,0,0q�$	��Top=�$��14(�$	��108���� -   $" DisplayTransiverDataCheckBox��$
 TrueY�$��Transiver data��$
��Left=�$��691,101,0,0q�$	��Top=�$��14(�$	��121���� -   $ DisplaySenderCheckBox��$ FalseY�$��Sender��$
��Left=�$��691,159,0,0q�$	��Top=�$��14(��� -   $&  ClearInputFieldAfterSendCheckBox��$��Clear after send��$
��Left=�$��691,302,0,0q�$	��Top=�$��14(��� -   $ AutoConnectToPortCheckBox��$
 TrueY�$��Auto connect��$
��Left=�$��691,325,0,0q�$	��Top=�$��14(��� -   $ ColorOutputCheckBox��$
 TrueY�$��Color RX output��$
��Left=�$��691,348,0,0q�$	��Top=�$��14(�