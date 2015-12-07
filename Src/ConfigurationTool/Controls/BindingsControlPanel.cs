using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera;
using Chimera.Interfaces.Overlay;
using Chimera.Interfaces;
using System.Reflection;
using System.IO;
using System.Xml;
using Chimera.Config;
using Chimera.ConfigurationTool.Controls;

namespace ConfigurationTool.Controls {
    public partial class BindingsControlPanel : UserControl {
        private IEnumerable<Type> mInterfaces;
        private List<Type> mMultiInterfaces = new List<Type>();
        private List<Type> mExclusiveInterfaces = new List<Type>();
        private Dictionary<string, List<Binding>> mBindings = new Dictionary<string, List<Binding>>();
        private Dictionary<ListViewItem, Binding> mBindingsByItem = new Dictionary<ListViewItem, Binding>();
        private XmlDocument mDocument;
        private string mFile;

        public static bool Loading = false;

        public BindingsControlPanel() {
            InitializeComponent();

            mMultiInterfaces.Add(typeof(ISystemPlugin));
            mMultiInterfaces.Add(typeof(IFeatureFactory));
            mMultiInterfaces.Add(typeof(IFeatureTransitionFactory));
            mMultiInterfaces.Add(typeof(ITriggerFactory));
            mMultiInterfaces.Add(typeof(ISelectionRendererFactory));
            mMultiInterfaces.Add(typeof(ITransitionStyleFactory));
            mMultiInterfaces.Add(typeof(IStateFactory));
            mMultiInterfaces.Add(typeof(IAxis));

            mExclusiveInterfaces.Add(typeof(IOutputFactory));
            mExclusiveInterfaces.Add(typeof(IMediaPlayer));

            mInterfaces = mExclusiveInterfaces.Concat(mMultiInterfaces);
        }

        public BindingsControlPanel(string folder) : this() {
            mFile = Path.GetFullPath(Path.Combine(folder, "Bindings.xml"));

            //InitialiseInterfaces();
            //LoadDocument();
            loader.DoWork += Startup;
            //loader.RunWorkerAsync();
        }

        private void Startup(object source, DoWorkEventArgs args) {
            InitialiseInterfaces();
            LoadDocument();
        }


        public void LoadDocument() {
            mDocument = new XmlDocument();
            if (File.Exists(mFile)) {
                mDocument.Load(mFile);

                Loading = true;

                foreach (var node in mDocument.GetElementsByTagName("bind").OfType<XmlElement>()) {
                    if (node.ParentNode.NodeType != XmlNodeType.Comment) {
                        Binding binding = mBindings.Values.SelectMany(g => g).FirstOrDefault(b => b.Matches(node));
                        if (binding != null)
                            Invoke(new Action(() => binding.Item.Checked = true));
                    }
                }

                Loading = false;
            }
        }

        private class InterfaceComparer : IEqualityComparer<Type> {
            public bool Equals(Type x, Type y) {
                return x.Name == y.Name;
            }

            public int GetHashCode(Type obj) {
                return obj.Name.GetHashCode();
            }
        }

        private static InterfaceComparer sInterfaceComparer = new InterfaceComparer();
        private static HashSet<String> sFailedAssemblies = new HashSet<String>();

        private IEnumerable<Type> LoadTypes(Assembly assembly) {
            try {
                return assembly.GetTypes().
                            Where(t =>
                                !t.IsAbstract &&
                                !t.IsInterface &&
                                t.GetInterfaces().Intersect(mInterfaces, sInterfaceComparer).Count() > 0).
                                OrderBy(t => t.Name).
                                OrderBy(t => t.GetInterfaces()[0].Name);
            }
            catch (ReflectionTypeLoadException e) {
                if (!sFailedAssemblies.Contains(assembly.FullName)) {
                    sFailedAssemblies.Add(assembly.FullName);
                    string assemblyName = assembly.FullName.Split(',')[0];
                    string msg = "Problem loading types for " + assemblyName + ". " + e.Message;

                    foreach (var ex in e.LoaderExceptions)
                        msg += ex.Message;
                    Console.WriteLine(msg);

                    MessageBox.Show(msg, assemblyName + " Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return new Type[0];
            }
        }

        public void InitialiseInterfaces() {
            mBindings.Clear();

            string folder = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            folder = Path.GetFullPath(Path.Combine(folder, "../"));

            var assemblies = ConfigurationFolderPanel.LoadAssemblies(folder);

            Console.WriteLine("\n\n\nProcessing " + Path.GetDirectoryName(mFile) + ".");

            //Iterate through every assembly in the folder where the tool is running
            foreach (var assembly in assemblies) {
                ListViewGroup g = null;

                string assemblyName = assembly.FullName.Split(',')[0];

                //Iterate through every class which implements one of the interfaces on the interfaces list
                var types = LoadTypes(assembly);


                Console.WriteLine("Loading {1,3} interface implementations from {0}.", assemblyName, types.Count());

                //Iterate through every class which implements one of the interfaces on the interfaces list
                foreach (var clazz in types) {

                    var intrface = clazz.GetInterfaces().Intersect(mInterfaces, sInterfaceComparer).First();

                    Invoke(new Action(() => {
                        if (g == null) {
                            g = new ListViewGroup(assemblyName);
                            BindingsList.Groups.Add(g);
                            mBindings.Add(assemblyName, new List<Binding>());
                        }

                        ListViewItem it = new ListViewItem(g);

                        it.SubItems.Add(new ListViewItem.ListViewSubItem(it, clazz.Name));
                        it.SubItems.Add(new ListViewItem.ListViewSubItem(it, intrface.Name));

                        var fields = clazz.GetFields();
                        FieldInfo details = clazz.GetFields().FirstOrDefault(f => f.Name == "Details");

                        if (details != null)
                            it.SubItems.Add(new ListViewItem.ListViewSubItem(it, details.GetValue(null).ToString()));

                        Binding binding = new Binding(assemblyName, clazz, intrface, it);
                        mBindings[assemblyName].Add(binding);
                        mBindingsByItem.Add(it, binding);

                        BindingsList.Items.Add(it);
                    }));
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
            InitialiseInterfaces();
        }

        private class Binding {
            public XmlNode Node;
            public string AssemblyName;
            public Type Interface;
            public Type Class;
            public ListViewItem Item;
            private string mInterfaceName;
            private string mClassName;

            public Binding(string assemblyName, Type clazz, Type intrface, ListViewItem it) {
                AssemblyName = assemblyName;
                Class = clazz;
                Interface = intrface;
                Item = it;
                
                mClassName = Class.FullName + ", " + AssemblyName;
                mInterfaceName = Interface.FullName + ", " + Interface.Assembly.FullName.Split(',')[0];
            }
            
            //Merge conflict - dunno which version is correct
	    public string Service {
                //get { return mInterfaceName; }
                get { return Interface.FullName + ", " + Path.GetFileNameWithoutExtension(Interface.Assembly.Location); }
            }
            public string To {
                //get { return mClassName; }
                get { return Class.FullName + ", " + AssemblyName; }
            }


            public XmlNode CreateNode(XmlDocument doc) {
                XmlNode node = doc.CreateElement("bind");

                XmlAttribute service = doc.CreateAttribute("service");
                XmlAttribute to = doc.CreateAttribute("to");
                XmlAttribute scope = doc.CreateAttribute("scope");

                service.Value = Service;
                to.Value = To;
                scope.Value = "singleton";

                node.Attributes.Append(service);
                node.Attributes.Append(to);
                node.Attributes.Append(scope);

                Node = node;
                return node;
            }

            public bool Matches(XmlNode node) {
                XmlAttribute service = node.Attributes["service"];
                XmlAttribute to = node.Attributes["to"];

                if (service == null || to == null)
                    return false;

                bool match = service.Value == Service && to.Value == To;
                if (match)
                    Node = node;

                return match;
            }

            public string HeaderCommentStr {
                get {
                    return "                        " + AssemblyName.Replace("Lib", "").ToUpper() + " BINDINGS                         ";
                }
            }

            public void Enable(XmlDocument doc) {
                if (BindingsControlPanel.Loading)
                    return;

                if (Node == null)
                    CreateNode(doc);

                XmlComment comment = doc.FirstChild.ChildNodes.OfType<XmlComment>().FirstOrDefault(c => c.InnerText == HeaderCommentStr);
                if (comment == null) {
                    comment = doc.CreateComment(HeaderCommentStr);
                    doc.FirstChild.AppendChild(comment);
                }

                doc.FirstChild.InsertAfter(Node, comment);
            }

            public void Disable(XmlDocument doc) {
                if (Node != null)
                    doc.FirstChild.RemoveChild(Node);
            }


            internal void CheckedChanged(XmlDocument doc, string file) {
                if (Item.Checked)
                    Enable(doc);
                else
                    Disable(doc);
                doc.Save(file);
            }

            public bool IsBound {
                get { return Item != null && Item.Checked; }
            }
        }

        private void BindingsList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (mDocument != null) {
                if (!File.Exists(mFile))
                    CreateFile();

                mBindingsByItem[e.Item].CheckedChanged(mDocument, mFile);
            }
        }

        internal IEnumerable<Type> GetBoundClasses<Interface>() {
            Type t = typeof(Interface);
            return mBindingsByItem.Values.Where(b => b.IsBound && b.Interface == t).Select(b => b.Class);
        }

        private void CreateFile() {
            File.Create(mFile).Close();

            XmlElement root = mDocument.CreateElement("module");
            XmlAttribute nameAttr = mDocument.CreateAttribute("name");
            nameAttr.Value = "ChimeraBindings";

            root.Attributes.Append(nameAttr);
            mDocument.AppendChild(root);

        }
    }
}

