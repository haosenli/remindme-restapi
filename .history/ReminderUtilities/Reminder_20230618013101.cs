using System.Text.RegularExpressions;

namespace ReminderUtilities
{
    /// <summary>
    /// Parses and encapsulates data for reminders.
    /// <summary>
    public class Reminder
    {
        public readonly string authorId;
        public readonly string messageChannelId;
        public readonly string messageContent;
        public readonly string utcTime;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="Reminder"/> class from the SocketUserMessage.
        /// If the time units in the message is unsupported, the messageContent will be overwritten by an
        /// Exception message, and the utcTime will be set to the current time. This is done so that the
        /// Reminder will be sent to the Discord user immediately for them to correct their reminder.
        /// </summary>
        public Reminder(string authorId, string messageChannelId, string messageContent)
        {
            // Resource on mentioning users:
            // https://stackoverflow.com/questions/48855693/discord-net-how-to-make-bot-ping-users-with
            this.authorId = authorId;
            this.messageChannelId = messageChannelId;
            try
            {
                (this.utcTime, this.messageContent) = parseMessage(messageContent);
            }
            catch (NotSupportedException ex)
            {
                this.utcTime = DateTime.UtcNow.ToString();
                this.messageContent = ex.Message;
            }
        }

        /// <summary>
        /// Parses a message to extract the utc time and message contents.
        /// </summary>
        /// <returns>A String utc time and String message content parsed from the message.</returns>
        /// <exception cref="NotSupportedException">Thrown when a time unit is unsupported.</exception>
        private (String, String) parseMessage(string messageContent)
        {
            string[] messageParts = messageContent.Split(' ');

            // Tokenize the raw time input
            string rawTime = messageParts[0];
            string[] timeValues = SplitTime(rawTime);
            int timeOffset = 0;

            // Calculate total time offset
            foreach (string timeValue in timeValues)
            {
                int splitIndex = timeValue.Length - 1;
                int timeAmount = int.Parse(timeValue[0 .. splitIndex]);
                char timeUnit = char.ToLower(timeValue[splitIndex]);

                switch (timeUnit)
                {
                    case 'w':
                        timeOffset += timeAmount * 604800;
                        break;

                    case 'd':
                        timeOffset += timeAmount * 86400;
                        break;
                    
                    case 'h':
                        timeOffset += timeAmount * 3600;
                        break;

                    case 'm':
                        timeOffset += timeAmount * 60;
                        break;
                    
                    case 's':
                        timeOffset += timeAmount;
                        break;

                    default:
                        throw new NotSupportedException("Time unit '' not supported. Only w (weeks), d (days), h (hours), m (minutes), s (seconds) are allowed.");
                }
            }

            // Calculate time in UTC
            String utcTime = DateTime.UtcNow.AddSeconds(timeOffset).ToString();

            // Parse reminder message
            string content = String.Join(" ", messageParts[1 ..]);
            return (utcTime, content);
        }

        /// <summary>
        /// Splits a string time input into an array of time.
        /// </summary>
        /// <returns>An array of string times.</returns>
        static string[] SplitTime(string input)
        {
            string pattern = @"\d+[wdhmsWDHMS]";
            return Regex.Matches(input, pattern)
                        .Cast<Match>()
                        .Select(match => match.Value)
                        .ToArray();
        }
    }
}
