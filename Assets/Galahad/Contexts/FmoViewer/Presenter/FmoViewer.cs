using Galahad.Contexts.FmoViewer.Domain;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Preference;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class FmoViewer:MonoBehaviour
    {
        [SerializeField] private FragmentationRepository FragmentationRepository;
        [SerializeField] private AtomColorPalette _atomColorPalette;

        private void Init(FragmentationRepository fragmentationRepository)
        {
            fragmentationRepository.Fragment.FragmentAtoms.ToList().ForEach(x =>
            {
                var fragmentPresenter=new FragmentPresenter().Inject(x).Create(this.transform);
                fragmentPresenter.Model.Atoms.ToList().ForEach(atom =>
                {
                 var atompresenter=new AtomPresenter().Inject(atom).Create(fragmentPresenter.transform);
                });
            });
        }

        private void Start()
        {
            Init(FragmentationRepository);
        }
    }
}