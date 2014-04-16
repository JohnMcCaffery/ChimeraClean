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

namespace ConfigurationTool {
    public partial class BindingsControlPanel : UserControl {
        private List<Type> mInterfaces = new List<Type>();
        private List<Type> mExclusiveInterfaces = new List<Type>();

        public BindingsControlPanel() {
            InitializeComponent();

            //InitialiseInterfaces();
        }

        private void InitialiseInterfaces() {
            string folder = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            var files = Directory.GetFiles(folder);
            var assemblyFiles = files.Where(f => {
                string ex = Path.GetExtension(f).ToUpper();
                return ex == "DLL";
            });
            var assemblies = Directory.GetFiles(folder).Where(f => Path.GetExtension(f).ToUpper() == "DLL").Select(f => Assembly.LoadFile(f));
            foreach (var assembly in Directory.GetFiles(folder).Where(f => Path.GetExtension(f).ToUpper() == "DLL").Select(f => Assembly.LoadFile(f))) {
                Console.WriteLine(assembly.Location);
            }
        }

        private void loadButton_Click(object sender, EventArgs e) {
            mInterfaces.Add(typeof(ISystemPlugin));
            mInterfaces.Add(typeof(IFeatureFactory));
            mInterfaces.Add(typeof(IFeatureTransitionFactory));
            mInterfaces.Add(typeof(ITriggerFactory));
            mInterfaces.Add(typeof(ISelectionRendererFactory));
            mInterfaces.Add(typeof(ITransitionStyleFactory));
            mInterfaces.Add(typeof(IStateFactory));
            mInterfaces.Add(typeof(IAxis));

            mExclusiveInterfaces.Add(typeof(IOutputFactory));
            mExclusiveInterfaces.Add(typeof(IMediaPlayer));

            InitialiseInterfaces();
        }
    }
}
