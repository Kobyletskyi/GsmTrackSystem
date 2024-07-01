using System;
using JetBrains.Annotations;
using NodaTime;
using NodaTime.TimeZones;

namespace Api.Helpers
{
    public static class TimeZoneFactory
    {
        public static CientTimeZone Create(ZoneInterval interval){
            return new CientTimeZone(interval);
        } 
    }

    public class CientTimeZone : DateTimeZone
    {
        private ZoneInterval interval;
        public CientTimeZone(ZoneInterval interval) 
            : base(
                interval.Name, 
                interval.Savings.Ticks==0,
                interval.Savings.Ticks<0?interval.WallOffset:interval.WallOffset+interval.Savings,
                interval.Savings.Ticks>0?interval.WallOffset:interval.WallOffset+interval.Savings)
        {
            this.interval = interval;
        }

        public override ZoneInterval GetZoneInterval(Instant instant)
        {
            return new ZoneInterval(
                interval.Name,
                interval.Start,
                interval.End,
                interval.WallOffset,
                interval.Savings);
            }
    }
}