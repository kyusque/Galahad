using UniRx;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Domain.ValueObject
{
    public class Position: Vector3ReactiveProperty
    {
        public Position(Vector3 position)
        {
            Value = position;
        }
    }
}