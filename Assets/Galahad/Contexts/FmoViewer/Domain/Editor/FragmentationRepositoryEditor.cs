using UnityEditor;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    [CustomEditor(typeof(FragmentationRepository))]
    public class FragmentationRepositoryEditor:UnityEditor.Editor
    {
        public FragmentationRepository fragmentationRepository;
        

        private void OnEnable()
        {
            fragmentationRepository=(FragmentationRepository)target;
        }

        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("newcut"))
            {
                fragmentationRepository.NewAutoResidueCut(fragmentationRepository.PdbRepository.Pdb);
            }
            if (GUILayout.Button("AutCut"))
            {
                fragmentationRepository.AutoCut(fragmentationRepository.PdbRepository.Pdb);
            }

            if (GUILayout.Button("ResidueCut"))
            {
                fragmentationRepository.AutoResidueCut();
            }
            base.OnInspectorGUI();
            
        }
    }
}