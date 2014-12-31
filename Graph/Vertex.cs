using System;
using System.Collections.Generic;
using System.Linq;

namespace Graph
{
    public class Vertex
    {
        public HashSet<Edge> Edges { get; private set; }
        public IVertexValue Value { get; set; }

        public IEnumerable<Vertex> Neighbors
        {
            get
            {
                return Edges.SelectMany(x => x.Vertices).Where(x => x != this);
            }
        }

        public Vertex()
        {
            Edges = new HashSet<Edge>();
        }

        public bool HasEdge(Edge edge)
        {
            return Edges.Contains(edge);
        }

        public bool HasNeighbor(Vertex vertex)
        {
            return Edges.Any(x => x.Vertices.Contains(this) && x.Vertices.Contains(vertex));
        }

        public Edge Connect(Vertex newNeighbor)
        {
            if (HasNeighbor(newNeighbor))
                throw new InvalidOperationException("already neighbors");
            return new Edge(this, newNeighbor);
        }

        public int Degrees()
        {
            return Edges.Count();
        }

        public bool IsIsolated()
        {
            return !Edges.Any();
        }

        public bool IsLeaf()
        {
            return Degrees() == 1;
        }

        public void Disconnect()
        {
            if (IsIsolated())
                throw new InvalidOperationException("vertex is already disconnected");

            var edges = new List<Edge>(Edges);
            edges.ForEach(x => x.Clear());
        }
    }
}
