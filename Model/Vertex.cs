namespace Graph.Model
{
    /// <summary>
    /// A vertex.
    /// </summary>
    public class Vertex
    {
        /// <summary>
        /// The set of edges leading to adjacent vertices.
        /// </summary>
        public HashSet<Edge> Edges { get; }

        /// <summary>
        /// An enumerable of neighbors to this vertex.
        /// </summary>
        public IEnumerable<Vertex> Neighbors
        {
            get
            {
                return Edges.SelectMany(x => x.Vertices).Where(x => x != this);
            }
        }

        /// <summary>
        /// The graph that this vertex belongs to.
        /// </summary>
        public AbstractGraph Graph { get; internal set; }

        /// <summary>
        /// The value that corresponds with the vertex.
        /// </summary>
        public IVertexValue Value { get; set; }

        /// <summary>
        /// A vertex of a graph.
        /// </summary>
        public Vertex(AbstractGraph graph, IVertexValue value)
        {
            Edges = new HashSet<Edge>();
            Graph = graph;
            Value = value;
        }

        /// <summary>
        /// Does the vertex have a neighbor that connects the two with the specified edge?
        /// </summary>
        /// <param name="edge">The edge in question.</param>
        /// <returns>True if the edge connects this vertex and a neighbor, false otherwise.</returns>
        public bool HasEdge(Edge edge)
        {
            return Edges.Contains(edge);
        }

        /// <summary>
        /// Does the vertex have the specified neighbor?
        /// </summary>
        /// <param name="vertex">The vertex neighbor in question.</param>
        /// <returns>True if the vertex is a neighbor, false otherwise.</returns>
        public bool HasNeighbor(Vertex vertex)
        {
            return Edges.Any(x => x.Vertices.Contains(this) && x.Vertices.Contains(vertex));
        }

        /// <summary>
        /// Connect this vertex to another non-neighbor vertex.
        /// </summary>
        /// <param name="newNeighbor">The vertex to become this vertex's new neighbor.</param>
        /// <returns>The connecting edge.</returns>
        public Edge Connect(Vertex newNeighbor)
        {
            if (HasNeighbor(newNeighbor))
            {
                throw new InvalidOperationException("already neighbors");
            }

            var edge = new Edge(this, newNeighbor);
            Edges.Add(edge);
            Graph.Edges.Add(edge);

            return edge;
        }

        /// <summary>
        /// The number of edges that this vertex has connecting to adjacent vertices.
        /// </summary>
        /// <returns>The number of edges that this vertex has connecting to adjacent vertices.</returns>
        public int Degrees()
        {
            return Edges.Count;
        }

        /// <summary>
        /// Determines whether the vertex has no neighbors (is isolated).
        /// </summary>
        /// <returns>True if the vertex is isolated, false if it has at least one neighbor.</returns>
        public bool IsIsolated()
        {
            return Edges.Count == 0;
        }

        /// <summary>
        /// Determines whether the vertex is a leaf (has only one neighbor).
        /// </summary>
        /// <returns>True if the vertex is a leaf, false if otherwise.</returns>
        public bool IsLeaf()
        {
            return Degrees() == 1;
        }

        /// <summary>
        /// Disconnects this vertex from all other vertices.
        /// </summary>
        public void Disconnect()
        {
            if (IsIsolated())
            {
                throw new InvalidOperationException("vertex is already disconnected");
            }

            var edges = new List<Edge>(Edges);
            edges.ForEach(x => x.Clear());
            Edges.Clear();
            edges.ForEach(x => Edges.Remove(x));
        }
    }
}
