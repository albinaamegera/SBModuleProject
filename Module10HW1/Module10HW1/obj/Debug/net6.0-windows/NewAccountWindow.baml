   M S B A M L   `   `   `  ����F  BModule10HW1, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Module10HW1Module10HW1  5   	   "    Module10HW1.NewAccountWindow   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:Module10HW1     5   	     � Title$  New Bank Account��5   	   $	��100��5	   	   $	��250��5
   	    � SizeToContent$ WidthAndHeight=�5   	    � WindowStartupLocation$ CenterOwner=�5   	    � 
ResizeMode$ NoResize=�5   	   .��5      � $��5q�6    � 	Resources 5   
    �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� E    �� 5      " ��6   .�5      ��  � ��$ 2q�6+   �� 5       � ��$ Right=�68   �� 5       � ��$ 12(�6-   5      �� 5      " ��6   .�5      ��  � ��$ 2q�6+   �� 5       � ��$ 20��6+   �� 5       � ��$ 80��6*   �� 5       � ��$ 10(�6-   5      �� 5      " ��6   .�5      ��  � ��$	 0,5q�6+   �� 5       � ��$ 10(�6-   5      5       � RowDefinitions 5    
   �� 5!      $
��Auto �6   �� 5"      $
��Auto �6   �� 5#      $
��Auto �6   	 � ColumnDefinitions	 5%   
   �� 5&      $
��Auto �6   �� 5'      $
��Auto �6   .+�5)   
   �� $��0��6   .��6    Account Type60   �� 5*   
   $��1��6   .��6   Start Amounts6,   �� 5+   
   -   5        
 ��Name$
 AccTypeSwitcher��5+      $��2q�5,      $
��Left=�5-      $��20��5.      $	��160��5/      $��1��50      $��0��51      52      �� 53   
    �� Orientation$ Vertical=�6   $��1��54      $��1��55      $��2q�56      .+�57      �� -   5         ��Name$ startAmount��57      $��Enter Amount��58      �� 59      -   5        $
 rate��59      $��Enter Interested Rate��5:      $y�Hidden=�5;      5<      �� 5=   
   $ 
Horizontal=�6   $��2��5>      $��2��5?      $��Right=�5@      .+�5A      �� -   5         ��Name$ confirmButton��5A      $��Confirm��5B      5C      �� 5D      -   5        $ canselButton��5D      $��Cansel��5E      5F      5G      5H      5I      