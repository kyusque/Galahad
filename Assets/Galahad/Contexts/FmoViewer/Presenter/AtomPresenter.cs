using System;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using Galahad.Contexts.FmoViewer.Preference;
using Galahad.Contexts.FmoViewer.Preference.FmoMeshPreference;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Presenter
{
    public class AtomPresenter:MonoBehaviour
    {
        [SerializeField] private Atom model;
        public Atom Model => model;

        public AtomPresenter Inject(Atom atom)
        {
            this.model = atom as Atom;
            gameObject.transform.position = model.Position.Value;
            gameObject.name = atom.AtomName.ToString() + atom.AlternateLocationIndicator;
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
        
        public AtomPresenter Inject(IAtomColorPalette palette)
        {
            var meshRenderer = gameObject.AddComponent<MeshRenderer>();
            meshRenderer.material = palette.AtomMaterials[(model).ElementSymbol];
            return this;
        }

    }
}