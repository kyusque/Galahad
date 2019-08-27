using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Event;
using Galahad.Contexts.FmoViewer.Preference;
using Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference;
using UniRx;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class FragmentPresenter:MonoBehaviour
    {
        [SerializeField]private FragmentAtom model;
        [SerializeField] private Events events = Events.None;

        public FragmentAtom Model => model;

        public FragmentPresenter Inject(FragmentAtom model)
        {
            //自分を返す
            this.model = model;
            var o = gameObject;
            o.name = this.model.ResidueName;
<<<<<<< HEAD
//            o.tag = "Fragment";
=======
            o.tag = "Fragment";
>>>>>>> 4d93b47f06fd7afaff7b64001b84bc5b207dd74b
            this.ObserveEveryValueChanged(x => x.events)
                .Where(x => x == Events.None)
                .Subscribe(_ =>
                {
                    var atompresenter = GetComponentsInChildren<AtomPresenter>();
                    foreach (var atomPresenter in atompresenter)
                    {
                        atomPresenter.Events = Events.None;
                    }
                })
                .AddTo(this);
            this.ObserveEveryValueChanged(x => x.events)
                .Where(x => x == Events.FragmentSelect)
                .Subscribe(_ =>
                {
                    var atompresenter = GetComponentsInChildren<AtomPresenter>();
                    foreach (var atomPresenter in atompresenter)
                    {
                        atomPresenter.Events = Events.FragmentSelect;
                    }
                })
                .AddTo(this);
            return this;
        }



    }
}