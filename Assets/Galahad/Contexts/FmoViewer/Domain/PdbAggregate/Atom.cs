using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class Atom:ISerializationCallbackReceiver
    {
        [SerializeField] private int atomSerialNumber;
        [SerializeField] private AtomName atomName;
        [SerializeField] private int alternateLocationIndicator;
        [SerializeField] private ResidueName residueName;
        [SerializeField] private string chainId;
        [SerializeField] private int residueSequenceNumber;
        [SerializeField] private string codeForInsertionOfResidues;
        [SerializeField] private Vector3 position;
        [SerializeField] private float occupancy;
        [SerializeField] private float tempratureFactor;
        [SerializeField] private ElementSymbol elementSymbol;
        [SerializeField] private int formalCharge;
        public Atom(string atomSerialNumber, string atomName, string alternateLocationIndicator, string residueName,
            string chainId,string residueSequenceNumber ,string codeForInsertionOfResidues, string x, string y, string z, string occupancy, string temperatureFactor,
            string elementSymbol, string formalCharge)
        {
            AtomSerialNumber=new AtomSerialNumber(int.Parse(atomSerialNumber));
            AtomName = (AtomName) Enum.Parse(typeof(AtomName), atomName);
            AlternateLocationIndicator = !int.TryParse(alternateLocationIndicator, out var alt) ? new AlternateLocationIndicator(0) : new AlternateLocationIndicator(alt);
            ResidueName = (ResidueName) Enum.Parse(typeof(ResidueName), residueName);
            ChainId=new ChainId(chainId);
            ResidueSequencsNumber=new ResidueSequencsNumber(int.Parse(residueSequenceNumber));
            CodeForInsertionOfResidues=new CodeForInsertionOfResidues(codeForInsertionOfResidues);
            Position=new Position(new Vector3(float.Parse(x),float.Parse(y),float.Parse(z)));
            Occupancy=!float.TryParse(occupancy,out var occuResult)?new Occupancy(0f) : new Occupancy(float.Parse(occupancy));
            TemperatureFactor=!float.TryParse(temperatureFactor,out var tempResult)?new TemperatureFactor(0f) : new TemperatureFactor(float.Parse(temperatureFactor));
            ElementSymbol = (ElementSymbol) Enum.Parse(typeof(AtomicNumber), elementSymbol);
            FormalCharge = !int.TryParse(formalCharge.Substring(1,1)+formalCharge.Substring(0,1),out var foResult) ? new FormalCharge(0) : new FormalCharge(foResult);
           
        }
        public AtomSerialNumber AtomSerialNumber { get; private set; }
        public AtomName AtomName { get; set; }
        public AlternateLocationIndicator AlternateLocationIndicator { get; set; }
        public ResidueName ResidueName { get; set; }
        public ChainId ChainId { get; private set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; private set; }
        public CodeForInsertionOfResidues CodeForInsertionOfResidues { get; set; }
        public Position Position { get; set; }
        public  Occupancy Occupancy { get; set; }
        public  TemperatureFactor TemperatureFactor { get; set; }
        public  ElementSymbol ElementSymbol { get; set; }
        public FormalCharge FormalCharge { get; set; }

        public void OnBeforeSerialize()
        {
            atomSerialNumber = AtomSerialNumber?.Value ?? -1;
            atomName = AtomName;
            alternateLocationIndicator = AlternateLocationIndicator?.Value ?? 0;
            residueName = ResidueName;
            chainId = ChainId?.Value ?? "";
            residueSequenceNumber = ResidueSequencsNumber?.Value ?? 0;
            codeForInsertionOfResidues = CodeForInsertionOfResidues?.Value ?? "";
            position = Position?.Value ?? Vector3.zero;
            occupancy = Occupancy?.Value ?? 0f;
            tempratureFactor = TemperatureFactor?.Value ?? 0f;
            elementSymbol = ElementSymbol;
            formalCharge = FormalCharge?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            AtomSerialNumber=new AtomSerialNumber(atomSerialNumber);
            AtomName = atomName;
            AlternateLocationIndicator=new AlternateLocationIndicator(alternateLocationIndicator);
            ResidueName = residueName;
            ChainId=new ChainId(chainId);
            ResidueSequencsNumber=new ResidueSequencsNumber(residueSequenceNumber);
            CodeForInsertionOfResidues=new CodeForInsertionOfResidues(codeForInsertionOfResidues);
            Position=new Position(position);
            Occupancy=new Occupancy(occupancy);
            TemperatureFactor=new TemperatureFactor(tempratureFactor);
            ElementSymbol = elementSymbol;
            FormalCharge = new FormalCharge(formalCharge);
        }
    }

    public class Atoms
    {
        private readonly List<Atom> _atoms;
        public Atoms(List<Atom> atoms)
        {
            _atoms = atoms;
        }

        public Atoms():this(new List<Atom>())
        {
            
        }

        public Atom this[AtomSerialNumber index] => _atoms.FirstOrDefault(x => x.AtomSerialNumber.Value == index.Value);

        public List<Atom> ToList()
        {
            return _atoms;
        }

        public Atoms Add(Atom atom)
        {
            _atoms.Add(atom);
            return new Atoms(_atoms);
        }

        public Atoms AddAtoms(string pdbLine)
        {
            if (pdbLine.Length>79)
            {
                _atoms.Add(new Atom(
                    pdbLine.Substring(6, 5),
                    pdbLine.Substring(11, 4),
                    pdbLine.Substring(15, 1),
                    pdbLine.Substring(17, 3),
                    pdbLine.Substring(21, 1),
                    pdbLine.Substring(22, 4),
                    pdbLine.Substring(26, 1),
                    pdbLine.Substring(30, 8),
                    pdbLine.Substring(38, 8),
                    pdbLine.Substring(46, 8),
                    pdbLine.Substring(54, 6),
                    pdbLine.Substring(60, 6),
                    pdbLine.Substring(76, 2),
                    pdbLine.Substring(78, 2)));
            }
            else
            {
                _atoms.Add(new Atom(
                    pdbLine.Substring(6, 5),
                    pdbLine.Substring(11, 4),
                    pdbLine.Substring(15, 1),
                    pdbLine.Substring(17, 3),
                    pdbLine.Substring(21, 1),
                    pdbLine.Substring(22, 4),
                    pdbLine.Substring(26, 1),
                    pdbLine.Substring(30, 8),
                    pdbLine.Substring(38, 8),
                    pdbLine.Substring(46, 8),
                    pdbLine.Substring(54, 6),
                    pdbLine.Substring(60, 6),
                    pdbLine.Substring(76, 2),
                    "0 "));
            }
            return new Atoms(_atoms);
        }
    }
}