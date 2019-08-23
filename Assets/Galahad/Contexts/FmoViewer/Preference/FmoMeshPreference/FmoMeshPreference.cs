using UnityEngine;
using Zenject;

namespace Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference
{
    [CreateAssetMenu(menuName="Installers/Galahad/FmoViewer/FmoMeshPrefernce")]
    public partial class FmoMeshPreference
    {
        [SerializeField]private Mesh mesh;
    }
    
#if UNITY_WSA
    public partial class FmoAtomMeshPreference : ScriptableObject {}
#else
    public partial class FmoMeshPreference : ScriptableObjectInstaller
    {
        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
    
#endif
    
    partial class FmoMeshPreference: IFmoMeshPreference
    {
        Mesh IFmoMeshPreference.Mesh
        {
            get { return mesh; }
        }

    }

}