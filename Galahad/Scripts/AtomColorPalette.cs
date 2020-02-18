using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;
using UnityEngine;

namespace Galahad.Scripts
{
    [CreateAssetMenu]
    public  class AtomColorPalette : ISerializationCallbackReceiver
    {
        [SerializeField] private List<Material> materials;
        [SerializeField] private List<string> names;

        public Dictionary<AtomicNumber, Material> Dictionary { get; private set; } =
            new Dictionary<AtomicNumber, Material>();

        public void OnBeforeSerialize()
        {
            names = Dictionary.Keys.Select(x => x.ToString()).ToList();
            materials = Dictionary.Values.ToList();
        }

        public void OnAfterDeserialize()
        {
            Dictionary = new Dictionary<AtomicNumber, Material>();
            var i = 0;
            foreach (var atomType in Enum.GetValues(typeof(AtomicNumber)).Cast<AtomicNumber>())
            {
                Dictionary.Add(atomType, materials[i]);
                i++;
            }
        }
    }


}