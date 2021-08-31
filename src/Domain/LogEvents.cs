using Microsoft.Extensions.Logging;

namespace Domain
{
    /// <summary>
    /// LogEvents is a set of static log event ids
    /// Events are categorized in 3 levels:
    /// All informational events starts with 1, followed by 3 numbers (e.g. 1001)
    /// All warning events starts with 2, followed by 3 numbers (e.g. 2001)
    /// All error events starts with 3, followed by 3 numbers (e.g. 3001)
    /// </summary>
    public static class LogEvents
    {
        // informational events
        public static EventId OrderCreateEvent = new EventId(1001, nameof(OrderCreateEvent));
        public static EventId OrderGetEvent = new EventId(1002, nameof(OrderGetEvent));
        public static EventId OrderGetListEvent = new EventId(1003, nameof(OrderGetListEvent));
        public static EventId OrderDeleteEvent = new EventId(1004, nameof(OrderDeleteEvent));
        public static EventId OrderUpdateEvent = new EventId(1005, nameof(OrderUpdateEvent));

        public static EventId WorkerCreateEvent = new EventId(1006, nameof(WorkerCreateEvent));
        public static EventId WorkerGetEvent = new EventId(1007, nameof(WorkerGetEvent));
        public static EventId WorkerGetListEvent = new EventId(1008, nameof(WorkerGetListEvent));
        public static EventId WorkerDeleteEvent = new EventId(1009, nameof(WorkerDeleteEvent));
        public static EventId WorkerUpdateEvent = new EventId(1010, nameof(WorkerUpdateEvent));

        public static EventId ExpenseGetListEvent = new EventId(1011, nameof(ExpenseGetListEvent));

        // warning events
        public static EventId OrderNotFoundEvent = new EventId(2001, nameof(OrderNotFoundEvent));
        public static EventId WorkerNotFoundEvent = new EventId(2001, nameof(WorkerNotFoundEvent));

        // error events
    }
}