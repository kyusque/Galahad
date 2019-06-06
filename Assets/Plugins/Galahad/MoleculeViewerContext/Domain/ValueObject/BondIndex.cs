namespace Galahad.MoleculeViewerContext.Domain.ValueObject
{
    public class BondIndex
    {
        public BondIndex(int index)
        {
            Value = index;
        }

        public int Value { get; }
    }
}