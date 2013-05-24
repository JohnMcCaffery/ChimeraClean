using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Drawables;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.IO;
using Chimera.Overlay;
using Chimera.Interfaces;

namespace Chimera.Flythrough.Overlay {
    public class Step : XmlLoader {
        private readonly int mStep;
        private readonly int mSubtitleTimeoutS = 20;

        private readonly OverlayPlugin mManager;
        private readonly IMediaPlayer mPlayer;
        private readonly Text mSubtitlesText;
        private readonly Action mTickListener;
        private readonly string mVoiceoverFile;

        private readonly Dictionary<int, string> mSubtitles = new Dictionary<int, string>();

        private OverlayImage mImage;
        private Queue<int> mSubtitleTimes;

        private DateTime mStarted;
        private DateTime mLastSubtitle;

        public int StepNum {
            get { return mStep; }
        }

        public Step(FlythroughState state, XmlNode node, Text subititlesText, int subtitleTimeoutS, IMediaPlayer player) {
            if (node.Attributes["Step"] == null && !int.TryParse(node.Attributes["Step"].Value, out mStep))
                throw new ArgumentException("Unable to load slideshow step. A valid 'Step' attribute must be supplied.");

            mPlayer = player;
            mManager = state.Manager;
            mStep = GetInt(node, -1, "Step");
            if (mStep == -1)
                throw new ArgumentException("Unable to load step ID. A valid Step attribute is expected.");

            XmlAttribute voiceoverAttribute = node.Attributes["Voiceover"];
            if (voiceoverAttribute != null && File.Exists(voiceoverAttribute.Value)) {
                if (mPlayer != null)
                    mVoiceoverFile = Path.GetFullPath(voiceoverAttribute.Value);
                else
                    Console.WriteLine("Unable to load voiceover for flythrough step. No MediaPlayer supplied.");
            }

            mSubtitlesText = subititlesText;

            if (mSubtitlesText != null) {
                mTickListener = new Action(mCoordinator_Tick);
                mSubtitleTimeoutS = subtitleTimeoutS;
                XmlNode subtitlesNode = node.SelectSingleNode("child::Subtitles");
                if (subtitlesNode != null) {
                    foreach (XmlNode child in subtitlesNode.ChildNodes) {
                        if (child is XmlElement) {
                            int time = child.Attributes["Time"] != null ? int.Parse(child.Attributes["Time"].Value) : 0;
                            mSubtitles.Add(time, child.InnerText);
                        }
                    }
                }
            }

            XmlNode imageNode = node.SelectSingleNode("child::Image");
            if (imageNode != null) {
                try {
                    mImage = state.Manager.MakeImage(imageNode, "flythrough step " + (mStep + 1));
                    state.AddFeature(mImage);
                    mImage.Active = false;
                } catch (Exception e) { }
            }
        }

        public void Start() {
            mSubtitleTimes = new Queue<int>(mSubtitles.Keys.OrderBy(i=>i));

            if (mSubtitles.Count > 0) {
                mStarted = DateTime.Now;
                mManager.Coordinator.Tick += mTickListener;
            }

            if (mVoiceoverFile != null)
                mPlayer.PlayAudio(mVoiceoverFile);

            if (mImage != null) {
                mImage.Active = true;
                mManager[mImage.Window].ForceRedrawStatic();
            }

            //TODO - play voiceover file
        }

        public void Finish() {
            if (mImage != null) {
                mImage.Active = false;
                mManager[mImage.Window].ForceRedrawStatic();
            }

            if (mVoiceoverFile != null)
                mPlayer.StopPlayback();

            if (mSubtitlesText != null)
                mSubtitlesText.Active = false;
            mManager.Coordinator.Tick += mTickListener;
        }

        private void mCoordinator_Tick() {
            if (mSubtitleTimes.Count > 0 && DateTime.Now.Subtract(mStarted).TotalSeconds > mSubtitleTimes.Peek()) {
                mSubtitlesText.TextString = mSubtitles[mSubtitleTimes.Dequeue()];
                mLastSubtitle = DateTime.Now;
            } else if (DateTime.Now.Subtract(mLastSubtitle).TotalSeconds > mSubtitleTimeoutS)
                mSubtitlesText.TextString = "";
        }
    }
}
