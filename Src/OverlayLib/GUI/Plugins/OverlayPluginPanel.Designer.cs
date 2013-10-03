namespace Chimera.Overlay.GUI.Plugins {
    partial class OverlayPluginPanel {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.mainTab = new System.Windows.Forms.TabControl();
            this.infoTab = new System.Windows.Forms.TabPage();
            this.stateList = new System.Windows.Forms.ListBox();
            this.changeStateButton = new System.Windows.Forms.Button();
            this.windowsTab = new System.Windows.Forms.TabPage();
            this.opacitySlider = new System.Windows.Forms.TrackBar();
            this.launchButton = new System.Windows.Forms.Button();
            this.statesTab = new System.Windows.Forms.TabPage();
            this.triggersTab = new System.Windows.Forms.TabPage();
            this.windowTransitionsTab = new System.Windows.Forms.TabPage();
            this.imageTransitionsTab = new System.Windows.Forms.TabPage();
            this.rendererTab = new System.Windows.Forms.TabPage();
            this.featuresTab = new System.Windows.Forms.TabPage();
            this.factoriesTab = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.stateFactoriesList = new System.Windows.Forms.ListBox();
            this.transitionStyleFactoriesList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.splitContainer5 = new System.Windows.Forms.SplitContainer();
            this.triggerFactoriesList = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.selectionRendererFactoriesList = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer4 = new System.Windows.Forms.SplitContainer();
            this.imageTransitionFactoriesList = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            this.featuresFactoryList = new System.Windows.Forms.ListBox();
            this.label6 = new System.Windows.Forms.Label();
            this.stateSelector = new Chimera.Overlay.GUI.StateSelector();
            this.triggerSelector = new Chimera.Overlay.GUI.TriggerSelector();
            this.transitionStyleSelector = new Chimera.Overlay.GUI.TransitionStyleSelector();
            this.imageTransitionSelector = new Chimera.Overlay.GUI.ImageTransitionSelector();
            this.selectionRendererSelector = new Chimera.Overlay.GUI.SelectionRendererSelector();
            this.featureSelector = new Chimera.Overlay.GUI.FeatureSelector();
            this.mainTab.SuspendLayout();
            this.infoTab.SuspendLayout();
            this.windowsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).BeginInit();
            this.statesTab.SuspendLayout();
            this.triggersTab.SuspendLayout();
            this.windowTransitionsTab.SuspendLayout();
            this.imageTransitionsTab.SuspendLayout();
            this.rendererTab.SuspendLayout();
            this.featuresTab.SuspendLayout();
            this.factoriesTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).BeginInit();
            this.splitContainer5.Panel1.SuspendLayout();
            this.splitContainer5.Panel2.SuspendLayout();
            this.splitContainer5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).BeginInit();
            this.splitContainer4.Panel1.SuspendLayout();
            this.splitContainer4.Panel2.SuspendLayout();
            this.splitContainer4.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainTab
            // 
            this.mainTab.Controls.Add(this.infoTab);
            this.mainTab.Controls.Add(this.windowsTab);
            this.mainTab.Controls.Add(this.statesTab);
            this.mainTab.Controls.Add(this.triggersTab);
            this.mainTab.Controls.Add(this.windowTransitionsTab);
            this.mainTab.Controls.Add(this.imageTransitionsTab);
            this.mainTab.Controls.Add(this.rendererTab);
            this.mainTab.Controls.Add(this.featuresTab);
            this.mainTab.Controls.Add(this.factoriesTab);
            this.mainTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainTab.Location = new System.Drawing.Point(0, 0);
            this.mainTab.Name = "mainTab";
            this.mainTab.SelectedIndex = 0;
            this.mainTab.Size = new System.Drawing.Size(634, 400);
            this.mainTab.TabIndex = 0;
            this.mainTab.TabIndexChanged += new System.EventHandler(this.mainTab_TabIndexChanged);
            this.mainTab.VisibleChanged += new System.EventHandler(this.mainTab_VisibleChanged);
            // 
            // infoTab
            // 
            this.infoTab.Controls.Add(this.stateList);
            this.infoTab.Controls.Add(this.changeStateButton);
            this.infoTab.Location = new System.Drawing.Point(4, 22);
            this.infoTab.Name = "infoTab";
            this.infoTab.Padding = new System.Windows.Forms.Padding(3);
            this.infoTab.Size = new System.Drawing.Size(626, 374);
            this.infoTab.TabIndex = 0;
            this.infoTab.Text = "Info";
            this.infoTab.UseVisualStyleBackColor = true;
            // 
            // stateList
            // 
            this.stateList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stateList.DisplayMember = "Name";
            this.stateList.FormattingEnabled = true;
            this.stateList.Location = new System.Drawing.Point(6, 10);
            this.stateList.Name = "stateList";
            this.stateList.Size = new System.Drawing.Size(614, 329);
            this.stateList.TabIndex = 3;
            this.stateList.DoubleClick += new System.EventHandler(this.stateList_DoubleClick);
            // 
            // changeStateButton
            // 
            this.changeStateButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.changeStateButton.Location = new System.Drawing.Point(6, 345);
            this.changeStateButton.Name = "changeStateButton";
            this.changeStateButton.Size = new System.Drawing.Size(614, 23);
            this.changeStateButton.TabIndex = 2;
            this.changeStateButton.Text = "Change State";
            this.changeStateButton.UseVisualStyleBackColor = true;
            this.changeStateButton.Click += new System.EventHandler(this.changeStateButton_Click);
            // 
            // windowsTab
            // 
            this.windowsTab.Controls.Add(this.opacitySlider);
            this.windowsTab.Controls.Add(this.launchButton);
            this.windowsTab.Location = new System.Drawing.Point(4, 22);
            this.windowsTab.Name = "windowsTab";
            this.windowsTab.Padding = new System.Windows.Forms.Padding(3);
            this.windowsTab.Size = new System.Drawing.Size(626, 374);
            this.windowsTab.TabIndex = 8;
            this.windowsTab.Text = "Windows";
            this.windowsTab.UseVisualStyleBackColor = true;
            // 
            // opacitySlider
            // 
            this.opacitySlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.opacitySlider.Location = new System.Drawing.Point(6, 32);
            this.opacitySlider.Maximum = 100;
            this.opacitySlider.Name = "opacitySlider";
            this.opacitySlider.Size = new System.Drawing.Size(614, 45);
            this.opacitySlider.TabIndex = 1;
            this.opacitySlider.Scroll += new System.EventHandler(this.opacitySlider_Scroll);
            // 
            // launchButton
            // 
            this.launchButton.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.launchButton.Location = new System.Drawing.Point(6, 6);
            this.launchButton.Name = "launchButton";
            this.launchButton.Size = new System.Drawing.Size(614, 20);
            this.launchButton.TabIndex = 0;
            this.launchButton.Text = "Launch";
            this.launchButton.UseVisualStyleBackColor = true;
            this.launchButton.Click += new System.EventHandler(this.launchButton_Click);
            // 
            // statesTab
            // 
            this.statesTab.Controls.Add(this.stateSelector);
            this.statesTab.Location = new System.Drawing.Point(4, 22);
            this.statesTab.Name = "statesTab";
            this.statesTab.Padding = new System.Windows.Forms.Padding(3);
            this.statesTab.Size = new System.Drawing.Size(626, 374);
            this.statesTab.TabIndex = 1;
            this.statesTab.Text = "States";
            this.statesTab.UseVisualStyleBackColor = true;
            // 
            // triggersTab
            // 
            this.triggersTab.Controls.Add(this.triggerSelector);
            this.triggersTab.Location = new System.Drawing.Point(4, 22);
            this.triggersTab.Name = "triggersTab";
            this.triggersTab.Padding = new System.Windows.Forms.Padding(3);
            this.triggersTab.Size = new System.Drawing.Size(626, 374);
            this.triggersTab.TabIndex = 2;
            this.triggersTab.Text = "Triggers";
            this.triggersTab.UseVisualStyleBackColor = true;
            // 
            // windowTransitionsTab
            // 
            this.windowTransitionsTab.Controls.Add(this.transitionStyleSelector);
            this.windowTransitionsTab.Location = new System.Drawing.Point(4, 22);
            this.windowTransitionsTab.Name = "windowTransitionsTab";
            this.windowTransitionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.windowTransitionsTab.Size = new System.Drawing.Size(626, 374);
            this.windowTransitionsTab.TabIndex = 3;
            this.windowTransitionsTab.Text = "Window Transitions";
            this.windowTransitionsTab.UseVisualStyleBackColor = true;
            // 
            // imageTransitionsTab
            // 
            this.imageTransitionsTab.Controls.Add(this.imageTransitionSelector);
            this.imageTransitionsTab.Location = new System.Drawing.Point(4, 22);
            this.imageTransitionsTab.Name = "imageTransitionsTab";
            this.imageTransitionsTab.Padding = new System.Windows.Forms.Padding(3);
            this.imageTransitionsTab.Size = new System.Drawing.Size(626, 374);
            this.imageTransitionsTab.TabIndex = 4;
            this.imageTransitionsTab.Text = "Image Transitions";
            this.imageTransitionsTab.UseVisualStyleBackColor = true;
            // 
            // rendererTab
            // 
            this.rendererTab.Controls.Add(this.selectionRendererSelector);
            this.rendererTab.Location = new System.Drawing.Point(4, 22);
            this.rendererTab.Name = "rendererTab";
            this.rendererTab.Padding = new System.Windows.Forms.Padding(3);
            this.rendererTab.Size = new System.Drawing.Size(626, 374);
            this.rendererTab.TabIndex = 5;
            this.rendererTab.Text = "Selection Renderers";
            this.rendererTab.UseVisualStyleBackColor = true;
            // 
            // featuresTab
            // 
            this.featuresTab.Controls.Add(this.featureSelector);
            this.featuresTab.Location = new System.Drawing.Point(4, 22);
            this.featuresTab.Name = "featuresTab";
            this.featuresTab.Padding = new System.Windows.Forms.Padding(3);
            this.featuresTab.Size = new System.Drawing.Size(626, 374);
            this.featuresTab.TabIndex = 6;
            this.featuresTab.Text = "Features";
            this.featuresTab.UseVisualStyleBackColor = true;
            // 
            // factoriesTab
            // 
            this.factoriesTab.Controls.Add(this.splitContainer1);
            this.factoriesTab.Location = new System.Drawing.Point(4, 22);
            this.factoriesTab.Name = "factoriesTab";
            this.factoriesTab.Padding = new System.Windows.Forms.Padding(3);
            this.factoriesTab.Size = new System.Drawing.Size(626, 374);
            this.factoriesTab.TabIndex = 7;
            this.factoriesTab.Text = "Factories";
            this.factoriesTab.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer3);
            this.splitContainer1.Size = new System.Drawing.Size(620, 368);
            this.splitContainer1.SplitterDistance = 204;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.Controls.Add(this.stateFactoriesList);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.transitionStyleFactoriesList);
            this.splitContainer2.Panel2.Controls.Add(this.label2);
            this.splitContainer2.Size = new System.Drawing.Size(204, 368);
            this.splitContainer2.SplitterDistance = 100;
            this.splitContainer2.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "States";
            // 
            // stateFactoriesList
            // 
            this.stateFactoriesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.stateFactoriesList.DisplayMember = "Name";
            this.stateFactoriesList.FormattingEnabled = true;
            this.stateFactoriesList.Location = new System.Drawing.Point(3, 20);
            this.stateFactoriesList.Name = "stateFactoriesList";
            this.stateFactoriesList.Size = new System.Drawing.Size(94, 342);
            this.stateFactoriesList.TabIndex = 0;
            // 
            // transitionStyleFactoriesList
            // 
            this.transitionStyleFactoriesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.transitionStyleFactoriesList.DisplayMember = "Name";
            this.transitionStyleFactoriesList.FormattingEnabled = true;
            this.transitionStyleFactoriesList.Location = new System.Drawing.Point(4, 20);
            this.transitionStyleFactoriesList.Name = "transitionStyleFactoriesList";
            this.transitionStyleFactoriesList.Size = new System.Drawing.Size(94, 342);
            this.transitionStyleFactoriesList.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Transition Styles";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.IsSplitterFixed = true;
            this.splitContainer3.Location = new System.Drawing.Point(0, 0);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.splitContainer5);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.splitContainer4);
            this.splitContainer3.Size = new System.Drawing.Size(412, 368);
            this.splitContainer3.SplitterDistance = 204;
            this.splitContainer3.TabIndex = 0;
            // 
            // splitContainer5
            // 
            this.splitContainer5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer5.IsSplitterFixed = true;
            this.splitContainer5.Location = new System.Drawing.Point(0, 0);
            this.splitContainer5.Name = "splitContainer5";
            // 
            // splitContainer5.Panel1
            // 
            this.splitContainer5.Panel1.Controls.Add(this.triggerFactoriesList);
            this.splitContainer5.Panel1.Controls.Add(this.label3);
            // 
            // splitContainer5.Panel2
            // 
            this.splitContainer5.Panel2.Controls.Add(this.selectionRendererFactoriesList);
            this.splitContainer5.Panel2.Controls.Add(this.label4);
            this.splitContainer5.Size = new System.Drawing.Size(204, 368);
            this.splitContainer5.SplitterDistance = 98;
            this.splitContainer5.TabIndex = 0;
            // 
            // triggerFactoriesList
            // 
            this.triggerFactoriesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.triggerFactoriesList.DisplayMember = "Name";
            this.triggerFactoriesList.FormattingEnabled = true;
            this.triggerFactoriesList.Location = new System.Drawing.Point(3, 20);
            this.triggerFactoriesList.Name = "triggerFactoriesList";
            this.triggerFactoriesList.Size = new System.Drawing.Size(94, 342);
            this.triggerFactoriesList.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Triggers";
            // 
            // selectionRendererFactoriesList
            // 
            this.selectionRendererFactoriesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.selectionRendererFactoriesList.DisplayMember = "Name";
            this.selectionRendererFactoriesList.FormattingEnabled = true;
            this.selectionRendererFactoriesList.Location = new System.Drawing.Point(5, 20);
            this.selectionRendererFactoriesList.Name = "selectionRendererFactoriesList";
            this.selectionRendererFactoriesList.Size = new System.Drawing.Size(94, 342);
            this.selectionRendererFactoriesList.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(103, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Selection Renderers";
            // 
            // splitContainer4
            // 
            this.splitContainer4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer4.IsSplitterFixed = true;
            this.splitContainer4.Location = new System.Drawing.Point(0, 0);
            this.splitContainer4.Name = "splitContainer4";
            // 
            // splitContainer4.Panel1
            // 
            this.splitContainer4.Panel1.Controls.Add(this.imageTransitionFactoriesList);
            this.splitContainer4.Panel1.Controls.Add(this.label5);
            // 
            // splitContainer4.Panel2
            // 
            this.splitContainer4.Panel2.Controls.Add(this.featuresFactoryList);
            this.splitContainer4.Panel2.Controls.Add(this.label6);
            this.splitContainer4.Size = new System.Drawing.Size(204, 368);
            this.splitContainer4.SplitterDistance = 100;
            this.splitContainer4.TabIndex = 0;
            // 
            // imageTransitionFactoriesList
            // 
            this.imageTransitionFactoriesList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.imageTransitionFactoriesList.DisplayMember = "Name";
            this.imageTransitionFactoriesList.FormattingEnabled = true;
            this.imageTransitionFactoriesList.Location = new System.Drawing.Point(4, 20);
            this.imageTransitionFactoriesList.Name = "imageTransitionFactoriesList";
            this.imageTransitionFactoriesList.Size = new System.Drawing.Size(92, 342);
            this.imageTransitionFactoriesList.TabIndex = 5;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Image Transitions";
            // 
            // featuresFactoryList
            // 
            this.featuresFactoryList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.featuresFactoryList.DisplayMember = "Name";
            this.featuresFactoryList.FormattingEnabled = true;
            this.featuresFactoryList.Location = new System.Drawing.Point(1, 20);
            this.featuresFactoryList.Name = "featuresFactoryList";
            this.featuresFactoryList.Size = new System.Drawing.Size(96, 342);
            this.featuresFactoryList.TabIndex = 6;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Features";
            // 
            // stateSelector
            // 
            this.stateSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateSelector.Location = new System.Drawing.Point(3, 3);
            this.stateSelector.Name = "stateSelector";
            this.stateSelector.SelectedItem = null;
            this.stateSelector.Size = new System.Drawing.Size(620, 368);
            this.stateSelector.TabIndex = 0;
            // 
            // triggerSelector
            // 
            this.triggerSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.triggerSelector.Location = new System.Drawing.Point(3, 3);
            this.triggerSelector.Name = "triggerSelector";
            this.triggerSelector.SelectedItem = null;
            this.triggerSelector.Size = new System.Drawing.Size(620, 368);
            this.triggerSelector.TabIndex = 0;
            // 
            // transitionStyleSelector
            // 
            this.transitionStyleSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.transitionStyleSelector.Location = new System.Drawing.Point(3, 3);
            this.transitionStyleSelector.Name = "transitionStyleSelector";
            this.transitionStyleSelector.SelectedItem = null;
            this.transitionStyleSelector.Size = new System.Drawing.Size(620, 368);
            this.transitionStyleSelector.TabIndex = 0;
            // 
            // imageTransitionSelector
            // 
            this.imageTransitionSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageTransitionSelector.Location = new System.Drawing.Point(3, 3);
            this.imageTransitionSelector.Name = "imageTransitionSelector";
            this.imageTransitionSelector.SelectedItem = null;
            this.imageTransitionSelector.Size = new System.Drawing.Size(620, 368);
            this.imageTransitionSelector.TabIndex = 0;
            // 
            // selectionRendererSelector
            // 
            this.selectionRendererSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.selectionRendererSelector.Location = new System.Drawing.Point(3, 3);
            this.selectionRendererSelector.Name = "selectionRendererSelector";
            this.selectionRendererSelector.SelectedItem = null;
            this.selectionRendererSelector.Size = new System.Drawing.Size(620, 368);
            this.selectionRendererSelector.TabIndex = 0;
            // 
            // featureSelector
            // 
            this.featureSelector.Dock = System.Windows.Forms.DockStyle.Fill;
            this.featureSelector.Location = new System.Drawing.Point(3, 3);
            this.featureSelector.Name = "featureSelector";
            this.featureSelector.SelectedItem = null;
            this.featureSelector.Size = new System.Drawing.Size(620, 368);
            this.featureSelector.TabIndex = 0;
            // 
            // OverlayPluginPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainTab);
            this.Name = "OverlayPluginPanel";
            this.Size = new System.Drawing.Size(634, 400);
            this.mainTab.ResumeLayout(false);
            this.infoTab.ResumeLayout(false);
            this.windowsTab.ResumeLayout(false);
            this.windowsTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.opacitySlider)).EndInit();
            this.statesTab.ResumeLayout(false);
            this.triggersTab.ResumeLayout(false);
            this.windowTransitionsTab.ResumeLayout(false);
            this.imageTransitionsTab.ResumeLayout(false);
            this.rendererTab.ResumeLayout(false);
            this.featuresTab.ResumeLayout(false);
            this.factoriesTab.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.splitContainer5.Panel1.ResumeLayout(false);
            this.splitContainer5.Panel1.PerformLayout();
            this.splitContainer5.Panel2.ResumeLayout(false);
            this.splitContainer5.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer5)).EndInit();
            this.splitContainer5.ResumeLayout(false);
            this.splitContainer4.Panel1.ResumeLayout(false);
            this.splitContainer4.Panel1.PerformLayout();
            this.splitContainer4.Panel2.ResumeLayout(false);
            this.splitContainer4.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer4)).EndInit();
            this.splitContainer4.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl mainTab;
        private System.Windows.Forms.TabPage infoTab;
        private System.Windows.Forms.TabPage statesTab;
        private System.Windows.Forms.TabPage triggersTab;
        private System.Windows.Forms.TabPage windowTransitionsTab;
        private System.Windows.Forms.TabPage imageTransitionsTab;
        private System.Windows.Forms.TabPage rendererTab;
        private System.Windows.Forms.TabPage featuresTab;
        private System.Windows.Forms.TabPage factoriesTab;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private System.Windows.Forms.SplitContainer splitContainer5;
        private System.Windows.Forms.SplitContainer splitContainer4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox stateFactoriesList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ListBox transitionStyleFactoriesList;
        private System.Windows.Forms.ListBox triggerFactoriesList;
        private System.Windows.Forms.ListBox selectionRendererFactoriesList;
        private System.Windows.Forms.ListBox imageTransitionFactoriesList;
        private System.Windows.Forms.ListBox featuresFactoryList;
        private System.Windows.Forms.Button changeStateButton;
        private System.Windows.Forms.ListBox stateList;
        private System.Windows.Forms.TabPage windowsTab;
        private System.Windows.Forms.Button launchButton;
        private System.Windows.Forms.TrackBar opacitySlider;
        private StateSelector stateSelector;
        private TriggerSelector triggerSelector;
        private TransitionStyleSelector transitionStyleSelector;
        private ImageTransitionSelector imageTransitionSelector;
        private SelectionRendererSelector selectionRendererSelector;
        private FeatureSelector featureSelector;
#if DEBUG
        private System.Windows.Forms.TabPage statsTab;
        private System.Windows.Forms.TabControl statsTabs;
#endif
    }
}
