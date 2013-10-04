using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces.Overlay;
using Chimera.GUI.Controls;

namespace Chimera.Overlay.GUI.Plugins {
    public partial class OverlayPluginPanel : UserControl {
        private readonly Dictionary<string, StatisticsPanel> mStatsPanels = new Dictionary<string, StatisticsPanel>();
        private OverlayPlugin mOverlayPlugin;
        private StatisticsPanel mCurrentPanel;

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
            foreach (var factory in overlayPlugin.GetFactories<IFeatureTransition>())
                imageTransitionFactoriesList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<ITransitionStyle>())
                transitionStyleFactoriesList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<IFeature>())
                featuresFactoryList.Items.Add(factory);
            foreach (var factory in overlayPlugin.GetFactories<ISelectionRenderer>())
                selectionRendererFactoriesList.Items.Add(factory);

#if DEBUG
            foreach (var frameManager in mOverlayPlugin.OverlayManagers) {
                StatisticsPanel p = new StatisticsPanel(frameManager.Statistics, mOverlayPlugin.Core);
                p.Dock = DockStyle.Fill;
                mStatsPanels.Add(frameManager.Name, p);

                TabPage page = new TabPage();
                page.Text = frameManager.Name;
                page.Name = frameManager.Name;
                page.Controls.Add(p);

                statsTabs.Controls.Add(page);
            }
#endif
        }

        void mOverlayPlugin_StateChanged(State state) {
            Invoke(() => stateList.SelectedItem = state);
        }

        private void Invoke(Action a) {
            if (!InvokeRequired)
                a();
            else if (Created && !IsDisposed && !Disposing)
                base.BeginInvoke(a);
        }

        private void changeStateButton_Click(object sender, EventArgs e) {
            if (stateList.SelectedItem != null)
                mOverlayPlugin.CurrentState = (State) stateList.SelectedItem;
        }

        private void stateList_DoubleClick(object sender, EventArgs e) {
            changeStateButton_Click(sender, e);
        }

        private void mainTab_TabIndexChanged(object sender, EventArgs e) {
            if (mainTab.SelectedTab == infoTab)
                stateList.SelectedItem = mOverlayPlugin.CurrentState;
        }

        private void mainTab_VisibleChanged(object sender, EventArgs e) {
            mainTab_TabIndexChanged(sender, e);
        }

        private void launchButton_Click(object sender, EventArgs e) {
            mOverlayPlugin.LaunchOverlays();
        }

        private void opacitySlider_Scroll(object sender, EventArgs e) {
            mOverlayPlugin.Opacity = opacitySlider.Value / 100.0;
        }

#if DEBUG
        private void statsTabs_SelectedIndexChanged(object sender, EventArgs e) {
            if (mCurrentPanel != null)
                mCurrentPanel.Active = false;

            if (mainTab.SelectedTab == statsTab) {
                mCurrentPanel = mStatsPanels[statsTabs.SelectedTab.Name];
                mCurrentPanel.Active = true;
            }
        }
#endif
    }
}
