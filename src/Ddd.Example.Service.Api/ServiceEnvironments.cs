using System;

namespace Ddd.Example.Service.Api
{

    internal static class ServiceEnvironments
    {

        public static bool IsLocal() => Convert.ToBoolean(Environment.GetEnvironmentVariable("LOCAL"));
    }
}
