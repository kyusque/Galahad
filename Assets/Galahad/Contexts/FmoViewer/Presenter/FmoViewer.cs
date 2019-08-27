using Galahad.Contexts.FmoViewer.Domain;
using Galahad.Contexts.FmoViewer.Preference;
using Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference;
using Galahad.Contexts.FmoViewer.Preference.GldBondColor;
using UnityEditor;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class FmoViewer:MonoBehaviour
    {
        [SerializeField] private FragmentationRepository FragmentationRepository;
        [SerializeField] private AtomColorPalette atomColorPalette;
        [SerializeField] private EventAtomColorPalette eventAtomColorPalette;
        [SerializeField] private FmoBondColor fmoBondColor;
        [SerializeField] private FmoMeshPreference fmoMeshPreference;

        private void Init(FragmentationRepository fragmentationRepository)
        {
            fragmentationRepository.Fragment.FragmentAtoms.ToList().ForEach(x =>
            {
                var fragment = CreateGameObject(this.transform);
                var fragmentPresenter = fragment.AddComponent<FragmentPresenter>().Inject(x);
                fragmentPresenter.Model.Atoms.ToList().ForEach(atom =>
                {
                    var atomObject = CreateGameObject(fragmentPresenter.transform)
                        .AddComponent<AtomPresenter>()
                        .Inject(atom)
                        .Inject(atomColorPalette,eventAtomColorPalette)          
                        .Inject(fmoMeshPreference);

                });
                
            });
        }

        private GameObject CreateGameObject(Transform parent)
        {
            var gameObject = new GameObject{name = ToString(),transform = {parent = parent, localPosition = parent.position}};
            return gameObject;
        }

        public static void AddTag(string tagname)
        {
            UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
            if ((asset != null) && (asset.Length > 0))
            {
                SerializedObject so = new SerializedObject(asset[0]);
                SerializedProperty tags = so.FindProperty("tags");

                for (int i = 0; i < tags.arraySize; ++i)
                {
                    if (tags.GetArrayElementAtIndex(i).stringValue == tagname)
                    {
                        return;
                    }
                }

                int index = tags.arraySize;
                tags.InsertArrayElementAtIndex(index);
                tags.GetArrayElementAtIndex(index).stringValue = tagname;
                so.ApplyModifiedProperties();
                so.Update();
            }
        }


        private void Start()
        {
            Init(FragmentationRepository);
        }

    }
}