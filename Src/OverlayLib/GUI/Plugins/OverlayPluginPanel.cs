using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;

namespace Chimera.Overlay.GUI.Plugins {
    public partial class OverlayPluginPanel : UserControl {
        private OverlayPlugin mOverlayPlugin;

        public OverlayPluginPanel() {
            InitializeComponent();
        }

        public OverlayPluginPanel(OverlayPlugin overlayPlugin)
            : this() {
            mOverlayPlugin = overlayPlugin;

            if (overlayPlugin.CurrentState != null)
                stateList.SelectedItem = overlayPlugin.CurrentState;

            mOverlayPlugin.StateChanged += new Action<State>(mOverlayPlugin_StateChanged);

            foreach (var state in mOverlayPlugin.States)
                stateList.Items.Add(state);

            stateSelector.Init(overlayPlugin.States);
            triggerSelector.Init(overlayPlugin.Triggers);
            imageTransitionSelector.Init(overlayPlugin.ImageTransitions);
            transitionStyleSelector.Init(overlayPlugin.Transitions);
            featureSelector.Init(overlayPlugin.Features);
            selectionRendererSelector.Init(overlayPlugin.Renderers);

            foreach (var factory in overlayPlugin.GetFactories<State>())
                stateFactoriesList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<ITrigger>())
                triggerFactoriesList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<IImageTransition>())
                imageTransitionFactoriesList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<ITransitionStyle>())
                transitionStyleFactoriesList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<IFeature>())
                featuresFactoryList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<ISelectionRenderer>())
                selectionRendererFactoriesList.Items.Add(factory);
        }

        void mOverlayPlugin_StateChanged(State state) {
            Invoke(() => stateList.SelectedItem = state);
        }

        private void Invoke(Action a) {
            if (!InvokeRequired)
                a();
            else if (Created && !IsDisposed && !Disposing)
                base.Invoke(a);
        }

        private void changeStateButton_Click(object sender, EventArgs e) {
            if (stateList.SelectedItem != null)
                mOverlayPlugin.CurrentState = (State) stateList.SelectedItem;
        }
    }
}
