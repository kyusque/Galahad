namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class AtomSerialNumber
    {
        public AtomSerialNumber(int i)
        {
            Value = i;
        }

        public int Value { get; }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
    
}