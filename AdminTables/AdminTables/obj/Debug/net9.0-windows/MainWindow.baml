   M S B A M L   `   `   `  ����F  BAdminTables, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AdminTablesAdminTables  5   	       AdminTables.MainWindow   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:AdminTables     5   	     � Title$  
MainWindow��5   	   $
��1080��6   $
��1920��6*    � WindowState$ 	Maximized=�67    � 
ResizeMode$ CanResizeWithGrip=�6O   ���#1E1E1E.��5	      �  � RowDefinitions 5
   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6    � ColumnDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $��* �6   .+�5   
   �� $��1��6   $��1��6!   $��Авторизация��61   $��24(�6D   $
��Bold"�6R   $��Center=�6d   $��Center=�6�   $��0,0,0,20q�6�   
�������6�   �� 5   
   $��2��6   $��1��6"   $��Center=�62   $��Center=�6O   $	��300��6j   .+�5      �� $��Email:��6   $��16(�6&   $��0,0,0,5q�64   
�������6E   �� 5      -   5         ��Name$ EmailTextBox��5      $	��300��6,   $��30��68   $��0,0,0,15q�6D   $��20(�6V   $��admin@example.com��6d   �� 5      $��	Password:��6   $��16(�6)   $��0,0,0,5q�67   
�������6H   G� 5       -   5         G�Name$ PasswordBox��5       $	��300��6/   $��30��6;   $��0,0,0,15q�6G   $��20(�6Y    G� Password$	 123��6g   �� 5#      -   5         ��Name$ LoginButton��5#      $��Login��6*   $	��300��6:   $��40��6F   ���#007ACC
�������$��16(�6z   6�   5$      5%      5&      