using System;

namespace Galahad.MoleculeViewerContext.Domain.ValueObject
{
    public class FormalCharge
    {
        public FormalCharge(int magnitude)
        {
            Value = magnitude;
        }

        public int Value { get; }

        public static FormalCharge GenerateInstance(int magnitude)
        {
            return new FormalCharge(magnitude);
        }

        public override string ToString()
        {
            if (Value > 0) return Value + "+";
            if (Value == 0) return "";
            return Math.Abs(Value) + "-";
        }

        public static FormalCharge operator +(FormalCharge a, FormalCharge b)
        {
            return new FormalCharge(a.Value + b.Value);
        }

        public static FormalCharge operator +(FormalCharge a, int b)
        {
            return new FormalCharge(a.Value + b);
        }

        public static FormalCharge operator -(FormalCharge a, int b)
        {
            return new FormalCharge(a.Value - b);
        }

        public FormalCharge Reduce(int magnitude = 1)
        {
            return this - magnitude;
        }

        public FormalCharge Oxidize(int magnitude = 1)
        {
            return this + magnitude;
        }
    }
}