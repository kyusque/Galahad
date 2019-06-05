using System;
using System.Collections.Generic;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Domain.MoleculeAggregate
{
    [Serializable]
    public class Molecule : ISerializationCallbackReceiver
    {
        [SerializeField] private List<Atom> atoms;
        [SerializeField] private List<Bond> bonds;
        [SerializeField] private string identifier;
        [SerializeField] private Vector3 offsetPosition;
        [SerializeField] private string title;

        public Molecule(Atoms atoms, Bonds bonds, Position offsetPosition, string title)
        {
            Atoms = atoms;
            Bonds = bonds;
            Identifier = Guid.NewGuid().ToString();
            OffsetPosition = offsetPosition;
            Title = title;
        }

        public Position OffsetPosition { get; private set; }

        public string Identifier { get; private set; }
        public string Title { get; private set; }
        public Atoms Atoms { get; private set; }
        public Bonds Bonds { get; private set; }

        public void OnBeforeSerialize()
        {
            identifier = Identifier;
            title = Title;
            atoms = Atoms?.ToList();
            bonds = Bonds?.ToList();
            offsetPosition = OffsetPosition?.Value ?? Vector3.zero;
        }

        public void OnAfterDeserialize()
        {
            Identifier = identifier ?? Guid.NewGuid().ToString();
            Title = title;
            Atoms = new Atoms(atoms);
            Bonds = new Bonds(bonds);
            OffsetPosition = new Position(offsetPosition);
        }
    }
}