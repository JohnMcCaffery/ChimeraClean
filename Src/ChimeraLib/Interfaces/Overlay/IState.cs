using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chimera.Overlay;

namespace Chimera.Interfaces.Overlay {
    public interface IState {

        /// <summary>
        /// The states for the individual windows.
        /// </summary>
        IWindowState[] WindowStates {
            get;
        }

        /// <summary>
        /// All the transitions transition this state to other states.
        /// </summary>
        StateTransition[] Transitions {
            get;
        }

        /// <summary>
        /// The unique name for the state.
        /// </summary>
        string Name {
            get;
        }

        /// <summary>
        /// Whether the state is currently active.
        /// </summary>
        bool Active {
            get;
            set;
        }

        /// <summary>
        /// Add a new transition to another state.
        /// </summary>
        /// <param name="StateTransition">The new transition to add.</param>
        void AddTransition(StateTransition transition);

        /// <summary>
        /// Add a feature to the be drawn on one of the windows for the state.
        /// </summary>
        /// <param name="window">The window to draw the feature on.</param>
        /// <param name="feature">The feature to draw.</param>
        void AddFeature(string window, IDrawable feature);
    }
}
