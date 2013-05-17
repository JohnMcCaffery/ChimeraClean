using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay.Drawables;

namespace Chimera.Flythrough.Overlay {
    public class Step {
        private int mStep;
        private Coordinator mCoordinator;
        private Action mTickListener;
        private bool mDisplaySubtitles;
        private string mVoiceoverFile;
        private Text mSubitlesText;
        private OverlayImage mCurrentImage;

        private Dictionary<int, OverlayImage> mImages = new Dictionary<int, OverlayImage>();
        private Dictionary<int, string> mSubtitles = new Dictionary<int, string>();

        private Queue<int> mImageTimes;
        private Queue<int> mSubtitleTimes;

        public int StepNum {
            get { return mStep; }
        }

        public void Trigger() {
            mImageTimes = new Queue<int>(mImages.Keys.OrderBy(i=>i));
            mSubtitleTimes = new Queue<int>(mSubtitles.Keys.OrderBy(i=>i));

            mCoordinator.Tick += mTickListener;
        }

        public void Finish() {
            mCoordinator.Tick += mTickListener;
        }

        private void mCoordinator_Tick() {
        }
    }
}
