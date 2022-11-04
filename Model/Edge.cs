namespace Graph.Model
{
    public class Edge
    {
        /// <summary>
        /// The vertices on either end of the edge.
        /// </summary>
        public HashSet<Vertex> Vertices { get; }

        /// <summary>
        /// The weight of the edge, i.e., the cost it takes to traverse.
        /// </summary>
        public int Weight { get; set; }

        /// <summary>
        /// An edge is the "line" that connects two vertices in a graph.
        /// </summary>
        /// <param name="left">One vertex.</param>
        /// <param name="right">The other vertex.</param>
        public Edge(Vertex left, Vertex right)
        {
            Vertices = new HashSet<Vertex> {left, right};
            Weight = 1;
            left.Edges.Add(this);
            right.Edges.Add(this);
        }

        /// <summary>
        /// Determines whether the edge connects the specified vertex to another.
        /// </summary>
        /// <param name="vertex">The vertex to search for.</param>
        /// <returns>True if the edge connects the specified vertex, false if otherwise.</returns>
        public bool HasVertex(Vertex vertex)
        {
            return Vertices.Contains(vertex);
        }

        /// <summary>
        /// Clears this edge's vertices.
        /// </summary>
        public void Clear()
        {
            foreach (var vertex in Vertices.Where(x => x.Edges.Contains(this)))
            {
                vertex.Edges.Remove(this);
            }

            Vertices.Clear();
        }
    }
}
