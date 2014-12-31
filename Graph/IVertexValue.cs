namespace Graph
{
    public interface IVertexValue
    {
        bool IsEnterableFrom(IVertexValue source);
    }
}
