using Google.Protobuf.WellKnownTypes;

namespace ProductivityEvent.API
{
    internal static class ProductivityScoreCalculator
    {
        internal static double Calculate()
        {
            return 0.0;
        }

        internal static object Calculate(List<ProductivityEventData> list)
        {
            IEnumerable<ProductivityEventData> query = list.Where(ped => ped.EventStart == true).OrderBy(ped => ped.Timestamp);
            double sum = 0.0;

            foreach(var ped in query)
            {
                ProductivityEventData nextItem = query.Where(ped2 => ped2.Timestamp > ped.Timestamp).FirstOrDefault();

                if (nextItem != null)
                {
                    sum += Math.Round((nextItem.Timestamp - ped.Timestamp).Value.TotalSeconds / 60, 0) * ped.ProductivityScore;
                }
                else
                {
                    sum += Math.Round((new DateTimeOffset(DateTime.Now) - ped.Timestamp).Value.TotalSeconds / 60, 0) * ped.ProductivityScore;
                }
            }
            return sum;
        }
    }
}
