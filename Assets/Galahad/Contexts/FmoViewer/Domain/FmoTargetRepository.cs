using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Galahad.Contexts.FmoViewer.Domain.FmoTargetAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEngine;
using Zenject;

namespace Galahad.Contexts.FmoViewer.Domain
{
    [CreateAssetMenu(fileName = "FmoTargetRepository", menuName = "Installers/FmoTargetRepository")]
    public class FmoTargetRepository : ScriptableObjectInstaller<FmoTargetRepository>
    {
        public FmoTarget FmoTarget;
        public void readcpf(string cpf)
        {
	        var _fragments=new List<Fragment>();
				using (StreamReader file=new StreamReader(cpf))
				{
					var atomsf = new Dictionary<int, List<Atom>>();
//					var ifies = new Dictionary<int, List<IGldIfie>>();
//					var ifie=new List<IGldIfie>();
					var tagsf=new Dictionary<int,string>();
//					var bonds=new Dictionary<int,List<IGldBond>>();
					var fragmentbond=new List<Fragment>();
					string[] infolist;
					file.ReadLine();
				infolist = file.ReadLine().Split(new[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
				int moleculenumber = int.Parse(infolist[0]);
				int fragmentnumber =int.Parse(infolist[1]);
				
				for (int i = 0; i < moleculenumber; i++)
				{
					infolist = file.ReadLine().Split(new[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
					if (!atomsf.Keys.Contains(int.Parse(infolist[5])))
					{
						atomsf.Add(int.Parse(infolist[5]),new List<Atom>());
						tagsf.Add(int.Parse(infolist[5]),infolist[3]);
					}
					atomsf[int.Parse(infolist[5])].Add(new Atom(new AtomId( int.Parse(infolist[0])),
						(AtomicNumber)Enum.Parse(typeof(AtomicNumber),infolist[1]),
						new Position( new Vector3(float.Parse(infolist[6]),float.Parse(infolist[7]),float.Parse(infolist[8]))) ,new Charge(0)));
				}
				
				for (int i = 0; i < fragmentnumber / 16+1; i++)
				{
					file.ReadLine();
				}

				for (int i = 0; i < fragmentnumber/16+1; i++)
				{
					infolist = FileSeparationToStrings(file);
					for (int j = 0; j < infolist.Length; j++)
					{
//						fragmentbond.Add(new Fragment( j+1,int.Parse(infolist[j])));
					}
				}
//
//				foreach (var fragment in fragmentbond)
//				{
//					if (!bonds.Keys.Contains(fragment.FragmentId))
//					{
//					bonds.Add(fragment.FragmentId,new List<IGldBond>());
//					if (fragment.fragmentbondnumber != 0)
//					{
//						for (int i = 0; i < fragment.fragmentbondnumber; i++)
//						{
//							infolist = GldFunction.FileSeparationToStrings(file);
//							var bond = GldFunction.MakeGldBondfromId(atomsf, int.Parse(infolist[0]),
//								int.Parse(infolist[1]));
//							bonds[fragment.Id].Add(bond);
//						}
//					}	
//					}
//				}

				for (int i = 0; i < Conbination2(fragmentnumber); i++)
				{
					file.ReadLine();
				}

				for (int i = 0; i < fragmentnumber; i++)
				{
					file.ReadLine();
				}

				for (int i = 0; i < 7; i++)
				{
					file.ReadLine();
				}

				for (int i = 0; i < fragmentnumber; i++)
				{
					file.ReadLine();
				}

				for (int i = 0; i <Conbination2(fragmentnumber); i++)
				{
					infolist =file.ReadLine().Split(new[] {" "}, System.StringSplitOptions.RemoveEmptyEntries);
//					ifie.Add(new GldIfie((float.Parse(infolist[0])+float.Parse(infolist[1])+float.Parse(infolist[2]))));
				}
				for (int i = 1; i < fragmentnumber+1; i++)//FragIfieListの第二引数はfragment番号なので１からスタートが望ましい
				{
//					var ifieifie=new GldIfie();
//					ifies.Add(i,((IGldIfie)ifieifie).FragIfiesList(fragmentnumber,i,ifie));
				}
//
//				foreach (var key in atomsf.Keys)
//				{
//					var atomforbond = atomsf[key];
//					foreach (var atom in atomforbond)
//					{
//						foreach (var atom2 in atomforbond)
//						{
//							if (bonds[key].Contains(new GldBond(atom2,atom))||atom==atom2)
//							{
//								break;
//							}
//							var r = (atom.Position.Value - atom2.Position.Value).sqrMagnitude;
//							if (r<2.5f&&r>0f )
//							{
//								var bond=new GldBond(atom,atom2);
//								bonds[key].Add(bond);
//							}
//						}
//					}
//				}
				foreach (int key in atomsf.Keys)
				{
					_fragments.Add(new Fragment(new FragmentId(key),new Atoms(atomsf[key]) )); 
				}
				
				}
			
			
			FmoTarget=new FmoTarget(new Fragments(_fragments));
        }
        string[] FileSeparationToStrings(StreamReader file)
        {
	        return  file.ReadLine().Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
        }
        int Conbination2(int n)
        {
	        return n * (n - 1) / 2;
        }
        public override void InstallBindings()
        {
        }
    }
}