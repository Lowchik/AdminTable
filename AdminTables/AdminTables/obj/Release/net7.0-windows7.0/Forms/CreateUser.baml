   M S B A M L   `   `   `  ���� F  BAdminTables, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null5clr-namespace:AdminTables.FormsAdminTables.Forms  "    AdminTables.Forms.CreateUser   W SPresentationCore, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=7.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=7.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     1localclr-namespace:AdminTables.Forms       � Title$  
CreateUser��$	��450��$	��800�����#1E1E1E.���  � RowDefinitions �� $
��Auto ��� $��* �.+��� $/��)Создание пользователя��$��24(�$
��Bold"�$��Center=�$��0,0,0,20q�
�������� $��1��$��Center=�$	��Top=� � ColumnDefinitions �� $
��Auto ��� $	��300 � �� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto ��� $
��Auto �.+��� $��Имя:��$��Center=�$
��0,10q�
��������� -   $��FirstNameTextBox��$��1��$	��200��$
��0,10q��� $��Фамилия:��$��1��$��Center=�$
��0,10q�
��������� -   $��LastNameTextBox��$��1��$��1��$	��200��$
��0,10q��� $��Почта:��$��2��$��Center=�$
��0,10q�
��������� -   $��EmailTextBox��$��1��$��2��$	��200��$
��0,10q��� $��Пароль:��$��3��$��Center=�$
��0,10q�
�������G� -   $��PasswordBox��$��1��$��3��$	��200��$
��0,10q��� $��Телефон:��$��4��$��Center=�$
��0,10q�
��������� -   $��PhoneTextBox��$��1��$��4��$	��200��$
��0,10q��� $��	Роль:��$��5��$��Center=�$
��0,10q�
��������� -   $��RoleComboBox��$��1��$��5��$	��200��$
��0,10q��� -    ��Name$ PositionLabel��$��Должность:��$��6��$��Center=�$
��0,10q�
�������$y�	Collapsed=��� -    ��Name$ PositionTextBox��$��1��$��6��$	��200��$
��0,10q�$y�	Collapsed=��� -	   $ VehicleTypeLabel��$9��3Тип транспортного средства:��$��7��$��Center=�$
��0,10q�
�������$y�	Collapsed=��� -
   $ VehicleTypeTextBox��$��1��$��7��$	��200��$
��0,10q�$y�	Collapsed=��� $��8��$��2�� �� Orientation$ 
Horizontal=�$��Center=�$��0,20,0,0q�.+��� -   $
��Save��$	��100��$
��10,0q��� -   $��Cancel��$	��100��$
��10,0q�