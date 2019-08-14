namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class FragmentId
    {
        public FragmentId(int id)
        {
            Value = id;
        }

        public FragmentId()
        {
            
        }

        public int Value { get; }
    }
}