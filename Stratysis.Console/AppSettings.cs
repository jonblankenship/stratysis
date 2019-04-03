using System;
using System.Collections.Generic;
using System.Text;
using Stratysis.Domain.Settings;

namespace Stratysis.Console
{
    public class AppSettings: IAppSettings
    {
        public string QuandlApiKey { get; set; }

        public string TestSetting { get; set; }
    }
}
