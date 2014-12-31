using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Graph;
using NUnit.Framework;

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
            for (var x = 0; x < 100; x++)
                graph.Add(new Vertex());
            var vertices = graph.Vertices.ToList();
            var rand = new Random((int) DateTime.Now.Ticks);
            for (var x = 0; x < 1000; x++)
            {
                var vertex1 = vertices[rand.Next(0, 99)];
                var vertex2 = vertices[rand.Next(0, 99)];
                if (vertex1 == vertex2 || vertex1.HasNeighbor(vertex2))
                    continue;
                var edge = graph.Connect(vertex1, vertex2);
                edge.Weight = rand.Next(1, 5);
            }

            var source = vertices[rand.Next(0, 99)];
            var destination = vertices[rand.Next(0, 99)];
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
            var graph = new SquareGraph(5, 5, true, true);
            graph.Initialize();

            var stopwatch = Stopwatch.StartNew();
            stopwatch.Start();

            var largeGrid = new SquareGraph(250, 250, false, true);
            largeGrid.Initialize();

            stopwatch.Stop();
            Console.WriteLine("Big graph (ms): " + stopwatch.Elapsed.Milliseconds);

            var rand = new Random((int) DateTime.Now.Ticks);
            for (var i = 0; i < 10000; i++)
            {
                var x = rand.Next(0, 125);
                var y = rand.Next(0, 125);

                var vertex = largeGrid.Grid[y, x];
                if (vertex.IsIsolated() || (x == 125 && y == 125))
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
            Console.WriteLine("Big graph shortest path 0,0 - 125,125 (ms): " + stopwatch.ElapsedMilliseconds);
            if (coordinates.Any())
                coordinates.ForEach(x => Console.WriteLine("  " + x.ToString()));
            else
                Console.WriteLine("No path.");
        }
    }
}
