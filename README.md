---
layout: default
---

# Micropolis Unity Version

Micropolis is a ground up C# rewrite of the MicropolisCore system using the Unity engine to create a full blown Micropolis native game that runs on Windows, Mac, and Unix. It includes a fully working city simulation game true to the original along with several new features and improvements.

![screenshot](https://raw.githubusercontent.com/bsimser/Micropolis/blob/gh-pages/images/micropolis.png)

## Installation

You'll be able to install a copy of Micropolis once we have an installer (using [Squirrel](https://github.com/Squirrel/Squirrel.Windows)). In the meantime you can download or clone this repository and run it from within Unity (it's not recommended to fork the repo at this time as there a lot of changes going on and the current codebase isn't stable yet).

## Documentation

Please see our [wiki](https://github.com/bsimser/Micropolis/wiki) for everything you wanted to know about Micropolis, but were afraid to ask.

## Latest News

For a [list of known issues see GitHub](https://github.com/bsimser/Micropolis/issues) or take a look at the [roadmap](https://github.com/bsimser/Micropolis/wiki/Roadmap) or by watching this repo and subscribing to notifications.

## License

True to the orignal, this game and it's code is licensed under the [MIT License](https://opensource.org/licenses/MIT) for anyone to use. 

## History

Thursday, January 10, 2008 was the day [Don Hopkins](https://github.com/SimHacker) released Micropolis, an open source release of the original City Simulator, [SimCity](https://en.wikipedia.org/wiki/SimCity). I've been involved with Micropolis ever since the day Don told me about the release of the code. I immediately got into it, helped promote it, fixed up things in the code, and wrote a series of (unfinished) [blog posts](https://weblogs.asp.net/bsimser/building-a-city-the-series) about it.

After all, this was based on the *original* SimCity source code right? How could I not do something with it.

The setup of the original code was hard. It required a ton of tools (SWIG, Tcl, Python, etc.) and resulted in a slow and klunky system that, when a crash occured, it was hard to tell if it was the graphics, the interpreter, the original code, or any one of the dozen or so subsystems in between.

Skip ahead a few years and, well, things haven't gone too far. A few people have ran with the code but it never became the massive thing that the original SimCity ever was (and I didn't think it would have). Instead there are a few ports of it to different platform (JavaScript, C#, etc.) but nothing to write home about.

So here we are with yet-another-port. I guess.

## This Project

This rewrite of Micropolis represents a culmination of sources from the original Micropolis source code release which was made up of modified TCL/Tk C code (based on the original X11 Multiplayer Unix release of SimCity, but with the Multiplayer bits removed) and the C++/Python rewrite done by [Don Hopkins](https://github.com/SimHacker). 

Using both of these codebases as a reference, this project builds a C# 2D game using the Unity engine for graphics and UI. The result is a game that runs on modern hardware as a single stand-alone executable with the ability to port and run on other platforms (iOS, Android, etc.). No special tools are required to run it except a computer. The installation is a single click download (with built-in automatic updates so you always have the latest version).

Going beyond the origina, the plan is to continue to evolve this desktop version with new features that brings the old school gameplay of city building and new ideas together in a modern package.

## Building

This project can be built using the free version of Unity v2017.3 on any platform the Unity Editor is supported. Just open the project and build. All code and assets are included. Micropolis makes use of the following (free) assets:
* [TextMesh Pro](https://assetstore.unity.com/packages/essentials/beta-projects/textmesh-pro-84126)
* [Cinemachine](https://assetstore.unity.com/packages/essentials/cinemachine-79898)

Both assets are free and from Unity Technologies and should be downloaded and installed prior to opening the project in Unity.

## Format

Text can be **bold**, _italic_, or ~~strikethrough~~.

[Link to another page](another-page).

There should be whitespace between paragraphs.

There should be whitespace between paragraphs. We recommend including a README, or a file with information about your project.

# [](#header-1)Header 1

This is a normal paragraph following a header. GitHub is a code hosting platform for version control and collaboration. It lets you and others work together on projects from anywhere.

## [](#header-2)Header 2

> This is a blockquote following a header.
>
> When something is important enough, you do it even if the odds are not in your favor.

### [](#header-3)Header 3

```js
// Javascript code with syntax highlighting.
var fun = function lang(l) {
  dateformat.i18n = require('./lang/' + l)
  return true;
}
```

```ruby
# Ruby code with syntax highlighting
GitHubPages::Dependencies.gems.each do |gem, version|
  s.add_dependency(gem, "= #{version}")
end
```

#### [](#header-4)Header 4

*   This is an unordered list following a header.
*   This is an unordered list following a header.
*   This is an unordered list following a header.

##### [](#header-5)Header 5

1.  This is an ordered list following a header.
2.  This is an ordered list following a header.
3.  This is an ordered list following a header.

###### [](#header-6)Header 6

| head1        | head two          | three |
|:-------------|:------------------|:------|
| ok           | good swedish fish | nice  |
| out of stock | good and plenty   | nice  |
| ok           | good `oreos`      | hmm   |
| ok           | good `zoute` drop | yumm  |

### There's a horizontal rule below this.

* * *

### Here is an unordered list:

*   Item foo
*   Item bar
*   Item baz
*   Item zip

### And an ordered list:

1.  Item one
1.  Item two
1.  Item three
1.  Item four

### And a nested list:

- level 1 item
  - level 2 item
  - level 2 item
    - level 3 item
    - level 3 item
- level 1 item
  - level 2 item
  - level 2 item
  - level 2 item
- level 1 item
  - level 2 item
  - level 2 item
- level 1 item

### Small image

![](https://assets-cdn.github.com/images/icons/emoji/octocat.png)

### Large image

![](https://guides.github.com/activities/hello-world/branching.png)


### Definition lists can be used with HTML syntax.

<dl>
<dt>Name</dt>
<dd>Godzilla</dd>
<dt>Born</dt>
<dd>1952</dd>
<dt>Birthplace</dt>
<dd>Japan</dd>
<dt>Color</dt>
<dd>Green</dd>
</dl>

```
Long, single-line code blocks should not wrap. They should horizontally scroll if they are too long. This line should be long enough to demonstrate this.
```

```
The final element.
```
