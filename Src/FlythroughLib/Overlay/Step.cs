using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Drawables;
using Chimera.Interfaces.Overlay;
using System.Xml;
using System.IO;

namespace Chimera.Flythrough.Overlay {
    public class Step {
        private readonly int mStep;

        private readonly Coordinator mCoordinator;
        private readonly Text mSubtitlesText;
        private readonly Action mTickListener;
        private readonly string mVoiceoverFile;

        private readonly Dictionary<int, OverlayImage> mImages = new Dictionary<int, OverlayImage>();
        private readonly Dictionary<int, string> mSubtitles = new Dictionary<int, string>();

        private OverlayImage mCurrentImage;
        private Queue<int> mImageTimes;
        private Queue<int> mSubtitleTimes;

        private DateTime mStarted;

        public int StepNum {
            get { return mStep; }
        }

        public Step(Coordinator coordinator, XmlNode node, Text subititlesText) {
            if (node.Attributes["Step"] == null && !int.TryParse(node.Attributes["Step"].Value, out mStep))
                throw new ArgumentException("Unable to load slideshow step. A valid 'Step' attribute must be supplied.");

            mTickListener = new Action(mCoordinator_Tick);
            mCoordinator = coordinator;

            XmlAttribute voiceoverAttribute = node.Attributes["Voiceover"];
            if (voiceoverAttribute != null && File.Exists(voiceoverAttribute.Value))
                mVoiceoverFile = Path.GetFullPath(voiceoverAttribute.Value);

            mSubtitlesText = subititlesText;

            if (mSubtitlesText != null) {
                XmlNode subtitlesNode = node.SelectSingleNode("child::Subtitles");
                if (subtitlesNode != null) {
                    foreach (XmlNode child in subtitlesNode.ChildNodes) {
                        int time = child.Attributes["Time"] != null ? int.Parse(child.Attributes["Time"].Value) : 0;
                        mSubtitles.Add(time, child.InnerText);
                    }
                }
            }

            XmlNode imagesNode = node.SelectSingleNode("child::Images");
            if (imagesNode != null) {
                foreach (XmlNode child in imagesNode.ChildNodes) {
                    int time = child.Attributes["Time"] != null ? int.Parse(child.Attributes["Time"].Value) : 0;
                    mImages.Add(time, coordinator.StateManager.MakeImage(node));
                }
            }
        }

        public void Trigger() {
            mImageTimes = new Queue<int>(mImages.Keys.OrderBy(i=>i));
            mSubtitleTimes = new Queue<int>(mSubtitles.Keys.OrderBy(i=>i));

            if (mImages.Count > 0 || mSubtitles.Count > 0) {
                mStarted = DateTime.Now;
                mCoordinator.Tick += mTickListener;
            }

            //TODO - play voiceover file
        }

        public void Finish() {
            if (mCurrentImage != null)
                mCurrentImage.Active = false;
            if (mSubtitlesText != null)
                mSubtitlesText.Active = false;
            mCoordinator.Tick += mTickListener;
            mCurrentImage = null;
        }

        private void mCoordinator_Tick() {
            if (mImageTimes.Count > 0 && DateTime.Now.Subtract(mStarted).TotalSeconds > mImageTimes.Peek()) {
                if (mCurrentImage != null)
                    mCurrentImage.Active = false;
                mCurrentImage = mImages[mImageTimes.Dequeue()];
                mCurrentImage.Active = true;
                mCoordinator[mCurrentImage.Window].OverlayManager.ForceRedrawStatic();
            }
            if (mSubtitleTimes.Count > 0 && DateTime.Now.Subtract(mStarted).TotalSeconds > mSubtitleTimes.Peek()) {
                mSubtitlesText.TextString = mSubtitles[mSubtitleTimes.Dequeue()];
            }
        }
    }
}
