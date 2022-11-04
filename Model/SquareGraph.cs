using Graph.Utilities;

namespace Graph.Model
{
    public class SquareGraph : AbstractGraph
    {
        /// <summary>
        /// The two-dimensional grid that sits on top of the more abstract graph.
        /// </summary>
        public Vertex[,] Grid { get; }

        /// <summary>
        /// Height of the graph. The 'y' value.
        /// </summary>
        public int Width { get; }
        /// <summary>
        /// Width of the graph. The 'x' value.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Does the graph wrap around from left to right?
        /// </summary>
        public bool WrapAround { get; }
        /// <summary>
        /// Does the graph permit traversal in a diagonal direction?
        /// </summary>
        public bool UseDiagonals { get; }

        internal readonly Dictionary<Vertex, Tuple<int, int>> TupleDictionary = new();
        private Dictionary<Vertex, Vertex> _cameFromDictionary = new();

        /// <summary>
        /// The six directions that this graph honors.
        /// </summary>
        public List<Direction> Directions { get; internal set; }

        /// <summary>
        /// SquareGraph constructor.
        /// </summary>
        /// <param name="width">Width of the graph. The 'x' value.</param>
        /// <param name="height">Height of the graph. The 'y' value.</param>
        /// <param name="wrapAround">Does the graph wrap around from left to right? Default is false.</param>
        /// <param name="useDiagonals">Does the graph permit traversal in a diagonal direction? Default is false.</param>
        public SquareGraph(int width, int height, bool wrapAround = false, bool useDiagonals = false)
        {
            if (width < 1)
            {
                throw new InvalidOperationException("Width must be positive");
            }

            if (height < 1)
            {
                throw new InvalidOperationException("Height must be positive");
            }

            Width = width;
            Height = height;

            Grid = new Vertex[Height, Width];
            WrapAround = wrapAround;
            UseDiagonals = useDiagonals;
            Directions = GetDirections();
        }

        /// <summary>
        /// Initializes the graph.
        /// </summary>
        public override void Initialize()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var vertex = new Vertex(this, new SquareGraphVertexValue(new Tuple<int, int>(y, x)));
                    Grid[y, x] = vertex;
                    Add(vertex);
                    TupleDictionary.Add(Grid[y,x], new Tuple<int, int>(y, x));
                }
            }

            for (var y = 1; y < Height - 1; y++)
            {
                for (var x = 1; x < Width - 1; x++)
                {
                    var current = Grid[y, x];
                    Connect(current, Grid[y - 1, x], false);
                    Connect(current, Grid[y + 1, x], false);
                    Connect(current, Grid[y, x - 1], false);
                    Connect(current, Grid[y, x + 1], false);

                    if (UseDiagonals)
                    {
                        Connect(current, Grid[y - 1, x - 1], false);
                        Connect(current, Grid[y - 1, x + 1], false);
                        Connect(current, Grid[y + 1, x - 1], false);
                        Connect(current, Grid[y + 1, x + 1], false);
                    }
                }
            }
            // top left
            Connect(Grid[0, 0], Grid[1, 0], false);
            Connect(Grid[0, 0], Grid[0, 1], false);
            if (UseDiagonals)
            {
                Connect(Grid[0, 0], Grid[1, 1], false);
            }

            // bottom left
            Connect(Grid[Height - 1, 0], Grid[Height - 2, 0], false);
            Connect(Grid[Height - 1, 0], Grid[Height - 1, 1], false);
            if (UseDiagonals)
            {
                Connect(Grid[Height - 1, 0], Grid[Height - 2, 1], false);
            }

            // top right
            Connect(Grid[0, Width - 1], Grid[0, Width - 2], false);
            Connect(Grid[0, Width - 1], Grid[1, Width - 1], false);
            if (UseDiagonals)
            {
                Connect(Grid[0, Width - 1], Grid[1, Width - 2], false);
            }

            // bottom right
            Connect(Grid[Height - 1, Width - 1], Grid[Height - 2, Width - 1], false);
            Connect(Grid[Height - 1, Width - 1], Grid[Height - 1, Width - 2], false);
            if (UseDiagonals)
            {
                Connect(Grid[Height - 1, Width - 1], Grid[Height - 2, Width - 2], false);
            }

            if (!WrapAround)
            {
                return;
            }

            for (var y = 0; y < Height; y++)
            {
                Connect(Grid[y, 0], Grid[y, Width - 1], false);
                if (!UseDiagonals)
                {
                    continue;
                }

                if (y > 0)
                {
                    Connect(Grid[y, 0], Grid[y - 1, Width - 1], false);
                }

                if (y < Height - 1)
                {
                    Connect(Grid[y, 0], Grid[y + 1, Width - 1], false);
                }
            }
        }

        /// <summary>
        /// Shortest path based on A*, modified to return the shortest past.
        /// </summary>
        /// <param name="source">The point where the search should begin.</param>
        /// <param name="destination">The desired endpoint of the search.</param>
        /// <returns>Returns a list of vertices that make the shortest path from the source to destination, or null if no path can be found.</returns>
        public override List<Vertex>? ShortestPath(Vertex source, Vertex destination)
        {
            if (!Vertices.Contains(source))
            {
                throw new ArgumentException("vertex not contained in graph", nameof(source));
            }

            if (!Vertices.Contains(destination))
            {
                throw new ArgumentException("vertex not contained in graph", nameof(destination));
            }

            var open = new Utilities.PriorityQueue<double, Vertex>();
            open.Enqueue(0, source);
            var closed = new HashSet<Vertex>();

            _cameFromDictionary = new Dictionary<Vertex, Vertex>();

            while (open.Count > 0)
            {
                var element = open.Peek();
                if (element.Value == destination)
                {
                    return ReconstructPath(destination);
                }

                element = open.Dequeue();
                closed.Add(element.Value);

                var vertex = element.Value;
                if (vertex.Neighbors.Any(x => !closed.Contains(x)))
                {
                    foreach (var neighbor in vertex.Neighbors.Where(x => !closed.Contains(x) && x.Value.IsEnterableFrom(vertex.Value)))
                    {
                        var gScore = element.Key + ManhattanDistance(source, neighbor);
                        if (open.All(x => x.Value != neighbor) || open.First(x => x.Value == neighbor).Key < gScore)
                        {
                            CameFrom(neighbor, vertex);
                            var fScore = gScore + ManhattanDistance(neighbor, destination);
                            if (open.Any(x => x.Value == neighbor))
                            {
                                open.Remove(open.First(x => x.Value == neighbor));
                            }

                            open.Enqueue(fScore, neighbor);
                        }
                    }
                }

                closed.Add(vertex);
            }

            return null;
        }

        protected double ManhattanDistance(Vertex source, Vertex target)
        {
            var sourceCoordinates = GetCoordinates(source);
            var targetCoordinates = GetCoordinates(target);

            return 0.01 *
                Math.Abs((sourceCoordinates.Item1 * targetCoordinates.Item2) -
                         (targetCoordinates.Item1 * sourceCoordinates.Item2));
        }

        /// <summary>
        /// Returns the y,x coordinates of a given vertex.
        /// </summary>
        /// <param name="vertex">The vertex to return coordinates for.</param>
        /// <returns>A tuple containing the y,x coordinates of the vertex.</returns>
        public Tuple<int, int> GetCoordinates(Vertex vertex)
        {
            if (!Vertices.Contains(vertex))
            {
                throw new InvalidOperationException("Vertex is not contained in graph.");
            }

            return TupleDictionary[vertex];
        }

        private void CameFrom(Vertex source, Vertex destination)
        {
            if (_cameFromDictionary.ContainsKey(source))
            {
                _cameFromDictionary.Remove(source);
            }

            _cameFromDictionary.Add(source, destination);
        }

        private List<Vertex> ReconstructPath(Vertex current)
        {
            var path = new List<Vertex> { current };
            while (_cameFromDictionary.ContainsKey(current))
            {
                current = _cameFromDictionary[current];
                path.Add(current);
            }

            var reversePath = new List<Vertex>(path);
            reversePath.Reverse();

            return reversePath;
        }

        protected List<Direction> GetDirections()
        {
            var directions = new List<Direction>
            {
                Direction.North,
                Direction.East,
                Direction.West,
                Direction.South
            };

            if (UseDiagonals)
            {
                directions.Add(Direction.Northeast);
                directions.Add(Direction.Southeast);
                directions.Add(Direction.Northwest);
                directions.Add(Direction.Southwest);
            }

            return directions;
        }
    }

    public class SquareGraphVertexValue : IVertexValue
    {
        private readonly Tuple<int, int> _tuple;

        public SquareGraphVertexValue(Tuple<int, int> value)
        {
            _tuple = value;
        }

        public override string ToString()
        {
            return _tuple.ToString();
        }

        public bool IsEnterableFrom(IVertexValue source)
        {
            return true;
        }

        public double EnteringCostModifier(IVertexValue source)
        {
            return 1.0;
        }
    }
}
