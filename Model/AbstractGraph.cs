namespace Graph.Model
{
    public abstract class AbstractGraph
    {
        public HashSet<Vertex> Vertices { get; }
        public HashSet<Edge> Edges { get; }

        protected AbstractGraph()
        {
            Vertices = new HashSet<Vertex>();
            Edges = new HashSet<Edge>();
        }

        public void Add(Vertex vertex)
        {
            if (vertex.Edges.Count > 0)
            {
                throw new InvalidOperationException("vertex is already connected to another vertex");
            }

            if (Vertices.Contains(vertex))
            {
                throw new InvalidOperationException("vertex is already in graph");
            }

            Vertices.Add(vertex);
        }

        public void Remove(Vertex vertex)
        {
            if (!Vertices.Contains(vertex))
            {
                throw new InvalidOperationException("vertex not contained in graph");
            }

            if (!vertex.IsIsolated())
            {
                throw new InvalidOperationException("vertex is connected; disconnect first");
            }

            Vertices.Remove(vertex);
        }

        public Edge? Connect(Vertex left, Vertex right, bool errorIfConnected = true)
        {
            if (left.HasNeighbor(right) || right.HasNeighbor(left))
            {
                if (errorIfConnected)
                {
                    throw new InvalidOperationException("vertices are already connected");
                }

                return null;
            }

            if (!Vertices.Contains(left) || !Vertices.Contains(right))
            {
                throw new InvalidOperationException("vertex is not in graph");
            }

            var edge = new Edge(left, right);
            Edges.Add(edge);

            return edge;
        }

        public void Disconnect(Vertex vertex)
        {
            if (!Vertices.Contains(vertex))
            {
                throw new InvalidOperationException("vertex is not in graph");
            }

            if (vertex.IsIsolated())
            {
                throw new InvalidOperationException("vertex is isolated");
            }

            var edges = new List<Edge>(vertex.Edges);
            vertex.Disconnect();
            edges.ForEach(x => Edges.Remove(x));
        }

        public void Disconnect(Vertex left, Vertex right)
        {
            if (!left.HasNeighbor(right) || !right.HasNeighbor(left))
            {
                throw new InvalidOperationException("vertices are not connected");
            }

            if (!Vertices.Contains(left) || !Vertices.Contains(right))
            {
                throw new InvalidOperationException("vertex is not in graph");
            }

            var edge = Edges.First(x => x.HasVertex(left) && x.HasVertex(right));
            Edges.Remove(edge);
            edge.Clear();
        }

        public abstract void Initialize();

        public abstract List<Vertex>? ShortestPath(Vertex source, Vertex destination);
    }
}
