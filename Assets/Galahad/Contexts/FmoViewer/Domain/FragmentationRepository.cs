using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;
using Zenject;

namespace Galahad.Contexts.FmoViewer.Domain
{
    [CreateAssetMenu(fileName = "Fragmentation", menuName = "Installers/FragmentationRepository")]
    public class FragmentationRepository : ScriptableObjectInstaller<FragmentationRepository>
    {
        public PdbRepository PdbRepository;
        public FragmentCutInformation FragmentCutInformation;

        public void WriteAjf(string readpath,string writepath,string savename)
        {
            using (var reader=new StreamReader(readpath))
            {
                using (var writer=new StreamWriter(writepath))
                {
                    string line;
                    while ((line=reader.ReadLine())!=null)
                    {
                        if (line.Contains("{{{BASENAME}}}"))
                        {
                             var linesplit= line.Split( string[] "{{{BASENAME}}}",StringSplitOptions.None)};
                        }
                        writer.Write();
                    }
                }
            }
                
        }
        public void AutoCut(Pdb pdb)
        {
            FragmentCutInformation = null;
            Debug.Log("start");
            var fragmentCuts=new FragmentCuts();
            var fragmentBonds=new FragmentBonds();
            var endAtoms = pdb.Atoms.GetEndOxAtoms();
            for (var i=0;i< pdb.DefaultFragmentMount();i++)
            {
                fragmentCuts.Add(new FragmentCut(new FragmentId(i + 1), new Atoms(),new Hetatms(), new ResidueSequencsNumber(pdb.FirstFragmentId()+i)));
                fragmentBonds.Add(new FragmentId(i + 1));
            }
            FragmentCutInformation=new FragmentCutInformation(fragmentCuts,fragmentBonds);
            
            foreach (AtomName value in Enum.GetValues(typeof(AtomName)))
            {
                
                switch (value)
                {
                    case AtomName.CA:
                        var ca = pdb.Atoms.Get(value);
                        foreach (var atom in ca.ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                FragmentCutInformation.FragmentCuts[atom.ResidueSequencsNumber].Add(atom);
                            }
                            else
                            {
                                FragmentCutInformation.FragmentCuts[atom.ResidueSequencsNumber].Add(atom);
                                FragmentCutInformation.FragmentBonds[FragmentCutInformation.FragmentCuts[atom.ResidueSequencsNumber].FragmentId].AddGiveAtom(atom);
                            }
                        }
                        break;
                    case AtomName.C:
                        foreach (var atom in pdb.Atoms.Get(value).ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                FragmentCutInformation.FragmentCuts[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value-1)].Add(atom);
                            }
                            else
                            {
                                FragmentCutInformation.FragmentCuts[atom.ResidueSequencsNumber].Add(atom);
                                FragmentCutInformation.FragmentBonds[
                                        new FragmentId(fragmentCuts[atom.ResidueSequencsNumber].FragmentId.Value )]
                                    .AddGetAtom(atom);
                            };
                        }
                        break;
                    case AtomName.O:
                        foreach (var atom in pdb.Atoms.Get(value).ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                FragmentCutInformation.FragmentCuts[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value-1)].Add(atom);
                            }
                            else
                            {
                                FragmentCutInformation.FragmentCuts[atom.ResidueSequencsNumber].Add(atom);
                            };
                        }
                        break;
                    default:
                        if (pdb.Atoms.Exists(value))
                        {
                            foreach (var atom in pdb.Atoms.Get(value).ToList())
                            {
                                FragmentCutInformation.FragmentCuts[atom.ResidueSequencsNumber].Add(atom);
                            }
                        }
                        break;
                }
            }

            foreach (var hetatm in pdb.Hetatms.ToList())
            {
                FragmentCutInformation.FragmentCuts[hetatm.ResidueSequencsNumber].Add(hetatm);
            }

        }

        public void AutoResidueCut()
        {
            var fragmentCuts = FragmentCutInformation.FragmentCuts.ToList();
            var n= FragmentCutInformation.FragmentCuts.Count();
            for (var i = 0; i < n; i++)
            {
                FragmentCutInformation.FragmentCuts.BeforeCutAtoms(new FragmentId(n - i));
                var m = FragmentCutInformation.FragmentCuts[n - i].Atoms.Count();
                for (var j = 0; j <m; j++)
                {
                    switch (FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1].AtomName)
                    {
                        case AtomName.CA:
                            break;
                        case AtomName.HA:
                            break;
                        case AtomName.C:
                            break;
                        case AtomName.H:
                            break;
                        case AtomName.N:
                            break;
                        case AtomName.H1:
                            break;
                        case AtomName.H2:
                            break;
                        case AtomName.H3:
                            break;
                        case AtomName.O:
                            break;
                        case AtomName.OX:
                            break;
                        case AtomName.CB:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.CD:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.CE:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.CG:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.CH:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.CZ:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HB:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HC:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HD:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HE:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HG:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HH:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HZ:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.ND:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.NE:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.NH:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.NZ:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.OD:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.OE:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.OG:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.OH:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.SD:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.SG:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HD1:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HD2:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HE2:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HG1:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HG2:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HH1:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HH2:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        case AtomName.HH3:
                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(FragmentCutInformation.FragmentCuts[n - i].Atoms[m-j-1],new FragmentId(n-i));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                }
//                foreach (var atom in atoms)
//                {
//                    switch (atom.AtomName)
//                    {
//                        case AtomName.CA:
//                            break;
//                        case AtomName.HA:
//                            break;
//                        case AtomName.C:
//                            break;
//                        case AtomName.H:
//                            break;
//                        case AtomName.N:
//                            break;
//                        case AtomName.H1:
//                            break;
//                        case AtomName.H2:
//                            break;
//                        case AtomName.H3:
//                            break;
//                        case AtomName.O:
//                            break;
//                        case AtomName.OX:
//                            break;
//                        case AtomName.CB:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.CD:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.CE:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.CG:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.CH:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.CZ:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HB:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HC:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HD:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HE:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HG:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HH:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HZ:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.ND:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.NE:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.NH:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.NZ:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.OD:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.OE:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.OG:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.OH:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.SD:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.SG:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HD1:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HD2:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HE2:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HG1:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HG2:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HH1:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HH2:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        case AtomName.HH3:
//                            FragmentCutInformation.FragmentCuts.AtomMoveToNextFragmentCut(atom,new FragmentId(n-i));
//                            break;
//                        default:
//                            throw new ArgumentOutOfRangeException();
//                    }   
//                }
            }
            
        }

        public void Cut(Atoms atoms, Atom ca,Atom co)
        {
            FragmentCutInformation.FragmentCuts.Add(
                new FragmentCut(new FragmentId(FragmentCutInformation.FragmentCuts.Count() + 1)));
            
        }
        
    
        public override void InstallBindings()
        {
        }
    }
}