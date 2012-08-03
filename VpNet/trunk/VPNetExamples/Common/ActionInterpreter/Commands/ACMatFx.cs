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
using VPNetExamples.Common.ActionInterpreter.Types;

namespace VPNetExamples.Common.ActionInterpreter.Commands
{
    /// <summary>
    /// Material effects (matfx) are visual effects that can be applied to object surfaces. Different types of material effects are available.
    /// </summary>
    [Serializable]
    public sealed class ACMatFx : IActionCommand
    {
        private MatFxType _type;
        private string _texture;
        private float _coef;
        private int _tag;
        private TextureBlendType _blend;
        private string _name;
        private bool _isGlobal;

        /// <summary>
        /// Initializes a new instance of the <see cref="ACMatFx"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="texture">The texture.</param>
        /// <param name="coef">The coef.</param>
        /// <param name="tag">The tag.</param>
        /// <param name="blendSource">The blend source.</param>
        /// <param name="blendDestination">The blend destination.</param>
        /// <param name="name">The name.</param>
        /// <param name="isGlobal">if set to <c>true</c> [is global].</param>
        public ACMatFx(MatFxType type, string texture, float coef, int tag, TextureBlendType blend, string name, bool isGlobal)
        {
            _type = type;
            _texture = texture;
            _coef = coef;
            _tag = tag;
            _blend = blend;
            _name = name;
            _isGlobal = isGlobal;
        }
        public ACMatFx(){}

        /// <summary>
        /// The blend argument is only used for dual texturing. It defines the blending mode. It sets the source and destination blend function used in blended transparency and antialiasing operations. The source function specifies the factor that is multiplied by the source color; this value is added to the product of the destination factor and the destination color. The source is the underlaying texture and the destination is the overlayed texture.
        /// </summary>
        /// <value>The blend.</value>
        [ACItemBinding("blend", CommandInterpretType.NameValuePairs)]
        [Description("The blend argument is only used for dual texturing. It defines the blending mode. It sets the source and destination blend function used in blended transparency and antialiasing operations. The source function specifies the factor that is multiplied by the source color; this value is added to the product of the destination factor and the destination color. The source is the underlaying texture and the destination is the overlayed texture.")]
        [Browsable(true)]
        public TextureBlendType Blend
        {
            get { return _blend; }
            set { _blend = value; }
        }

        /// <summary>
        /// The tag argument specifies the tag number of the surface to render on. Any tag that exists on the used object can be used, including tags of jointed avatars. A value of 0 (zero), also the default value, means to apply the material effect onto all surfaces of the geometry.
        /// </summary>
        /// <value>The tag.</value>
        [ACItemBinding("tag", CommandInterpretType.NameValuePairs)]
        [Description("The tag argument specifies the tag number of the surface to render on. Any tag that exists on the used object can be used, including tags of jointed avatars. A value of 0 (zero), also the default value, means to apply the material effect onto all surfaces of the geometry.")]
        [Browsable(true)]
        public int Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }

        /// <summary>
        /// The global argument will cause triggers to initiate the command for all users have the object in view. Without it, the command will be triggered exclusively for the user who activates the trigger (bump, activate, adone). By default, commands are not global.
        /// </summary>
        /// <value><c>true</c> if this instance is global; otherwise, <c>false</c>.</value>
        [ACItemBinding("global", CommandInterpretType.Flag)]
        [Description("The global argument will cause triggers to initiate the command for all users have the object in view. Without it, the command will be triggered exclusively for the user who activates the trigger (bump, activate, adone). By default, commands are not global.")]
        [Browsable(true)]
        public bool IsGlobal
        {
            get { return _isGlobal; }
            set { _isGlobal = value; }
        }

        /// <summary>
        /// The name argument can be used for remote control, applying the matfx command onto the named object within view of the same owner.
        /// </summary>
        /// <value>The name.</value>
        [ACItemBinding("name", CommandInterpretType.NameValuePairs)]
        [Description("The name argument can be used for remote control, applying the matfx command onto the named object within view of the same owner.")]
        [Browsable(true)]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// The coef argument sets the coefficient for the used effect, which means the strength of the effect. It can have a value between 0.05 and 1.0. The default coefficient is 0.333.
        /// </summary>
        /// <value>The coef.</value>
        [ACItemBinding("coef", CommandInterpretType.NameValuePairs)]
        [Description("The coef argument sets the coefficient for the used effect, which means the strength of the effect. It can have a value between 0.05 and 1.0. The default coefficient is 0.333.")]
        [Browsable(true)]
        public float Coef
        {
            get { return _coef; }
            set { _coef = value; }
        }

        /// <summary>
        /// The tex argument specifies an optional texture to use for the selected material effect. It can be either a environment map, a bump map or an onverlayed texture, depending on the used type. The texture must be found in the textures subfolder of the worlds object path. The keyword self used as texture name will use the material's original texture, if any, for the material effect. If self is chosen and the object has no texture specified, no effect will be generated. If the tex argument is not given, the default texture in the subfolder of the browser's installation directory (/default/textures/gloss.png) is used.
        /// </summary>
        /// <value>The texture.</value>
        [ACItemBinding("texture", CommandInterpretType.NameValuePairs)]
        [Description("The tex argument specifies an optional texture to use for the selected material effect. It can be either a environment map, a bump map or an onverlayed texture, depending on the used type. The texture must be found in the textures subfolder of the worlds object path. The keyword self used as texture name will use the material's original texture, if any, for the material effect. If self is chosen and the object has no texture specified, no effect will be generated. If the tex argument is not given, the default texture in the subfolder of the browser's installation directory (/default/textures/gloss.png) is used.")]
        [Browsable(true)]
        public string Texture
        {
            get { return _texture; }
            set { _texture = value; }
        }

        /// <summary>
        /// The type argument is an integer value that can be either in the range from 0 to 4 or in the range from 10 to 14. If no type option is specified, type 1 is used as default.
        /// </summary>
        /// <value>The type.</value>
        [ACItemBinding("type", CommandInterpretType.NameValuePairs)]
        [Description(" The type argument is an integer value that can be either in the range from 0 to 4 or in the range from 10 to 14. If no type option is specified, type 1 is used as default.")]
        [Browsable(true)]
        public MatFxType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        [Browsable(false)]
        public string LiteralPart { get; set; }

        #region ILiteralAction Members

        [Browsable(false)]
        public string LiteralAction
        {
            get { return "matfx"; }
        }

        #endregion

        #region ICommandGroups Members

        [Browsable(false)]
        public System.Collections.Generic.IList<ICommandGroup> CommandGroups
        {
            get;
            set;
        }

        #endregion
    }
}
