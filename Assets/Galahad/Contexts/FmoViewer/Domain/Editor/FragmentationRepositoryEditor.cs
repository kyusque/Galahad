using UnityEditor;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    [CustomEditor(typeof(FragmentationRepository))]
    public class FragmentationRepositoryEditor:UnityEditor.Editor
    {
        public FragmentationRepository fragmentationRepository;
        [SerializeField] private string templete;
        [SerializeField] private Object Templete;

        private void OnEnable()
        {
            fragmentationRepository=(FragmentationRepository)target;
        }

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("import"))
            {
                fragmentationRepository.NewAutoResidueCut(fragmentationRepository.PdbRepository.Pdb);
            }
            if (GUILayout.Button("Residuecut"))
            {
                if (fragmentationRepository.Fragment.State.ResidueCut)
                {
                    return;
                }
                fragmentationRepository.NewAutoResidueCut();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("cut"))
            {
                var window= EditorWindow.GetWindow<CutWindow>();
                window.Fragment=fragmentationRepository.Fragment;
            }
            EditorGUILayout.EndHorizontal();
            Templete = EditorGUILayout.ObjectField("templete",Templete, typeof(Object));
            if (Templete!=null)
            {
                templete = AssetDatabase.GetAssetPath(Templete);
            }
            if (GUILayout.Button("saveajf"))
            {
                fragmentationRepository.Save(templete);
            }
//            if (GUILayout.Button("AutCut"))
//            {
//                fragmentationRepository.AutoCut(fragmentationRepository.PdbRepository.Pdb);
//            }
//
//            if (GUILayout.Button("ResidueCut"))
//            {
//                fragmentationRepository.AutoResidueCut();
//            }
            base.OnInspectorGUI();
            
        }
    }
}