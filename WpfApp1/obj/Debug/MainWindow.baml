   M S B A M L   `   `   `  ����B  >WpfApp1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:WpfApp1WpfApp1  5   	       WpfApp1.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   'localclr-namespace:WpfApp1     5   	   -   5          � Title$  Port monitor v0.1��5   	   $��519.6��6#   $��853.6��62   .��5	      � $��
0,0,-0.4,3q�6   .+�5
   
   �� $��Display��6   $
��Left=�6$   $	��178��6?   $��
672,10,0,0q�6L   $	��Top=�6`   $	��156��6x   $��14(�6�   �� 5   
   -   5         ��Name$ outputField��5   6   $��14(�6   $��0 3 0 0q�6$   $
��Left=�6K   $	��404��6f   $��
226,63,0,0q�6s   $	��Top=�6�   $	��437��6�   . �5      /� 5      �� 5   
   $��Actions��6   $
��Left=�6$   $	��109��6?   $��
10,358,0,0q�6L   $	��Top=�6`   $	��194��6x   $��14(�6�   �� 5   
   $��Main settings��6   $
��Left=�6*   $	��190��6E   $��	10,10,0,0q�6R   $	��Top=�6e   $	��194��6}   $��14(�6�   �� 5   
   -   5         ��Name$ PortsComboBox��5      $
��Left=�6*   $��
112,45,0,0q�6E   $	��Top=�6Y   $��78��6q   �� 5   
   $��	Com port:��6   $
��Left=�6$   $��	20,43,0,0q�6?   $	��Top=�6R   $��14(�6j   �� 5   
   $��
Baud rate:��6   $
��Left=�6%   $��	20,72,0,0q�6@   $	��Top=�6S   $��14(�6k   �� 5   
   -   5        $ BaudRateValuesComboBox��5      $
��Left=�63   $��
112,75,0,0q�6N   $	��Top=�6b   $��78��6z   �� 5   
   $��
Data bits:��6   $
��Left=�6%   $��
20,101,0,0q�6@   $	��Top=�6T   $��14(�6l   �� 5   
   -   5        $ DataBitsValuesComboBox��5      $
��Left=�63   $��112,106,0,0q�6N   $	��Top=�6c   $��78��6{   �� 5   
   $��Parity:��6   $
��Left=�6"   $��
20,130,0,0q�6=   $	��Top=�6Q   $��14(�6i   �� 5   
   -   5        $ ParityValuesComboBox��5      $
��Left=�61   $��112,136,0,0q�6L   $	��Top=�6a   $��78��6y   �� 5   
   $��
Stop bits:��6   $
��Left=�6%   $��
20,159,0,0q�6@   $	��Top=�6T   $��14(�6l   �� 5   
   -   5        $ StopBitsValuesComboBox��5      $
��Left=�63   $��112,165,0,0q�6N   $	��Top=�6c   $��78��6{   �� 5   
   -   5         ��Name$ ConnectToComPortButton��5      $
��Open��61   $
��Left=�6@   $��
20,393,0,0q�6[   $	��Top=�6o   $��85��6�   $��14(�6�   $��23��6�   �� 5   
   $��Additional settings��6   $
��Left=�60   $	��134��6K   $��
10,209,0,0q�6X   $	��Top=�6l   $	��194��6�   $��14(�6�   �� 5   
   $��Write tiomeout:��6   $
��Left=�6*   $��
20,268,0,0q�6E   $	��Top=�6Y   $��14(�6q   �� 5   
   $��Read tiomeout:��6   $
��Left=�6)   $��
20,234,0,0q�6D   $	��Top=�6X   $��14(�6p   �� 5   
   -	   5         ��Name$ ReadTimeoutTextBox��5      $
��Left=�6.   $��23��6I   $��129,237,0,0q�6U    �� TextWrapping$
 Wrap=�6j   $
��1000��6~   $	��Top=�6�   $��61��6�   $��14(�6�   �� 5   
   -
   5        $ WriteTimeoutTextBox��5      $
��Left=�6/   $��23��6J   $��129,272,0,0q�6V   $
 Wrap=�6k   $
��1000��6   $	��Top=�6�   $��61��6�   $��14(�6�   �� 5    
   -   5        $ DisconnectToComPortButton��5       $��Close��64   $
��Left=�6D   $	��Top=�6_   $��85��6w   $��112,393,0,0q�6�   $��14(�6�   $��23��6�   6�   �� 5!   
   -   5        $ SendDataButton��5!      $
��Send��6)   $
��Left=�68   $��
598,22,0,0q�6S   $	��Top=�6g   $��65��6   $��23��6�   $��14(�6�   �� 5"   
   -   5        $��Clear output��5"      $
��Left=�6(   $��
20,427,0,0q�6C   $	��Top=�6W   $	��177��6o   $��23��6{   $��14(�6�   6�   �� 5#   
   -   5        $ SendDataTextBox��5#   B   {�.  $
��Left=�6[   $��23��6v   $��
226,22,0,0q�6�   $
 Wrap=�6�   $	��Top=�6�   $	��367��6�   $��14(�6�   �� 5$   
   $��Display mode��6   $
��Left=�6)   $��91��6D   $	��Top=�6P   $	��156��6h   $��672,195,0,0q�6t   $��14(�6�   �� 5%   
   -   5         ��Name$ CommandTimeCheckBox��5%   %    d� 	IsChecked$ FalseY�6   $��Command time��6B   $
��Left=�6Y   $	��Top=�6t   $��
691,72,0,0q�6�   $	��121��6�   $��14(�6�   	� 5&   
    	� 	GroupName$ DisplayMode��6   $
 TrueY�6.   $��Standart��6?   $
��Left=�6R   $��691,232,0,0q�6m   $	��Top=�6�   $��14(�6�   	� 5'   
   $ DisplayMode��6   $��Correspondence��6.   $
��Left=�6G   $��691,258,0,0q�6b   $	��Top=�6w   $��14(�6�   �� 5(   
   -   5        $ SystemLogsCheckBox��5(      $
 TrueY�6/   $��System logs��6@   $
��Left=�6V   $��
691,44,0,0q�6q   $	��Top=�6�   $��14(�6�   �� 5)   
   -   5        $ EndOfLineComboBox��5)      $
��Left=�6.   $��112,308,0,0q�6I   $	��Top=�6^   $��78��6v   $��23��6�   �� 5*   
   $��End of line:��6   $
��Left=�6'   $��
20,302,0,0q�6B   $	��Top=�6V   $��14(�6n   �� 5+   
   -   5        $! DisplayReceiverDataCheckBox��5+      $
 TrueY�68   $��Receiver data��6I   $
��Left=�6a   $��691,130,0,0q�6|   $	��Top=�6�   $��14(�6�   $	��108��6�   �� 5,   
   -   5        $" DisplayTransiverDataCheckBox��5,      $
 TrueY�69   $��Transiver data��6J   $
��Left=�6c   $��691,101,0,0q�6~   $	��Top=�6�   $��14(�6�   $	��121��6�   �� 5-   
   -   5        $ DisplaySenderCheckBox��5-   %   $ FalseY�6   $��Sender��6D   $
��Left=�6U   $��691,159,0,0q�6p   $	��Top=�6�   $��14(�6�   �� 5.   
   -   5        $&  ClearInputFieldAfterSendCheckBox��5.      $��Clear after send��6=   $
��Left=�6X   $��691,302,0,0q�6s   $	��Top=�6�   $��14(�6�   �� 5/   
   -   5        $ AutoConnectToPortCheckBox��5/      $
 TrueY�66   $��Auto connect��6G   $
��Left=�6^   $��691,325,0,0q�6y   $	��Top=�6�   $��14(�6�   �� 50   
   -   5        $ ColorOutputCheckBox��50      $
 TrueY�60   $��Color RX output��6A   $
��Left=�6[   $��691,348,0,0q�6v   $	��Top=�6�   $��14(�6�   51      52      