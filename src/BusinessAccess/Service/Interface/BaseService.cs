using System;
using Serilog;
using DataAccess.Entity;
using Asset.Common.Timing;
using DataAccess.Entity.EnumType;

namespace BusinessAccess.Service.Interface
{
    public abstract class BaseService
    {
        public int CaculateMinutesOfConfig(SystemConfiguration obj)
        {
            try
            {
                if (obj == null)
                {
                    return 30;
                }
                if (obj.ValueUnit.Equals(Unit.days.ToString()))
                {
                    return Int32.Parse(obj.Value) * 1440;
                }
                if (obj.ValueUnit.Equals(Unit.weeks.ToString()))
                {
                    return Int32.Parse(obj.Value) * 7 * 1440;
                }
                if (obj.ValueUnit.Equals(Unit.minutes.ToString()))
                {
                    return Int32.Parse(obj.Value);
                }
                if (obj.ValueUnit.Equals(Unit.hour.ToString()))
                {
                    return Int32.Parse(obj.Value) * 60;
                }
                if (obj.ValueUnit.Equals(Unit.months.ToString()))
                {
                    var totals = 0;
                    var value = Int32.Parse(obj.Value);
                    var current = Clock.Now;
                    for (int i = 0; i < value; i++)
                    {
                        int days = DateTime.DaysInMonth(current.Year, current.Month + i);
                        totals = totals + days;
                    }
                    return totals * 1440;
                }
            }
            catch (Exception e) { Log.Error("Something wrong when get system config {e}", e); }
            return 30;
        }
    }
}
