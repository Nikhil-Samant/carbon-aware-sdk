﻿using CarbonAware;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CarbonAware.Plugins.BasicJsonPlugin
{
    public class CarbonAwareBasicDataPlugin : ICarbonAwarePlugin
    {
        public string Name => "CarbonAwareLogicSamplePlugin";

        public string Description => "Example plugin for Carbon Aware SDK";

        public string Author => "Green Software Foundation";

        public string Version => "0.0.1";

        public object URL => "http://github.com/greensoftwarefoudation";

        private ICarbonAwareStaticDataService CarbonAwareStaticDataService { get; }
        private List<EmissionsData> _emissionsData { get; }
        public CarbonAwareBasicDataPlugin(ICarbonAwareStaticDataService carbonAwareStaticDataServiceService)
        {
            this.CarbonAwareStaticDataService = carbonAwareStaticDataServiceService;
            this._emissionsData = this.CarbonAwareStaticDataService.GetData();
        }

        public EmissionsData GetEmissionsDataForLocationByTime(string location, DateTime time)
        {
            var edList = _emissionsData.Where(ed => ed.Location.Equals(location)).ToList();

            var beforeTime = edList.Where(ed => ed.Time <= time).ToList();

            if (beforeTime.Count == 0)
            {
                return EmissionsData.None;
            }

            var maxTime = beforeTime.Max(ed => ed.Time);

            return beforeTime.FirstOrDefault(ed => ed.Time == maxTime);
        }

        public EmissionsData GetEmissionsDataForLocationByTimeWindow(string location, TimeWindow timeWindow)
        {
            return _emissionsData.FirstOrDefault(ed => ed.Location.Equals(location));
        }

        public List<EmissionsData> GetEmissionsDataForLocationsByTime(List<string> locations, DateTime time)
        {
            return _emissionsData.Where(ed => locations.Contains(ed.Location)).ToList();
        }

        public List<EmissionsData> GetEmissionsDataForLocationsByTimeWindow(List<string> locations, TimeWindow timeWindow)
        {
            return _emissionsData.Where(ed => locations.Contains(ed.Location)).ToList();
        }
        
        public EmissionsData GetBestEmissionsDataForLocationsByTime(List<string> locations, DateTime time)
        {
            return _emissionsData.Where(ed => locations.Contains(ed.Location)).MinBy(ed => ed.Rating);
        }
    }
}
