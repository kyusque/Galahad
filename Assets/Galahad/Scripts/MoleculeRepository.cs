using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.MoleculeViewer.Domain;
using Galahad.MoleculeViewerContext.Domain;
using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;
using UnityEngine;
using Zenject;

namespace Galahad.Scripts
{
    [CreateAssetMenu]
    public class MoleculeRepository : ScriptableObjectInstaller, IMoleculeRepository
    {
        private List<Molecule> _runtimeMolecules;
        [SerializeField] private List<Molecule> molecules;

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

        public override void InstallBindings()
        {
            Container.BindInstance(this);
        }
    }
}