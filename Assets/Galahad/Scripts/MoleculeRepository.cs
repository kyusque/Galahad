using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.MoleculeViewer.Domain;
using Galahad.Contexts.MoleculeViewer.Domain.MoleculeAggregate;
using Galahad.Contexts.MoleculeViewer.Domain.ValueObject;
using UnityEngine;


namespace Galahad.Scripts
{
    [CreateAssetMenu]
    public class MoleculeRepository : ScriptableObject, IMoleculeRepository
    {
        private List<Molecule> _runtimeMolecules;
        [SerializeField] private List<Molecule> molecules = new();

        public string runtimeJson;

        public List<Molecule> ToList()
        {
            return molecules;
        }

        public string GetMoleculeJson(string identifier)
        {
            return JsonUtility.ToJson(GetMolecule(identifier), true);
        }

        public string GetMoleculeJson(int index)
        {
            return JsonUtility.ToJson(GetMolecule(index), true);
        }

        public Molecule GetMolecule(string identifier)
        {
            var molecule = _runtimeMolecules.FirstOrDefault(x => x.Identifier == identifier);
            if (molecule != null) return molecule;
            molecule = molecules.FirstOrDefault(x => x.Identifier == identifier);
            if (molecule != null) molecule = JsonUtility.FromJson<Molecule>(JsonUtility.ToJson(molecule));
            return molecule;
        }

        public void SetMolecules(List<Molecule> molecules)
        {
            this.molecules = molecules;
        }

        public Molecule GetMolecule(int index)
        {
            var molecule = molecules[index];
            molecule = JsonUtility.FromJson<Molecule>(JsonUtility.ToJson(molecule));
            _runtimeMolecules.Add(molecule);
            return molecule;
        }

        private void OnEnable()
        {
            _runtimeMolecules = new List<Molecule>();
        }

        public void AddMolecule(Molecule molecule)
        {
            molecules.Add(molecule);
        }

        public void AddMoleculeFromSmiles(string smiles)
        {
            var maxNumAtoms = 20;
            var exactNumAtoms = new int[] {-1};
            var numBonds = new int[] {0};
            var atomicNums = new int[maxNumAtoms];
            var atomCharges = new int[maxNumAtoms];
            var positons = new double[3 * maxNumAtoms];
            var bondConnections = new int[maxNumAtoms * maxNumAtoms];
            var bondOrders = new double[maxNumAtoms * maxNumAtoms];
            if (RDKitWrapper.OptFromSmiles(smiles, atomicNums, atomCharges, positons, bondConnections, bondOrders,
                    exactNumAtoms, numBonds) < 0) return;
            var atoms = new List<Atom>();
            for (var i = 0; i < exactNumAtoms[0]; i++)
            {
                var atom = new Atom(
                    new AtomIndex(i),
                    (AtomicNumber) Enum.ToObject(typeof(AtomicNumber), atomicNums[i]),
                    new Position(new Vector3((float) positons[3 * i + 0], (float) positons[3 * i + 1],
                        (float) positons[3 * i + 2])),
                    new FormalCharge(atomCharges[i])
                );
                atoms.Add(atom);
            }

            var bonds = new List<Bond>();
            for (var i = 0; i < numBonds[0]; i++)
            {
                var bond = new Bond(new AtomIndex(bondConnections[2 * i + 0]),
                    new AtomIndex(bondConnections[2 * i + 1]), new BondOrder((int) bondOrders[i]));
                bonds.Add(bond);
            }

            molecules.Add(new Molecule(new Atoms(atoms), new Bonds(bonds), new Position(Vector3.zero), ""));
        }


        public void UpdateMoleculeFromJson(string json)
        {
            if (molecules == null)
                molecules.Append(JsonUtility.FromJson<Molecule>(json));
            else
                JsonUtility.FromJsonOverwrite(json, molecules[0]);
        }

        public void UpdateRuntimeMoleculeFromJson(string json)
        {
            if (_runtimeMolecules == null)
                _runtimeMolecules.Append(JsonUtility.FromJson<Molecule>(json));
            else
                JsonUtility.FromJsonOverwrite(json, _runtimeMolecules[0]);
        }
    }
}