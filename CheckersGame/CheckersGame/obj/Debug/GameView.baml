   M S B A M L   `   `   `  ����G  CCheckersGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CheckersGameCheckersGame  5   	   +clr-namespace:CheckersGameCheckersGame  5   	       CheckersGame.GameView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:CheckersGame     5   	   *ctxclr-namespace:CheckersGame     5   	   -   5           WindowBackgroundBrush#���   5
   	     � Title$  GameView��5   	   $	��450��6   $	��800��6'    � DataContext 5        CheckersGame.FieldVM  5   
    � 	Resources 5       � Offset � Color  DataTemplate_Level2
 � Tag  DataTemplate_Level1 �� Orientation��%  &	        &	 �     &	 i    �� 5   
   .��5      � $ 0Q�6   $ 
DarkSalmon��6&   � 5      $	 0.5Q�6   $ 	Chocolate��6(   � 5      $ 1Q�6   $ Khaki��6&   5      �� 5   
   .R�5      �� -   5        $��50��5      $��50��6     6@   �� .��5      �� $��45��6:   $��45��6E   ��6   �� *FieldImagePath+6E   5      5      �� 5   
   .R�5      �� #���  66   ��6   �� ��5       �� 5!      .R�5"      �� $ 
Horizontal=�6%   5#      5%      5&      5(      .��5)      �� $ 
Horizontal=�6   .+�5*   
   �� $��10q�6   .+�5+      �� -   5        $��lblPlayer1Name��5+      �� 5,      $"��Numar de piese ramase in joc��6   �� 5-      -   5        $��tbPieseRamase��5-      $��Center=�6U   ��6)   �� *WhitePiecesNumberInGame+6U   �� 5.      -   5        $��
White_Turn��5.      $��20��6&   $��Images\piesa_alba.jpg��61   $��10,50,10,20q�6P   $y�Hidden=�6e   �� 5/      -   5        $��lblWhite_Turn��5/      $��Alb la mutare��6*   $��Center=�6B   $��
10,0,10,30q�6_   $y�Hidden=�6s   �� 50      $ 
Horizontal=�6   .+�51      �� $ ��Sarituri multiple permise:��6   �� 52      -   5        $��lbl1SarituriMultiple��52      ��64   �� *MultipleJumps+53      54      � 55   
   $	��0,5q�6   .+�56      �� #���  6:   ��6   �� *Board+6:   57      �� 58   
   $��10q�6   .+�59      �� -   5        $��lblPlayer2Name��59      �� 5:      -	   5        $��Piese_Rosii_Ramase��5:      $"��Numar de piese ramase in joc��6.   6W   �� 5;      -
   5        $��tbPieseRosiiRamase��5;      $��Center=�6X   ��6.   �� *RedPiecesNumberInGame+6X   �� 5<      -   5        $��Red_Turn��5<      $��20��6$   $��Images\piesa_rosie.jpg��6/   $��10,50, 10,20q�6O   �� 5=      -   5        $��lblRed_Turn��5=      $��Rosu la mutare��6(   $��Center=�6A   $��
10,0,10,30q�6^   �� 5>      $ 
Horizontal=�6   .+�5?      �� $ ��Sarituri multiple permise:��6   �� 5@      -   5        $��lbl2SarituriMultiple��5@      ��64   �� *MultipleJumps+5A      5B      5C      5D      