# Micropolis Unity Version

This is a ground-up rewrite of the MicropolisCore system built with the Unity engine.

## History

I've been involved with Micropolis ever since the day [Don Hopkins](https://github.com/SimHacker) told me about the release of the code. I immediately got into it, helped promote it, fixed up things in the code, and wrote a series of (unfinished) [blog posts](https://weblogs.asp.net/bsimser/building-a-city-the-series) about it.

After all, this was the *original* SimCity source code right? How could I not.

The setup of the original code was hard. It required a ton of tools (SWIG, Tcl, Python, etc.) and resulted in a slow and klunky system that, when a crash occured, it was hard to tell if it was the graphics, the interpreter, the original code, or any one of the dozen or so subsystems in between.

Skip ahead a few years and, well, things haven't gone too far. A few people have ran with the code but it never became the massive thing that the original SimCity ever was (and I didn't think it would have). Instead there are a few ports of it to different platform (JavaScript, C#, etc.) but nothing to write home about.

So here we are with yet-another-port I guess.

## This Project

This project takes two sources: the original C code for SimCity; and the C++ code written by Don for feeding into SWIG and generating whatever output you wanted (in the case of the original code release, Python). 

Using both as a reference this project builds a 2D C# game using the Unity engine. The result will be a game that runs on modern hardware as a single stand-alone executable true to the original and able to port to other platforms (Linux, OSX, iOS, Android, etc.)

True to the orignal, this game is fully open sourced and licensed under the [MIT License](https://opensource.org/licenses/MIT) for anyone to use. Enjoy!
