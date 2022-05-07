/*using System;

namespace Digital_wellbeing
{
    public class ResetChecker
    {
        public const int HourToResetAt = 3;
        public readonly DateTimeOffset LastOpened;
        private DateTimeOffset CurrentDatetime;

        public ResetChecker(int lastOpenedUnixSecs)
        {
            LastOpened = DateTimeOffset.FromUnixTimeSeconds(lastOpenedUnixSecs);
            CurrentDatetime = DateTimeOffset.Now;
        }
        public bool ShouldReset()
        {
            DateTimeOffset currentDatetime = DateTimeOffset.Now;
            bool isEligibleForReset =
                LastOpened.Day != currentDatetime.Day
                || LastOpened.Month != currentDatetime.Month
                || LastOpened.Year != currentDatetime.Year
                || LastOpened.Hour < HourToResetAt;

            return isEligibleForReset && currentDatetime.Hour >= HourToResetAt;
        }
    }
}*/