﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleServer
{
    public class Message
    {
        public int Id {  get; set; }
        public string Text { get; set; }
        public DateTime TimeSend { get; set; }
        public string UserIP { get; set; }
    }
}
