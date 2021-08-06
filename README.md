# MyProjects
 
"Lights-Off Project" in limbajul C++ (+ interfață grafică SFML)
"Checkers" in Wpf (C#)
"Breakout Boost" in limbajul Java

#Description *Checkers* project

Checkers is a project designed in MVVM.
The application starts with a general menu which has two buttons: the first one opens the game rules, and the second one is a way to intermediate settings in which one, players name could be changed, the players could choose the *multiple jumps* option (see The Rules) or to play the last saved game.
During match, each piece of each color on the board is counted (the game ends when one of the players has no longer pieces on the board) and it's marked who"s turn is at the moment.
If one of the players wants to exit before the game ends, he will have the option to save the current state of it.
The most important element of this application is the board. It is represented by an Observable collection <Observable collection<Field>>. Field is a special class that retains information about a board field ( the color of it, if it"s empty or contains a piece, the color and the state of this piece - king or not) and methods about the moves allowed.
