   M S B A M L   `   `   `  ����  ParallelCordinates#    ParallelCordinates.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     6F     � Title$  Parallel Coordinates��5   	   $	��356��6&   $	��531��63    � 
ResizeMode$ CanMinimize=�6?    � Icon$ icon.ico��5   	   .��5      � .+�5   
   �� ��5	      �� 5
       �� EndPoint$ 	0.504,1.5&�6&    �� 
StartPoint$ 
0.504,0.03&�6;   .��5      �  � Color$ 
GhostWhite��6#    � Offset$ 0Q�66   � 5      $ 
WhiteSmoke��6#   $ 3Q�66   5      5      �� 5   
   $
��Left=�6   $	��299��5      $��	10,10,0,0q�5      $	��Top=�5      $	��497��5      .@�5      �� $��Data Preparation��6   $	��130��60   .��5      � ���	#FFE5E5E5$��0,2,0,-2q�6.   .+�5      �� -   5        $��Select Data Source��5      $
��Left=�6:   $��	12,30,0,0q�6U   $	��Top=�6h   $	��116��6�   6�   �� 5      -   5         ��Name$ FileNameLbl��5      $�� ��62   $
��Left=�6=   $��
131,28,0,0q�6X   $	��Top=�6l   $	��195��6�   �� 5      -   5        $��DataInterpretationBtn��5      $��View Data Interpretation��6:   $
��Left=�6]   $��
331,30,0,0q�6x   {�.  $	��Top=�6�   $	��141��6�   6�   �� 5      -   5        $��	LaunchBtn��5      $��Launch Visualization��6.   $
��Left=�6M   $��
43,219,0,0q�6h   $	��Top=�6|   $	��399��6�   $��22��6�   {�.  6�   �� 5       $��Visualization Constraints��6   $
��Left=�6@   $��	11,73,0,0q�6[   $	��Top=�6n   $	��180��6�   $��14(�6�   �� 5!      -   5        $��MinColumnWidthTxtBx��5!      $
��Left=�69   $��23��6T   $��166,106,0,0q�6`    �� TextWrapping$
 Wrap=�6u   $	��200��6�   $	��Top=�6�   $��30��6�   6�   �� 5"      $��Min Column Width: ��6   $
��Left=�69   $��
40,102,0,0q�6T   $	��Top=�6h   �� 5#      $��px��6   $
��Left=�6)   $��192,102,0,0q�6D   $	��Top=�6Y   $��22��6q   �� 5$      -   5        $��MaxUniqueEntriesTxtBx��5$      $
��Left=�6;   $��23��6V   $��166,133,0,0q�6b   $
 Wrap=�6w   $��25��6�   $	��Top=�6�   $��30��6�   6�   �� 5%      $��Max Unique Entries:��6   $
��Left=�6:   $��
40,129,0,0q�6U   $	��Top=�6i   �� 5&      -   5        $��BeginNumericAproxTxtBx��5&      $
��Left=�6<   $��23��6W   $��166,160,0,0q�6c   $
 Wrap=�6x   $��10��6�   $	��Top=�6�   $��30��6�   6�   �� 5'      $��Begin Numeric Aprox:��6   $
��Left=�6;   $��
40,156,0,0q�6V   $	��Top=�6j   �� 5(      $��Cumulative Line Width:��6   $
��Left=�6=   $��238,102,0,0q�6X   $	��Top=�6m   �� 5)      -   5        	 ��Name$	 AverageLineWidthTxtBx��5)      $
��Left=�6=   $��23��6X   $��371,106,0,0q�6d   $
 Wrap=�6y   $��50��6�   $	��Top=�6�   $��30��6�   6�   �� 5*      $��px��6   $
��Left=�6)   $��397,102,0,0q�6D   $	��Top=�6Y   $��22��6q   �� 5+      $��Line Width Variance��6   $
��Left=�6:   $��238,129,0,0q�6U   $	��Top=�6j   �� 5,      -	   5        
 ��Name$
 LineWidthVarianceSlider��5,      $
��Left=�6>   $��361,134,0,0q�6Y   $	��Top=�6n    � Maximum$ 3Q�6�    �� TickPlacement$ BottomRight=�6�    � Minimum$ 1Q�6�   $��58��6�    �� IsSnapToTickEnabled .   �� IsMoveToPointEnabled .  � Value$	 1.2Q�6�   �� 5-      $��Hide Filtered Results��6   $
��Left=�6<   $��238,156,0,0q�6W   $	��Top=�6l   �� 5.      -
   5        $��FilterTxtBx��5.   *   $�� ��6   $
��Left=�6=   $��379,164,0,0q�6X   $	��Top=�6m   5/      50      �� 51      $
��Help��6   $	��130��6$   .��52      � ���	#FFE5E5E5.+�53      �� $
��Left=�6!   $	��261��54      $��4,3,0,0q�55      $	��Top=�56      $	��479��57      .@�58      �� $��
File Input��6"   $	��130��66   .��59      � ���	#FFE5E5E5.+�5:   "   �� $
��Left=�6,   $��7,2,0,0q�5;      $
��Wrap=�5<      $	��Top=�5=      $	��462��5>      $	��227��6+   �� 69   $!�Accepted Data Input Formats��6=   �� 6b   �� 6n   $�        .xlsx��6r   �� 6�   $� .txt��6�   �� 6�   �� 6�   �� 6�   �� 6�   $�Expected File Format��6�   �� 6�   �� 6�   $�        .xlsx��6�   �� 6  �� 6  $a�[                Beginning at cell A1, fill row 1 with attribute fields, one per cell. Every��6  �� 6x  �� 6�  $<�6                subsequent row is a single data entry.��6�  �� 6�  �� 6�  $�        .txt��6�  �� 6�  �� 6�  $i�c                Every line in the text file is one full data entry. Entry data fields are delimited��6�  �� 6k  �� 6w  $g�a                by a single tab character. If there is no value for a field, simply enter another��6{  �� 6�  �� 6�  $A�;                tab and begin entering the next data field.��6�  �� 6;  �� 6G  $�General��6K  �� 6\  �� 6h  $]�W        Empty data entries will be replaced with [Not Available]. Additionally, if data��6l  �� 6�  �� 6�  $`�Z        entries are surrounded by [] characters it will be treated as an empty data filed.��6�  6B  5?      5@      �� 5A      $��Input Arguments��6"   $	��130��6;   .��5B      � ���	#FFE5E5E5.+�5C   "   �� $
��Left=�6,   $��7,2,0,0q�5D      $
��Wrap=�5E      $	��Top=�5F      $	��462��5G      $	��227��6+   �� 69   $�Input Arguments��6=   �� 6V   �� 6b   $�        ��6f   �� 6x   �� 6�   $ �        Min Column Width: ��6�   �� 6�   �� 6�   $J�D                Define the minimum number of pixels between columns.��6�   �� 6
  �� 6  $�	��6  �� 6)  �� 65  $"�        Max Unique Entries: ��69  �� 6_  �� 6k  $Z�T                Columns with more than this number of unique non-numeric entries are��6o  �� 6�  �� 6�  $&�                 filtered out.   ��6�  �� 6  �� 6  $�	��6  �� 6&  �� 62  $#�        Begin Numeric Aprox: ��66  �� 6]  �� 6i  $^�X                If a column containing only numeric or empty values exceeds this number,��6m  �� 6�  �� 6�  $a�[                than points are not directly mapped to this column but rather approximated ��6�  �� 6D  �� 6P  $:�4                based on the range of its extremes. ��6T  �� 6�  �� 6�  $�	 ��6�  �� 6�  �� 6�  $�	��6�  6�  5H      5I      �� 5J      $��Features��6"   $	��130��64   .��5K      � ���	#FFE5E5E5.+�5L   "   �� $
��Left=�6,   $��7,2,0,0q�5M      $
��Wrap=�5N      $	��Top=�5O      $	��462��5P      $	��227��6+   �� 69   $�View Data Interpretation��6=   �� 6_   �� 6k   $b�\        This feature displays up to the first 21 entries of your data to verify the provided��6o   �� 6�   �� 6�   $/�)        input file was parsed correctly. ��6�   �� 6  �� 6$  �� 6*  �� 66  $ �Parallel Coordinates in 2D��6:  �� 6^  �� 6j  $Y�S        A method of viewing data in two dimensions. Note that for each column there��6n  �� 6�  �� 6�  $W�Q        is the ability to click on it and drag it to a new location, inserting it��6�  �� 66  �� 6B  $Y�S        between the columns the mouse up event occurs between. Additionally, points��6F  �� 6�  �� 6�  $]�W        can be filtered out by clicking and dragging the cursor on portions of a single��6�  �� 6  �� 6   $Y�S        column. Clicking again on the column in question will free the data filter.��6$  �� 6�  �� 6�  $�	 ��6�  �� 6�  �� 6�  $�	��6�  6�  5Q      5R      5S      5T      5U      �� 5V      $��About��6   $��130px��6%   .��5W      � ���	#FFE5E5E5.+�5X      �� $
��Left=�6    $��	10,10,0,0q�5Y      $
��Wrap=�5Z      $	��Top=�5[      $	��471��5\      $	��247��6   �� 6-   $8�2Parallel Coordinates: A data visualization method.��61   �� 6m   �� 6y   �� 6   �� 6�   $�Developed By��6�   �� 6�   $� ��6�   �� 6�   �� 6�   $�        Aaron Battershell��6�   �� 6�   �� 6�   �� 6�   �� 6  $�Written With��6	  �� 6  �� 6+  $3�-        Windows Presentation Foundation (WPF)��6/  �� 6f  �� 6r  �� 6x  �� 6�  $�View the source at��6�  �� 6�  �� 6�  $=�7        GitHub.com/AaronBattershell/ParallelCoordinates��6�  �� 6�  �� 6  �� 6  �� 6  �� 6  �� 6%  $!�(ɔ) 2015 Aaron Battershell��6)  �� 6M  �� 6Y  �� 6_  �� 6k  6r  5]      5^      5_      5`      5a      