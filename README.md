# Micropolis Unity Version

Micropolis is a ground-up C# rewrite of the MicropolisCore system using the Unity engine to create a full blown Micropolis native game that runs on Windows, Mac, and Unix. It includes a fully working city simulation game true to the original along with several new features and improvements.

## Installation

You'll be able to install a copy of Micropolis once we have an installer (using [Squirrel](https://github.com/Squirrel/Squirrel.Windows)). In the meantime you can download or clone this repository and run it from within Unity (it's not recommended to fork the repo at this time as there a lot of changes going on and the current codebase isn't stable yet).

## Documentation

See the documentation [Table of Contents](wiki) for an overview of Micropolis and everything known about it and it's origin, SimCity.

## History

Thursday, January 10, 2008 was the day [Don Hopkins](https://github.com/SimHacker) released Micropolis, an open source release of the original City Simulator, [SimCity](https://en.wikipedia.org/wiki/SimCity). I've been involved with Micropolis ever since the day Don told me about the release of the code. I immediately got into it, helped promote it, fixed up things in the code, and wrote a series of (unfinished) [blog posts](https://weblogs.asp.net/bsimser/building-a-city-the-series) about it.

After all, this was based on the *original* SimCity source code right? How could I not do something with it.

The setup of the original code was hard. It required a ton of tools (SWIG, Tcl, Python, etc.) and resulted in a slow and klunky system that, when a crash occured, it was hard to tell if it was the graphics, the interpreter, the original code, or any one of the dozen or so subsystems in between.

Skip ahead a few years and, well, things haven't gone too far. A few people have ran with the code but it never became the massive thing that the original SimCity ever was (and I didn't think it would have). Instead there are a few ports of it to different platform (JavaScript, C#, etc.) but nothing to write home about.

So here we are with yet-another-port. I guess.

## This Project

It represents a culmination of sources from the original Micropolis source code release which was made up of modified TCL/Tk C code (based on the original X11 Multiplayer Unix release of SimCity, but with the Multiplayer bits removed) and the C++/Python rewrite done by [Don Hopkins](https://github.com/SimHacker). 

Using both of these codebases as a reference this project builds a C# 2D game using the Unity engine for graphics and UI. The result is a game that runs on modern hardware as a single stand-alone executable true to the original and able to port to other platforms (Linux, OSX, iOS, Android, etc.). No special tools are required to run it except a computer and installation is a single click download (with built-in automatic updates so you always have the latest version).

True to the orignal, this game is fully open sourced and licensed under the [MIT License](https://opensource.org/licenses/MIT) for anyone to use. 

Enjoy!
