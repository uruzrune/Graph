using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class HexGraph : SquareGraph
    {
        public HexOrientation Orientation { get; private set; }

        public List<Direction> Directions { get; private set; }

        public Dictionary<Tuple<HexOrientation, bool>, Tuple<int, int>[]> Transformations = GetHexTransformations();

        public HexGraph(int width, int height, HexOrientation orientation = null, bool wrapAround = false) : base(width, height, wrapAround, false)
        {
            Orientation = orientation ?? HexOrientation.HorizontalOdd;
            Directions = GetDirections(Orientation);
        }

        public override void Initialize()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var vertex = new Vertex(this, new SquareGraphVertexValue(new Tuple<int, int>(y, x)));
                    Grid[y, x] = vertex;
                    Add(vertex);
                    TupleDictionary.Add(Grid[y, x], new Tuple<int, int>(y, x));
                }
            }

            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    foreach (var direction in Directions)
                    {
                        var targetCoords = CalculateCoordinates(y, x, direction);
                        var targetY = targetCoords.Item1;
                        var targetX = targetCoords.Item2;

                        if (targetY < 0 || targetY >= Height)
                            continue;
                        if (!WrapAround && (targetX < 0 || targetX >= Width))
                            continue;

                        if (targetX < 0)
                            Connect(Grid[y, x], Grid[targetY, Width - 1], false);
                        else if (targetX >= Width)
                            Connect(Grid[y, x], Grid[targetY, 0], false);
                        else 
                            Connect(Grid[y, x], Grid[targetY, targetX], false);
                    }
                }
            }
        }

        public Tuple<int, int> CalculateCoordinates(int y, int x, Direction direction)
        {
            if (x < 0 || y < 0 || x > Width - 1 || y > Height - 1)
                throw new InvalidOperationException("coordinates located outside bounds of square graph");

            var transformation = Transformations.First(t => t.Key.Item1 == Orientation && t.Key.Item2 == (y%2 == 1)).Value;
            int index;
            if (Orientation == HexOrientation.HorizontalOdd || Orientation == HexOrientation.HorizontalEven)
            {
                if (direction == Direction.Northeast)
                    index = 0;
                else if (direction == Direction.East)
                    index = 1;
                else if (direction == Direction.Southeast)
                    index = 2;
                else if (direction == Direction.Southwest)
                    index = 3;
                else if (direction == Direction.West)
                    index = 4;
                else if (direction == Direction.Northwest)
                    index = 5;
                else
                    throw new InvalidOperationException("unknown direction for orientation");
            }
            else
            {
                if (direction == Direction.North)
                    index = 0;
                else if (direction == Direction.Northeast)
                    index = 1;
                else if (direction == Direction.Southeast)
                    index = 2;
                else if (direction == Direction.South)
                    index = 3;
                else if (direction == Direction.Southwest)
                    index = 4;
                else if (direction == Direction.Northwest)
                    index = 5;
                else
                    throw new InvalidOperationException("unknown direction for orientation");
            }
            var transformationValue = transformation[index];
            return new Tuple<int, int>(y + transformationValue.Item1, x + transformationValue.Item2);
        }

        public static List<Direction> GetDirections(HexOrientation orientation)
        {
            var directions = new List<Direction>
            {
                Direction.Northeast,
                Direction.Southeast,
                Direction.Northwest,
                Direction.Southwest
            };
            if (orientation == HexOrientation.HorizontalOdd || orientation == HexOrientation.HorizontalEven)
            {
                directions.Add(Direction.East);
                directions.Add(Direction.West);
            }
            else
            {
                directions.Add(Direction.North);
                directions.Add(Direction.South);
            }

            return directions;
        }

        public static Dictionary<Tuple<HexOrientation,bool>, Tuple<int,int>[]> GetHexTransformations()
        {
            return new Dictionary<Tuple<HexOrientation, bool>, Tuple<int, int>[]>
            {
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.HorizontalOdd, false), new[]
                    {
                        new Tuple<int, int>(-1, 0),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(1, -1),
                        new Tuple<int, int>(0, -1),
                        new Tuple<int, int>(-1, -1)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.HorizontalOdd, true), new[]
                    {
                        new Tuple<int, int>(-1, 1),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(0, -1),
                        new Tuple<int, int>(-1, 0)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.HorizontalEven, false), new[]
                    {
                        new Tuple<int, int>(-1, 1),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(0, -1),
                        new Tuple<int, int>(-1, 0)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.HorizontalEven, true), new[]
                    {
                        new Tuple<int, int>(-1, 0),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(1, -1),
                        new Tuple<int, int>(0, -1),
                        new Tuple<int, int>(-1, -1)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.VerticalOdd, false), new[]
                    {
                        new Tuple<int, int>(-1, 0),
                        new Tuple<int, int>(-1, 1),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(0, -1),
                        new Tuple<int, int>(-1, -1)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.VerticalOdd, true), new[]
                    {
                        new Tuple<int, int>(-1, 0),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(1, -1),
                        new Tuple<int, int>(0, -1)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.VerticalEven, false), new[]
                    {
                        new Tuple<int, int>(-1, 0),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(1, -1),
                        new Tuple<int, int>(0, -1)
                    }
                },
                {
                    new Tuple<HexOrientation, bool>(HexOrientation.VerticalEven, true), new[]
                    {
                        new Tuple<int, int>(-1, 0),
                        new Tuple<int, int>(-1, 1),
                        new Tuple<int, int>(0, 1),
                        new Tuple<int, int>(1, 0),
                        new Tuple<int, int>(0, -1),
                        new Tuple<int, int>(-1, -1)
                    }
                }
            };
        }
    }
}
