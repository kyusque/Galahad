namespace Galahad.Contexts.MoleculeViewer.Domain.ValueObject
{
    public class AtomIndex
    {
        public AtomIndex(int i)
        {
            Value = i;
        }

        public int Value { get; }
    }
}