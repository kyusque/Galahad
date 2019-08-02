namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class TemperatureFactor
    {
        public TemperatureFactor(float temperatureFactor)
        {
            Value = temperatureFactor;
        }
        public float Value { get; }
     
    }
}