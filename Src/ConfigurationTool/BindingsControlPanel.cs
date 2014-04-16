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

namespace ConfigurationTool {
    public partial class BindingsControlPanel : UserControl {
        private IEnumerable<Type> mInterfaces;
        private List<Type> mMultiInterfaces = new List<Type>();
        private List<Type> mExclusiveInterfaces = new List<Type>();
        private Dictionary<Assembly, List<Binding>> mBindings = new Dictionary<Assembly, List<Binding>>();

        public BindingsControlPanel() {
            InitializeComponent();
        }

        public void LoadDocument(string bindingsFile) {
            XmlDocument doc = new XmlDocument();
            doc.Load(bindingsFile);

            foreach (var node in doc.GetElementsByTagName("bind").OfType<XmlElement>()) {
                if (node.ParentNode.NodeType != XmlNodeType.Comment) {
                    Binding binding = mBindings.Values.SelectMany(g => g).FirstOrDefault(b => b.Matches(node));
                    if (binding != null)
                        binding.Item.Checked = true;
                }
            }
        }

        private void InitialiseInterfaces() {
            string folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var files = Directory.GetFiles(folder);

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


                    mBindings[assembly].Add(new Binding(assembly, clazz, intrface, it));

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
            public bool Bound;
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

                service.Value = Service;
                to.Value = To;

                node.Attributes.Append(service);
                node.Attributes.Append(to);

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

        }
    }
}

