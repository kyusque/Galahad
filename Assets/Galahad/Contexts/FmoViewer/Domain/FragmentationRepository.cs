using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEditor;
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
        public Dictionary<int,Bonds> Bonds;

        public void Save(string templeteajf)
        {
//        System.Diagnostics.Process.Start(@"C:","/select,");
////        System.Diagnostics.Process.Start()
//        using (var myProcess = new Process())
//        {
//            EditorUtility.SaveFilePanel("","","","")
//        }
            var path = EditorUtility.SaveFilePanel("save ajf", "", "", "ajf");
            if (path.Length <= 0) return;
            var pat = Path.GetFileNameWithoutExtension(path);
            if (File.Exists(path))
            {
                if (EditorUtility.DisplayDialog("overwrite?", pat + ".ajf already exists.Overwrite or Not", "overwrite",
                        "choose other name") == false)
                {
                    return;
                }
                
            }
            else
            {
                File.Create(path);
            }
            WriteAjf(templeteajf,path,pat,pat);
        }

        public void WriteAjf(string readpath,string writepath,string saveName,string basisSet)
        {
            using (var reader=new StreamReader(readpath))
            {
                using (var writer=new StreamWriter(writepath))
                {
                    string line;
                    while ((line=reader.ReadLine())!=null)
                    {
                        Debug.Log(line);
                        if (line.Contains("{{{BASENAME}}}"))
                        {
                            var n=line.LastIndexOf("{{{BASENAME}}}");
                            Debug.Log(
                                line.Substring(0, n) 
                                +saveName
                                +line.Substring(n+14,line.Length - n-14));
                            writer.WriteLine(
                                line.Substring(0, n) 
                                +saveName
                                +line.Substring(n+14,line.Length - n-14));
                        }
                        else if (line.Contains("{{{TOTAL_CHARGE}}}"))
                        {
                            var n = line.IndexOf("{{{TOTAL_CHARGE}}}");
                            Debug.Log(
                                line.Substring(0, n - 1)
                                +Fragment.TotalCharge()
                                +line.Substring(n +18 ,line.Length - n-18));
                            writer.WriteLine(
                                line.Substring(0, n - 1)
                                +Fragment.TotalCharge()
                                +line.Substring(n +18 ,line.Length - n-18));
                        }
                        else if (line.Contains("{{{BASIS_SET}}}"))
                        {
                            var n = line.IndexOf("{{{BASIS_SET}}}");
                            Debug.Log(
                                line.Substring(0, n - 1)
                                +basisSet 
                                +line.Substring(n+ 14,line.Length - n-14));
                            writer.WriteLine(
                                line.Substring(0, n - 1)
                                +basisSet 
                                +line.Substring(n+ 14,line.Length - n-14));
                        }
                        else if (line.Contains("{{{NUM_FRAGS}}}"))
                        {
                            var n = line.IndexOf("{{{NUM_FRAGS}}}");
                            Debug.Log(
                                line.Substring(0, n) 
                                +Fragment.NumFrag()
                                +line.Substring(n + 14,line.Length - n-14));
                            writer.WriteLine(
                                line.Substring(0, n) 
                                + Fragment.NumFrag()
                                +line.Substring(n + 14,line.Length - n-14));
                        }
                        else if (line.Contains("{{{ABINITMP_FRAGMENT}}}"))
                        {
                            writer.WriteLine("&FRAGMENT");
                            writer.Write(WriteFragment());
                            writer.WriteLine("/");
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
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (!fragmentAtom.Atoms.Contains()) continue;
                if (frg.Length>80)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentAtom.Atoms.Count().ToString(),8}";
            }
            foreach (var fragmentHetatm in Fragment.FragmentHetatms.ToList())
            {
                if (!fragmentHetatm.Hetatms.Contains()) continue;
                if (frg.Length > 80)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentHetatm.Hetatms.Count().ToString(),8}";
            }
            write += Environment.NewLine + frg;
            frg = "";
            //原子数
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (!fragmentAtom.Atoms.Contains()) continue;
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentAtom.Atoms.TotalCharge(),8}";
            }

            foreach (var fragmentHetatm in Fragment.FragmentHetatms.ToList())
            {
                if (!fragmentHetatm.Hetatms.Contains()) continue;
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                frg += $"{fragmentHetatm.Hetatms.TotalCharge(),8}";
            }
            write += Environment.NewLine + frg;
            frg = "";
            //形式電荷
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (frg.Length>79)
                {
                    write += Environment.NewLine + frg;
                    frg = "";
                }
                if (fragmentAtom.Atoms.Contains())
                {
                    frg += $"{fragmentAtom.Bonds.BondNum(),8}";
                }
            }
//            foreach (var fragmentHetatm in Fragment.FragmentHetatms.ToList())
//            {
//                if (frg.Length>79)
//                {
//                    write += Environment.NewLine + frg;
//                    frg = "";
//                }
//                if (fragmentHetatm.Hetatms.Contains())
//                {
//                    frg += $"{fragmentHetatm.Hetatms.TotalCharge(),8}";
//                }
//            }
            write += Environment.NewLine + frg;
            frg = "";
            //結合本数
            //
            foreach (var fragmentAtom in Fragment.FragmentAtoms.ToList())
            {
                if (!fragmentAtom.Atoms.Contains()) continue;
                foreach (var atom in fragmentAtom.Atoms.ToList())
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
            foreach (var hetems in Fragment.FragmentHetatms.ToList())
            {
                foreach (var hetem in hetems.Hetatms.ToList())
                {
                    if (frg.Length>79)
                    {
                        write += Environment.NewLine + frg;
                        frg = "";
                    }
                    frg += $"{hetem.AtomSerialNumber.Value,8}";
                }
            }
            write += Environment.NewLine + frg;
            frg = "";
            //原子の番号
            //
            foreach (var bond in Bonds)
            {
                frg += $"{bond.Value.False().Value,8}" + $"{bond.Value.True().Value,8}";
                write += Environment.NewLine + frg;
                frg = "";
            }
            //結合しているフラグメントの原子番号
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
            Bonds=new Dictionary<int, Bonds>();
            Debug.Log("start");
            var fragmentAtoms=new FragmentAtoms();
            var fragmentHetatms = new FragmentHetatms();
            var fragmentBonds=new FragmentBonds();
            var endAtoms = pdb.Atoms.GetEndOxAtoms();
            Fragment=new Fragment(fragmentAtoms,fragmentHetatms,fragmentBonds);
            Fragment.State=new State();
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
                                Fragment.FragmentAtoms.AddCa(new FragmentId(ca.IndexOf(atom)+1),atom).Add(new Bond(ca.IndexOf(atom)+1,atom.ToString(),false));
                                Bonds.Add(ca.IndexOf(atom)+1,new Bonds().Add(new Bond(ca.IndexOf(atom)+1,atom.ToString(),false)));
                                Fragment.FragmentBonds.AddNewCa(new FragmentId(ca.IndexOf(atom)+1),atom);
                                
                            }
                        }
                        break;
                    case AtomName.C:
                        var c=pdb.Atoms.Get(value);
                        foreach (var atom in c.ToList())
                        {
                            if (endAtoms.Exists(atom.ResidueSequencsNumber))
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber(atom.ResidueSequencsNumber.Value)].Add(atom);
                            }
                            else
                            {
                                Fragment.FragmentAtoms[new ResidueSequencsNumber( atom.ResidueSequencsNumber.Value+1)]
                                    .Add(atom)
                                    .Add(new Bond(c.IndexOf(atom)+1,atom.ToString(),true));
                                Bonds[c.IndexOf(atom) + 1].Add(new Bond(c.IndexOf(atom) + 1, atom.ToString(), true));
                                Fragment.FragmentBonds[Fragment.FragmentAtoms[atom.ResidueSequencsNumber].FragmentId]
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
                Bonds.Add(Bonds.Count+1,Fragment.FragmentAtoms
                    .AddLastNew()
                    .MoveOther(new FragmentId(n - i))
                    .OneResidueCut(new FragmentId(n - i),Bonds.Count+1));
            }
            Fragment.State.Cut();
        } 
        
    
        public override void InstallBindings()
        {
        }
    }
}