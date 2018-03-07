using AgentLibrary;
using AgentLibrary.DialogueItems;
using AgentLibrary.Memories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace AgentApplication.AddedClasses
{
    class TravelSearchingCompleteItem : AsynchronousDialogueItem
    {
        private OutputAction outputAction;
        private double maxWaitingTime;
        private int millisecondMaxWaitingTime;
        private int step = 100;
        private MapControl mapControl;
        private string interestID;
        public TravelSearchingCompleteItem()
        {
        }

        public TravelSearchingCompleteItem(string id, string interestID, double maxWaitingTime,MapControl mapControl)
        {
            this.interestID = interestID;
            this.id = id;
            this.maxWaitingTime = maxWaitingTime;
            this.mapControl = mapControl;
        }

        public override void Initialize(Agent ownerAgent)
        {
            base.Initialize(ownerAgent);
            millisecondMaxWaitingTime = (int)Math.Round(1000 * maxWaitingTime);
        }

        protected override void RunLoop(List<object> parameterList)
        {
            //pops out the remaining items
            MemoryItem origSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_3);
            MemoryItem destSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_4);

            string originalContext = ownerAgent.WorkingMemory.CurrentContext;
            int totalWaitingTime = 0;
            bool hasExpired = false;


            ownerAgent.SendExpression("squint"); //tells the agent to really think

            string nextID = OutputAction.TargetID;
            while (!(mapControl.LoadState == MapControl.LOADSTATE.DestinationComplete||mapControl.LoadState == MapControl.LOADSTATE.LocationComplete)  && !hasExpired )
            {
                totalWaitingTime += step;
                hasExpired = totalWaitingTime >= millisecondMaxWaitingTime;
                System.Threading.Thread.Sleep(step);
            }

            if(hasExpired)
            {
                ownerAgent.SendSpeechOutput("Failed to search for the trip");
            }
            else if((mapControl.LoadState == MapControl.LOADSTATE.DestinationComplete) && !hasExpired && mapControl.DepartureTimeListBox.Items.Count > 0)
            {

                ownerAgent.SendSpeechOutput("I found your trip, the transport departs from " + mapControl.Departure + " at " + mapControl.DepartureTimeListBox.Items[0]+ " o' clock");

                ownerAgent.SendExpression("nodhead");
               if (origSought != null && destSought != null)
                {
                    string orig = origSought.GetContent().ToString();
                    string dest = destSought.GetContent().ToString();

                    //tries to remember the gathered position
                    ItemHandler.StoreShortcut(ownerAgent, orig, mapControl.Departure,false);
                    ItemHandler.StoreShortcut(ownerAgent, dest, mapControl.Arrival,false);

                }
                    
            }
            else if(((mapControl.LoadState == MapControl.LOADSTATE.LocationComplete)))
            {
                ownerAgent.SendExpression("nodhead");
                if (mapControl.LocationsOfInterest.Count > 0)
                {
                    ownerAgent.SendSpeechOutput("I found " + mapControl.AddressesOfInterest.Count + " locations nearby, " + mapControl.LocationsOfInterest[0] + " is closest. Do you want to go?");
                    nextID = interestID;


                    ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_3, origSought.GetContent().ToString());

                }
                else
                    ownerAgent.SendSpeechOutput("I could not find any " + destSought.GetContent().ToString() + " places near you.");
            }
            else
            {

                ownerAgent.SendExpression("shakehead");
                ownerAgent.SendSpeechOutput("Uh oh. Something went wrong.");
            }
            AsynchronousDialogueItemEventArgs e = new AsynchronousDialogueItemEventArgs(originalContext, outputAction.TargetContext, nextID);
            OnRunCompleted(e);
        }

        
        [DataMember]
        public OutputAction OutputAction
        {
            get { return outputAction; }
            set { outputAction = value; }
        }

        [DataMember]
        public double WaitingTime
        {
            get { return maxWaitingTime; }
            set { maxWaitingTime = value; }
        }


    }
}
