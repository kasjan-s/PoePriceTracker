using PoePriceTracker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.DataVisualization.Charting;
using System.Web.Mvc;
using System.Drawing;
using System.IO;

namespace PoePriceTracker.Controllers
{
    public class ItemController : Controller
    {

        public const string DATA_CONNECTION_STRING = @"Data Source=tcp:tiard1bxk6.database.windows.net,1433;Initial Catalog=PoEPriceTracker_db;User Id=PoEPriceTracker@tiard1bxk6;Password=DrAgOn11poe";

        // 
        // GET: /Item/ 

        public ActionResult Index()
        {
            ItemNamesDataContext itemNamesDB = new ItemNamesDataContext(DATA_CONNECTION_STRING);
            ViewBag.Items = from item in itemNamesDB.ItemNames
                            orderby item.Name
                            select item.Name;

            return View();
        }

        // 
        // GET: /Item/Price/ 

        public ActionResult Price(string itemName, string leagueName = "Warbands")
        {
            ViewBag.Name = itemName;
            ViewBag.League = leagueName;

            return View();
        }

        public FileResult CreateChart(string itemName, string leagueName)
        {
            ItemsDataContext itemsDB = new ItemsDataContext(DATA_CONNECTION_STRING);
            var result = from item in itemsDB.Items
                         where item.Name == itemName &&
                               item.League == leagueName
                         orderby item.Timestamp ascending
                         select item;

            Chart chart = new Chart();
            chart.Width = 900;
            chart.Height = 600;
            chart.BackColor = Color.LightGray;
            chart.BorderlineDashStyle = ChartDashStyle.Solid;
            chart.BorderlineWidth = 1;
            chart.Palette = ChartColorPalette.BrightPastel;
            chart.BorderlineColor = Color.Black;
            chart.RenderType = RenderType.BinaryStreaming;
            chart.BorderSkin.SkinStyle = BorderSkinStyle.Emboss;
            chart.AntiAliasing = AntiAliasingStyles.All;
            chart.TextAntiAliasingQuality = TextAntiAliasingQuality.Normal;
            chart.Titles.Add(CreateTitle(string.Format("{0} in {1} league", itemName, leagueName)));
            chart.Legends.Add(new Legend());
            chart.Series.Add(CreateSeries("Items listed", result, x => (float)x.MaxPrice, Color.Blue));
            chart.Series.Add(CreateSeries("Median Price", result, x => (float)x.MedianPrice, Color.Green));
            chart.Series.Add(CreateSeries("Mean Price", result, x => (float)x.MeanPrice, Color.Orange));
            chart.Series.Add(CreateSeries("Min Price", result, x => (float)x.MinPrice, Color.Violet));
            chart.Series.Add(CreateSeries("Max Price", result, x => (float)x.MaxPrice, Color.Red));
            chart.ChartAreas.Add(CreateChartArea("Items listed", "Number of listings"));
            chart.ChartAreas.Add(CreateChartArea("Median Price"));
            chart.ChartAreas.Add(CreateChartArea("Mean Price"));
            chart.ChartAreas.Add(CreateChartArea("Min Price"));
            chart.ChartAreas.Add(CreateChartArea("Max Price"));

            MemoryStream ms = new MemoryStream();
            chart.SaveImage(ms);
            return File(ms.GetBuffer(), @"image/png");
        }

        public Title CreateTitle(string itemName)
        {
            Title title = new Title();
            title.Text = itemName;
            title.ShadowColor = Color.FromArgb(32, 0, 0, 0);
            title.Font = new Font("Trebuchet MS", 14F, FontStyle.Bold);
            title.ShadowOffset = 3;
            title.ForeColor = Color.DarkBlue;
            return title;
        }

        public Series CreateSeries(string name, IOrderedQueryable<Item> items, Func<Item, float> selector, Color color)
        {
            Series seriesDetail = new Series();
            SeriesChartType chartType = SeriesChartType.Line;
            seriesDetail.Name = name;
            seriesDetail.IsValueShownAsLabel = false;
            seriesDetail.Color = color;
            seriesDetail.ChartType = chartType;
            seriesDetail.BorderWidth = 2;
            DataPoint point;

            foreach (Item item in items)
            {
                point = new DataPoint();
                point.AxisLabel = item.Timestamp.ToShortDateString();
                point.YValues = new double[] { (double) selector(item) };
                seriesDetail.Points.Add(point);
            }
            seriesDetail.ChartArea = name;
            return seriesDetail;
        }

        public ChartArea CreateChartArea(string name, string yAxisTitle = "Chaos Orb Equivalent")
        {
            ChartArea chartArea = new ChartArea();
            chartArea.Name = name;
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisY.Title = yAxisTitle;
            chartArea.AxisX.IsLabelAutoFit = false;
            chartArea.AxisY.IsLabelAutoFit = false;
            chartArea.AxisX.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        7F, FontStyle.Regular);
            chartArea.AxisY.LabelStyle.Font =
               new Font("Verdana,Arial,Helvetica,sans-serif",
                        7F, FontStyle.Regular);
            chartArea.AxisX.LabelStyle.Angle = 15;
            chartArea.AxisY.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.MajorGrid.LineColor = Color.FromArgb(64, 64, 64, 64);
            chartArea.AxisX.Interval = 2;

            return chartArea;
        }
    }
}