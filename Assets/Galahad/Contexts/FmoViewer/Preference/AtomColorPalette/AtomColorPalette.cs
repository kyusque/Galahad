using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;
using Zenject;

namespace Galahad.Contexts.FmoViewer.Preference
{
    [CreateAssetMenu(fileName = "AtomColorPalette", menuName = "Installers/Galahad/FmoViewer/AtomColorPallete")]
    public partial class AtomColorPalette : ScriptableObjectInstaller<AtomColorPalette>
    {
        [SerializeField] private List<Material> materials;
        private Dictionary<ElementSymbol, Material> _atomMaterirals = new Dictionary<ElementSymbol, Material>();
        
    }
    
#if UNITY_WSA
    public partial class GldAtomColorPalette : ScriptableObject{}
#else
    public partial class AtomColorPalette
    {
        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
    
#endif
    public partial class AtomColorPalette:IAtomColorPalette
    {
        Dictionary<ElementSymbol, Material> IAtomColorPalette.AtomMaterials
        {
            get { return _atomMaterirals; }
        }
    }
    
    partial class AtomColorPalette : ISerializationCallbackReceiver
    {
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            materials = _atomMaterirals.Values.ToList();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _atomMaterirals = new Dictionary<ElementSymbol, Material>();
            var i = 0;
            foreach (var atomType in Enum.GetValues(typeof(ElementSymbol)).Cast<ElementSymbol>())
            {
                _atomMaterirals.Add(atomType, materials[i]);
                i++;
            }
        }
    }
}