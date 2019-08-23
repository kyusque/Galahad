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
            return this;
        }

        public FragmentPresenter Create(Transform parent)
        {
            var gameObject = new GameObject()
                {name = model.ResidueName, transform = {parent = parent, localPosition = parent.transform.position}};
            var presenter = gameObject.AddComponent<FragmentPresenter>();
            presenter.Inject(model);
            return presenter;
        }


    }
}