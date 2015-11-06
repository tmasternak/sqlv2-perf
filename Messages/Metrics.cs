using System;
using Metrics;

namespace Messages
{
    public static class Metrics
    {
        static string CSV_REPORTS_PATH = @"C:\SqlTransportTests_v2\" + DateTime.UtcNow.ToString("yyyyMMdd_hh_mm");

        public static void Init()
        {
            Metric.Config.WithHttpEndpoint("http://localhost:1234/");
            Metric.Config.WithReporting(report => report.WithCSVReports(CSV_REPORTS_PATH, TimeSpan.FromSeconds(1)));
        }

    }
}
