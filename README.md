Graph
=====

A simple graph class with adjacency lists and shortest past via Dijkstra's algorithm and A*.

There are likely libraries out there that accomplish far more than what is contained in this simple library. This was simply an academic exercise with the hopes that this code would be useful to someone, somewhere.

Basic Facts
-----------

A simple graph (SimpleGraph) is not bound to a grid. 

SquareGraph is a graph bound to a grid with (y,x) coordinates. Movement along the diagonal directions is optional.

HexGraph is a graph bound to a grid with (y,x) coordinates in the form of either a horizontal or vertical grid. It has four separate configurations (HorizontalOdd, HorizontalEven, VerticalOdd, VerticalEven) as described in this document: http://www.redblobgames.com/grids/hexagons/

The value for a vertex must implement IVertexValue. This provides two helper methods for determining whether pathing can happen between two adjacent vertices. This is mainly a convention for using this library for games, so that there is a distinction between the underlying graph (which would represent terrain, which changes infrequently) and vegetation and structures (which change frequently). 

Copyright
---------

Protected by the MIT License. http://opensource.org/licenses/MIT

(c) 2014 Alexander J Skrabut.
