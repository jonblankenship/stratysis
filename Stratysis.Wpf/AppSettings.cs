﻿using Stratysis.Domain.Settings;

namespace Stratysis.Wpf
{
    public class AppSettings : IAppSettings
    {
        public string QuandlApiKey { get; set; }

        public string OandaApiKey { get; set; }

        public string QuandlFolderPath { get; set; }

        public string TestSetting { get; set; }

        public decimal DefaultCommission { get; set; }
    }
}
