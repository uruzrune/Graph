using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class SquareGraph : AbstractGraph
    {
        public Vertex[,] Grid { get; private set; }

        public int MaxWidth = 1024;
        public int MaxHeight = 768;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool WrapAround { get; private set; }
        public bool UseDiagonals { get; private set; }

        private readonly Dictionary<Vertex, Tuple<int, int>> _tupleDictionary = new Dictionary<Vertex, Tuple<int, int>>();
        private Dictionary<Vertex, Vertex> _cameFromDictionary; 

        public SquareGraph(int width, int height, bool wrapAround = false, bool useDiagonals = false)
        {
            if (width > MaxWidth || width < 1)
                throw new InvalidOperationException();

            Width = width;
            Height = height;

            Grid = new Vertex[Height, Width];
            WrapAround = wrapAround;
            UseDiagonals = useDiagonals;
        }

        public override void Initialize()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    var vertex = new Vertex();
                    Grid[y, x] = vertex;
                    Add(vertex);
                    _tupleDictionary.Add(Grid[y,x], new Tuple<int, int>(y, x));
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
                Connect(Grid[0, 0], Grid[1, 1], false);
            // bottom left
            Connect(Grid[Height - 1, 0], Grid[Height - 2, 0], false);
            Connect(Grid[Height - 1, 0], Grid[Height - 1, 1], false);
            if (UseDiagonals)
                Connect(Grid[Height - 1, 0], Grid[Height - 2, 1], false);
            // top right
            Connect(Grid[0, Width - 1], Grid[0, Width - 2], false);
            Connect(Grid[0, Width - 1], Grid[1, Width - 1], false);
            if (UseDiagonals)
                Connect(Grid[0, Width - 1], Grid[1, Width - 2], false);
            // bottom right
            Connect(Grid[Height - 1, Width - 1], Grid[Height - 2, Width - 1], false);
            Connect(Grid[Height - 1, Width - 1], Grid[Height - 1, Width - 2], false);
            if (UseDiagonals)
                Connect(Grid[Height - 1, Width - 1], Grid[Height - 2, Width - 2], false);

            if (!WrapAround) 
                return;

            for (var y = 0; y < Height; y++)
            {
                Connect(Grid[y, 0], Grid[y, Width - 1], false);
                if (!UseDiagonals) 
                    continue;

                if (y > 0)
                    Connect(Grid[y, 0], Grid[y - 1, Width - 1], false);
                if (y < Height - 1)
                    Connect(Grid[y, 0], Grid[y + 1, Width - 1], false);
            }
        }

        public override List<Vertex> ShortestPath(Vertex source, Vertex destination)
        {
            if (!Vertices.Contains(source))
                throw new ArgumentException("vertex not contained in graph", "source");
            if (!Vertices.Contains(destination))
                throw new ArgumentException("vertex not contained in graph", "destination");

            var open = new PriorityQueue<double, Vertex>();
            open.Enqueue(0, source);
            var closed = new HashSet<Vertex>();

            _cameFromDictionary = new Dictionary<Vertex, Vertex>();

            while (open.Any())
            {
                var element = open.Peek();
                if (element.Value == destination)
                    return ReconstructPath(destination);

                element = open.Dequeue();
                closed.Add(element.Value);

                if (element.Value.Neighbors.Any(x => !closed.Contains(x)))
                {
                    foreach (var neighbor in element.Value.Neighbors.Where(x => !closed.Contains(x)))
                    {
                        var gScore = element.Key + ManhattanDistance(source, neighbor);
                        if (open.All(x => x.Value != neighbor) || open.First(x => x.Value == neighbor).Key < gScore)
                        {
                            CameFrom(neighbor, element.Value);
                            var fScore = gScore + ManhattanDistance(neighbor, destination);
                            if (open.Any(x => x.Value == neighbor))
                                open.Remove(open.First(x => x.Value == neighbor));
                            open.Enqueue(fScore, neighbor);
                        }
                    }
                }
                closed.Add(element.Value);
            }

            return null;
        }

        protected double ManhattanDistance(Vertex source, Vertex target)
        {
            var sourceCoordinates = GetCoordinates(source);
            var targetCoordinates = GetCoordinates(target);

            return 10 * 0.001 *
                Math.Abs(sourceCoordinates.Item1 * targetCoordinates.Item2 -
                         targetCoordinates.Item1 * sourceCoordinates.Item2);
        }

        public Tuple<int, int> GetCoordinates(Vertex vertex)
        {
            if (!Vertices.Contains(vertex))
                throw new InvalidOperationException("Vertex is not contained in graph.");

            return _tupleDictionary[vertex];
        }

        private void CameFrom(Vertex source, Vertex destination)
        {
            if (_cameFromDictionary.ContainsKey(source))
                _cameFromDictionary.Remove(source);
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
    }
}
