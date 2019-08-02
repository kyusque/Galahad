namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class ChainId
    {
        public ChainId(string id)
        {
            Value = id;
        }
        public string Value { get; }
    }
}