using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace NSB17WebApiInjection.Controllers
{
    public class SampleController : ApiController
    {
        IEndpointInstance bus;

        public SampleController( IEndpointInstance bus)
        {
            this.bus = bus;
        }

        public string Get() 
        {
            return "Hi, there";
        }
    }
}
