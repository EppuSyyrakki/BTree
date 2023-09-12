# BTree

##Behavior Tree AI editor for Unity

The library leans heavily on an external Unity library called [XNode](https://github.com/Siccity/xNode/wiki) by Thor Brigsted. It provides the base node and graph classes and the 
editor code to represent them visually in a specialised Unity editor window. The graph and node inherit from XNode’s corresponding classes, allowing them to share any functionality 
present in the base classes. However, as noted in XNode’s documentation, its built-in functionality is limited to viewing and editing graphs, so the node logic and the system querying 
the tree is implemented with custom code. The whole library can be summarised with 10 base classes.

![Description of the library inheritance](ReadmeResources/lib_structure.png)

