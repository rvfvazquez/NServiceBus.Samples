﻿using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace NSB21WebAPIHosting.Controllers
{
    public class SamplesController : ApiController
    {
        IBus bus;

        public SamplesController(IBus bus)
        {
            this.bus = bus;
        }

        public string Get()
        {
            return "Hi, there";
        }
    }
}
