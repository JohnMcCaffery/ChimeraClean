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
        private int mStep;
        private Coordinator mCoordinator;
        private Action mTickListener;
        private string mVoiceoverFile;
        private Text mSubtitlesText;
        private OverlayImage mCurrentImage;

        private Dictionary<int, OverlayImage> mImages = new Dictionary<int, OverlayImage>();
        private Dictionary<int, string> mSubtitles = new Dictionary<int, string>();

        private Queue<int> mImageTimes;
        private Queue<int> mSubtitleTimes;

        private DateTime mStarted;

        public int StepNum {
            get { return mStep; }
        }

        public Step(Coordinator coordinator, XmlNode node, bool displaySubtitles) {
            if (node.Attributes["Step"] == null && !int.TryParse(node.Attributes["Step"].Value, out mStep))
                throw new ArgumentException("Unable to load slideshow step. A valid 'Step' attribute must be supplied.");

            mTickListener = new Action(mCoordinator_Tick);
            mCoordinator = coordinator;

            XmlAttribute voiceoverAttribute = node.Attributes["Voiceover"];
            if (voiceoverAttribute != null && File.Exists(voiceoverAttribute.Value))
                mVoiceoverFile = Path.GetFullPath(voiceoverAttribute.Value);

            if (displaySubtitles) {
                XmlNode subtitlesNode = node.SelectSingleNode("child::Subtitles");
                if (subtitlesNode != null) {
                    foreach (XmlNode child in subtitlesNode.ChildNodes) {
                        if (child.Name == "Setup")
                            mSubtitlesText = coordinator.StateManager.MakeText(child);
                        else {
                            int time = child.Attributes["Time"] != null ? int.Parse(child.Attributes["Time"].Value) : 0;
                            mSubtitles.Add(time, child.InnerText);
                        }
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
            HashSet<string> redraw = new HashSet<string>();
            if (mImageTimes.Count > 0 && DateTime.Now.Subtract(mStarted).TotalSeconds > mImageTimes.Peek()) {
                if (mCurrentImage != null)
                    mCurrentImage.Active = false;
                mCurrentImage = mImages[mImageTimes.Dequeue()];
                mCurrentImage.Active = true;
                redraw.Add(mCurrentImage.Window);
            }
            if (mSubtitleTimes.Count > 0 && DateTime.Now.Subtract(mStarted).TotalSeconds > mSubtitleTimes.Peek()) {
                mSubtitlesText.Active = true;
                mSubtitlesText.TextString = mSubtitles[mSubtitleTimes.Dequeue()];
                redraw.Add(mSubtitlesText.Window);
            }
            foreach (var window in redraw)
                mCoordinator[window].OverlayManager.ForceRedrawStatic();
        }
    }
}
