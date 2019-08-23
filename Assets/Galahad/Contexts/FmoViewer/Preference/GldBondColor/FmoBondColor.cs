using UnityEngine;
using Zenject;

namespace Galahad.Contexts.FmoViewer.Preference.GldBondColor
{
    [CreateAssetMenu(menuName = "Installers/Galahad/FmoViewer/GldBond")]
    public partial class FmoBondColor
    {
        [SerializeField] private Material material;
    }
    
#if UNITY_WSA
    partial class FMOBondColor : ScriptableObject{}
#else
    partial class FmoBondColor : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
    
#endif
    public partial class FmoBondColor:IFmoBondColor
    {
        Material IFmoBondColor.Material
        {
            get { return material; }
        }
    }
}