using System;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using Galahad.Contexts.FmoViewer.Event;
using Galahad.Contexts.FmoViewer.Preference;
using Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference;
using UniRx;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class AtomPresenter:MonoBehaviour
    {
        [SerializeField] private Atom model;
        [SerializeField] private Events events=Events.None;
        public Atom Model => model;

        public Events Events
        {
            get => events;
            set => events = value;
        }

        public AtomPresenter Inject(Atom atom)
        {
            this.model = atom as Atom;
            this.gameObject.transform.position = model.Position.Value;
            GameObject o;
            (o = this.gameObject).name = atom.AtomName.ToString() + atom.AlternateLocationIndicator;
<<<<<<< HEAD
//            o.tag = "Atom";
=======
            o.tag = "Atom";
>>>>>>> 4d93b47f06fd7afaff7b64001b84bc5b207dd74b
            return this;
        }
        
        public AtomPresenter Inject(IFmoMeshPreference mesh)
        {
            var meshFilter = gameObject.AddComponent<MeshFilter>();
            meshFilter.mesh = mesh.Mesh;
            switch ((model).ElementSymbol)
            {
                case ElementSymbol.H:
                    transform.localScale = new Vector3(0.5f,0.5f,0.5f);
                    break;
                case ElementSymbol.C:
                    break;
                case ElementSymbol.N:
                    break;
                case ElementSymbol.O:
                    break;
                case ElementSymbol.S:
                    break;
                case ElementSymbol.Cl:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return this;
        }
        
        public AtomPresenter Inject(IAtomColorPalette palette,IEventAtomColorPalette eventAtomColorPalette)
        {
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            this.ObserveEveryValueChanged(x => events)
                .Where(x=>x==Events.None)
                .Subscribe(_ =>
                {
                    meshRenderer.material = palette.AtomMaterials[model.ElementSymbol];
                })
                .AddTo(this);
            this.ObserveEveryValueChanged(x => events)
                .Where(x=>x!=Events.None)
                .Subscribe(x =>
                {
                    meshRenderer.material =  eventAtomColorPalette.EventMaterials[x];
                })
                .AddTo(this);
            
            return this;
        }


    }
}