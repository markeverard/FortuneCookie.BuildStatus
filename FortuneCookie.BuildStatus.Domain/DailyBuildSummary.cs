using System;

namespace FortuneCookie.BuildStatus.Domain
{
    /// <summary>
    /// Aggreaget class for build states over the last perios / day
    /// </summary>
    public class DailyBuildSummary
    {
        public DateTime Date { get; set; }
        public int TotalBuilds { get; set; }
        public int FailedBuilds { get; set; }
        public int NumberChangedFiles { get; set; }

        public int GoodBuilds { get { return TotalBuilds - FailedBuilds; } }
        public int GoodPercentage
        {
            get
            {
                if (TotalBuilds == 0)
                    return 0;

                return Convert.ToInt32(100 * (Convert.ToDouble(GoodBuilds) /Convert.ToDouble(TotalBuilds)));
            }
        }
    }
}