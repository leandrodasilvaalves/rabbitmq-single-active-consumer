using Prometheus;

namespace Poc.MassTransit.SingleContainer;

public class Metric
{
    public static readonly Histogram Consumer = Metrics.CreateHistogram(
        "consumer_processing_time", string.Empty, new HistogramConfiguration
        {
            Buckets = Histogram.PowersOfTenDividedBuckets(0, 2, 10),
        });
}