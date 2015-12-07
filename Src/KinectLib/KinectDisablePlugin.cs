using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Chimera.Config;
using Chimera.Plugins;
using log4net;
using OpenMetaverse;

namespace Chimera.Kinect
{
    public class KinectDisableConfig : ConfigBase
    {
        public Keys DisableKey;

        public override string Group
        {
            get { return "KinectDisable"; }
        }

        protected override void InitConfig()
        {
            DisableKey = GetEnum<Keys>("KinectDisableKey", Keys.F8, "The key to disable the kinect.", LogManager.GetLogger("KinectDisable"));
        }
    }

    class KinectDisablePlugin : ISystemPlugin
    {
        #region ISystemPlugin Members

        public void Init(Core core)
        {
            mCore = core;
            config = new KinectDisableConfig();
            Key = config.DisableKey;
            log = LogManager.GetLogger("KinectDisable");
            hotkey = new Hotkey();
            hotkey.KeyCode = Key;
            hotkey.Pressed += new HandledEventHandler(hostkey_keypressed);
            if(mCore.HasPlugin<KinectMovementPlugin>())
                mInput = mCore.GetPlugin<KinectMovementPlugin>();
            if(mCore.HasPlugin<SimpleKinectCursor>())
                mCursor = mCore.GetPlugin<SimpleKinectCursor>();
        }

        public event Action<IPlugin, bool> EnabledChanged;
        private bool mEnabled = true;
        private Core mCore;
        private KinectDisableConfig config;
        private Keys Key;
        private Form mForm;
        private ILog log;
        private Hotkey hotkey;
        private KinectMovementPlugin mInput;
        private SimpleKinectCursor mCursor;
        private bool mDisabled = false;

        public Control ControlPanel
        {
            get
            {
                return new UserControl();
            }
        }

        public bool Enabled
        {
            get { return mEnabled; }
            set
            {
                if (mEnabled != value)
                {
                    mEnabled = value;
                    if (EnabledChanged != null)
                        EnabledChanged(this, value);
                }
            }
        }

        public string Name
        {
            get { return "KiectDisable"; }
        }

        public string State
        {
            get { throw new NotImplementedException(); }
        }

        public Config.ConfigBase Config
        {
            get { throw new NotImplementedException(); }
        }

        public void Close() { }

        public void Draw(Graphics g, Func<Vector3, Point> to2D, Action redraw, Perspective perspective)
        {
        }

        public void SetForm(Form form)
        {
            mForm = form;
            if (hotkey.GetCanRegister(mForm))
            {
                hotkey.Register(mForm);
            }
        }

        #endregion

        private void hostkey_keypressed(object sender, HandledEventArgs args)
        {
            log.WarnFormat("Disable callback called");
            mDisabled = !mDisabled;
            Disable(mDisabled);

            args.Handled = true;
        }

        private void Disable(bool disable) {
            if (mInput != null) {
                mInput.Enabled = !disable;
                //Originally disable had its own property. Don't know whether enable is analogous
                //mInput.Disabled = disable;
            }
            if (mCursor != null) {
                mCursor.Disabled = disable;
            }
        }
    }
}
