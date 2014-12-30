using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class Edge
    {
        public HashSet<Vertex> Vertices { get; private set; }
        public int Weight { get; set; }

        public Edge(Vertex left, Vertex right)
        {
            Vertices = new HashSet<Vertex> {left, right};
            Weight = 1;
            left.Edges.Add(this);
            right.Edges.Add(this);
        }

        public bool HasVertex(Vertex vertex)
        {
            return Vertices.Contains(vertex);
        }

        public void Clear()
        {
            foreach (var vertex in Vertices.Where(x => x.Edges.Contains(this)))
                vertex.Edges.Remove(this);
            Vertices.Clear();
        }
    }
}
