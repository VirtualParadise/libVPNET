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
using VPNetExamples.Common.ActionInterpreter;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VPNetExamples.Common.ActionInterpreter.Types;
using VpNet.Core.Structs;
using VpNetFramework.Common.ActionInterpreter.Attributes;
using VpNetFramework.Common.ActionInterpreter.Interfaces;
using VpNetFramework.Common.ActionInterpreter.Types;

namespace VpNetFramework.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The light command causes an object to emit light into the surrounding scene. The light emits from the center of
    /// the object (as determined by its bounding box, not its origin) and shines on any surrounding polygons facing
    /// towards the object. The light command has many optional arguments
    /// </summary>
    [Serializable]
    public sealed class ACLight : IActionCommand, IActionCommandName
    {
        private LightType _type;
        private Color _color;
        private float _brightness;
        private float _radius;
        private LightFxType _fx;
        private float _time;
        private float _angle;
        private string _name;
        private float _pitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACLight"/> class.
        /// </summary>
        public ACLight()
        {
                
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActionCommandLight"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="color">The color.</param>
        /// <param name="brightness">The brightness.</param>
        /// <param name="radius">The radius.</param>
        /// <param name="fx">The fx.</param>
        /// <param name="time">The time.</param>
        /// <param name="angle">The angle.</param>
        /// <param name="name">The name.</param>
        /// <param name="pitch">The pitch.</param>
        public ACLight(LightType type, Color color, float brightness, float radius, LightFxType fx, float time, float angle, string name, float pitch)
        {
            _type = type;
            _color = color;
            _brightness = brightness;
            _radius = radius;
            _fx = fx;
            _time = time;
            _angle = angle;
            _name = name;
            _pitch = pitch;
        }

        /// <summary>
        /// Gets or sets the pitch.
        /// </summary>
        /// <value>The pitch.</value>
        [ACItemBinding("pitch", CommandInterpretType.NameValuePairs)]
        public float Pitch
        {
            get { return _pitch; }
            set { _pitch = value; }
        }

        /// <summary>
        /// The name specifies the name of another nearby object to place the new light on.
        /// Note that light sources require additional CPU time to render and thus should be used sparingly. Each new light source added to a scene will reduce the frame rate by a small amount. Excessive use of extra lights can severely impact the performance of an area or world. Some graphics cards or drivers may limit the number of light sources which will render at the same time on a given computer system.
        /// Also, note that the degree to which a light source "shines" on a particular object is also a function of the diffuse component of the surface lighting properties of that object. The lower the diffuse lighting component of the object, the less a light source will influence it. If an object has no diffuse lighting, light sources will not have any effect on it.
        /// Each object may have only one light source applied to it at a time.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding("name",CommandInterpretType.NameValuePairs)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The angle and pitch arguments control "spot" light sources. The angle specifies how wide (in degrees) a cone of light emits from the spot; the default is 45 degrees. The pitch specifies the angle up from straight down that the spot light points. The default pitch is 0, meaning spot light point straight down by default.
        /// </summary>
        /// <value>The angle.</value>
        [ACItemBinding("angle", CommandInterpretType.NameValuePairs)]
        public float Angle
        {
            get { return _angle; }
            set { _angle = value; }
        }

        /// <summary>
        /// The fx argument specifies one of several optional lighting "effects" that can be applied to the light source. All of the effects cause the brightness of the object to vary over time.
        /// </summary>
        /// <value>The fx.</value>
        [ACItemBinding("fx", CommandInterpretType.NameValuePairs)]
        public LightFxType Fx
        {
            get { return _fx; }
            set { _fx = value; }
        }

        /// <summary>
        /// The time argument specifies the interval in seconds for the blink, pulse, fadein, and fadeout effects. It has no effect for the other effects. The default time is 1 second.
        /// </summary>
        /// <value>The time.</value>
        [ACItemBinding("time", CommandInterpretType.NameValuePairs)]
        public float Time
        {
            get { return _time; }
            set { _time = value; }
        }

        /// <summary>
        /// The radius specifies the maximum distance away the light shines, in meters. Objects beyond this distance from the light will receive no illumination from the light. The default radius is 10 meters. The radius is particularly useful for preventing lights from shining outside the room in which they are placed; since Active Worlds does not currently support shadows, walls and other objects do not stop lights from shining into adjacent rooms or buildings. Note that the radius is subject to a maximum value as set in the world features for each world.
        /// </summary>
        /// <value>The radius.</value>
        [ACItemBinding("radius", CommandInterpretType.NameValuePairs)]
        public float Radius
        {
            get { return _radius; }
            set { _radius = value; }
        }

        /// <summary>
        ///The brightness specifies how brightly the light source shines. Brightness is specified as a positive floating-point value. The default brightness is 0.5.
        /// </summary>
        /// <value>The brightness.</value>
        [ACItemBinding("brightness", CommandInterpretType.NameValuePairs)]
        public float Brightness
        {
            get { return _brightness; }
            set { _brightness = value; }
        }

        /// <summary>
        /// The color argument specifies the color of the light source and can either be specified as one of many preset word values or as a "raw" hexadecimal value giving the red/green/blue component values (the same format as used for the "BGCOLOR=" tag in HTML).
        /// </summary>
        /// <value>The color.</value>
        [ACItemBinding("color", CommandInterpretType.NameValuePairs)]
        public Color Color
        {
            get { return _color; }
            set { _color = value; }
        }

        /// <summary>
        /// The type specifies the type of light source, which can be either "point" or "spot". "Point" light sources shine equally in all directions, and are the default if no type is specified. "Spot" light sources shine a "cone" of light in a particular direction.
        /// </summary>
        /// <value>The type.</value>
        [ACItemBinding("type", CommandInterpretType.NameValuePairs)]
        public LightType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "light"; }
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
