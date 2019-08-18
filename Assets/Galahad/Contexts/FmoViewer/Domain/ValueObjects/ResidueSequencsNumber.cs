namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class ResidueSequencsNumber
    {
        
        public ResidueSequencsNumber(int i)
        {
            Value = i;
        }

        public ResidueSequencsNumber():this(-1){}

        public int Value { get; }
        public override string ToString()
        {
            return Value.ToString();
        }
    }
}