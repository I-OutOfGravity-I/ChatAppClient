   M S B A M L   `   `   `  ����D  @ClientApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null%clr-namespace:ClientApp	ClientApp  5   	       ClientApp.Login   5   	   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   )localclr-namespace:ClientApp     5   	   
������   � Title$  Login��5	   	   $	��400��6   $	��700��6$    � 
ResizeMode$ NoResize=�60    � WindowStartupLocation$ CenterScreen=�6F    � WindowStyle$
 None=�6k    � AllowsTransparency$
 True��6~   .��5
      � -   5         �Name$ 	LoginGrid��5
      $y�Visible=�6   
������ .+�5   
   ��  �� CornerRadius$ 	0,0,20,20��6   ��5      �� 5       j� Stretch$
 Fill=�6    �� ImageSource$$ Resources/HHEK-Background.jpeg��6,   	 �� BitmapScalingMode$	 HighQuality=�6Y   5      � 5   
   -   5        $ TopBar��5      $	��Top=�6   $��20��67   
��� ���
 K� ZIndex$
 1��6W   5      � 5   
   $��355,0,0,300q�6   $	��250��6$   $��80��60   
������ .+�5      �� $��
0,0,-59,10q�6   $ 10,10,10,10��6)   ��5      �� 5      $ Uniform=�6!   $$ Resources/hertz_logo_web_d.png��63   5      5      � 5   
   $��Center=�6   $��Center=�6*   $	��250��6G   $	��230��6S   ��5      �� 5       �� Color$ Black��6"    �� Opacity$	 0.4Q�60   .+�5      ��  J� Effect 5      -  'System.Windows.Media.Effects.BlurEffect  5          Radius$	 230Q�6!      
KernelType$ Gaussian=�6.   5"      �� 5#      $��Login��6   $��Center=�6$   $	��Top=�6A   $��20q�6Y   $��40(�6e   
�������6�   �� 5$      -   5         ��Name$ Server��5$      
�������$��	127.0.0.1��6U   $��20��6f   $	��130��6r   $��Center=�6~   6�   �� 5%      -   5        $ Username��5%      
�������$��Benutzername��6Y   $��20��6m   $	��130��6y   $��Center=�6�   $��0,50,0,0q�6�   6�   �� 5&      -   5        $��Start Chatting��5&      $��90��6/   $��30��6:   $��Center=�6[   $��
0,155,0,45q�6x   �� 5'      -   5         ��Name$ fehler��5'   ,   $
��Wrap=�6   $�� ��6<   
���  ��$	��200��6U   $��35��6a    �� TextAlignment$ Center=�6m   $��12(�6�   $��Center=�6�   $��	0,190,0,0q�6�   � 5)      -   5        $ loading��5)      $y�Hidden=�6$   $��	0,190,0,0q�68   .+�5*      � .+�5+      F� $��39��6   $��39��6&   
���   �$��4��6A   ��5,      B� 5-      .��5.   "   E�  E� 
StartPoint$
 17,0&�6-   .��5/   &   ��  �� Size$ 17,17��61    �� RotationAngle$ 90Q�6>    �� 
IsLargeArc$
 True��6Q    �� SweepDirection$ 	Clockwise=�6c    �� Point$ 17,34&�6~   50   #   51      z�54      �� 55      -   5        rotateTransform�� �� Angle$ 0Q�55   G    �� CenterX$ 17Q�6Q    �� CenterY$ 17Q�6^    � Triggers 58      :� 59       :� RoutedEvent$ FrameworkElement.Loaded��6+   .[�5:   "   �� .��5;   &   �� .��5<   *   W�  �� 
TargetName$ rotateTransform��5=   !    �� TargetProperty$ Angle�5>   !   	  W� To  3605?   !   ! g� Duration$! 0:0:1D�6*   " g� RepeatBehavior$" Forever��6;   5@   '   5A   #   5B      5D      5E      � 5G      .+�5H      F� $��39��6   $��39��6&   
��� ���$��4��6B   ��5I      B� 5J      .��5K   "   E� $
 17,0&�6-   .��5L   &   �� $ 17,17��61   $ 90Q�6>   $
 True��6Q   $ Counterclockwise=�6c   $ 17,34&�6�   5M   #   5N      z�5Q      �� 5R      -	   5        rotateTransform1��$ 0Q�5R   H   $ 17Q�6R   $ 17Q�6_    5U      :� 5V      $ FrameworkElement.Loaded��6+   .[�5W   "   �� .��5X   &   �� .��5Y   *   W� $ rotateTransform1��5Z   !   $ Angle�5[   !     3605\   !   $! 0:0:1D�6*   $" Forever��6;   5]   '   5^   #   5_      5a      5b      5c      5e      5f      5h      