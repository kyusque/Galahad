using UnityEngine;

namespace Galahad.Contexts.MoleculeViewer.Domain.ValueObject
{
    public class Position
    {
        public Vector3 Value;
        public Position(Vector3 position)
        {
            Value = position;
        }
    }
}