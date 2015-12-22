using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Plugins;
using Chimera.Flythrough.GUI;
using System.Threading;
using System.IO;
using log4net;

namespace Chimera.Flythrough {
    public class VideospherePlugin : PluginBase<VideospherePanel> {
        private ILog Logger = LogManager.GetLogger("Videosphere");
        private FlythroughConfig mConfig = new FlythroughConfig();
        private FlythroughPlugin mFlythroughPlugin;
        private PhotospherePlugin mPhotospherePlugin;
        private bool mFlythroughFinished;

        public VideospherePlugin(FlythroughPlugin flythroughPlugin, PhotospherePlugin photospherePlugin)
            : base("Videosphere", plugin => new VideospherePanel(plugin as VideospherePlugin)) {
        }

        public override void Init(Core core) {
            base.Init(core);


            mFlythroughPlugin = core.GetPlugin<FlythroughPlugin>();
            mPhotospherePlugin = core.GetPlugin<PhotospherePlugin>();

            mCore.Closed += new Action<Core,System.Windows.Forms.KeyEventArgs>(mFlythroughPlugin_SequenceFinished);
            mFlythroughPlugin.SequenceFinished += new EventHandler(mFlythroughPlugin_SequenceFinished);
        }

        void mFlythroughPlugin_SequenceFinished(object sender, EventArgs e) {
            mFlythroughFinished = true;
        }

        public override Config.ConfigBase Config {
            get { return mConfig; }
        }

        public string Flythrough {
            get { return mConfig.VideosphereFlythrough; }
            set { mConfig.VideosphereFlythrough = value; }
        }

        public void Record() {
            if (mConfig.VideosphereFlythrough == null) {
                Logger.Warn("Unable to record videosphere. No flythrough specified.");
                return;
            }
            if (!File.Exists(mConfig.VideosphereFlythrough)) {
                Logger.Warn("Unable to record videosphere. Specified flythrough does not exist (" + mConfig.VideosphereFlythrough + ").");
                return;
            }
            mFlythroughPlugin.Load(Flythrough);
            int lastTime = 0;
            mFlythroughPlugin.Time = lastTime;
            string originalPhotosphereName = mPhotospherePlugin.PhotosphereName;
            string originalPhotosphereFolder = mPhotospherePlugin.PhotosphereFolder;

            mPhotospherePlugin.PhotosphereName = mConfig.VideosphereName;
            mPhotospherePlugin.PhotosphereFolder = Path.Combine(mConfig.VideosphereFolder, mConfig.VideosphereName);

            mFlythroughFinished = false;

            int frame = 1;
            Thread t =new Thread (() => {
                while (!mFlythroughFinished) {
                    lastTime = mFlythroughPlugin.Time;
                    mPhotospherePlugin.PhotosphereName = mConfig.VideosphereName + "_" + (frame++);
                    mPhotospherePlugin.TakePhotosphere();
                    //What units?
                    mFlythroughPlugin.Time += mConfig.VideosphereTimeIncrement;
                }
                mPhotospherePlugin.PhotosphereName = originalPhotosphereName;
                mPhotospherePlugin.PhotosphereFolder = originalPhotosphereFolder;
            });
            t.Name = "Videosphere thread";
            t.Start();
        }

        public void Stop() {
            mFlythroughFinished = true;
        }
    }
}
