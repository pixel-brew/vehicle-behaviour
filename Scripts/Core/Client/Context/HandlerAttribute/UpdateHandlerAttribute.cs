using System;
using UnityEngine;

namespace Core.Client.Context
{
    [AttributeUsage(AttributeTargets.Method)]
    public class UpdateHandlerAttribute : Attribute
    {
        public const long MinUpdatePeriod = 30L;
       
        public readonly long UpdatePeriod;

        /// <summary>
        /// Attribute for method which will be called every time period
        /// </summary>
        /// <param name="updatePeriod">Update period in ms.</param>
        public UpdateHandlerAttribute(long updatePeriod = MinUpdatePeriod)
        {
            if (updatePeriod < MinUpdatePeriod)
            {
                Debug.LogWarning($"update handler :: found a method with a refresh rate less than {MinUpdatePeriod}ms, check this");
                updatePeriod = MinUpdatePeriod;
            }
            UpdatePeriod = updatePeriod;
        }
    }
}