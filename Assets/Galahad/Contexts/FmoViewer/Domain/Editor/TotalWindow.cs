using System;
using System.IO;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;
using Directory = System.IO.Directory;
using Object = UnityEngine.Object;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    public class TotalWindow:EditorWindow
    {
        private Object _pdb;
        private string pdb;
        private PdbRepository _pdbRepository;
        private Vector2 scrolhetatm,scrolatom,scroldelete;
        private int hetatmCut,atomCut;
        private string[] templates;
        [MenuItem("Editor/totalwindow")]
        public static void Init()
        {
            var window =  (TotalWindow) EditorWindow.GetWindow(typeof(TotalWindow));
            window.titleContent=new GUIContent("TotalEditor");
            window.Show();
        }

        private void OnGUI()
        {
            if (GUILayout.Button("CreateRepository"))
            {
                var name = EditorUtility.SaveFilePanel("New Repository",
                    "/Assets/Galahad/FmoViewer/Domain/PdbRepository", "", "asset");
                Debug.Log(Path.GetDirectoryName(name));
                var n=name.LastIndexOf("Assets", StringComparison.Ordinal);
                if (name.Length>0)
                {
                    var repository = CreateInstance<PdbRepository>();
                    _pdbRepository = repository;
                    AssetDatabase.CreateAsset(repository,name.Substring(n,name.Length-n));
                    AssetDatabase.Refresh();
                }
                
            }
            if (GUILayout.Button("PDBFile Download"))
            {
                var path = EditorUtility.OpenFilePanel("Download", "", "pdb");
                if (path.Length>0)
                {
                    _pdbRepository.ReadPdb(path);
                }
            }
            _pdb = EditorGUILayout.ObjectField("pdbfile", _pdb,typeof(Object));
            if (_pdb != null)
            {
                pdb = AssetDatabase.GetAssetPath(_pdb);
            }
            pdb = EditorGUILayout.TextField("pdbfile", pdb);
            _pdbRepository = (PdbRepository) EditorGUILayout.ObjectField("PdbRepository", _pdbRepository, typeof(PdbRepository));
            if (_pdbRepository)
            {
                if (GUILayout.Button("readpdb"))
                {
                    _pdbRepository.ReadPdb(pdb);
                }
                if (GUILayout.Button("cut"))
                {
                    _pdbRepository.AutoCut();
                }
                if (GUILayout.Button("residuecut"))
                {
                    if (_pdbRepository.Fragment.State.ResidueCut)
                    {
                        return;
                    }
                    _pdbRepository.NewAutoResidueCut();
                }

                templates = Directory.GetDirectories("Assets/Galahad/Contexts/FmoViewer/Data/templates");
                if (EditorGUILayout.DropdownButton(new GUIContent("templetes") ,FocusType.Passive))
                {
                    PopupWindow.Show(GUILayoutUtility.GetRect(0,0),new TempletePopUp());
                }
                
                if (GUILayout.Button(""))
                {
                    _pdbRepository.Save();
                }

                using (new GUILayout.HorizontalScope())
                {
                hetatmCut = GUILayout.Toolbar (hetatmCut, new string[]{ "hetemcut", "atomcut", "delete atom" });
                }
                if (hetatmCut==0)
                {
                    
                    scrolhetatm= EditorGUILayout.BeginScrollView(scrolhetatm,GUI.skin.box);
                    _pdbRepository.Fragment.FragmentHetatms.ToList().ForEach(x =>
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
                 if (hetatmCut==1)
                {
                    scrolatom= EditorGUILayout.BeginScrollView(scrolatom,GUI.skin.box);
                   _pdbRepository.Fragment.FragmentAtoms.ToList().ForEach(x =>
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
                   EditorGUILayout.EndScrollView();
                }

                 if (hetatmCut==2)
                 {
                     scroldelete= EditorGUILayout.BeginScrollView(scroldelete,GUI.skin.box);
                     _pdbRepository.Fragment.FragmentAtoms.ToList().ForEach(x =>
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
                                 atom.Select=EditorGUILayout.Toggle(atom.Select);
                                 if (atom.Select)
                                 {
                                     if (GUILayout.Button("delete")&&EditorUtility.DisplayDialog("delete Atom ??", "Are you really delete Atom?",
                                             "delete","no"))
                                     {
                                         atom.Select = false;
                                         x.Atoms.Remove(atom);
                                         return;
                                     }
                                 }
                                 EditorGUILayout.EndHorizontal();
                             });
                             EditorGUILayout.EndVertical();
                         }
                     });
                         EditorGUILayout.EndScrollView();
                 }
            }
        }
    }
}