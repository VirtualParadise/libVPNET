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
using System.Collections.Generic;
using VPNetExamples.Common.ActionInterpreter.Interfaces;
using VpNet.Core.EventData;
using VpNet.Core.Structs;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// The animate command assigns an animation to the object. It is by far the most complex and probably least understood
    /// action command. For this reason, the following help pages have been added to address specific uses of the animate
    /// command:
    /// Application of a single texture
    /// Applying a sequence of textures
    /// Using the animate command as a timer
    /// </summary>
    [Serializable]
    [Obsolete("Currently not implemented in this VPNet implementation.")]
    public sealed class ACAnimate : IActionCommand
    {
        private readonly string _tag;
        private readonly string _mask;
        private readonly VpObject _model;
        private readonly string _animationName;
        private readonly int _imageCount;
        private readonly int _frameCount;
        private readonly long _frameDelay;
        private readonly List<int> _frameList;
        private readonly bool _isGLobal;



        /// <summary>
        /// The optional tag argument specifies the tag number of the polygon on the object to which the animation is applied.
        /// If omitted, the animation is applied to every polygon on the object.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is G lobal; otherwise, <c>false</c>.
        /// </value>
        public bool IsGLobal
        {
            get { return _isGLobal; }
        }

        /// <summary>
        /// The optional framelist argument allows you to specify the order of images to be displayed in the animation. This 
        /// argument must be specified if imagecount and framecount are different. The default frame list is 1 through
        /// imagecount.
        /// </summary>
        /// <value>The frame list.</value>
        public List<int> FrameList
        {
            get { return _frameList; }
        }

        /// <summary>
        /// The framedelay argument specifies the time in milliseconds between each frame of the animation. Note that low
        /// numbers for the framedelay may cause the animation to drop frames in order to keep up on slower systems. A 
        /// framedelay of 0 means animate at the same rate as the browser's current frame rate. The framedelay argument is a
        /// 64-bit integer (max value: 18446744073709551615)
        /// </summary>
        /// <value>The frame delay.</value>
        public long FrameDelay
        {
            get { return _frameDelay; }
        }

        /// <summary>
        /// The framecount argument specifies the total number of frames in the animation sequence. Note this does not have to
        /// be the same as imagecount since a given image can be used more than once in an animation sequence.
        /// </summary>
        /// <value>The frame count.</value>
        public int FrameCount
        {
            get { return _frameCount; }
        }

        /// <summary>
        /// The imagecount argument specifies the total number of unique textures in the animation.
        /// </summary>
        /// <value>The image count.</value>
        public int ImageCount
        {
            get { return _imageCount; }
        }

        /// <summary>
        /// The animation-name argument specifies the base name of the textures to be used in the animation.
        /// </summary>
        /// <value>The name of the animation.</value>
        public string AnimationName
        {
            get { return _animationName; }
        }

        /// <summary>
        /// The object-name argument specifies the name of the object to apply the animation to. This argument must be
        /// specified. If the animation is to be applied to this object, specify the keyword me as the object name. Object
        /// names are assigned via the name command. The Animate command always requires the use of an object name, and will 
        /// therefore recognize the name me as meaning the object on which the command is found.
        /// </summary
        /// <value>The model.</value>
        public VpObject Model
        {
            get { return _model; }
        }

        /// <summary>
        /// If mask is specified, the animation is masked, and Active Worlds will attempt to download and apply a
        /// corresponding mask file for each texture in the animation sequence. The nomask option explicitly specifies that
        /// textures applied by the animation are not to be masked. By default, animations are not masked. By default,
        /// animations are now not masked. If you have an animation that uses mask files to apply transparency to parts of the
        /// textures involved, you must now explicitly specify the mask option or all textures in the animation will be
        /// applied unmasked.
        /// </summary>
        /// <value>The mask.</value>
        public string Mask
        {
            get { return _mask; }
        }

        /// <summary>
        /// The optional tag argument specifies the tag number of the polygon on the object to which the animation is applied.
        /// If omitted, the animation is applied to every polygon on the object.
        /// </summary>
        /// <value>The tag.</value>
        public string Tag
        {
            get { return _tag; }
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="ActionAnimate"/> class.
        /// </summary>
        /// <param name="tag">The tag.</param>
        /// <param name="mask">The mask.</param>
        /// <param name="model">The model.</param>
        /// <param name="animationName">Name of the animation.</param>
        /// <param name="imageCount">The image count.</param>
        /// <param name="frameCount">The frame count.</param>
        /// <param name="frameDelay">The frame delay.</param>
        /// <param name="frameList">The frame list.</param>
        /// <param name="isGLobal">if set to <c>true</c> [is G lobal].</param>
        public ACAnimate(string tag, string mask, VpObject model, string animationName, int imageCount, int frameCount, Int64 frameDelay, List<int> frameList, bool isGLobal)
        {
            _tag = tag;
            _mask = mask;
            _model = model;
            _animationName = animationName;
            _imageCount = imageCount;
            _frameCount = frameCount;
            _frameDelay = frameDelay;
            _frameList = frameList;
            _isGLobal = isGLobal;
        }

        public ACAnimate(){}

        #region IActionInterpreter<ActionAnimate> Members

        public ACAnimate FromString(string action)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ILiteralAction Members

        public string LiteralAction
        {
            get { return "animate"; }
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
