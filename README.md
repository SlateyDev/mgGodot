# MonoGame Godot

This project is an attempt to replicate Godot functionalities in MonoGame.

The basic idea is I have used several game engines and wanted to make one myself. I decided I would base it on
Godot which I like and try to replicate the functionalities to make my own game engine. The engine tries to
bring as much of the godot functionality across so it can be programmed using C# using a familiar API.

My hopes for the engine is that someday, I will be able to make a game in Godot, and easily transfer that game
to MonoGame with little to no changes to the underlying C# game code that was made under Godot. This will
improve platform support for anything I make and give me the option of implementing other game systems easier
(such as a different physics system, or navigation system).

## FAQ

Disclaimer: No-one has yet asked me any questions about this project. I'm just trying to pre-empt some.

#### Q. Why MonoGame. Why not just use Godot?

1. Platform Support - 
MonoGame has better platform support for C# based games. Currently you cannot make C# games with Godot 4 that
run on a Web Browser. Depending on the platform you want to use, C# may not be an option for you. This may
have improved by the time you read this, but when I started making this engine I wasn't happy with the
platform support and wanted the option of releasing on other platforms if I ever got the chance.
2. Systems -
For a while now I have had issues working with some of Godot's systems. 3D Physics has been problematic
(people are suggested to use GodotJolt to overcome this), the navigation system has limited functionality and
is not fast enough to regenerate navmeshes on the fly, the IK system isn't yet complete. Switching to
using MonoGame allows me to introduce other systems to replace those I don't like easier.
3. Why NOT MonoGame -
I really just wanted to write something in MonoGame and I have used Godot for over 6 years now. So with that
engine experience and API understanding I decided to replicate the SceneTree structure in a MonoGame based
engine. I hope others find it useful for porting, or learning, or whatever.