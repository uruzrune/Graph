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
        private SquareGraph _graph;

        [SetUp]
        public void SetUp()
        {
            _graph = new SquareGraph(5, 5, true, true);
            _graph.Initialize();
        }

        [Test]
        public void PathTesting()
        {
            Assert.IsTrue(_graph.Grid[0,0].HasNeighbor(_graph.Grid[1,0]));
            Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[0, 1]));
            Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[1, 1]));
            Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[0, 4]));

            var path = _graph.ShortestPath(_graph.Grid[0, 0], _graph.Grid[0, 4]).Select(x => _graph.GetCoordinates(x)).ToList();
            Assert.IsTrue(path.Count == 2);

            //Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[1, 0]));
            //Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[0, 1]));
            //Assert.IsFalse(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[1, 1]));
            //Assert.IsTrue(_graph.Grid[0, 0].HasNeighbor(_graph.Grid[0, 4]));
        }

        [Test]
        public void LargeGridTiming()
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
