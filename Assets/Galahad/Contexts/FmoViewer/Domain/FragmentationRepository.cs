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
        public Fragment Fragment;

        public void WriteAjf(string readpath,string writepath,string saveName,string basisSet)
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
                            writer.WriteLine(
                                line.Substring(0, line.IndexOf("{{{BASENAME}}}") - 1) + saveName +
                                line.Substring(line.LastIndexOf("{{{BASENAME}}}") + 1),
                                line.Length - line.LastIndexOf("{{{BASENAME}}}"));
                        }
                        else if (line.Contains("{{{TOTAL_CHARGE}}}"))
                        {
                            writer.WriteLine(
                                line.Substring(0, line.IndexOf("{{{TOTAL_CHARGE}}}") - 1) +
                                FragmentCutInformation.FragmentCuts.TOTAL_CHARGE() +
                                line.Substring(line.LastIndexOf("{{{TOTAL_CHARGE}}}") + 1),
                                line.Length - line.LastIndexOf("{{{TOTAL_CHARGE}}}"));
                        }
                        else if (line.Contains("{{{BASIS_SET}}}"))
                        {
                            writer.WriteLine(
                                line.Substring(0, line.IndexOf("{{{BASIS_SET}}}") - 1) +
                                basisSet +
                                line.Substring(line.LastIndexOf("{{{BASIS_SET}}}") + 1),
                                line.Length - line.LastIndexOf("{{{BASIS_SET}}}"));
                        }
                        else if (line.Contains("{{{NUM_FRAGS}}}"))
                        {
                            writer.WriteLine(
                                line.Substring(0, line.IndexOf("{{{NUM_FRAGS}}}") - 1) +
                                basisSet +
                                line.Substring(line.LastIndexOf("{{{NUM_FRAGS}}}") + 1),
                                line.Length - line.LastIndexOf("{{{NUM_FRAGS}}}"));
                        }
                        else if (line.Contains("{{{ABINITMP_FRAGMENT}}}"))
                        {
                            writer.WriteLine("&FRAGMENT");
                            writer.Write(WriteFragment());
                        }
                        else
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
            }
        }

        private string WriteFragment()
        {
            var frg = "";
            var write = "";
            foreach (var fragmentCut in FragmentCutInformation.FragmentCuts.ToList())
            {
                if (fragmentCut.Atoms.Contains())
                {
                    if (frg.Length>80)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{fragmentCut.Atoms.Count().ToString(),8}";
                }
                else if (fragmentCut.Hetatms.Contains())
                {
                    if (frg.Length > 80)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{fragmentCut.Hetatms.Count().ToString(),8}";
                }
            }
            write += Environment.NewLine + frg;
            frg = "";
            foreach (var fragmentCut in FragmentCutInformation.FragmentCuts.ToList())
            {
                if (fragmentCut.Atoms.Contains())
                {
                    if (frg.Length>79)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{fragmentCut.Atoms.TotalCharge(),8}";
                }
                else if(fragmentCut.Hetatms.Contains())
                {
                    if (frg.Length>79)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{fragmentCut.Hetatms.TotalCharge(),8}";
                }
            }
            write += Environment.NewLine + frg;
            frg = "";
            foreach (var fragmentCut in FragmentCutInformation.FragmentCuts.ToList())
            {
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                if (fragmentCut.Atoms.Contains())
                {
                    frg += $"{fragmentCut.Atoms.TotalCharge(),8}";
                    
                }
                else if (fragmentCut.Hetatms.Contains())
                {
                    frg += $"{fragmentCut.Hetatms.TotalCharge(),8}";
                }
            }
            write += Environment.NewLine + frg;
            frg = "";
            foreach (var fragmentCut in FragmentCutInformation.FragmentCuts.ToList())
            {
                if (fragmentCut.Atoms.Contains())
                {
                    foreach (var atom in fragmentCut.Atoms.ToList())
                    {
                        if (frg.Length>79)
                        {
                            write += Environment.NewLine + frg;
                            frg = "";
                        }
                        frg += $"{atom.AtomSerialNumber.Value,8}";
                    }
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                else
                {
                    foreach (var hetatm in fragmentCut.Hetatms.ToList())
                    {
                        if (frg.Length>79)
                        {
                            write += Environment.NewLine + frg;
                            frg = "";
                        }
                        frg += $"{hetatm.AtomSerialNumber,8}";
                    }
                }
                write += Environment.NewLine + frg;
                frg = "";
            }
            foreach (var fragmentBond in FragmentCutInformation.FragmentBonds.ToList())
            {
                frg += $"{fragmentBond.CA.AtomSerialNumber,8}" + $"{fragmentBond.CO.AtomSerialNumber,8}";
                write += Environment.NewLine + frg;
                frg = "";
            }
            return write;
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

        } //旧fragmentcutinformation

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

        public void NewAutoResidueCut(Pdb pdb)
        {
            Fragment = null;
            Debug.Log("start");
            var fragmentAtoms=new FragmentAtoms();
            var fragmentHetatms = new FragmentHetatms();
            var fragmentBonds=new FragmentBonds();
            var endAtoms = pdb.Atoms.GetEndOxAtoms();
            Fragment=new Fragment(fragmentAtoms,fragmentHetatms,fragmentBonds);
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
                                Fragment.FragmentAtoms.AddCa(new FragmentId(ca.IndexOf(atom)+1),atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms.AddCa(new FragmentId(ca.IndexOf(atom)+1),atom);
                                Fragment.FragmentBonds.AddNewCa(new FragmentId(ca.IndexOf(atom)+1),atom);
                            }
                        }
                        break;
                    case AtomName.C:
                        foreach (var atom in pdb.Atoms.Get(value).ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value)].Add(atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber( atom.ResidueSequencsNumber.Value+1)].Add(atom);
                                Fragment.FragmentBonds[Fragment.FragmentAtoms[new ResidueSequencsNumber( atom.ResidueSequencsNumber.Value+1)]]
                                    .AddGetAtom(atom);
                            };
                        }
                        break;
                    case AtomName.O:
                        foreach (var atom in pdb.Atoms.Get(value).ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value)].Add(atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber( atom.ResidueSequencsNumber.Value+1)].Add(atom);
                            };
                        }
                        break;
                    default:
                        if (pdb.Atoms.Exists(value))
                        {
                            foreach (var atom in pdb.Atoms.Get(value).ToList())
                            {
                                Fragment.FragmentAtoms[atom.ResidueSequencsNumber].Add(atom);
                            }
                        }
                        break;
                }
            }

            foreach (var hetatm in pdb.Hetatms.ToList())
            {
                if (Fragment.FragmentHetatms.Exists(hetatm.ResidueSequencsNumber)==false)
                {
                    Fragment.FragmentHetatms.Add(hetatm.ResidueSequencsNumber);
                }
                Fragment.FragmentHetatms[hetatm.ResidueSequencsNumber].Add(hetatm);
            }
        }//ちょっと綺麗にしたPDB＝＞fragment

        public void NewAutoResidueCut()
        {
            var n= Fragment.FragmentAtoms.Count();
            for (var i = 0; i < n; i++)
            {
                if (Fragment.FragmentAtoms[new FragmentId(n-i)].ResidueName=="GLY")
                {
                    continue;
                }
                Fragment.FragmentAtoms.AddLastNew().MoveOther(new FragmentId(n - i)).ResidueCut(new FragmentId(n - i));
            }
            
        } 
        
    
        public override void InstallBindings()
        {
        }
    }
}