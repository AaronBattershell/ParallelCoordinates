   M S B A M L   `   `   `  ����  ParallelCordinates.    (ParallelCordinates.ParallelCoordinates2D   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          �Name$  window��5   	    � Title$ Parallel Coordinates 2D��5   	    � WindowState$ 	Maximized=�5   	    � Icon$ icon.ico��5   	   .��5	      �� -   5        $��canvasContainer��5	      $��0,0,0,0q�5
   	   $��0,0,0,0q�5      ��. $
��Auto=�5   	   $
��Auto=�5   	   .��5   
   �� -   5        $��canvas��5      
������ .+�5      �� -   5        $��defaultSettingsBtn��5      $�� ��6/    �� Left$ 10��6:   
 �� Top$ 10��6K   $��30��6[   $��30��6f   ��5      �� 5       �� ImageSource$ Gear.png��6!   5      �� 5      -   5        $��defaultSettingsGrid��5      $ 10��60   $ 10��6A   
�������$��1q�6h   ._�5      � $	��232��6   $	��262��6$   
�������.+�5      �� $��Visualization Constraints��6   $
��Left=�6@   $��	21,21,0,0q�6[   $	��Top=�6n   $	��180��6�   $��14(�6�   �� 5      -   5        $��MinColumnWidthTxtBx��5      $
��Left=�69   $��23��6T   $��
219,54,0,0q�6`    �� TextWrapping$
 Wrap=�6t   $	��Top=�6�   $��30��6�   6�   �� 5      $��Minimum Column Width: ��6   $
��Left=�6=   $��	43,50,0,0q�6X   $	��Top=�6k   �� 5      $��px��6   $
��Left=�6)   $��420,102,0,0q�6D   $	��Top=�6Y   $��22��6q   �� 5      -   5        $��BeginNumericAproxTxtBx��5      $
��Left=�6<   $��23��6W   $��
219,81,0,0q�6c   $
 Wrap=�6w   $	��Top=�6�   $��30��6�   6�   �� 5      $"��Begin Numeric Approximation:��6   $
��Left=�6C   $��	43,77,0,0q�6^   $	��Top=�6q   �� 5      -   5         ��Name$ TotalLineThicknessTxtBx��5      $
��Left=�6?   $��23��6Z   $��219,108,0,0q�6f   $
 Wrap=�6{   $	��Top=�6�   $��30��6�   6�   �� 5      $ ��Cumulative Line Thickness:��6   $
��Left=�6A   $��
43,104,0,0q�6\   $	��Top=�6p   �� 5       $��Line Width Variance��6   $
��Left=�6:   $��
43,131,0,0q�6U   $	��Top=�6i   �� 5!      -	   5        	 ��Name$	 LineWidthVarianceSlider��5!      $
��Left=�6>   $��204,134,-3,0q�6Y   $	��Top=�6o   
 � Maximum$
 3Q�6�    �� TickPlacement$ BottomRight=�6�    � Minimum$ 1Q�6�   $��58��6�    �� IsSnapToTickEnabled .   �� IsMoveToPointEnabled . 6�   �� 5"      $��Hide Filtered Results��6   $
��Left=�6<   $��
43,158,0,0q�6W   $	��Top=�6k   �� 5#      -
   5        $��FilterTxtBx��5#   *   $�� ��6   $
��Left=�6=   $��227,165,0,0q�6X   $	��Top=�6m   �� 5$      -   5        $��Apply��5$      $
��Left=�6-   $��
77,200,0,0q�6H   $	��Top=�6\   $��75��6t   6   �� 5%      -   5        $��Close��5%      $
��Left=�6-   $��172,200,0,0q�6H   $	��Top=�6]   $��75��6u   6�   5&      5'      5(      5)      5*      