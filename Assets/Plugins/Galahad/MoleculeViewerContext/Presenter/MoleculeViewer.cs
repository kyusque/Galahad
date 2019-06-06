using System.Collections.Generic;
using Galahad.Infrastructure;
using Galahad.MoleculeViewerContext.Domain;
using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;
using Galahad.MoleculeViewerContext.View;
using UniRx;
using UnityEngine;
using Zenject;

namespace Galahad.MoleculeViewerContext.Presenter
{
    public class MoleculeViewer : MonoBehaviour, ISerializationCallbackReceiver
    {
        private IMoleculeRepository _moleculeRepository;
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

        [Inject]
        public void Inject(MoleculeRepository moleculeRepository)
        {
            _moleculeRepository = moleculeRepository;
            molecules = _moleculeRepository.ToList();
        }

        public void Inject(AtomPresenter atomPrefab)
        {
            this.atomPrefab = atomPrefab;
        }

        public void Inject(MoleculePresenter moleculePrefab)
        {
            this.moleculePrefab = moleculePrefab;
        }

        public void Inject(BondPresenter bondPrefab)
        {
            this.bondPrefab = bondPrefab;
        }
    }
}