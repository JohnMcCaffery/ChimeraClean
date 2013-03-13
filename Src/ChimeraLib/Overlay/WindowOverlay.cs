using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using OpenMetaverse;
using Chimera.Util;

namespace Chimera.Overlay {
    public class WindowOverlay {
        /// <summary>
        /// The menu items which render onto/can be triggered from this section of the main menu.
        /// </summary>
        private readonly List<MainMenuItem> mItems = new List<MainMenuItem>();
        /// <summary>
        /// The background image behind all the options on the main menu.
        /// </summary>
        private Bitmap mMainMenuBG;
        /// <summary>
        /// The bitmap which represents the un changing elements of whatever is currently being rendered.
        /// </summary>
        private Bitmap mStaticBG;
        /// <summary>
        /// The bitmap which represents the un changing elements of the main menu.
        /// </summary>
        private Bitmap mStaticMenu;
        /// <summary>
        /// The selectable area which, when triggered, will return to the main menu.
        /// </summary>
        private ImageArea mThumbnailSelectable;
        /// <summary>
        /// The window this section of the main menu renders onto.
        /// </summary>
        private Window mWindow;
        /// <summary>
        /// The menu this menu overlay section is part of.
        /// </summary>
        private MainMenu mMenu;

        public WindowOverlay(IEnumerable<MainMenuItem> items) {
            mItems = new List<MainMenuItem>(items);
            mMainMenuBG = new Bitmap("MenuBG.jpeg");
        }

        public MainMenuItem[] MenuItems {
            get { return mItems.ToArray(); }
        }

        private void RecalculateStatic() {
            mStaticBG = new Bitmap(mWindow.Monitor.Bounds.Width, mWindow.Monitor.Bounds.Height);
            Graphics graphics = Graphics.FromImage(mStaticBG);
            Rectangle clip = new Rectangle(0, 0, mStaticBG.Width, mStaticBG.Height);
            if (mMenu.Active) {
                // -- draw the background image here --
                //using (Brush b = new SolidBrush(Color.FromArgb(Byte.MaxValue / 2, Color.Pink))) {
                //graphics.FillRectangle(b, clipRectangle);
                //}
                graphics.DrawImage(new Bitmap(mMainMenuBG, clip.Size), clip.Location);
                foreach (var item in mItems)
                    item.Menu.DrawStatic(graphics, clip);

                RecalculateThumbnail();
            } else {
                mMenu.SelectedState.DrawStatic(graphics, clip, mWindow);
                mThumbnailSelectable.DrawStatic(graphics, clip);
            }
        }

        private void RecalculateThumbnail() {
            double s = mMenu.MainMenuSelectableSize;
            //mThumbnailSelectable = new ImageArea(mWindow, mStaticBG, 1 - s, 0, 1, s);
            mThumbnailSelectable = new ImageArea(mWindow, mStaticBG, 0, 0, 1, s);
            mThumbnailSelectable.Selected += new Action<ISelectable>(mThumbnailSelectable_Selected);
        }

        public void Init(MainMenu menu, Window window) {
            mMenu = menu;
            mWindow = window;

            menu.StateSelected += newState => RecalculateStatic();
            menu.MainMenuSelected += () => RecalculateStatic();
            mWindow.MonitorChanged += (w, s) => {
                RecalculateStatic();
                if (!mMenu.Active)
                    RecalculateThumbnail();
            };

            foreach (var item in mItems) {
                item.Init(mMenu, window);
                item.Menu.ImageChanged += (source, args) => RecalculateStatic();
                item.Menu.Selected += selectable => ItemSelected(item);
                item.State.Activated += state => ItemActivated(item);
            }

            RecalculateStatic();
        }

        public void DrawMenu(Graphics graphics, Rectangle clipRectangle) {
            Draw(graphics, clipRectangle, bgGraphics => {
                foreach (var item in mItems)
                    item.Menu.DrawDynamic(bgGraphics, clipRectangle);
            });
        }

        public  void DrawState(IOverlayState state, Graphics graphics, Rectangle clipRectangle) {
            Draw(graphics, clipRectangle, overlayGraphics => {
                state.DrawDynamic(overlayGraphics, clipRectangle, mWindow);
                mThumbnailSelectable.DrawDynamic(overlayGraphics, clipRectangle);
            });
        }

        public  void DrawInBetween(IOverlayState state, double scale, Graphics graphics, Rectangle clipRectangle) {
            Draw(graphics, clipRectangle, overlayGraphics => {
                Size s = new Size(clipRectangle.Width, (int) (clipRectangle.Height * scale));
                //Point p = new Point(clipRectangle.Width - s.Width, 0);
                Point p = new Point(0, 0);
                overlayGraphics.DrawImage(new Bitmap(mThumbnailSelectable.Image, s), p);
            });
        }

        private void Draw(Graphics graphics, Rectangle clipRectangle, Action<Graphics> draw) {
            Bitmap staticImg = new Bitmap(mStaticBG);
            Graphics overlayGraphics = Graphics.FromImage(staticImg);

            draw(overlayGraphics);

            graphics.DrawImage(staticImg, 0, 0);
        }

        public void DrawCursor(Graphics graphics, Rectangle clipRectangle) {
            float r = 10f;
            if (clipRectangle.Contains(mWindow.Cursor))
                graphics.FillEllipse(Brushes.Red, (float)mWindow.CursorX - r, (float)mWindow.CursorY - r, r * 2, r * 2);
        }

        private void mThumbnailSelectable_Selected(ISelectable selectable) {
            mThumbnailSelectable.Active = false;
            mMenu.SelectMainMenu();
        }

        private void ItemSelected(MainMenuItem item) {
            mMenu.SelectState(item.State);
        }

        private void ItemActivated(MainMenuItem state) {
            mThumbnailSelectable.Active = true;
        }
    }
}
