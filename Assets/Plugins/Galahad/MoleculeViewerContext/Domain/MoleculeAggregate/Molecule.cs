using System;
using System.Collections.Generic;
using Galahad.MoleculeViewerContext.Domain.ValueObject;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Domain.MoleculeAggregate
{
    [Serializable]
    public class Molecule: ISerializationCallbackReceiver
    {
        [SerializeField] private List<Atom> atoms;
        [SerializeField] private List<Bond> bonds;        
        
        private Atoms _atoms;
        private Bonds _bonds;

        public Molecule(Atoms atoms, Bonds bonds)
        {
            _atoms = atoms;
            _bonds = bonds;
        }

        public Atoms Atoms => _atoms;

        public Bonds Bonds => _bonds;


        public void OnBeforeSerialize()
        {
            atoms = _atoms?.ToList();
            bonds = _bonds?.ToList();
        }

        public void OnAfterDeserialize()
        {
            _atoms = new Atoms(atoms);
            _bonds = new Bonds(bonds);
        }
    }
}
