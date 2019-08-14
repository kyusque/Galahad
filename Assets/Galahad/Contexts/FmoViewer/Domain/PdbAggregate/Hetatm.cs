using System;
using System.Collections.Generic;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.PdbAggregate
{
    [Serializable]
    public class Hetatm:ISerializationCallbackReceiver
    {
        [SerializeField] private int atomSerialNumber;
        [SerializeField] private int alternateLocationIndicator;
        [SerializeField] private AtomName atomName;
        [SerializeField] private string hetatmResidueName;
        [SerializeField] private string chainId;
        [SerializeField] private int residueSequencsNumber;
        [SerializeField] private string codeForInsertionOfResidues;
        [SerializeField] private Vector3 position;
        [SerializeField] private float occupancy;
        [SerializeField] private float temperatureFactor;
        [SerializeField] private ElementSymbol elementSymbol;
        [SerializeField] private int formalCharge;
        public Hetatm(string atomSerialNumber,
            string atomName,
            string alternateLocationIndicator,
            string hetatmResidueName,
            string chainId,
            string residueSequencsNumber,
            string codeForInsertionOfResidues,
            string x,string y,string z,
            string occupancy,
            string temperatureFactor,
            string elementSymbol,
            string formalCharge)
        {
            AtomSerialNumber=new AtomSerialNumber(int.Parse(atomSerialNumber));
            AtomName = (AtomName) Enum.Parse(typeof(AtomName), atomName);
            AlternateLocationIndicator = !int.TryParse(alternateLocationIndicator,out var altResult) ? new AlternateLocationIndicator(0) : new AlternateLocationIndicator(altResult);
            HetatmResidueName = new HetatmResidueName(hetatmResidueName);
            ChainId = new ChainId(chainId);
            ResidueSequencsNumber=new ResidueSequencsNumber(int.Parse(residueSequencsNumber));
            CodeForInsertionOfResidues=new CodeForInsertionOfResidues(codeForInsertionOfResidues);
            Position=new Position(new Vector3(float.Parse(x),float.Parse(y),float.Parse(z)));
            Occupancy = !float.TryParse(occupancy,out var occuResult) ? new Occupancy(0f) : new Occupancy(occuResult);
            TemperatureFactor=!float.TryParse(temperatureFactor,out var tempResult)?new TemperatureFactor(0f) : new TemperatureFactor(tempResult);
            ElementSymbol = !Enum.TryParse<ElementSymbol>(elementSymbol, out var elEnum) ? ElementSymbol.Mg : elEnum;
            FormalCharge = !int.TryParse(formalCharge.Substring(1,1)+formalCharge.Substring(0,1), out var foResult) ? new FormalCharge(0) : new FormalCharge(foResult);
        }
        public AtomSerialNumber AtomSerialNumber { get; private set; }
        public AtomName AtomName { get; private set; }
        public AlternateLocationIndicator AlternateLocationIndicator { get; private set; }
        public HetatmResidueName HetatmResidueName { get; private set; }
        public ChainId ChainId { get; private set; }
        public ResidueSequencsNumber ResidueSequencsNumber { get; private set; }
        public CodeForInsertionOfResidues CodeForInsertionOfResidues { get; private set; }
        public Position Position { get; private set; }
        public Occupancy Occupancy { get; private set; }
        public TemperatureFactor TemperatureFactor { get; private set; }
        public ElementSymbol ElementSymbol { get; private set; }
        public FormalCharge FormalCharge { get; private set; }


        public void OnBeforeSerialize()
        {
            atomSerialNumber = AtomSerialNumber?.Value ?? 0;
            atomName = AtomName;
            alternateLocationIndicator = AlternateLocationIndicator?.Value ?? 0;
            hetatmResidueName = HetatmResidueName?.Value ?? "??";
            chainId = ChainId?.Value ?? "";
            residueSequencsNumber = ResidueSequencsNumber?.Value ?? 0;
            codeForInsertionOfResidues = CodeForInsertionOfResidues?.Value ?? "";
            position = Position?.Value ?? Vector3.zero;
            occupancy = Occupancy?.Value ?? 0f;
            temperatureFactor = TemperatureFactor?.Value ?? 0f;
            elementSymbol = ElementSymbol;
            formalCharge = FormalCharge?.Value ?? 0;
        }

        public void OnAfterDeserialize()
        {
            AtomSerialNumber=new AtomSerialNumber(atomSerialNumber);
            AtomName = atomName;
            AlternateLocationIndicator=new AlternateLocationIndicator(alternateLocationIndicator);
            HetatmResidueName=new HetatmResidueName(hetatmResidueName);
            ChainId=new ChainId(chainId);
            ResidueSequencsNumber=new ResidueSequencsNumber(residueSequencsNumber);
            CodeForInsertionOfResidues=new CodeForInsertionOfResidues(codeForInsertionOfResidues);
            Position=new Position(position);
            Occupancy=new Occupancy(occupancy);
            TemperatureFactor=new TemperatureFactor(temperatureFactor);
            ElementSymbol = elementSymbol;
            FormalCharge=new FormalCharge(formalCharge);
        }
    }
    public class Hetatms
    {
        private readonly List<Hetatm> _hetatms;
        public Hetatms(List<Hetatm> hetatms)
        {
            _hetatms = hetatms;
        }

        public Hetatms() : this(new List<Hetatm>())
        {
            
        }

        public Hetatm this[AtomSerialNumber atomSerialNumber] =>
            _hetatms.FirstOrDefault(x => x.AtomSerialNumber.Value == atomSerialNumber.Value);

        public List<Hetatm> ToList()
        {
            return _hetatms;
        }

        public Hetatms Add(Hetatm hetatm)
        {
            _hetatms.Add(hetatm);
            return new Hetatms(_hetatms);
        }

        public bool Contains()
        {
            return _hetatms.Count > 0;
        }

        public int Count()
        {
            return _hetatms?.Count??0;
        }

        public Hetatms AddHetatms(string line)
        {
            if (line.Length>79)
            {
                _hetatms.Add(new Hetatm(
                    line.Substring(6,5),
                    line.Substring(11,4),
                    line.Substring(15,1),
                    line.Substring(17,3),
                    line.Substring(21,1),
                    line.Substring(22,4),
                    line.Substring(26,1),
                    line.Substring(30,8),
                    line.Substring(38,8),
                    line.Substring(46,8),
                    line.Substring(54,6),
                    line.Substring(60,6),
                    line.Substring(76,2),
                    line.Substring(78,2)
                ));
            }
            else
            {
                _hetatms.Add(new Hetatm(
                    line.Substring(6,5),
                    line.Substring(11,4),
                    line.Substring(15,1),
                    line.Substring(17,3),
                    line.Substring(21,1),
                    line.Substring(22,4),
                    line.Substring(26,1),
                    line.Substring(30,8),
                    line.Substring(38,8),
                    line.Substring(46,8),
                    line.Substring(54,6),
                    line.Substring(60,6),
                    line.Substring(76,2),
                    "0 "
                ));
            }

            return new Hetatms(_hetatms);
        }

        public Hetatms Remove(Hetatm hetatm)
        {
            _hetatms.Remove(hetatm);
            return new Hetatms(_hetatms);
        }

        public Hetatms Clear()
        {
            _hetatms.Clear();
            return new Hetatms(_hetatms);
        }

        public Hetatms MoveTo(Hetatms hetatms)
        {
            if (!hetatms.Contains()) return this;
            foreach (var hetatm in hetatms.ToList())
            {
                _hetatms.Add(hetatm);
            }

            hetatms.Clear();
            return new Hetatms(_hetatms);

        }
    }
}