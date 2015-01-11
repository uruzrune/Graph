namespace Graph
{
    public interface IVertexValue
    {
        /// <summary>
        /// Determines whether the target value's vertex can be entered from the designated source value's vertex.
        /// </summary>
        /// <param name="source">The source vertex where the movement is originating from.</param>
        /// <returns>True if this value can be entered from the source value, false if not.</returns>
        bool IsEnterableFrom(IVertexValue source);

        /// <summary>
        /// Provides a cost modifier for traversing the edge from the designated source value's vertex to this value's vertex.
        /// </summary>
        /// <param name="source">The source vertex where the movement is originating from.</param>
        /// <returns>A modifier to be multiplied against the edge weight between the source value's vertex and this value's vertex.</returns>
        double EnteringCostModifier(IVertexValue source);
    }
}
