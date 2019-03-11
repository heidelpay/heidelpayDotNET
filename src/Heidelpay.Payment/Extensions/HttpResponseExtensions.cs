using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace System.Net.Http
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Returns true, if the value does not equal 200
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsError(this HttpResponseMessage message)
        {
            return message.StatusCode != System.Net.HttpStatusCode.OK;
        }
    }
}
