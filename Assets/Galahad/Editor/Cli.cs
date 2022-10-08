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

        public static void AddSmilesIntoMoleculeRepository()
        {
            var args = TrimArgs(System.Environment.GetCommandLineArgs());
            if (args.Length < 2)
            {
                throw new Exception();
            }
            Debug.Log(args[0]);
            Debug.Log(args[1]);
            var moleculeRepository = AssetDatabase.LoadAssetAtPath<MoleculeRepository>(args[0]);
            var moleculeRepositoryCopy = GameObject.Instantiate<MoleculeRepository>(moleculeRepository);
            moleculeRepositoryCopy.AddMoleculeFromSmiles(args[1]);
            var tempFile =  Path.GetFileNameWithoutExtension(args[0]) + "_org";
            AssetDatabase.RenameAsset(args[0], tempFile);
            AssetDatabase.CreateAsset(moleculeRepositoryCopy, args[0]);
            AssetDatabase.SaveAssets();
            AssetDatabase.DeleteAsset(Path.Combine(Path.GetDirectoryName(args[0]), tempFile + ".asset"));
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