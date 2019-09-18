using System;
using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using Galahad.Contexts.FmoViewer.Event;
using Galahad.Contexts.FmoViewer.Presenter;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    public class CutWindow:EditorWindow
    {
        private FragmentationRepository Fragmentation;
        private PdbRepository _pdbRepository;
        private Vector2 scrol,cutscrol;
        private bool giveatom;
        private bool givenatom;
        private void OnGUI()
        {
            if (Selection.gameObjects.Length>0)
            {
                foreach (var gameObject in Selection.gameObjects)
                {
                    if (gameObject.name.Length<3)
                    {
                        continue;
                    }
                    switch (gameObject.name.Substring(0,3))
                    {
                        case  "LYS":
                        case " VAL" :
                        case "PHE":
                        case " GLY":
                        case "ARG":
                        case" CYS":
                        case" GLU":
                        case "LEU":
                        case "ALA":
                        case "MET":
                        case" HIS":
                        case "ASP":
                        case "ASN":
                        case "TYR":
                        case "SER":
                        case "TRP":
                        case "THR":
                        case "GLN":
                        case "ILE":
                        case "PRO":
                        case "CANULL":
                            var presenter= gameObject.GetComponent<FragmentPresenter>();
                            var x = presenter.Model;
                            var atompresenters = presenter.GetComponentsInChildren<AtomPresenter>();
                            EditorGUILayout.BeginHorizontal();
                            EditorGUILayout.LabelField($"{x.FragmentId,6}"+$"{x.ResidueName,5}",GUILayout.Width(80));
                            if (GUILayout.Button("cut"))
                            {
                                if (EditorUtility.DisplayDialog("cut","Do you want cut?","cut" ,"no"))
                                {
                                    
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            cutscrol= EditorGUILayout.BeginScrollView(cutscrol,GUI.skin.box);
                            EditorGUILayout.BeginVertical(GUI.skin.box);
                            foreach (var atomPresenter in atompresenters)
                            {
//                                if(atomPresenter.Model.ElementSymbol==ElementSymbol.H) continue;
                                
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField($"{atomPresenter.Model.AtomSerialNumber.ToString(),6}"+$"{atomPresenter.Model.AtomName+atomPresenter.Model.AlternateLocationIndicator.ToString(),5}",GUILayout.Width(100));
                                
                                atomPresenter.Events = (Events) EditorGUILayout.EnumPopup("atom:", atomPresenter.Events);
                                var oo = EditorGUILayout.Toggle(atomPresenter.Events == Events.AtomSelect||atomPresenter.Events==Events.GetAtom||atomPresenter.Events==Events.GiveAtom);
                                if (oo&&atomPresenter.Events==Events.GetAtom)
                                {
                                    atomPresenter.Events = Events.GetAtom;
                                }
                                else if(oo==false&&atomPresenter.Events==Events.GiveAtom)
                                {
                                    
                                    atomPresenter.Events = Events.GiveAtom;
                                }
                                else if(oo)
                                {
                                    
                                    atomPresenter.Events = Events.AtomSelect;
                                }
                                else
                                {
                                    atomPresenter.Events = Events.None;
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            x.Atoms.ToList().ForEach(atom =>
                            {
                            
                                if (atom.ElementSymbol==ElementSymbol.H)
                                {
                                    return;
                                }
                                EditorGUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField($"{atom.AtomSerialNumber.ToString(),6}"+$"{atom.AtomName+atom.AlternateLocationIndicator.ToString(),5}",GUILayout.Width(100));
                                atom.Select=EditorGUILayout.Toggle( atom.Select);
                                EditorGUILayout.EndHorizontal();
                            });
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndScrollView();
                            break;
                    }
                }
            }

            if (_pdbRepository!=null)
            {
                
            }

            EditorGUILayout.LabelField("cut");
            Fragmentation =
                (FragmentationRepository) EditorGUILayout.ObjectField(Fragmentation,typeof(FragmentationRepository)) ;
            if (Fragmentation == null) return;
            scrol= EditorGUILayout.BeginScrollView(scrol,GUI.skin.box);
            Fragmentation.Fragment.FragmentAtoms.ToList().ForEach(x =>
            {
                x.Init();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{x.FragmentId.ToString(),6}"+$"{x.ResidueName,5}",GUILayout.Width(80));
                x.Select = EditorGUILayout.Toggle( x.Select);
                EditorGUILayout.EndHorizontal();
                if (x.Select)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.window);
                    x.Atoms.ToList().ForEach(atom =>
                    {
                        if (atom.ElementSymbol==ElementSymbol.H)
                        {
                            return;
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"{atom.AtomSerialNumber.ToString(),6}"+$"{atom.AtomName+atom.AlternateLocationIndicator.ToString(),5}",GUILayout.Width(100));
                        atom.Select=EditorGUILayout.Toggle( atom.Select);
                        EditorGUILayout.EndHorizontal();
                    });
                    EditorGUILayout.EndVertical();
                }
            });
            Fragmentation.Fragment.FragmentHetatms.ToList().ForEach(x =>
            {
                x.Init();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{x.FragmentId.ToString(),6}"+$"{x.Name,5}",GUILayout.Width(80));
                x.Select = EditorGUILayout.Toggle( x.Select);
                EditorGUILayout.EndHorizontal();
                if (x.Select)
                {
                    EditorGUILayout.BeginVertical(GUI.skin.window);
                    EditorGUILayout.LabelField("choose give atom");
                    x.Hetatms.ToList().ForEach(atom =>
                    {
                        if (atom.ElementSymbol==ElementSymbol.H)
                        {
                            return;
                        }
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.LabelField($"{atom.AtomSerialNumber.ToString(),6}"+$"{atom.AtomName+atom.AlternateLocationIndicator.ToString(),5}",GUILayout.Width(100));
                        atom.Select=EditorGUILayout.Toggle( atom.Select);
                        EditorGUILayout.EndHorizontal();
                    });
                    EditorGUILayout.EndVertical();
                }
            });
            EditorGUILayout.EndScrollView();
        }

        public void Inject(PdbRepository pdbRepository)
        {
            _pdbRepository = pdbRepository;
        }

        public CutWindow Inject(FragmentationRepository fragmentationRepository)
        {
            Fragmentation = fragmentationRepository;
            return this;
        }
    }
}