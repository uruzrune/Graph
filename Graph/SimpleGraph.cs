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

        public override List<Vertex> ShortestPath(Vertex source, Vertex destination)
        {
            var distances = new Dictionary<Vertex, int>();
            var previous = new Dictionary<Vertex, Vertex>();
            var queue = new PriorityQueue<int, Vertex>();
            distances.Add(source, 0);
            foreach (var vertex in Vertices)
            {
                if (vertex != source)
                    distances.Add(vertex, Vertices.Count * Edges.Count + 1);
                queue.Enqueue(1, vertex);
            }
            while (queue.Any())
            {
                var vertex = queue.DequeueValue();
                if (vertex == destination)
                {
                    var sequence = new List<Vertex>();
                    var element = destination;
                    while (previous.ContainsKey(element))
                    {
                        sequence.Add(element);
                        element = previous[element];
                    }
                    if (sequence.Any())
                        sequence.Add(source);
                    sequence.Reverse();
                    return sequence;
                }
                foreach (var neighbor in vertex.Neighbors)
                {
                    var alt = distances[vertex] + vertex.Edges.First(x => x.HasVertex(neighbor)).Weight;
                    if (alt < distances[neighbor])
                    {
                        distances[neighbor] = alt;
                        previous[neighbor] = vertex;
                    }
                }
            }
            return null;
        }
    }
}