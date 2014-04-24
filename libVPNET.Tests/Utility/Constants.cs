using System.Configuration;

namespace VP.Tests
{
    static class Strings
    {
        /// <summary>
        /// Sample string of acceptable length
        /// </summary>
        public const string Sample = "Hello, world!";

        /// <summary>
        /// Sample string with unicode characters
        /// </summary>
        public const string SampleUnicode = "你好世界‼❗❢ ꩷႟";

        /// <summary>
        /// Sample URL of acceptable length
        /// </summary>
        public const string SampleURL = "http://contoso.com";

        /// <summary>
        /// Sample URL of acceptable length
        /// </summary>
        public const string SampleURLUnicode = "https://公司.香港";

        /// <summary>
        /// A 297 length string with unicode multi-byte characters, which should be
        /// automatically split into three strings by the SDK where expected
        /// </summary>
        public const string TooLong = @"鬖鰝鰨 桏毢涒 溙滊煁 鋈 耇胇, 穊 鬎鯪鯠 寁崏庲 柦柋, 簻臗藱 嶭嶴憝 釂鱞 斠 唗哱 鞻饙騴 湹渵焲 緳 綒 蘌轗 垺垼娕 驧鬤鸕 礯籔羻 詵貄 胾臷菨 碢禗禈 瞵瞷矰 槄 鈁陾 岋巠帎 歾炂盵 蓪, 誙 鱙鷭黂 姎岵帔 汫汭沎 珆玸 蛶觢豥 譺鐼霺 瓂癚 蜸, 痵痽 薠薞薘 莃荶衒 舿萐菿 槶. 澉 虥諰諨 嬏嶟樀 娭屔, 韎 曒檃檑 袟袘觕 榾毄, 慖摲摓 虰豖阹 腷腯葹 禠 犵艿 刲匊呥 輐銛靾 坁妢 腶 抩枎 殍涾烰 鏙闛颾 齸圞趲 摮, 圞趲 蒮蒛蜙 躆轖轕 渳湥牋 銪, 饡驦 秎穾籺 駓駗鴀 筩 獿譿躐 佹侁刵 鸙讟钃 澉 殧澞, 刲匊呥 眊砎粁 疿疶砳 鶭黮 墐";
        
        /// <summary>
        /// A non-existant world. Not guaranteed.
        /// </summary>
        public const string World404 = "__null__1921";
    }

    static class Names
    {
        public const string Watch = "SDKTestWatchdog";
        public const string Punch = "SDKPunch";
        public const string Judy  = "SDKJudy";
        public const string Data  = "SDKCmdrData";
    }

    static class Uniservers
    {
        public static Uniserver Invalid = new Uniserver
        {
            CanonicalName = "Invalid",
            Host = "127.0.0.1",
            Port = 10101
        };

        public static Uniserver Timeout = new Uniserver
        {
            CanonicalName = "Timeout",
            Host = "example.com",
            Port = 80
        };
    }

    static class Samples
    {
        public static AvatarPosition AvPosition = new AvatarPosition(10, 20, 30, 40, 50);
        
        public static Vector3 Vector3 = new Vector3(8, 16, 32);

        public static Rotation Rotation = new Rotation(1, 2, 3, 0);

        public static Color Color = new Color(11,22,33);

        public static ChatEffect ChatEffect = ChatEffect.BoldItalic;

        public static VPObject VPObject
        {
            get
            {
                return new VPObject("sign5.rwx#sample", Vector3, Rotation)
                {
                    Description = "This is a test object\nfrom libVPNET unit tests.\nIf found, please\ncontact the owner",
                    Action      = "create sign, rotate 15; SDKTestObject",
                    Type        = 0,
                    Data        = new byte[8]
                };
            }
        }
    }
}
