namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class AtomId
    {
        public AtomId(int id)
        {
            Value = id;
        }
        public int Value { get; }
    }
}