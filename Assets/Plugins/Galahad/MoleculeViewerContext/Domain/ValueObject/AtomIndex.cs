namespace Galahad.MoleculeViewerContext.Domain.ValueObject
{
    public class AtomIndex
    {
        private int _index;
        public AtomIndex(int i)
        {
            _index = i;
        }

        public int Value => _index;
    }
}