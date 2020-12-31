Tagger_Example.psd Usage:
=========================

Use Tagger_Example.psd as the template for creating new Tags and Ratings
Images are 256 x 256 pixels, 72 pixels/inch, transparent background
Tags are positioned on album artwork background to minimize
obscuring original album title and any other album cover text

Rating Template Naming Format/Order (Top-most to Bottom-most):
==============================================================
Stars_1 = One Star
Stars_2 = Two Stars
Stars_3 = Three Stars
Stars_4 = Four Stars
Stars_5 = Five Stars

Tag Template Naming Format/Order (Top-most to Bottom-most):
===========================================================
Lead = Red Arrow Upper Quill
Lead Bonus = Red Arrow Lower Quill
Rhythm = Green Arrow Upper Quill
Rhythm Bonus = Green Arrow Lower Quill
Bass = Blue Arrow Upper Quill
Bass Bonus = Blue Arrow Lower Quill
Vocals = Yellow Arrow Head
DD = Dynamic Difficulty (optional - all RS2014 Remastered CDLC should have Dynamic Difficulty)

Background Template Naming Format/Order Top-most to Bottom-most):
=================================================================
Custom_Tags_Stars = Purple Background with Boarder
Custom_Stars = Stars Purple Background Only
Custom_Tags = CDLC on Purple Background with Boarder
Background = Transparent Background (Place Holder for Tag and/or Rating)

Other Reference Layers:
=======================
Album Art
Other Original Tag Art

Photoshop Script Usage (Photoshop-Export-Layers-to-Files-Fast):
===================================================================
>Photoshop-Export-Layers-to-Files-Fast>Export Layers To Files (Fast).jsx
to quickly export PhotoShop layers as individual PNG files
There are file naming dependencies so follow example PNG naming conventions
without any deviation.

Be sure to generate and include a preview [template name]_prev.png file
that contains a complete tag preview as you would like it displayed in CFSM.

Credit goes to Frack, Lovroman, Motive, and Zagatozee for original tagger concepts, and PSD artwork