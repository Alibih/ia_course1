using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AgentApplication.AddedClasses;
using System.Text.RegularExpressions;

namespace AgentApplication.AddedClasses
{
    public partial class MapControl : UserControl
    {
        private int standardSplitterPosition = 0;
        private int standardBrowserHeightOffset = 0;
        List<string> addressesOfInterest = new List<string>();
        List<string> locationsOfInterest = new List<string>();
        private int interestPointer = 0;
        public enum LOADSTATE
        {
            Nothing,
            DestinationComplete,
            DestinationUnfinished,
            LocationComplete,
            LocationUnfinished
        }
        private LOADSTATE loadState = LOADSTATE.Nothing;
        public MapControl()
        {

            InitializeComponent();
            leftCenterContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            leftCenterContainer.IsSplitterFixed = true;
            departureStationLabel.AutoSize = false;
            arrivalStationLabel.AutoSize = false;
            standardSplitterPosition = leftCenterContainer.SplitterDistance;
            standardBrowserHeightOffset = webBrowser1.Location.Y;
        }

        
        #region map
        public void NavigateLocations(string interest, string origin)
        {
            loadState = LOADSTATE.LocationUnfinished;
            googleHtmlString = "";
            interestPointer = 0;

            webBrowser1.Navigate("https://www.google.com/maps/search/?api=1&query="+ interest + "+at+"+ origin);
            webBrowser1.Refresh();
        }
        public void NavigateDestination(string destination, string origin, string time)// string travelstr, TravelItem travelItem)
        {
            loadState = LOADSTATE.DestinationUnfinished;
            webBrowser1.Navigate("https://rrp.vasttrafik.se/#!P|TP!S|" + origin + "!Z|" + destination + "!date|!time|" + time + "!timeSel|depart!start|1");
            webBrowser1.Refresh();
            webBrowser2.Navigate("https://rrp.vasttrafik.se/#!P|TP!S|" + origin + "!Z|" + destination + "!date|!time|" + time + "!timeSel|depart!start|1");
            webBrowser2.Refresh();
            interestPointer = 0;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string test = ItemHandler.TRAVEL_MARK + ItemHandler.SEP + ItemHandler.ORIGIN + "chalmers" + ItemHandler.SEP + ItemHandler.TIME + "" + ItemHandler.SEP + ItemHandler.DESTINATION + "lindholmen";
            //NavigateDestination(test);
            Console.WriteLine("testing striing : " + test);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Console.WriteLine(webBrowser1.DocumentText);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            htmljsShowMap();
            //webBrowser1.Document.InvokeScript("Hafas.ps.pub('/GUI/map/show', [{trigger: this, ev: event, template: 'Common_MapPageHeaderDefault'}]); return true;");//', [{trigger: this, ev: event, template: 'Common_MapPageHeaderDefault'}]); return false;");
        }


        //looks if the needed element is in the html file.
        private Boolean IsFullyLoaded()
        {
            try
            {
                if (webBrowser1.Url != null)
                {

                    switch (loadState)
                    {
                        case LOADSTATE.DestinationUnfinished:
                        case LOADSTATE.DestinationComplete:
                             webBrowser1.Location = new Point(0, standardBrowserHeightOffset);
                            leftCenterContainer.Panel1Collapsed = false;
                            //västtrafik destination//västtrafik destination
                            if (webBrowser1.Url.AbsoluteUri.StartsWith("https://rrp"))
                            {
                                if (webBrowser1.Document != null)
                                {
                                    if (webBrowser1.Document.Body != null)
                                    {
                                        if (webBrowser1.Document.Body.OuterHtml != null)
                                        {
                                            //searches for something that only shows up when all jscode has fully compiled
                                            if (webBrowser1.Document.Body.OuterHtml.Contains("HFS_ConnectionId_PT_0"))
                                            {
                                                return true;

                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case LOADSTATE.LocationComplete:
                        case LOADSTATE.LocationUnfinished:
                            leftCenterContainer.SplitterDistance = standardSplitterPosition;
                            if (webBrowser1.Url.AbsoluteUri.StartsWith("https://www.google"))
                            {

                                if (!googleHtmlString.Contains("<h3 class=\"section-result-title\"> <span jstcache="))
                                {
                                    foreach (HtmlElement element in webBrowser1.Document.All)
                                    {
                                        if (element.Id == "content-container")
                                        {
                                            googleHtmlString = "";
                                            googleHtmlString += element.InnerHtml;
                                        }
                                    }
                                }
                                else
                                {
                                    return true;
                                }

                            }
                            webBrowser1.Location = new Point(0, 0);
                            leftCenterContainer.Panel1Collapsed = true;
                            break;
                    }

                }
            }
            finally
            { }
            return false;

        }


        private void browser_SizeChanged(object sender, System.EventArgs e)
        {
            htmljsShowMap();
        }
        #region htmlStringParsers
        //forces the web to show the map
        private void htmljsShowMap()
        {
            if (IsFullyLoaded())
                try
                {
                    webBrowser1.Document.InvokeScript("eval", new object[] { "Hafas.ps.pub('/GUI/map/show', [{ trigger: this, ev: event, template: 'Common_MapPageHeaderDefault'}]);" });
                }
                catch (Exception e)
                {
                    Console.WriteLine("err:" + e.Message);
                }
        }
        private void htmlParseLocationContent()
        {
            locationsOfInterest = getWordListAfter(googleHtmlString, "<h3 class=\"section-result-title\"> <span jstcache=", @"[^A-Za-z0-9åäöÅÄÖ:;'.,> ]+", 50, new[] { '<', '>' });
            addressesOfInterest = getWordListAfter(googleHtmlString , "section-result-location\">", @"[^A-Za-z0-9åäöÅÄÖ:;'.,-]", 100, new[] { '>' });
            if (locationsOfInterest != null && addressesOfInterest != null)
            {
                locationsOfInterest.RemoveAt(0);
                addressesOfInterest.RemoveAt(0);

            }
            else
            {
                departureStationLabel.Text = "";
                arrivalStationLabel.Text = "";
                departureListBox.Items.Clear();
                arrivalListBox.Items.Clear();
            }


            //All items has been set
            loadState = LOADSTATE.LocationComplete;
        }

        private void htmlParseDestinationContent()
        {
            List<string> times = getWordListAfter(webBrowser1.Document.Body.OuterHtml ,"\"hfs_timeRow\">", @"[^0-9:]",5,new[] { '<' });
            if (times != null)
            {
                departureListBox.Items.Clear();
                arrivalListBox.Items.Clear();
                string departure, arrival;
                getStations(out departure, out arrival);
                departureStationLabel.Text = departure;
                arrivalStationLabel.Text = arrival;
                for (int i = 0; i < times.Count; i++)
                {
                    if (i % 2 == 0)
                    {
                        departureListBox.Items.Add(times[i]);
                    }
                    else
                    {
                        arrivalListBox.Items.Add(times[i]);
                    }
                }

            }
            else
            {
                departureStationLabel.Text = "";
                arrivalStationLabel.Text = "";
                departureListBox.Items.Clear();
                arrivalListBox.Items.Clear();
            }


            //All items has been set
            loadState = LOADSTATE.DestinationComplete;
        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            fullLoadTimer.Enabled = true;
        }

        private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
        }

        private List<HtmlElement> getElementsByClassName(HtmlElementCollection collection, string classname)
        {
            List<HtmlElement> list = new List<HtmlElement>();
            foreach (HtmlElement elem in collection)
            {
                if (elem.GetAttribute("className") == classname)
                {
                    list.Add(elem);
                }
            }

            return list;
        }

        //these are tailored for västtrafik current västtrafik, will cause crash otherwise
        private List<string> getWordListAfter(string html ,string sep , string pattern, int length, char[] cutoffs)
        {
            List<string> wordList = new List<string>();
            string[] pieces = html.Split(new[] { sep }, StringSplitOptions.None); // splits the html file on [sep] locations (where time is)

            //looks in every piece and saves parts of those that has numbers.
            foreach (string piece in pieces)
            {
                string newpiece = piece.Substring(0, length);
                if(cutoffs.Length > 0)
                    newpiece = newpiece.Split(cutoffs)[cutoffs.Length-1];
                newpiece = Regex.Replace(newpiece, pattern, "");

                if(newpiece.Length <= length)
                    wordList.Add(newpiece);
                /*
                string newTime = piece.Substring(0, 5);

                Console.WriteLine(newTime);
                if (Char.IsDigit(piece[0]))
                {
                    wordList.Add(newTime);
                }
                */
            }
            return wordList;
        }

        private void getStations(out string departure, out string arrival)
        {
            arrival = "";
            departure = "";
            List<string> timeList = new List<string>();
            string sep = "class=\"hfs_station\">";
            string html = webBrowser1.Document.Body.OuterHtml;
            string[] pieces = html.Split(new[] { sep }, StringSplitOptions.None); // splits the html file on [sep] locations (where stations are located)
            int limitSize = 100;


            if (pieces.Length != 3)
            {
                return;
            }

            //scans for words
            while (arrival.Length < limitSize && pieces[1].Length > arrival.Length)
            {
                char newChar = pieces[2][arrival.Length];
                //records the first word coming after the seperator and saves it
                if (!(Char.IsLetterOrDigit(newChar) || newChar.Equals(' '))) // <- change this if country is needed
                    break;
                arrival += newChar;
            }
            while (departure.Length < limitSize && pieces[1].Length > arrival.Length)
            {
                char newChar = pieces[1][departure.Length];
                if (!(Char.IsLetterOrDigit(newChar) || newChar.Equals(' ')))
                    break;
                departure += newChar;
            }
            
        }
        #endregion htmlStringParsers

        //tries to show the map every second
        private void jsTimer_Tick(object sender, EventArgs e)
        {

            try
            {
                if (IsFullyLoaded())
                    switch (loadState)
                    {
                        case LOADSTATE.DestinationComplete:
                        case LOADSTATE.DestinationUnfinished:
                            if (!webBrowser1.Document.Body.OuterHtml.Contains(" visibility: visible; z-index: 100000;"))
                            {
                                htmljsShowMap();
                                
                            }
                            break;
                        case LOADSTATE.LocationComplete:
                        case LOADSTATE.LocationUnfinished:
                            break;

                    }
            }
        
            finally { }
            
        }
        string googleHtmlString = ""; 
        private void fullLoadTimer_Tick(object sender, EventArgs e)
        {
            if (IsFullyLoaded())
            {
                if (loadState == LOADSTATE.DestinationUnfinished)
                    htmlParseDestinationContent();
                else if (loadState == LOADSTATE.LocationUnfinished)
                    htmlParseLocationContent();
                fullLoadTimer.Enabled = false;
            }
        }
        public LOADSTATE LoadState { get => loadState; }
        public string Departure { get => departureStationLabel.Text; }
        public string Arrival { get =>  arrivalStationLabel.Text; }
        public ListBox DepartureTimeListBox{ get => departureListBox; }
        public ListBox ArrivalTimeListBox { get => departureListBox; }
        #endregion map


        public List<string> LocationsOfInterest { get => locationsOfInterest ; }
        public List<string> AddressesOfInterest { get => addressesOfInterest ; }
        public int InterestPointer { get => interestPointer; set => interestPointer = value; }
    }
}
