namespace System.Net.Http
{
    public static class HttpResponseExtensions
    {
        /// <summary>
        /// Returns true, if the value does not equal 200 or 201
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public static bool IsError(this HttpResponseMessage message)
        {
            // > 201, < 200
            return message.StatusCode > HttpStatusCode.Created || message.StatusCode < HttpStatusCode.OK;
        }
    }
}
