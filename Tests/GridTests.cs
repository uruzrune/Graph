using NUnit.Framework;
using Graph.Model;
using System.Diagnostics;

namespace Graph.Tests
{
    class GridTests
    {
        [SetUp]
        public void SetUp()
        {
        }

        [Test]
        public void SimpleGraphPathTesting()
        {
            var graph = new SimpleGraph();
            for (var i = 0; i < 10; i++)
            {
                graph.Add(new Vertex(graph, new SimpleGraphVertexValue(i)));
            }
            var verticesList = graph.Vertices.ToList();
            for (var i = 0; i < graph.Vertices.Count - 1; i++)
            {
                // straight line with weight = 1 per edge
                graph.Connect(verticesList[i], verticesList[i + 1]);
            }

            var source = graph.Vertices.First(x => x.Edges.Count == 1);
            var destination = graph.Vertices.First(x => x.Edges.Count == 1 && x != source);
            var path = graph.ShortestPath(source, destination);
        }

        [Test]
        public void SquareGraphPathTesting()
        {
            var graph = new SquareGraph(5, 5, true, true);
            graph.Initialize();
            Assert.Multiple(() =>
            {
                Assert.That(graph.Grid[0, 0].HasNeighbor(graph.Grid[1, 0]), Is.True);
                Assert.That(graph.Grid[0, 0].HasNeighbor(graph.Grid[0, 1]), Is.True);
                Assert.That(graph.Grid[0, 0].HasNeighbor(graph.Grid[1, 1]), Is.True);
                Assert.That(graph.Grid[0, 0].HasNeighbor(graph.Grid[0, 4]), Is.True);
            });

            var path = graph.ShortestPath(graph.Grid[0, 0], graph.Grid[0, 4])?.Select(x => graph.GetCoordinates(x)).ToList();
            Assert.That(path?.Count, Is.EqualTo(2));
        }

        [Test]
        public void LargeSquareGridTiming()
        {
            const int width = 50;
            const int height = 50;
            const int numVerticesToIsolate = 1000;

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            var largeGrid = new SquareGraph(width, height, false, true);
            largeGrid.Initialize();

            stopwatch.Stop();
            Console.WriteLine("Big graph (ms): " + stopwatch.Elapsed.Milliseconds);

            var rand = new Random((int) DateTime.Now.Ticks);
            for (var i = 0; i < numVerticesToIsolate; i++)
            {
                var x = rand.Next(0, width - 1);
                var y = rand.Next(0, height - 1);

                var vertex = largeGrid.Grid[y, x];
                if (vertex.IsIsolated() || (x == width - 1 && y == height - 1))
                {
                    continue;
                }

                largeGrid.Disconnect(vertex);
            }

            stopwatch.Reset();
            stopwatch.Start();
            var path = largeGrid.ShortestPath(largeGrid.Grid[0, 0], largeGrid.Grid[width - 1, height - 1]);
            var coordinates = path != null
                ? path.ConvertAll(largeGrid.GetCoordinates)
                : new List<Tuple<int, int>>();
            stopwatch.Stop();
            Console.WriteLine($"Big graph shortest path 0,0 - {width - 1},{height - 1} (ms): " + stopwatch.ElapsedMilliseconds);
            if (coordinates.Count > 0)
            {
                coordinates.ForEach(x => Console.WriteLine("  " + x.ToString()));
            }
            else
            {
                Console.WriteLine("No path.");
            }
        }

        [Test]
        public void BasicHexGridTests()
        {
            var grid = new HexGraph(25, 25);
            grid.Initialize();

            foreach (var vertex in grid.Vertices)
            {
                Console.WriteLine(grid.GetCoordinates(vertex));
                foreach (var edge in vertex.Edges)
                {
                    Console.WriteLine("  -> " + grid.GetCoordinates(edge.Vertices.First(x => x != vertex)));
                }
            }
        }
    }
}
