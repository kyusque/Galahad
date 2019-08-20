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
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}