using System;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class AtomPresenter:MonoBehaviour
    {
        [SerializeField] private Atom model;
        public Atom Model => model;

        public AtomPresenter Inject(Atom atom)
        {
            this.model = atom;
            return this;
        }

        public AtomPresenter Create(Transform parent)
        {
            var gameobject=GameObject.CreatePrimitive(PrimitiveType.Sphere);
            gameobject.name = model.AtomName.ToString();
            gameobject.transform.position = model.Position.Value;
            gameobject.transform.parent = parent;
            switch (model.ElementSymbol)
            {
                case ElementSymbol.H:
                    break;
                case ElementSymbol.He:
                    break;
                case ElementSymbol.Li:
                    break;
                case ElementSymbol.Be:
                    break;
                case ElementSymbol.B:
                    break;
                case ElementSymbol.C:
                    break;
                case ElementSymbol.N:
                    break;
                case ElementSymbol.O:
                    break;
                case ElementSymbol.F:
                    break;
                case ElementSymbol.Ne:
                    break;
                case ElementSymbol.Na:
                    break;
                case ElementSymbol.Mg:
                    break;
                case ElementSymbol.Al:
                    break;
                case ElementSymbol.Si:
                    break;
                case ElementSymbol.P:
                    break;
                case ElementSymbol.S:
                    break;
                case ElementSymbol.Cl:
                    break;
                case ElementSymbol.Ar:
                    break;
                case ElementSymbol.K:
                    break;
                case ElementSymbol.Ca:
                    break;
                case ElementSymbol.Sc:
                    break;
                case ElementSymbol.Ti:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            var presenter = gameobject.AddComponent<AtomPresenter>().Inject(model);
            return presenter;
        }
    }
}