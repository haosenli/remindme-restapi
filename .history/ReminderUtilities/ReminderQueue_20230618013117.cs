namespace ReminderUtilities
{
    /// <summary>
    /// A priority queue for <see cref="Reminders">.
    /// </summary>
    public class ReminderQueue
    {
        private PriorityQueue<Reminder, DateTime> reminderPQ;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReminderQueue"/> class.
        /// </summary>
        public ReminderQueue()
        {
            reminderPQ = new PriorityQueue<Reminder, DateTime>();
        }

        /// <summary>
        /// Adds a Reminder to the priority queue.
        /// </summary>
        public void addReminder(string authorId, string messageChannelId, string messageContent)
        {
            Reminder newReminder = new Reminder(authorId, messageChannelId, messageContent);
            DateTime timePriority = DateTime.Parse(newReminder.utcTime);
            reminderPQ.Enqueue(newReminder, timePriority);
        }

        /// <summary>
        /// Peeks at the reminder queue. Returns null on empty.
        /// </summary>
        /// <returns>A Reminder object if not empty, null otherwise.</returns>
        public Reminder? peekReminder()
        {
            if (reminderPQ.Count == 0)
            {
                return null;   
            }
            return reminderPQ.Peek();
        }

        /// <summary>
        /// Pops the reminder queue. Returns null on empty.
        /// </summary>
        /// <returns>A Reminder object if not empty, null otherwise.</returns>
        public Reminder? popReminder()
        {
            if (reminderPQ.Count == 0)
            {
                return null;   
            }
            return reminderPQ.Dequeue();
        }
    }
}