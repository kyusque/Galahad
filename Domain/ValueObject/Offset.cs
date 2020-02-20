using System;
using UnityEngine;

namespace Galahad.Domain.ValueObject
{
    [Serializable]
    public class Offset
    {
        public Vector3 position;
        public Vector3 rotation;
    }
}