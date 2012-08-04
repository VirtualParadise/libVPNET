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
using System.ComponentModel;
using VPNetExamples.Common.ActionInterpreter.Attributes;
using VPNetExamples.Common.ActionInterpreter.Interfaces;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    [Serializable]
    public sealed class ACTexture : IACName,IActionCommand
    {
        public ACTexture(){}

        /// <summary>
        /// The texture argument specifies the name of the texture to apply. The texture can reside on the world's object path or an external url can be specified
        /// </summary>
        /// <value>
        /// The texture.
        /// </value>
        /// <Author>8/4/2012 3:35 AM cube3</Author>
        [Browsable(true)]
        [Description("The texture argument specifies the name of the texture to apply. The texture can reside on the world's object path or an external url can be specified")]
        [ACItemBinding("", CommandInterpretType.SingleArgument)]
        public string Texture { get; set; }

        /// <summary>
        /// The optional mask argument specifies the name of a mask to apply to the texture. The mask must also reside on the world's object path.
        /// </summary>
        /// <value>
        /// The mask.
        /// </value>
        /// <Author>8/4/2012 3:35 AM cube3</Author>
        [Browsable(true)]
        [Description("The optional mask argument specifies the name of a mask to apply to the texture. The mask must also reside on the world's object path.")]
        [ACItemBinding("mask", CommandInterpretType.NameValuePairs)]
        public string Mask { get; set; }

        /// <summary>
        /// The optional tag argument specifies the numeric tag of the polygon to apply the texture to. Note that using tags does not allow the simultaneous application of different textures to different polygons on the same object; only one "texture" command can be in effect at a time. It is, however, possible to effect multiple surfaces of an object with the same command using the tag parameter if the surfaces have the same tag value.
        /// </summary>
        /// <value>
        /// The tag.
        /// </value>
        /// <Author>8/4/2012 3:36 AM cube3</Author>
        [Browsable(true)]
        [Description("The optional tag argument specifies the numeric tag of the polygon to apply the texture to. Note that using tags does not allow the simultaneous application of different textures to different polygons on the same object; only one \"texture\" command can be in effect at a time. It is, however, possible to effect multiple surfaces of an object with the same command using the tag parameter if the surfaces have the same tag value.")]
        [ACItemBinding("tag", CommandInterpretType.NameValuePairs)]

        public string Tag { get; set; }

        /// <summary>
        /// The optional name argument specifies the name of the object to apply the texture to. Object names are assigned via the name command.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        /// <Author>8/4/2012 3:38 AM cube3</Author>
        [Browsable(true)]
        [Description("The optional name argument specifies the name of the object to apply the texture to. Object names are assigned via the name command.")]
        [ACItemBinding("name", CommandInterpretType.NameValuePairs)]
        public string Name { get; set; }


        public string LiteralAction
        {
            get { return "texture"; }
        }

        /// <summary>
        /// Gets or sets the literal part, which contain the command's properties from the original action string.
        /// </summary>
        /// <value>The literal part.</value>
        public string LiteralPart
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the command groups.
        /// </summary>
        /// <value>The command groups.</value>
        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get;
            set;
        }
    }
}
