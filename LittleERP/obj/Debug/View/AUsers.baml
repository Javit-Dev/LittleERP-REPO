   M S B A M L   `   `   `  ����D  @LittleERP, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:LittleERP.ViewLittleERP.View  5   	       LittleERP.View.AUsers   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35n jSAPBusinessObjects.WPF.ViewerShared, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:LittleERP.View     5   	     � Title$  AUsers��5   	   $��567.6��6   $	��806��6'   .��5	      � .+�5
   
   �� -   5         ��Name$ lblName��5
      $��Name:��6!   $
��Left=�61   $��
150,33,0,0q�6L   $	��Top=�6`   $��24(�6x   �� 5   
   -   5        $ lblPassword��5      $��	Password:��6%   $
��Left=�69   $��
150,79,0,0q�6T   $	��Top=�6h   $��24(�6�   �� 5   
   -   5        $ lblRepeatPassword��5      $��Repeat Password:��6+   $
��Left=�6F   $��150,121,0,0q�6a   $	��Top=�6v   $��24(�6�   �� 5   
   -   5         ��Name$ txtName��5      $
��Left=�6#   $��42��6>   $��
402,38,0,0q�6J    �� TextWrapping$
 Wrap=�6^   $	��Top=�6r   $	��258��6�   $��24(�6�   �� 5   
   -   5         ��Name$ lstAssignedRoles��5      $
��Left=�6,   $	��100��6G   $��118,291,0,0q�6T   $	��Top=�6i   $	��189��6�   ��5      �� 5      .R�5      �� ��6    �� *name+68   5      5      �� 5   
   -   5        $ lstAllowedRoles��5      $
��Left=�6+   $	��100��6F   $��491,291,0,0q�6S   $	��Top=�6h   $	��191��6�   ��5      �� 5      .R�5      �� ��6    �� *name+68   5      5      �� 5   
   -   5        $ lblAssignedRoles��5      $��ASSIGNED ROLES��6*   $
��Left=�6C   $��115,240,0,0q�6^   $	��Top=�6s   $��24(�6�   $	��205��6�   �� 5   
   -   5        $ lblAllowedRoles��5      $��ALLOWED ROLES��6)   $
��Left=�6A   $��491,240,0,0q�6\   $	��Top=�6q   $��24(�6�   �� 5   
   -	   5         ��Name$ btnAddAssigned��5      $��->��6)   $
��Left=�69   $��365,312,0,0q�6T   $	��Top=�6i   $��75��6�   6�   �� 5   
   -
   5        $ btnAddAllowed��5      $��<-��6(   $
��Left=�68   $��365,360,0,0q�6S   $	��Top=�6h   $��75��6�   6�   �� 5    
   -   5        $ btnOk��5       $��OK��6    $
��Left=�6-   $��539,423,0,0q�6H   $	��Top=�6]   $��94��6u   $��36��6�   6�   G� 5!   
   -   5         G�Name$ pwdPassword��5!      $
��Left=�6+   $��
402,80,0,0q�6F   $	��Top=�6Z   $��42��6r   $	��258��6~   G� 5"   
   -   5        $ pwdPasswordRepeat��5"      $
��Left=�61   $��402,122,0,0q�6L   $	��Top=�6a   $��42��6y   $	��258��6�   �� 5#   
   -   5        $ 	btnCancel��5#      $��CANCEL��6$   $
��Left=�65   $��170,423,0,0q�6P   $	��Top=�6e   $��94��6}   $��36��6�   6�   �� 5$   
   -   5         ��Name$ chkFirstSession��5$      $��Change password on login��6,   $
��Left=�6O   $��402,180,0,0q�6j   $	��Top=�6   $��43��6�   $	��177��6�   5%      5'      5(      