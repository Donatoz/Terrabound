using System;
using System.Collections.Generic;
using System.Linq;

namespace Metozis.TeTwo.Extensions
{
    public static class SystemExtensions
    {
        public static T RandomElementByWeight<T>(this IEnumerable<T> sequence, int seed, Func<T, float> weightSelector) {
            float totalWeight = sequence.Sum(weightSelector);
            double itemWeightIndex =  new Random(seed).NextDouble() * totalWeight;
            float currentWeightIndex = 0;

            foreach(var item in from weightedItem in sequence select new { Value = weightedItem, Weight = weightSelector(weightedItem) }) {
                currentWeightIndex += item.Weight;

                if(currentWeightIndex >= itemWeightIndex)
                    return item.Value;

            }
            
            return default;
        }
    }
}