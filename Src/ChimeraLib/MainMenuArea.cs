using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Chimera {
    public class MainMenuItem : ImageArea {
        /// <summary>
        /// The menu this area is part of.
        /// </summary>
        private MainMenu mMenu;
        /// <summary>
        /// The images which can be used to render the area minimizing.
        /// </summary>
        private Bitmap[] mImages;
        /// <summary>
        /// The positions for the images as they are rendered as minimizing.
        /// </summary>
        private Rectangle[] mPositions;

        /// <summary>
        /// The smallest image in the minimization scale.
        /// </summary>
        public Bitmap FinalImage {
            get { return mImages[mPositions.Length-1]; }
        }

        public MainMenuItem(string imageFile, double x, double y, double w, double h)
            : base(imageFile, x, y, w, h) {
        }

        /// <summary>
        /// The position where the smallest image on the minimization scale should be rendered.
        /// </summary>
        public Rectangle FinalPosition {
            get { return mPositions[mPositions.Length-1]; }
        }
        /// <summary>
        /// Render a step toward minimization.
        /// </summary>
        /// <param name="step">The step to render.</param>
        public void RenderStep(int step) {
            throw new System.NotImplementedException();
        }

        /// <param name="menu">The main menu this item is part of.</param>
        public void Init(MainMenu menu) {
            base.Init(menu);
            mMenu = menu;
        }
    }
}
