using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Event;
using UnityEngine;
using Zenject;

namespace Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference
{
    [CreateAssetMenu(fileName = "EventAtomColorPalette", menuName = "Installers/EventAtomColorPalette")]
    public partial class EventAtomColorPalette : ScriptableObjectInstaller<EventAtomColorPalette>
    {
        [SerializeField] private List<Material> materials;
        private Dictionary<Events, Material> _atomMaterirals = new Dictionary<Events, Material>();
    }
    
#if UNITY_WSA
    public partial class EventAtomColorPalette : ScriptableObject{}
#else
    public partial class EventAtomColorPalette
    {
        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
    
#endif
    public partial class EventAtomColorPalette:IEventAtomColorPalette
    {
        Dictionary<Events, Material> IEventAtomColorPalette.EventMaterials
        {
            get { return _atomMaterirals; }
        }
    }
    
    partial class EventAtomColorPalette : ISerializationCallbackReceiver
    {
        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            materials = _atomMaterirals.Values.ToList();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            _atomMaterirals = new Dictionary<Events, Material>();
            var i = 0;
            foreach (var atomType in Enum.GetValues(typeof(Events)).Cast<Events>())
            {
                _atomMaterirals.Add(atomType, materials[i]);
                i++;
            }
        }
    }
}