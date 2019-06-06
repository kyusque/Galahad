using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.MoleculeViewerContext.Domain.ValueObject;
using UnityEngine;
using Zenject;

namespace Galahad.MoleculeViewerContext.View
{
    [CreateAssetMenu]
    public partial class AtomColorPalette : ISerializationCallbackReceiver
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

#if UNITY_WSA
    public partial class AtomColorPalette : ScriptableObject{}
#else
    public partial class AtomColorPalette : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }

#endif
}