using System;
using System.Collections.Generic;
using System.Text;
using RightEdge.Common;
using RightEdge.Indicators;

namespace RightEdge.Indicators
{
    //	Use the Indicator Attribute to provide information such as the
    //	name and description of your indicator.  If you do not include
    //	this attribute, your indicator will not show up in the indicator
    //	list.
    [Indicator(System.Drawing.KnownColor.Black,
        Author = "Ryan McLean",
        CompanyName = "1741 Group, LLC",
        Description = "Fred",
        GroupName = "1741 Group",
        HelpText = "This 'indicator' pulls data from the Federal Reserve's FRED system and plots it on a chart.",
        //	The Id attribute needs to be set to a unique code that will identify
        //	your indicator.  A GUID is a good candidate.
        Id = "{495F4F82-GD71-4449-FAF4-6B2D641FE991}",
        Name = "Fred",
        DefaultDrawingPane = "Fred")]
    //	Indicators must be marked Serializable in order to be used in
    //	trading systems.
    [Serializable]
    public class Fred : IndicatorBase
    {
        [NonSerialized]
        private FredData f;
        private string series = "NFCI";

        [ConstructorArgument("Series Name", ConstructorArgumentType.String, "NFCI", 1)]
        public Fred(string _series)
        {
            series = _series;
            this.ChartSettings.ChartPaneName = series;
            f = new FredData(series, BarFrequency.Daily);
        }

        public override RList<double> CalcSeriesValues(RList<BarData> bars)
        {
            return base.CalcSeriesValues(bars);
        }
        public override double CalcNextValue(BarData bar)
        {
            if (f == null)
                f = new FredData(series, BarFrequency.Daily);

            return f.GetObservation(bar.BarStartTime.Date);
        }
    }
}
