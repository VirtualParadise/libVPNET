/* **********************************************************************************
 *
 * Copyright (c) TCPX. All rights reserved.
 *
 * This source code is subject to terms and conditions of the Microsoft Public
 * License (Ms-PL). A copy of the license can be found in the license.txt file
 * included in this distribution.
 *
 * You must not remove this notice, or any other, from this software.
 *
 * **********************************************************************************/

using System;
using VPNetExamples.Common.ActionInterpreter.Attributes;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VPNetExamples.Common.ActionInterpreter.Types;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// With Environmental Subrendering (envi), the current scene from an object's camera perspective is rendered onto any surface of a geometry within the environment. In the future, this will allow us to implement picture in picture rendering as well as rendering in an external popup window, where the rendered content of the subview may differ from the main window, for upcoming features. The browser offers an action command to control subrendering. This technique can be used, for example, to render mirrors or to render environment maps onto an object's surface of any shape.
    /// All arguments to the envi action command are optional.
    /// </summary>
    [Serializable]
    public sealed class ACEnvi : IActionCommand, IActionCommandGlobal, IActionCommandName
    {
        private EnviType _type;
        private ResType _res;
        private int _upd;
        private float _zoom;
        private int _proj;
        private float _aspect;
        private float _clip;
        private float _time;
        private int _tag;
        private string _name;
        private GlobalType _isGlobal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandEnvi"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="res">The res.</param>
        /// <param name="upd">The upd.</param>
        /// <param name="zoom">The zoom.</param>
        /// <param name="proj">The proj.</param>
        /// <param name="aspect">The aspect.</param>
        /// <param name="clip">The clip.</param>
        /// <param name="time">The time.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="name">The name.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        public ACEnvi(EnviType type, ResType res, int upd, float zoom, int proj, float aspect, float clip, float time, int tag, string name, GlobalType isGlobal)
        {
            _type = type;
            _res = res;
            _upd = upd;
            _zoom = zoom;
            _proj = proj;
            _aspect = aspect;
            _clip = clip;
            _time = time;
            _tag = tag;
            _name = name;
            _isGlobal = isGlobal;
        }

        public ACEnvi(){}

        /// <summary>
        /// The global argument will cause triggers to initiate the command for all users have the object in view. Without it, the command will be triggered exclusively for the user who activates the trigger (bump, activate, adone). By default, commands are not global.
        /// </summary>
        /// <value><c>true</c> if this instance is global; otherwise, <c>false</c>.</value>
        [ACItemBinding("", CommandInterpretType.Flag)]
        public GlobalType IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        /// <summary>
        /// The name argument can be used for remote control, applying the envi command onto the named object within view of the same owner.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding("name",CommandInterpretType.NameValuePairs)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The tag argument specifies the tag number of the surface to render on. Any tag that exists on the used object can be used, including tags of jointed avatars. A value of 0 (zero), also the default value, means to render on all surfaces of the geometry.
        /// </summary>
        /// <value>The tag.</value>
        [ACItemBinding("tag", CommandInterpretType.NameValuePairs)]
        public int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// The time argument defines how long the subrender should stay active, meaning to continue rendering onto the object's surface if the upd argument is used. When the time elapses, the rendering surface will reset to its initial texture, or sign text or color. The value for time can take any positive floating point value. A value of 0.0 means 'infinitiv', where rendering will continue until the scene is left. The default value is 0.0.
        /// </summary>
        /// <value>The time.</value>
        [ACItemBinding("time", CommandInterpretType.NameValuePairs)]
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// The clip argument sets the far clipping for the subrenderer and can be in the range of 0.1 to 2500.0. The value is given in meters, and defines the distance of objects that should be considered by the subrender, similar to the far fog range value set in a world. The default value is the same as the default for far fog range in a world, 120.0 meters.
        /// </summary>
        /// <value>The clip.</value>
        [ACItemBinding("clip", CommandInterpretType.NameValuePairs)]
        public float Clip
        {
            get { return _clip; }
            set { _clip = value; }
        }

        /// <summary>
        /// The aspect argument represents the aspect ratio, which is the ratio of the width and the height of the rendering view. A typical square surface takes an aspect ratio of 1.0, the default. An aspect of higher than 1.0 stretches the width of the projection view, where an aspect lower than 1.0 shrinks the width of the view. This is in particular useful, if the projection surface is not squared.
        /// </summary>
        /// <value>The aspect.</value>
        [ACItemBinding("aspect", CommandInterpretType.NameValuePairs)]
        public float Aspect
        {
            get { return _aspect; }
            set { _aspect = value; }
        }

        /// <summary>
        ///The proj argument is an integer value that takes either the value 1 for perspective projection or 2 for parallel projection. A typical mirror has parallel projection. Typical environment maps use perspective projection, a 'distorted mirror view', if you would like so. Parallel projection require higher zoom factors and a slightly adjusted aspect ratio to achieve the imagination of a real mirror. The default value for the projection argument is 1, perspective projection.
        /// </summary>
        /// <value>The proj.</value>
        [ACItemBinding("proj", CommandInterpretType.NameValuePairs)]
        public int Proj
        {
            get { return _proj; }
            set { _proj = value; }
        }

        /// <summary>
        /// The zoom argument gives you the option to zoom in or zoom out the rendered view. The value must be higher than 0.0. There is no upper limit specified. A zoom factor of 1.0 is considered as browser standard. Values lower than 1.0 will zoom out, providing a wider perspective angle, a fish-eye perspective with extreme values. Values higher than 1.0 will zoom in, getting objects closer to the view. The default zoom factor is 1.0.
        /// </summary>
        /// <value>The zoom.</value>
        [ACItemBinding("zoom", CommandInterpretType.NameValuePairs)]
        public float Zoom
        {
            get { return _zoom; }
            set { _zoom = value; }
        }

        /// <summary>
        /// The upd argument sets the update rate of the rendered subscene. It can take any value in the range from 0 to 33. An update rate of 0 (zero) is a one time shot. One time shots will remain onto the surface, even there is no further rendering, regardless of the given time option value. Setting it to e.g. 15, will render 15 frames in a second onto the projection surface. The default update rate is 0, a one time shot.
        /// </summary>
        /// <value>The upd.</value>
        [ACItemBinding("upd", CommandInterpretType.NameValuePairs)]
        public int Upd
        {
            get { return _upd; }
            set { _upd = value; }
        }

        /// <summary>
        /// The res argument is the resolution used for the rendering. Valid values are 32, 64, 128, 256, 512 and 1024. Higher resolutions will result in better quality of the rendered subscene. Lower resolutions are rendered faster, but in lower quality. The default resolution is 128 pixels squared.
        /// </summary>
        /// <value>The res.</value>
        [ACItemBinding("res", CommandInterpretType.NameValuePairs)]
        public ResType Res
        {
            get { return _res; }
            set { _res = value; }
        }

        /// <summary>
        /// The type argument is an integer value that one of the value shown below, where i.e. type 0 will render a the environment from the model's view at it's bounding box center point considering the reflection angle from the main camera's current position; while type 1 will do the same plus mirroring the rendered image. The default is type 0.
        /// </summary>
        /// <value>The type.</value>
        [ACItemBinding("type", CommandInterpretType.NameValuePairs)]
        public EnviType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "envi"; }
        }

        public string LiteralPart { get; set; }

        #endregion

        #region ICommandGroups Members

        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get;
            set;
        }

        #endregion
    }
}
