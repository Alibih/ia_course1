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
    public class TravelInterestItem : DialogueItem
    {
        private OutputAction outputAction;
        string denyID;
        string denyContext;
        public TravelInterestItem(string id, string denyID, string denyContext)
        {
            this.denyContext = denyContext;
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

            string currOrigin = "";
            string currInterest = "";

            base.Run(parameterList, out targetContext, out targetID);

            string outputString = outputAction.GetString(ownerAgent.RandomNumberGenerator, null);

            MemoryItem origSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_3);
            MemoryItem interestSought = ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_4);


            if (origSought != null)
            {
                currOrigin = (string)origSought.GetContent();
            }
            if (interestSought != null)
            {
                currInterest = (string)interestSought.GetContent();
            }

            if (currOrigin!="")
            {
                string interest = "interest";
                //puts <Q3> = currOrigin, <Q4> = currDestination, <Q5> = currTime
                ownerAgent.SendSpeechOutput(ItemHandler.PutTermsOnTaggedString(outputString, 
                    new List<string>{ AgentConstants.QUERY_TAG_3, AgentConstants.QUERY_TAG_4}, 
                    new List<string>{ currOrigin,currInterest}));
                ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_3, currOrigin);
                ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_4, currInterest);
                ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_5, interest); //signifies that this is an interestSearch
            }

            //If the user did not try to add a value, assume he wants to cancel
            else
            {
                ownerAgent.SendSpeechOutput("Okay then.");
                targetContext = denyContext;
                targetID = denyID;
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
