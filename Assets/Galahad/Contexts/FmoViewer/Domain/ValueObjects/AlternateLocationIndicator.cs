namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class AlternateLocationIndicator
    {
        public AlternateLocationIndicator(int locationIndicator)
        {
            Value = locationIndicator;
        }
        public int Value { get; }
    }
}