using LoreSoft.Blazor.Controls;

using MediatR.CommandQuery.Queries;

namespace EstimatorX.Client.Extensions;

public static class DataSortExtensions
{
    public static List<EntitySort> ToEntitySort(this IEnumerable<DataSort> dataSorts)
    {
        if (dataSorts == null)
            return null;

        return dataSorts
            .Select(s => new EntitySort
            {
                Name = s.Property,
                Direction = s.Descending ? EntitySortDirections.Descending : EntitySortDirections.Ascending
            })
            .ToList();
    }

}
