using System;
using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;
using Galahad.Scripts;
using UnityEngine;

namespace Galahad.Contexts.MoleculeViewer.Presenter
{
    public class MoleculePresenter : MonoBehaviour
    {
        [SerializeField] public AtomColorPalette atomColorPalette;
        [SerializeField] public AtomPresenter atomPrefab;
        [SerializeField] public BondPresenter bondPrefab;
        [SerializeField] public Molecule molecule;

        private void OnDestroy()
        {
            Destroy(gameObject);
        }

        public void Init()
        {
            transform.localPosition = molecule.OffsetPosition.Value;

            molecule.Atoms.ToList().ForEach(x =>
            {
                var atomPresenter = Instantiate(atomPrefab, x.Position.Value, Quaternion.identity, transform);
                atomPresenter.model = x;
                atomPresenter.GetComponent<MeshRenderer>().material = atomColorPalette.Dictionary[x.AtomicNumber];
                if (x.AtomicNumber == AtomicNumber.H)
                    atomPresenter.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                atomPresenter.Init();
            });

            molecule.Bonds.ToList().ForEach(x =>
            {
                var bond = Instantiate(bondPrefab, transform);
                bond.model = x;
                bond.BeginAtom = molecule.Atoms[x.BeginAtomIndex];
                bond.EndAtom = molecule.Atoms[x.EndAtomIndex];
                bond.Init();
            });
        }

        private void Update()
        {
            if (transform.localPosition != molecule.OffsetPosition.Value)
                transform.localPosition = molecule.OffsetPosition.Value;
        }
    }
}