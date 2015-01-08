using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Graph;
using NUnit.Framework;
using NUnit.Framework.Constraints;

namespace UnitTests
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

            Assert.IsTrue(graph.Grid[0, 0].HasNeighbor(graph.Grid[1, 0]));
            Assert.IsTrue(graph.Grid[0, 0].HasNeighbor(graph.Grid[0, 1]));
            Assert.IsTrue(graph.Grid[0, 0].HasNeighbor(graph.Grid[1, 1]));
            Assert.IsTrue(graph.Grid[0, 0].HasNeighbor(graph.Grid[0, 4]));

            var path = graph.ShortestPath(graph.Grid[0, 0], graph.Grid[0, 4]).Select(x => graph.GetCoordinates(x)).ToList();
            Assert.IsTrue(path.Count == 2);

            //Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[1, 0]));
            //Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[0, 1]));
            //Assert.IsFalse(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[1, 1]));
            //Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[0, 4]));
        }

        [Test]
        public void LargeSquareGridTiming()
        {
            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            var largeGrid = new SquareGraph(250, 250, false, true);
            largeGrid.Initialize();

            stopwatch.Stop();
            Console.WriteLine("Big graph (ms): " + stopwatch.Elapsed.Milliseconds);

            var rand = new Random((int) DateTime.Now.Ticks);
            for (var i = 0; i < 10000; i++)
            {
                var x = rand.Next(0, 249);
                var y = rand.Next(0, 249);

                var vertex = largeGrid.Grid[y, x];
                if (vertex.IsIsolated() || (x == 249 && y == 249))
                    continue;

                largeGrid.Disconnect(vertex);
            }

            stopwatch.Reset();
            stopwatch.Start();
            var path = largeGrid.ShortestPath(largeGrid.Grid[0, 0], largeGrid.Grid[249, 249]);
            var coordinates = path != null
                ? path.Select(largeGrid.GetCoordinates).ToList()
                : new List<Tuple<int, int>>();
            stopwatch.Stop();
            Console.WriteLine("Big graph shortest path 0,0 - 249,249 (ms): " + stopwatch.ElapsedMilliseconds);
            if (coordinates.Any())
                coordinates.ForEach(x => Console.WriteLine("  " + x.ToString()));
            else
                Console.WriteLine("No path.");
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
