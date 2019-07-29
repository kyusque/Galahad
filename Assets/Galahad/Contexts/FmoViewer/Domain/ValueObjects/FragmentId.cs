namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class FragmentId
    {
        public FragmentId(int id)
        {
            Value = id;
        }
        public int Value { get; }
    }
}