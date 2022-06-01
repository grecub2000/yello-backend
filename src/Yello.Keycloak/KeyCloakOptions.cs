﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Yello.KeyCloak
{
    public class KeyCloakOptions
    {
        public string Host { get; set; }
        public string Authority { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Id { get; set; }
    }
}
