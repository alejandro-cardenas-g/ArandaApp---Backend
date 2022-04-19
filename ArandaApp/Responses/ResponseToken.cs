using ArandaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.Responses
{
    public class ResponseToken
    {

        public bool ok { get; set; }
        public string token { get; set; }
        public Auth data { get; set; }
        public string error { get; set; }

    }
}
