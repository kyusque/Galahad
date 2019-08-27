using Galahad.Contexts.FmoViewer.Domain;
using Galahad.Contexts.FmoViewer.Preference;
using Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference;
using Galahad.Contexts.FmoViewer.Preference.GldBondColor;
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
        

        private void Start()
        {
            Init(FragmentationRepository);
        }

    }
}