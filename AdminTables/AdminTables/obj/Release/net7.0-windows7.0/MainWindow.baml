   M S B A M L   `   `   `  ���� F  BAdminTables, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:AdminTablesAdminTables      AdminTables.MainWindow   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     +localclr-namespace:AdminTables       � Title$  
MainWindow��$
��1080��$
��1920�� � WindowState$ 	Maximized=� � 
ResizeMode$ CanResizeWithGrip=����#1E1E1E.���  � RowDefinitions �� $��* ��� $
��Auto ��� $
��Auto ��� $��* � � ColumnDefinitions �� $��* ��� $
��Auto ��� $��* �.+��� $��1��$��1��$��Авторизация��$��24(�$
��Bold"�$��Center=�$��Center=�$��0,0,0,20q�
��������� $��2��$��1��$��Center=�$��Center=�$	��300��.+��� $��Email:��$��16(�$��0,0,0,5q�
��������� -    ��Name$ EmailTextBox��$	��300��$��30��$��0,0,0,15q�$��20(�$��admin@example.com���� $��	Password:��$��16(�$��0,0,0,5q�
�������G� -    G�Name$ PasswordBox��$	��300��$��30��$��0,0,0,15q�$��20(� G� Password$	 123���� -    ��Name$ LoginButton��$��Login��$	��300��$��40�����#007ACC
�������$��16(�