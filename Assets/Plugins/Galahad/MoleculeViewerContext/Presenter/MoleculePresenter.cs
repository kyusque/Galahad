using Galahad.MoleculeViewerContext.Domain.MoleculeAggregate;
using UnityEngine;

namespace Galahad.MoleculeViewerContext.Presenter
{
    public class MoleculePresenter : MonoBehaviour
    {
        [SerializeField] private Molecule molecule;
        [SerializeField] private GameObject atomPrefab;
        [SerializeField] private GameObject bondPrefab;


        private void Start()
        {
            molecule.Atoms.ToList().ForEach(x =>
            {
                var atom = Instantiate(atomPrefab, x.Position.Value, Quaternion.identity, transform);
                atom.GetComponent<AtomPresenter>().Atom = x;
            });
            
            molecule.Bonds.ToList().ForEach(x =>
            {
                var bond = Instantiate(bondPrefab, transform);
                bond.GetComponent<BondPresenter>().Bond = x;
            });
        }
    }
}