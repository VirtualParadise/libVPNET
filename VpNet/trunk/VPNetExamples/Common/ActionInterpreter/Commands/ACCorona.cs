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

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The corona command places a corona over an object. 
    /// A corona is a transparent image that is stamped on to the 3D scene on top of an object whenever that object is visible to the user. Although similar in some ways to sprites, they differ in that their size does not vary with distance from the camera, and the entire corona is either visible or not visible depending on whether the corona's source object is currently visible (i.e. not obscured by any other object.) The primary purpose of coronas is for creating "halo" effects around local light sources.
    /// </summary>
    [Serializable]
    public sealed class ACCorona : IActionCommand, IActionCommandName
    {
        private string _texture;
        private string _mask;
        private float _size;
        private string _name;

        public ACCorona(string texture, string mask, int size, string name)
        {
            _texture = texture;
            _mask = mask;
            _size = size;
            _name = name;
        }

        public ACCorona(){}

        /// <summary>
        /// The optional name argument specifies the name of the object to place the corona on. Object names are assigned via the name command.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding("name", CommandInterpretType.NameValuePairs)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The optional size argument controls the size of the corona relative to the size of the 3D viewport. The default is 100.
        /// </summary>
        /// <value>The size.</value>
        [ACItemBinding("size", CommandInterpretType.NameValuePairs)]
        public float Size
        {
            get { return _size; }
            set { _size = value; }
        }

        /// <summary>
        /// The optional mask argument specifies a texture mask file for masking the corona texture, also from the world's object path. If no mask is specified, the texture is self-masking. Self-masking means a transparency mask for the texture is generated from a grayscale version of the same texture. Self-masking is usually sufficient for most corona effects.
        /// </summary>
        /// <value>The mask.</value>
        [ACItemBinding("mask", CommandInterpretType.NameValuePairs)]
        public string Mask
        {
            get { return _mask; }
            set { _mask = value; }
        }

        /// <summary>
        /// The texture specifies the name of the texture to use for the corona. The texture must come from the world's object path. While technically any texture can be used as a corona, obviously for a good effect the texture should be one that has been specifically designed to be a corona. If the object also has a light on it, the corona will automatically be colored the same as the light, if it is a non-white light. Thus, a single corona texture can be used for multiple light sources even if they are different colors.
        /// </summary>
        /// <value>The texture.</value>
        [ACItemBinding("texture", CommandInterpretType.NameValuePairs)]
        public string Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "corona"; }
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
