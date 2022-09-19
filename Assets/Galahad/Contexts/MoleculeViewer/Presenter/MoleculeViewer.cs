using System.Collections.Generic;
using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;
using Galahad.Scripts;
using UniRx;
using UnityEngine;

namespace Galahad.Contexts.MoleculeViewer.Presenter
{
    public class MoleculeViewer : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField] private MoleculeRepository _moleculeRepository;
        [SerializeField] public AtomColorPalette atomColorPalette;
        [SerializeField] public AtomPresenter atomPrefab;
        [SerializeField] public BondPresenter bondPrefab;
        [SerializeField] public MoleculePresenter moleculePrefab;
        [SerializeField] private List<Molecule> molecules;
        public int n;


        public void OnBeforeSerialize()
        {
            molecules = _moleculeRepository?.ToList();
        }

        public void OnAfterDeserialize()
        {
            _moleculeRepository?.SetMolecules(molecules);
        }

        private void Start()
        {
            molecules = _moleculeRepository.ToList();
            MoleculePresenter moleculePresenter = null;
            this.ObserveEveryValueChanged(_ => n)
                .Subscribe(x =>
                {
                    if (moleculePresenter != null)
                        Destroy(moleculePresenter);

                    var mol = _moleculeRepository.GetMolecule(x);
                    moleculePresenter = Instantiate(moleculePrefab, transform);
                    moleculePresenter.molecule = mol;
                    moleculePresenter.atomPrefab = atomPrefab;
                    moleculePresenter.bondPrefab = bondPrefab;
                    moleculePresenter.atomColorPalette = atomColorPalette;
                    moleculePresenter.Init();
                }).AddTo(this);
        }
    }
}