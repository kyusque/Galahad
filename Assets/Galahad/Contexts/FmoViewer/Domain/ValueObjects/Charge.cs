using System;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;

namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class Charge
    {
        public Charge(int magnitude)
        {
            Value = magnitude;
        }

        public int Value { get; }

        public static Charge GenerateInstance(int magnitude)
        {
            return new Charge(magnitude);
        }

        public override string ToString()
        {
            if (Value > 0) return Value + "+";
            if (Value == 0) return "";
            return Math.Abs(Value) + "-";
        }

        public static Charge operator +(Charge a, Charge b)
        {
            return new Charge(a.Value + b.Value);
        }

        public static Charge operator +(Charge a, int b)
        {
            return new Charge(a.Value + b);
        }

        public static Charge operator -(Charge a, int b)
        {
            return new Charge(a.Value - b);
        }

        public Charge Reduce(int magnitude = 1)
        {
            return this - magnitude;
        }

        public Charge Oxidize(int magnitude = 1)
        {
            return this + magnitude;
        }
    }
}