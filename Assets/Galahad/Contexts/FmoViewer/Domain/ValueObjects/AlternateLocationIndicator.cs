namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class AlternateLocationIndicator
    {
        public AlternateLocationIndicator(int locationIndicator)
        {
            Value = locationIndicator;
        }
        public int Value { get; }
        public override string ToString()
        {
            return Value==0 ? "" : Value.ToString();
        }
    }
}