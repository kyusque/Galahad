namespace Galahad.Contexts.MoleculeViewer.Domain.ValueObject
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