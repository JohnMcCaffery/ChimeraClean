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

        public void InitialiseInterfaces() {
            string folder = Path.GetDirectoryName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
            folder = Path.GetFullPath(Path.Combine(folder, "../"));

            string assemblyName = "";
            //Iterate through every assembly in the folder where the tool is running
            foreach (var assembly in 
                Directory.GetFiles(folder).
                Where(f => 
                    Path.GetExtension(f).ToUpper() == ".DLL" && 
                    !f.Contains("NuiLib") && 
                    !f.Contains("opencv") && 
                    !f.Contains("openjpeg") && 
                    !f.Contains("SlimDX")).
                Select(f => {
                    try {
                        /*
                        string copy = Path.Combine(Environment.CurrentDirectory, Path.GetFileName(f));
                        File.Copy(f, copy);
                        return Assembly.LoadFile(copy);
                        */
                        assemblyName = Path.GetFileNameWithoutExtension(f);
                        return Assembly.Load(File.ReadAllBytes(f));
                    } catch (Exception e) {
                        return null;
                    }
            }).Where(a => a != null)) {
                ListViewGroup g = null;

                //Iterate through every class which implements one of the interfaces on the interfaces list
                foreach (var clazz in 
                    assembly.GetTypes().
                    Where(t => 
                        !t.IsAbstract && 
                        !t.IsInterface && 
                        t.GetInterfaces().Intersect(mInterfaces).Count() > 0).
                        OrderBy(t => t.Name).
                        OrderBy(t => t.GetInterfaces()[0].Name)) {

                    var intrface = clazz.GetInterfaces().Intersect(mInterfaces).First();

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

            public Binding(string assemblyName, Type clazz, Type intrface, ListViewItem it) {
                AssemblyName = assemblyName;
                Class = clazz;
                Interface = intrface;
                Item = it;
            }


            public string Service {
                get { return Interface.FullName + ", " + Path.GetFileNameWithoutExtension(Interface.Assembly.Location); }
            }
            public string To {
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

