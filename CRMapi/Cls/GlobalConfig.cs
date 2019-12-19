using System;
using System.Collections.Generic;

namespace CRMapi.Cls
{
    public static class GlobalConfig
    {
        private static Model.Config _config { set; get; }
        public static void SetCOnfig(Model.Config config)
        {
            _config = config;
        }
        public static Model.Config GetConfig => _config;

    }
}
