   M S B A M L   `   `   `  ����G  CCheckersGame, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:CheckersGameCheckersGame  5   	   +clr-namespace:CheckersGameCheckersGame  5   	       CheckersGame.DameGameOver   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:CheckersGame     5   	   *ctxclr-namespace:CheckersGame     5   	     � Title$  DameGameOver��5	   	   $	��450��6   $	��800��6+   ��5
      �� 5   
    �� ImageSource$ Images\Background.png��6    � DataContext 5      $  CheckersGame.GameOverWinnersVM  5   
   .��5      �� .+�5   
   �� $��Winner:��6   $��20(�6"   $��Center=�60   $��
0,100,0,20q�6M   
�������6a   �� 5   
   $��Center=�6   $��Center=�60    �� Orientation$ 
Horizontal=�6M   .+�5      �� -   5        $��lblWinnerName��5      $��Name -��6)   $��20(�6:   
�������6H   �� 5      -   5        $��lblWinnerColor��5      $��Color��6*   $��20(�6:   
�������6H   5      �� 5   
   -   5        $��btnStatistics��5      $��
Statistics��6&   $��22(�6;   $	��150��6e   $��10q�6q   
�������6}   �� 5   
   -   5        $��lbStatistics��5      $y�Hidden=�6M   
���������6&   �� *EvidenceOfWins+6a   5      5      