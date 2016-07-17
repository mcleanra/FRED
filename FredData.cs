using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using RightEdge.Common;
using RightEdge.Indicators;
using Xaye.Fred;

public class FredData : IDisposable
{
	string symbol;
	string key = "47d159f5b869bbb903aea91e26d55ff7";

    public static Xaye.Fred.Fred fred;

    Series series;
    IEnumerable<Observation> observations;

	public FredData(string Symbol, BarFrequency frequency)
	{
		symbol = Symbol;
        if( fred == null )
            fred = new Xaye.Fred.Fred(key, true);
        Populate(frequency);
	}
	private void Populate(RightEdge.Common.BarFrequency Frequency)
	{
        Xaye.Fred.Frequency dataFrequency = ToFredFrequency(Frequency);
		
		try
		{
			series = fred.GetSeries(symbol);
            observations = series.Observations;
            /*
			observations = fred.GetSeriesObservations(symbol, 
				series.ObservationStart,
				series.ObservationEnd,
				DateTime.Now.Date,
				DateTime.Now.Date,
				fred.GetSeriesVintageDates(symbol),
				100000,
				0,
				SortOrder.Ascending,
				Transformation.None,
				series.Frequency,
				AggregationMethod.Average,
				OutputType.RealTime);
             */
			
		}
		catch(Exception e)
		{
			throw new Exception(e.Message);
		}
        
	}

    public Xaye.Fred.Frequency ToFredFrequency(RightEdge.Common.BarFrequency BarFrequency)
    {
        /*
        * Fred Frequencies
        None = 0,
        Daily = 1,
        Weekly = 2,
        BiWeekly = 3,
        Monthly = 4,
        Quarterly = 5,
        SemiAnnual = 6,
        Annual = 7,
        WeeklyFriday = 8,
        WeeklyThursday = 9,
        WeeklyWednesday = 10,
        WeeklyTuesday = 11,
        WeeklyMonday = 12,
        BiWeeklyWednesday = 13,
        BiWeeklyMonday = 14,
         */
        switch (BarFrequency)
        {
            case BarFrequency.Yearly: return Xaye.Fred.Frequency.Annual;
            case BarFrequency.Monthly: return Xaye.Fred.Frequency.Monthly;
            case BarFrequency.Weekly: return Xaye.Fred.Frequency.Weekly;
            default: return Xaye.Fred.Frequency.Daily;
        }
    }
    public RightEdge.Common.BarFrequency GetFrequency()
    {
        /*
		* Fred Frequencies
        None = 0,
        Daily = 1,
        Weekly = 2,
        BiWeekly = 3,
        Monthly = 4,
        Quarterly = 5,
        SemiAnnual = 6,
        Annual = 7,
        WeeklyFriday = 8,
        WeeklyThursday = 9,
        WeeklyWednesday = 10,
        WeeklyTuesday = 11,
        WeeklyMonday = 12,
        BiWeeklyWednesday = 13,
        BiWeeklyMonday = 14,
         */
        switch ((int)series.Frequency)
        {
            case 1: return RightEdge.Common.BarFrequency.Daily;
            case 4:
            case 5:
            case 6: return RightEdge.Common.BarFrequency.Monthly;
            case 7: return RightEdge.Common.BarFrequency.Yearly;
            default:
                return RightEdge.Common.BarFrequency.Weekly;
        }
    }
    public DateTime GetStartDate()
    {
        return series.ObservationStart;
    }
    public DateTime GetEndDate()
    {
        return series.ObservationEnd;
    }
    public double GetObservation(DateTime date)
    {
		double last = Double.NaN;
        foreach (Observation o in observations)
        {
			if( o.Date < date )
			{
				last = (double)o.Value;
				continue;
			}
			else return last;
        }
        return double.NaN;
    }

    public void Dispose()
    {
        fred = null;
    }
}
