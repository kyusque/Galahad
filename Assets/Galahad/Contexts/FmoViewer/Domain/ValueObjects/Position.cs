using UniRx;
using UnityEngine;


namespace Galahad.Contexts.FmoViewer.Domain.ValueObjects
{
    public class Position : Vector3ReactiveProperty
    {
        public Position(Vector3 position)
        {
            Value = position;
        }
    }
}