   M S B A M L   `   `   `  ����R  NSharpGL.WPF, Version=2.3.0.1, Culture=neutral, PublicKeyToken=27fc851303210b27>.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPFSharpGL.WPF  5   	    AssimpSample   AssimpSample.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   BsharpGL.clr-namespace:SharpGL.WPF;assembly=SharpGL.WPF     5   	   -   5          � Title$  SharpGL WPF Application��5   	   $	��350��6)   $	��525��66   .��5      � .+�5	   
      SharpGL.WPF.OpenGLControl  -   5        $��openGLControl��5
         DrawFPS .    RenderContextType$	 FBO=�5         	FrameRate$ 10Q�64   �� 5   
   -   5        $��slider1��5      $	��100��6     � Maximum$	 350Q�6,   $��31��6V   $��Bottom=�6b   $
��Left=�6}   $��
111,0,0,29q�6�    � Value$	 350Q�6�   �� 5   
   -   5        $��label1��5   _   $��Visina stuba:��6   
�������$��28��6@   $��	12,0,0,40q�6L   $��Bottom=�6m   $
��Left=�6�   $��93��6�   $��14(�6�   $��Right=�6�   �� 5   
   -   5        $��slider2��5   L   $��23��6   $
��Left=�6   $��
111,0,0,74q�68   $��Bottom=�6[   $	��100��6v   $ 10Q�6�   $	 0.2Q�6�    � Minimum$ 0Q�6�   �� 5   
   -   5        $��label2��5   ^   $��Cylinder fs:��6   $��28��6'   
�������$��	12,0,0,74q�6K   $��Bottom=�6l   $
��Left=�6�   $��93��6�   $��13(�6�   $��Right=�6�   �� 5   
   -   5        $��label3��5   v   $��Cube fs:��6   $��28��6#   
�������$
��Left=�6G   $��
12,0,0,110q�6b   $��Bottom=�6�   $��93��6�   $��13(�6�   $��Right=�6�   �� 5   
   -   5        $��slider3��5   M   $��23��6   $
��Left=�6   $��111,0,0,110q�68   $��Bottom=�6\   $	��100��6w   $ 10Q�6�   $	 100Q�6�   $ 10Q�6�   �� 5   
   -	   5        $��slider4��5   M   $��23��6   $
��Left=�6   $��111,0,0,148q�68   $��Bottom=�6\   $	��100��6w   $ 1Q�6�   $ 5Q�6�   $ 20Q�6�   �� 5   
   -
   5        $��label4��5   u   $��Brzina:��6   $��28��6"   
�������$
��Left=�6F   $��
12,0,0,148q�6a   $��Bottom=�6�   $��93��6�   $��13(�6�   $��Right=�6�   �� 5   
   -   5        $��label5��5   k   $��
F11 - Exit��6   
�������$��Right=�6<   $��	0,12,12,0q�6X   $��91��6y   $��Right=�6�   $��28��6�   $	��Top=�6�   �� 5   
   -   5        $��label6��5   �   $��C - Pokreni animaciju��6   $��28��60   
�������$��Right=�6S   $��	0,35,12,0q�6o   $	��Top=�6�   $	��137��6�   $��Right=�6�   $��13(�6�   5      5      