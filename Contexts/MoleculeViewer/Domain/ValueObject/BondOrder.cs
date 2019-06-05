namespace Galahad.Contexts.MoleculeViewer.Domain.ValueObject
{
    public class BondOrder
    {
        public BondOrder(int bondOrder)
        {
            Value = bondOrder;
        }

        public int Value { get; }

        public static BondOrder GenerateInstance(int bondOrder)
        {
            return new BondOrder(bondOrder);
        }

        public BondOrder Increase(int i)
        {
            return new BondOrder(Value + i);
        }

        public BondOrder Decrease(int i)
        {
            return new BondOrder(Value - i);
        }
    }
}