using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
using Galahad.Contexts.FmoViewer.Domain.ValueObjects;
using Galahad.Contexts.FmoViewer.Presenter;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    public class CutWindow:EditorWindow
    {
        public FragmentationRepository Fragmentation;
        public Fragment Fragment;
        private Vector2 scrol;
        private bool giveatom;
        private bool givenatom;
        private void OnGUI()
        {
            if (Selection.gameObjects.Length>0)
            {
            foreach (var gameObject in Selection.gameObjects)
            {
                if (gameObject.CompareTag("Fragment"))
                {
                    var presenter = gameObject.GetComponent<FragmentPresenter>();
                    var x = presenter.Model;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField($"{x.FragmentId,6}"+$"{x.ResidueName,5}",GUILayout.Width(80));
                    EditorGUILayout.EndHorizontal();
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
            }
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
    }
}