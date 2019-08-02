namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class Occupancy
    {
        public Occupancy(float occupancy)
        {
            Value = occupancy;
        }
        public float Value { get; }
    }
}