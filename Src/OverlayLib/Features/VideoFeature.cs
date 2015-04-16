using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Chimera.Interfaces;
using System.Xml;
using System.IO;
using Chimera.Interfaces.Overlay;
using System.Drawing;

namespace Chimera.Overlay.Features {
    public class VideoFeatureFactory : IFeatureFactory {
        private IMediaPlayer mPlayer;

        public VideoFeatureFactory(IMediaPlayer player) {
            mPlayer = player;
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node) {
            return new VideoFeature(manager, node, mPlayer);
        }

        public IFeature Create(OverlayPlugin manager, XmlNode node, Rectangle clip) {
            return new VideoFeature(manager, node, mPlayer, clip);
        }

        public string Name {
            get { return "Video"; }
        }
    }

    public class VideoFeature : ControlFeature<Control> {
        private IMediaPlayer mPlayer;
        private string mVideo;
        private bool mPlaying = false;

        protected override Func<Control> MakeControl {
            get { return () => mPlayer.Player; }
        }

        protected override Func<Control> MakeControlPanel {
            get { return () => new Control(); }
        }

        public VideoFeature(OverlayPlugin manager, XmlNode node, IMediaPlayer player)
            : base(manager, node, true) {

            Init(node, player);
        }

        public VideoFeature(OverlayPlugin manager, XmlNode node, IMediaPlayer player, Rectangle clip)
            : base(manager, node, true, clip) {

            Init(node, player);
        }

	private void Init(XmlNode node, IMediaPlayer player) {
            mPlayer = player;
            mVideo = GetString(node, null, "File");
            if (mVideo == null)
                throw new ArgumentException("Unable to load VideoFeature. No File attribute specified.");
            mVideo = Path.GetFullPath(mVideo);
            if (!File.Exists(mVideo))
                throw new ArgumentException("Unable to load VideoFeature. The file '" + mVideo + "' does not exist.");

            mPlayer.PlaybackFinished += new Action(mPlayer_VideoFinished);       
        }

        void mPlayer_VideoFinished() {
            mPlaying = false;
            base.Active = false;
        }

        public override bool Active {
            get { return base.Active; }
            set {
                if (value != base.Active) {
                    base.Active = value;
                    if (value) {
                        mPlaying = true;
                        mPlayer.PlayVideo(mVideo);
                    } else if (mPlaying) {
                        mPlayer.StopPlayback();
                    }
                }
            }
        }
    }
}
