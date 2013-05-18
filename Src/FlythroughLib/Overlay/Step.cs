using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Drawables;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.IO;
using Chimera.Overlay;

namespace Chimera.Flythrough.Overlay {
    public class Step : XmlLoader {
        private readonly int mStep;

        private readonly Coordinator mCoordinator;
        private readonly Text mSubtitlesText;
        private readonly Action mTickListener;
        private readonly string mVoiceoverFile;

        private readonly Dictionary<int, string> mSubtitles = new Dictionary<int, string>();

        private OverlayImage mImage;
        private Queue<int> mSubtitleTimes;

        private DateTime mStarted;

        public int StepNum {
            get { return mStep; }
        }

        public Step(FlythroughState state, XmlNode node, Text subititlesText) {
            if (node.Attributes["Step"] == null && !int.TryParse(node.Attributes["Step"].Value, out mStep))
                throw new ArgumentException("Unable to load slideshow step. A valid 'Step' attribute must be supplied.");

            mTickListener = new Action(mCoordinator_Tick);
            mCoordinator = state.Manager.Coordinator;
            mStep = GetInt(node, -1, "Step");
            if (mStep == -1)
                throw new ArgumentException("Unable to load step ID. A valid Step attribute is expected.");

            XmlAttribute voiceoverAttribute = node.Attributes["Voiceover"];
            if (voiceoverAttribute != null && File.Exists(voiceoverAttribute.Value))
                mVoiceoverFile = Path.GetFullPath(voiceoverAttribute.Value);

            mSubtitlesText = subititlesText;

            if (mSubtitlesText != null) {
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
                mImage = state.Manager.MakeImage(imageNode);
                state.AddFeature(mImage);
                mImage.Active = false;
            }
        }

        public void Start() {
            mSubtitleTimes = new Queue<int>(mSubtitles.Keys.OrderBy(i=>i));

            if (mSubtitles.Count > 0) {
                mStarted = DateTime.Now;
                mCoordinator.Tick += mTickListener;
            }

            if (mImage != null) {
                mImage.Active = true;
                mCoordinator[mImage.Window].OverlayManager.ForceRedrawStatic();
            }

            //TODO - play voiceover file
        }

        public void Finish() {
            if (mImage != null) {
                mImage.Active = false;
                mCoordinator[mImage.Window].OverlayManager.ForceRedrawStatic();
            }
            if (mSubtitlesText != null)
                mSubtitlesText.Active = false;
            mCoordinator.Tick += mTickListener;
        }

        private void mCoordinator_Tick() {
            if (mSubtitleTimes.Count > 0 && DateTime.Now.Subtract(mStarted).TotalSeconds > mSubtitleTimes.Peek()) {
                mSubtitlesText.TextString = mSubtitles[mSubtitleTimes.Dequeue()];
            }
        }
    }
}
