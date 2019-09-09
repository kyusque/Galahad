using System.Reflection;
using System.Reflection.Emit;
using UnityEditor;
using UnityEngine;

namespace Galahad.Contexts.FmoViewer.Domain.Editor
{
    public class TempletePopUp:PopupWindowContent
    {
        private enum MenuType
        {
            Play,
            Pause,
            Resume,
            Stop,
        }

        private float WindowWidth { get; } = 200;
        private float WindowHeight { 
            get {
                var height = 0.0f;
                var singleLineHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                // タイトル部分
                height += singleLineHeight;
                // ボタン部分
                height += System.Enum.GetValues(typeof(MenuType)).Length * singleLineHeight;
                // 最下部の余白
                height += EditorGUIUtility.standardVerticalSpacing;

                return height;
            }
        }

        /// <summary>
        /// サイズを取得する
        /// </summary>
        public override Vector2 GetWindowSize() => new Vector2(WindowWidth, WindowHeight);
        public override void OnGUI(Rect rect)
        {
            
        }

        static System.Type BuildEnum(string[] strings)
        {
            AssemblyName assemblyName=new AssemblyName{Name = "myAssembly"};
            AssemblyName asmName = new AssemblyName{ Name = "MyAssembly" };
            System.AppDomain domain = System.AppDomain.CurrentDomain;
            AssemblyBuilder asmBuilder = domain.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = asmBuilder.DefineDynamicModule("MyModule");
            EnumBuilder enumBuilder = moduleBuilder.DefineEnum("MyNamespace.MyEnum", TypeAttributes.Public, typeof(int));

            for (int i = 0; i < strings.Length; ++i)
                enumBuilder.DefineLiteral(strings[i], i + 1);

            return enumBuilder.CreateType();
        }
    }
    
}