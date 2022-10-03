using System;
using System.IO;
using System.Linq;
using Galahad.Scripts;
using UnityEditor;
using UnityEngine;

namespace Galahad.Editor
{
    public class Cli
    {
        
        public static void CreateMoleculeRepository()
        {
            try
            {
                var args = TrimArgs(System.Environment.GetCommandLineArgs());
                if (args.Length == 0)
                {
                    throw new Exception();
                }
                var moleculeRepository = ScriptableObject.CreateInstance<MoleculeRepository>();
                AssetDatabase.CreateAsset(moleculeRepository, Path.Combine("Assets/Galahad/Editor/Outputs", args[0]));
                AssetDatabase.SaveAssets();
            }
            catch
            {
                Debug.LogError("Usage: [command] PATH");
            }
        }
        
        private static string[] TrimArgs(string[] args)
        {
            var argsList = args.ToList();
            argsList.RemoveAt(0);
            while (argsList[0].StartsWith("-"))
            {
                argsList.RemoveAt(0);
                if (!argsList[0].StartsWith("-"))
                {
                    argsList.RemoveAt(0);
                }
            }
            return argsList.ToArray();
        }
    }
}