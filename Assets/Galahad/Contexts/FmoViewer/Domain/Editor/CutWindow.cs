using Galahad.Contexts.FmoViewer.Domain.PdbAggregate;
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
        private void OnGUI()
        {
            EditorGUILayout.LabelField("cut");
            Fragmentation =
                EditorGUILayout.ObjectField(Fragmentation,typeof(Object))as FragmentationRepository ;
            if (Fragmentation == null) return;
            scrol= EditorGUILayout.BeginScrollView(scrol,GUI.skin.box);
            Fragmentation.Fragment.FragmentAtoms.ToList().ForEach(x =>
            {
                x.Init();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{x.FragmentId.ToString(),6}"+$"{x.ResidueName,5}",GUILayout.Width(80));
                x.State.ResidueCut = EditorGUILayout.Toggle( x.State.ResidueCut);
                EditorGUILayout.EndHorizontal();
            });
            Fragmentation.Fragment.FragmentHetatms.ToList().ForEach(x =>
            {
                x.Init();
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{x.FragmentId.ToString(),6}"+$"{x.Name,5}",GUILayout.Width(80));
                x.State.ResidueCut = EditorGUILayout.Toggle( x.State.ResidueCut);
                EditorGUILayout.EndHorizontal();
            });
            EditorGUILayout.EndScrollView();
        }
    }
}