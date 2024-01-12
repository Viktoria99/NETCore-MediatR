using Microsoft.Extensions.Logging;


namespace Ddd.Example.Service.Api.Logging
{

    public struct ApplicationLogEvent
    {
        /// <summary>
        /// EVENT_DDD_EXAMPLE_SERVICE_NOT_FOUND.
        /// </summary>
        public static EventId EVENT_DDD_EXAMPLE_SERVICE_NOT_FOUND => new EventId(10001, nameof(EVENT_DDD_EXAMPLE_SERVICE_NOT_FOUND));

        /// <summary>
        /// EVENT_DDD_EXAMPLE_SERVICE_BAD_REQUEST.
        /// </summary>
        public static EventId EVENT_DDD_EXAMPLE_SERVICE_BAD_REQUEST => new EventId(10002, nameof(EVENT_DDD_EXAMPLE_SERVICE_BAD_REQUEST));

        /// <summary>
        /// EVENT_DDD_EXAMPLE_SERVICE_PRECONDITION_FAILED.
        /// </summary>
        public static EventId EVENT_DDD_EXAMPLE_SERVICE_PRECONDITION_FAILED => new EventId(10003, nameof(EVENT_DDD_EXAMPLE_SERVICE_PRECONDITION_FAILED));

        /// <summary>
        /// EVENT_DDD_EXAMPLE_SERVICE_ERROR.
        /// </summary>
        public static EventId EVENT_DDD_EXAMPLE_SERVICE_ERROR => new EventId(10004, nameof(EVENT_DDD_EXAMPLE_SERVICE_ERROR));
    }
}
