using UniRx;
using UnityEngine;

namespace Galahad.Contexts.MoleculeViewer.Domain.ValueObject
{
    public class Position : Vector3ReactiveProperty
    {
        public Position(Vector3 position)
        {
            Value = position;
        }
    }
}