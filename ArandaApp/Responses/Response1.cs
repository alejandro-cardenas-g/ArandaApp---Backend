using ArandaApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArandaApp.Responses
{
    public class Response1
    {
        public bool ok { get; set; }
        public Usuario data { get; set; }
        public string error { get; set; }
    }
}
