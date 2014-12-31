using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class SimpleGraph : AbstractGraph
    {
        public override void Initialize()
        {
        }

        /// <summary>
        /// A shortest-path algorithm based on Bellman-Ford, modified to return a shortest-list path.
        /// </summary>
        /// <param name="source">The point where the search should begin.</param>
        /// <param name="destination">The desired endpoint of the search.</param>
        /// <returns>Returns a list of vertices that make the shortest path from the source to destination, or null if no path can be found.</returns>
        public override List<Vertex> ShortestPath(Vertex source, Vertex destination)
        {
            var distances = Vertices.Where(x => x != source).ToDictionary(vertex => vertex, vertex => Int32.MaxValue);
            distances.Add(source, 0);
            var previous = new Dictionary<Vertex, Vertex>();
            var queue = new Queue<Vertex>();
            queue.Enqueue(source);
            while (queue.Any())
            {
                var workingVertex = queue.Dequeue();
                if (workingVertex == destination)
                {
                    var currentVertex = destination;
                    var resultList = new List<Vertex>();
                    while (currentVertex != null)
                    {
                        resultList.Add(currentVertex);
                        currentVertex = previous.ContainsKey(currentVertex) ? previous[currentVertex] : null;
                    }
                    resultList.Reverse();
                    return resultList;
                }
                foreach (var edge in workingVertex.Edges)
                {
                    var neighbor = edge.Vertices.First(x => x != workingVertex);
                    var newDistance =
                        distances[workingVertex] != Int32.MaxValue
                            ? distances[workingVertex] + edge.Weight
                            : Int32.MaxValue;

                    if (newDistance < distances[neighbor] && neighbor.Value.IsEnterableFrom(workingVertex.Value))
                    {
                        distances[neighbor] = newDistance;
                        if (!queue.Contains(neighbor))
                            queue.Enqueue(neighbor);
                        previous[neighbor] = workingVertex;
                    }
                }
            }
            return null;
        }
    }

    public class SimpleGraphVertexValue : IVertexValue
    {
        private readonly int _value;

        public SimpleGraphVertexValue(int value)
        {
            _value = value;
        }

        public bool IsEnterableFrom(IVertexValue source)
        {
            return true;
        }

        public override string ToString()
        {
            return Convert.ToString(_value);
        }
    }
}