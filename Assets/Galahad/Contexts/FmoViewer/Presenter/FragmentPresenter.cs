using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class FragmentPresenter:MonoBehaviour
    {
        [SerializeField]private FragmentAtom model;

        public FragmentAtom Model => model;

        public FragmentPresenter Inject(FragmentAtom model)
        {
            //自分を返す
            this.model = model;
            gameObject.name = this.model.ResidueName;
            return this;
        }



    }
}