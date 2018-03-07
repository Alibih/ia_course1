using AgentApplication.AddedClasses;
using AgentLibrary;
using AgentLibrary.DialogueItems;
using AgentLibrary.Memories;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AgentApplication.AddedClasses
{
    [DataContract]
    public class TravelPlanningComplete : DialogueItem
    {
        private OutputAction outputAction;


        private string travelID, interestID, failureID;

        MapControl mapControl;
        public TravelPlanningComplete(string id, string travelID, string interestID, string failureID, MapControl mapControl)
        {
            this.id = id;
            this.travelID = travelID;
            this.interestID = interestID;
            this.failureID = failureID;
            this.mapControl = mapControl;
        }


        public override Boolean Run(List<object> parameterList, out string targetContext, out string targetID)
        {
            base.Run(parameterList, out targetContext, out targetID);


            string currOrigin = "";
            string currDestination = "";
            string currTimeOrInterest = "";


            //Sets the query items at the right object

            MemoryItem origSought = ItemHandler.PeekItem(ownerAgent, AgentConstants.QUERY_TAG_3);
            MemoryItem destSought = ItemHandler.PeekItem(ownerAgent, AgentConstants.QUERY_TAG_4);
            MemoryItem timeSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_5);


            if (timeSought != null)
                currTimeOrInterest = (string)timeSought.GetContent();
            if (origSought != null && currOrigin == "")
                currOrigin = (string)origSought.GetContent();
            if (destSought != null && currDestination == "")
                currDestination = (string)destSought.GetContent();


            bool isTime = int.TryParse(currTimeOrInterest, out int time);
            isTime = (isTime && time.ToString().Length == 4) ;
            if (currOrigin != "" && (isTime || currTimeOrInterest == ""))
            {
                currDestination = ItemHandler.TryGetShortcutAddress(currDestination);
                currOrigin = ItemHandler.TryGetShortcutAddress(currOrigin);
                ownerAgent.SendSpeechOutput("Searching from " + currOrigin + " to " + currDestination +
                    ((isTime) ? (" at " + time / 100 + ":" + time % 100) : "")); // "at mmss only when its given
                mapControl.NavigateDestination(currDestination,currOrigin,currTimeOrInterest);

            }
            if(currTimeOrInterest == "interest")
            {
                currOrigin = ItemHandler.TryGetShortcutAddress(currOrigin);
                ownerAgent.SendSpeechOutput("finding out!"); // "at mmss only when its given
                mapControl.NavigateLocations(currDestination, currOrigin);
            }



            if (isTime || currTimeOrInterest == "")
            {
                targetContext = outputAction.TargetContext;
                targetID = travelID;
            }
            else
            {
                targetContext = outputAction.TargetContext;
                targetID = interestID;
            }
            return true;



        }




        [DataMember]
        public OutputAction OutputAction
        {
            get { return outputAction; }
            set { outputAction = value; }
        }
    }
}
