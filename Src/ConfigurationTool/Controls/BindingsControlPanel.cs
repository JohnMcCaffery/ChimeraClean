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
        private Dictionary<Assembly, List<Binding>> mBindings = new Dictionary<Assembly, List<Binding>>();
        private Dictionary<ListViewItem, Binding> mBindingsByItem = new Dictionary<ListViewItem, Binding>();
        private XmlDocument mDocument;

        public static bool Loading = false;

        public BindingsControlPanel() {
            InitializeComponent();
        }

        public void LoadDocument(string bindingsFile) {
            mDocument = new XmlDocument();
            mDocument.Load(bindingsFile);

            Loading = true;

            foreach (var node in mDocument.GetElementsByTagName("bind").OfType<XmlElement>()) {
                if (node.ParentNode.NodeType != XmlNodeType.Comment) {
                    Binding binding = mBindings.Values.SelectMany(g => g).FirstOrDefault(b => b.Matches(node));
                    if (binding != null)
                        binding.Item.Checked = true;
                }
            }

            Loading = false;
        }

        private void InitialiseInterfaces() {
            string folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);

            //Iterate through every assembly in the folder where the tool is running
            foreach (var assembly in 
                Directory.GetFiles(folder).
                Where(f => Path.GetExtension(f).ToUpper() == ".DLL").
                Select(f => {
                    try {
                        return Assembly.LoadFile(f);
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

                    if (g == null) {
                        g = new ListViewGroup(Path.GetFileNameWithoutExtension(assembly.Location));
                        BindingsList.Groups.Add(g);
                        mBindings.Add(assembly, new List<Binding>());
                    }

                    ListViewItem it = new ListViewItem(g);

                    it.SubItems.Add(new ListViewItem.ListViewSubItem(it, clazz.Name));
                    it.SubItems.Add(new ListViewItem.ListViewSubItem(it, intrface.Name));

                    var fields = clazz.GetFields();
                    FieldInfo details = clazz.GetFields().FirstOrDefault(f => f.Name == "Details");

                    if (details != null)
                        it.SubItems.Add(new ListViewItem.ListViewSubItem(it, details.GetValue(null).ToString()));

                    Binding binding = new Binding(assembly, clazz, intrface, it);
                    mBindings[assembly].Add(binding);
                    mBindingsByItem.Add(it, binding);

                    BindingsList.Items.Add(it);
                }
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
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

            InitialiseInterfaces();
        }

        private class Binding {
            public XmlNode Node;
            public Assembly Assembly;
            public Type Interface;
            public Type Class;
            public ListViewItem Item;

            public Binding(Assembly assembly, Type clazz, Type intrface, ListViewItem it) {
                Assembly = assembly;
                Class = clazz;
                Interface = intrface;
                Item = it;
            }


            public string Service {
                get { return Interface.FullName + ", " + Path.GetFileNameWithoutExtension(Interface.Assembly.Location); }
            }
            public string To {
                get { return Class.FullName + ", " + Path.GetFileNameWithoutExtension(Class.Assembly.Location); }
            }

            public XmlNode CreateNode(XmlDocument doc) {
                XmlNode node =doc.CreateElement("bind");

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
                    return "                        " + Path.GetFileNameWithoutExtension(Assembly.Location).Replace("Lib", "").ToUpper() + " BINDINGS                         ";
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


            internal void CheckedChanged(XmlDocument doc) {
                if (Item.Checked)
                    Enable(doc);
                else
                    Disable(doc);
            }
        }

        private void loadFileButton_Click(object sender, EventArgs e) {
            LoadDocument("Configs/Common/BindingsTest.xml");
        }

        private void BindingsList_ItemChecked(object sender, ItemCheckedEventArgs e) {
            if (mDocument != null)
                mBindingsByItem[e.Item].CheckedChanged(mDocument);
        }

        private void button1_Click(object sender, EventArgs e) {
            if (mDocument != null)
                mDocument.Save("Configs/Common/BindingsTest.xml");
        }

        internal IEnumerable<Type> GetBoundClasses<Interface>() {
            Type t = typeof(Interface);
            return mBindingsByItem.Values.Where(b => b.Interface == t).Select(b => b.Class);
        }
    }
}

