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
    public class TravelFilterItem : DialogueItem
    {
        private OutputAction outputAction;
        string denyID, failID;
        string denyContext;
        public TravelFilterItem(string id, string failID, string denyID, string denyContext)
        {
            this.denyContext = denyContext;
            this.failID = failID;
            this.denyID = denyID;
            this.id = id;
        }
        
        public override void Initialize(Agent ownerAgent)
        {
            base.Initialize(ownerAgent);
            foreach (Pattern pattern in outputAction.PatternList)
            {
                pattern.ProcessDefinition();
                //     pattern.ProcessDefinitionList();
            }
        }

        public override Boolean Run(List<object> parameterList, out string targetContext, out string targetID)
        {
            Boolean gotWrongRequest = false;
            Boolean gotNewInput = false;
            string currOrigin = "";
            string currDestination = "";
            string currTime = "";

            base.Run(parameterList, out targetContext, out targetID);

            string prefixString = outputAction.GetString(ownerAgent.RandomNumberGenerator, null);

            MemoryItem norigSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_1);
            MemoryItem ndestSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_2);
            MemoryItem origSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_3);
            MemoryItem destSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_4);
            MemoryItem timeSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_5);


            if (timeSought != null)
                currTime = (string)norigSought.GetContent();


            if (norigSought != null)
            {
                currOrigin = (string)norigSought.GetContent();
                gotNewInput = gotNewInput || (currOrigin != "");
            }
            if (ndestSought != null)
            {
                currDestination = (string)ndestSought.GetContent();
                gotNewInput = gotNewInput || (currDestination != "");
            }

            if (origSought != null && currOrigin == "")
            {
                currOrigin = (string)origSought.GetContent();
                gotWrongRequest = gotWrongRequest || (currOrigin == "");
            }
            if (destSought != null && currDestination == "")
            {
                currDestination = (string)destSought.GetContent();
                gotWrongRequest = gotWrongRequest || (currDestination == "");
            }


            /*
            //clears all queries
            StringMemoryItem matchMemoryClean = new StringMemoryItem();
            matchMemoryClean.TagList = speechQueries;
            matchMemoryClean.SetContent("");
            ownerAgent.WorkingMemory.AddItem(matchMemoryClean);
            */

            //if the user tried to add something, put items back on stack
            if (gotNewInput)
            {

                //puts <Q3> = currOrigin, <Q4> = currDestination, <Q5> = currTime
                ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_3, currOrigin);
                ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_4, currDestination);
                ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_5, currTime);
            }

            //If the user did not try to add a value, assume he wants to cancel
            else
            {
                ownerAgent.SendSpeechOutput("Okay then.");
                targetContext = denyContext;
                targetID = denyID;
                return true;
            }


            //User tried to give already given information
            if (gotWrongRequest)
            {
                bool originMissing = (currOrigin == "");

                ownerAgent.SendSpeechOutput("I already got your " + (!originMissing ? "origin" : "destination") + " please tell me your " + (originMissing ? "origin." : "destination."));

                targetContext = outputAction.TargetContext;
                targetID = failID;
                return true;
                
            }



            targetContext = outputAction.TargetContext;
            targetID = outputAction.TargetID;
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
