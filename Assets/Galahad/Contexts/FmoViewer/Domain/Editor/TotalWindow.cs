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
        private  string templete="Assets/Galahad/Contexts/FmoViewer/Data/templetes/sample.ajf";
        public string Templete1
        {
            get => templete;
            set => templete = value;
        }
        private string basis_set;

        public string BasisSet
        {
            get => basis_set;
            set => basis_set = value;
        }
        [SerializeField] private Object Templete=AssetDatabase.LoadAssetAtPath<Object>("Assets/Galahad/Contexts/FmoViewer/Data/templetes/sample.ajf");
        private Vector2 scrolhetatm,scrolatom,scroldelete;
        private int hetatmCut,atomCut;
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
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("templete",GUILayout.ExpandWidth(false));
                if (EditorGUILayout.DropdownButton(new GUIContent(templete) ,FocusType.Passive))
                {
////                     PopupWindow.Show(GUILayoutUtility.GetRect(0,0),new TempletePopUp());
                    var window = GetWindow<menuex>().Inject(this);
                    window.position = this.position;
                    window.Show();
                    
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.BeginHorizontal();
                templete = EditorGUILayout.TextField(templete);
                Templete = AssetDatabase.LoadAssetAtPath<Object>(templete);
                Templete = EditorGUILayout.ObjectField(Templete, typeof(Object));
                if (Templete!=null)
                {
                    templete = AssetDatabase.GetAssetPath(Templete);
                }
                EditorGUILayout.EndVertical();
                if (GUILayout.Button("save"))
                {
                    _pdbRepository.Save(templete);
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

    public class menuex : EditorWindow
    {
        [SerializeField] Color m_Color = Color.white;
        [SerializeField] private string templete="Assets/Galahad/Contexts/FmoViewer/Data/templetes/sample.ajf";
        [SerializeField] private string basis_set;
        private TotalWindow _totalWindow;


        void OnEnable()
        {
            titleContent = new GUIContent("preset");
        }

        public menuex Inject(TotalWindow totalWindow)
        {
            _totalWindow = totalWindow;
            return this;
        }

        // a method to simplify adding menu items
        void AddMenuItemForColor(GenericMenu menu, string menuPath, Color color)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            menu.AddItem(new GUIContent(menuPath), m_Color.Equals(color), OnColorSelected, color);
        }

        // the GenericMenu.MenuFunction2 event handler for when a menu item is selected
        void OnColorSelected(object color)
        {
            m_Color = (Color) color;
        }
        void AddMenuItemForTemplete(GenericMenu menu, string menuPath, string templetepath)
        {
            // the menu item is marked as selected if it matches the current value of m_Color
            menu.AddItem(new GUIContent(menuPath),templete.Equals(templetepath),OnTempleteSelected, templetepath);
        }
        void OnTempleteSelected(object userData)
        {
            _totalWindow.Templete1 = userData.ToString();
            templete =  userData.ToString();
        }

        void OnGUI()
        {
            // set the GUI to use the color stored in m_Color
            GUI.color = m_Color;
            EditorGUILayout.LabelField(templete);

            // display the GenericMenu when pressing a button
            if (GUILayout.Button("Select templete"))
            {
                // create the menu and add items to it
                GenericMenu menu = new GenericMenu();

                // forward slashes nest menu items under submenus
                AddMenuItemForColor(menu, "RGB/Red", Color.red);
                AddMenuItemForColor(menu, "RGB/Green", Color.green);
                AddMenuItemForColor(menu, "RGB/Blue", Color.blue);

                // an empty string will create a separator at the top level
                menu.AddSeparator("");

                AddMenuItemForColor(menu, "CMYK/Cyan", Color.cyan);
                AddMenuItemForColor(menu, "CMYK/Yellow", Color.yellow);
                AddMenuItemForColor(menu, "CMYK/Magenta", Color.magenta);
                // a trailing slash will nest a separator in a submenu
                menu.AddSeparator("CMYK/");
                AddMenuItemForColor(menu, "CMYK/Black", Color.black);

                menu.AddSeparator("");

                AddMenuItemForColor(menu, "White", Color.white);
                menu.AddSeparator("");
                var templetes=Directory.GetFiles("Assets/Galahad/Contexts/FmoViewer/Data/templetes/","*.ajf");
                foreach (var temp in templetes)
                {
                    AddMenuItemForTemplete(menu,Path.GetFileNameWithoutExtension(temp),temp);
                            
                }

                // display the menu
                menu.ShowAsContext();
            }
            if (GUILayout.Button("basis_set"))
            {
                // create the menu and add items to it
                GenericMenu menu = new GenericMenu();

                // forward slashes nest menu items under submenus
                AddMenuItemForColor(menu, "RGB/Red", Color.red);
                AddMenuItemForColor(menu, "RGB/Green", Color.green);
                AddMenuItemForColor(menu, "RGB/Blue", Color.blue);

                // an empty string will create a separator at the top level
                menu.AddSeparator("");

                AddMenuItemForColor(menu, "CMYK/Cyan", Color.cyan);
                AddMenuItemForColor(menu, "CMYK/Yellow", Color.yellow);
                AddMenuItemForColor(menu, "CMYK/Magenta", Color.magenta);
                // a trailing slash will nest a separator in a submenu
                menu.AddSeparator("CMYK/");
                AddMenuItemForColor(menu, "CMYK/Black", Color.black);

                menu.AddSeparator("");

                AddMenuItemForColor(menu, "White", Color.white);
                menu.AddSeparator("");
                

                // display the menu
                menu.ShowAsContext();
            }
        }
    }
    
}