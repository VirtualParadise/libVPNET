using System;
using System.Collections.Generic;
using VPNetExamples.Common;
using VPNetExamples.Common.ActionInterpreter.Commands;
using VPNetExamples.Common.ActionInterpreter.Triggers;
using VpNet.Core;
using VpNet.Core.EventData;
using VpNet.Core.Structs;

namespace VPNetExamples.WeatherBot
{
    internal class WeatherBot : BaseExampleBot
    {
        public WeatherBot(){}

        public WeatherBot(IInstance instance) : base(instance) { }

        private List<CurrentWeather> _locations;
        private VpObject _display;


        public override void Initialize()
        {
            _locations=new List<CurrentWeather>();
            Instance.EventQueryCellResult += Instance_EventQueryCellResult;
            Instance.QueryCell(-2, -1);
            Instance.EventObjectClick += EventObjectClick;
            // refresh cache every hour.
            AddTimer(new TimerT<WeatherBot>(CacheRefreshCallBack, this, 60000, 60000)).Start();
        }

        private void CacheRefreshCallBack(WeatherBot state)
        {
            foreach (var location in _locations)
            {
                var ws = new GlobalWeatherService.GlobalWeatherSoapClient();
                ws.BeginGetWeather(Interpreter.Find<ATCreate, ACName>(location.VpObject.Action).Name.Replace("ws-", ""),
                                   string.Empty, WeatherCacheCallback, new object[] {ws, location.VpObject});
            }
        }

        void Instance_EventQueryCellResult(IInstance sender, VpObject objectData)
        {
            var result = Interpreter.Find<ATCreate, ACName>(objectData.Action);
            if (result !=null && result.Name.StartsWith("ws-"))
            {
                //_locations.Add(objectData);
                var ws = new GlobalWeatherService.GlobalWeatherSoapClient();
                ws.BeginGetWeather(Interpreter.Find<ATCreate, ACName>(objectData.Action).Name.Replace("ws-", ""), string.Empty, WeatherCacheCallback, new object[]{ws,objectData});
            }
            else if (result != null && result.Name == "display-ws")
            {
                _display = objectData;
            }
        }

        private void WeatherCacheCallback(IAsyncResult ar)
        {
            var ws = (GlobalWeatherService.GlobalWeatherSoapClient) ((object[]) ar.AsyncState)[0];
            var vpObject = (VpObject)((object[])ar.AsyncState)[1];
            if (ar.IsCompleted)
            {
                try
                {
                    _locations.RemoveAll(p => p.VpObject.Id == vpObject.Id);
                    var  result = SerializationHelpers.Deserialize<CurrentWeather>(ws.EndGetWeather(ar), false);
                    result.VpObject = vpObject;
                    _locations.Add(result);
                }
                catch
                {
                    // no such location.
                }
            }
        }


        void EventObjectClick(IInstance sender, int sessionId, int objectId)
        {
            var o = _locations.Find(p => p.VpObject.Id == objectId);
            if (o != null)
            {
                
                var text = string.Format("Weather in\r\n\r\n{0}:\r\n\r\n{1}\r\n{2}\r\nHumidity: {3}",
                                         o.Location.Split(',')[0], o.Temperature, o.SkyConditions,
                                         o.RelativeHumidity);
                _display.Description = text;
                Instance.ChangeObject(_display);
            }
        }


        private void WeatherCallback(IAsyncResult ar)
        {
            var ws = (GlobalWeatherService.GlobalWeatherSoapClient)ar.AsyncState;
            if (ar.IsCompleted)
            {
                try
                {
                    var result = SerializationHelpers.Deserialize<CurrentWeather>(ws.EndGetWeather(ar), false);
                    var text = string.Format("Weather in\r\n\r\n{0}:\r\n\r\n{1}\r\n{2}\r\nHumidity: {3}",
                                             result.Location.Split(',')[0], result.Temperature, result.SkyConditions,
                                             result.RelativeHumidity);
                    //Instance.Say(text);
                    _display.Description = text;
                    Instance.ChangeObject(_display);
                }
                catch
                {
                    Instance.Say("No such location found.");
                }
            }
            else
            {
                
            }
        }

        public override void Disconnect()
        {
        }
    }
}
