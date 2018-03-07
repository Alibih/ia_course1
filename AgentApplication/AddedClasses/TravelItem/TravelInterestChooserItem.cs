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
    public class TravelInterestChooserItem : InputItem
    {
        private OutputAction outputAction;
        string denyContext, context;
        List<string> paths;
        public InputAction inputYesAction, inputNoAction, inputNextAction, inputBackAction;
        MapControl mapControl;
        public TravelInterestChooserItem(string id, string context, string denyContext,MapControl mapControl) : base(id,new List<string>(),int.MaxValue,int.MaxValue,"","")
        {
            this.mapControl = mapControl;
            this.context = context;
            this.denyContext = denyContext;
            this.id = id;
        }

        public override void Initialize(Agent ownerAgent)
        {
            base.Initialize(ownerAgent);

        }

        private void AddQueryTermsToWorkingMemory(List<string> queryTerms)
        {
            List<MemoryItem> queryMemoryItemList = new List<MemoryItem>();
            for (int ii = 0; ii < queryTerms.Count; ii++)
            {
                string queryTerm = queryTerms[ii];
                string queryTag = QueryTagList[ii];
                StringMemoryItem queryMemoryItem = new StringMemoryItem();
                queryMemoryItem.TagList.Add(queryTag);
                queryMemoryItem.SetContent(queryTerm);
                queryMemoryItemList.Add(queryMemoryItem);
            }
            ownerAgent.WorkingMemory.AddItems(queryMemoryItemList);


        }

        public override Boolean Run(List<object> parameterList, out string targetContext, out string targetID)
        {
            repetitionCount++;
            // These values are set in the derived classes, but must be initialized here, due to the "out" keyword.
            targetContext = "";
            targetID = "";

            base.Run(parameterList, out targetContext, out targetID);
            
            MemoryItem origSought = ItemHandler.PeekItem(ownerAgent, AgentConstants.QUERY_TAG_3);
            MemoryItem destSought = ItemHandler.PeekItem(ownerAgent, AgentConstants.QUERY_TAG_4);

            if (repetitionCount > MaximumRepetitionCount)  // Giving up after repeated incomprehensible inputs: Leave the dialogue
            {
                targetContext = FinalFailureTargetContext;
                targetID = FinalFailureTargetID;
                if (doReset)
                {
                    repetitionCount = 0;
                }
                return true;
            }
            else
            {
                string inputString = (string)parameterList[0];
                string tag = (string)parameterList[1]; // see Agent.HandleInput()
                Pattern matchingPattern = null;
                targetContext = context;

                //Boolean isMatching = inputYesAction.CheckMatch(inputString, tag, out matchingPattern);
                if (inputYesAction.CheckMatch(inputString, tag, out matchingPattern))
                {

                    ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_4);
                    if (origSought != null)
                    {
                        string currDestination = mapControl.AddressesOfInterest[mapControl.InterestPointer];
                        ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_3, origSought.GetContent().ToString());
                        ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_4, currDestination);
                        ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_5, "");

                        targetID = inputYesAction.TargetID;
                        targetContext = inputYesAction.TargetContext;
                    }
                }
                else if (inputNoAction.CheckMatch(inputString, tag, out matchingPattern))
                {
                    ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_3);
                    ownerAgent.SendSpeechOutput("Oh ok.");
                    targetID = inputNoAction.TargetID;
                    targetContext = inputNoAction.TargetContext;
                }
                else if (inputBackAction.CheckMatch(inputString, tag, out matchingPattern))
                {
                    mapControl.InterestPointer = mapControl.InterestPointer-1;
                    if (mapControl.InterestPointer < 0)
                    {
                        mapControl.InterestPointer = 0;
                        ownerAgent.SendSpeechOutput("you can't go back anymore.");
                        targetID = id;
                        targetContext = context;
                    }
                    else
                    {
                        targetID = inputBackAction.TargetID;
                        targetContext = inputBackAction.TargetContext;
                        ownerAgent.SendSpeechOutput("The previous item is  " + mapControl.LocationsOfInterest[mapControl.InterestPointer] + ". Do you want this one?");
                    }
                }
                else if (inputNextAction.CheckMatch(inputString, tag, out matchingPattern))
                {
                    mapControl.InterestPointer = mapControl.InterestPointer+1;
                    if (mapControl.InterestPointer > mapControl.AddressesOfInterest.Count)
                    {
                        ownerAgent.SendSpeechOutput("you can't go forward anymore.");
                        targetID = id;
                        targetContext = context;
                    }
                    else
                    {
                        ownerAgent.SendSpeechOutput("The next one is  " + mapControl.LocationsOfInterest[mapControl.InterestPointer] + ". Do you want this one?");
                        targetID = inputNextAction.TargetID;
                        targetContext = inputNextAction.TargetContext;
                    }
                }
            }


            if(targetID != id)
                ItemHandler.PopItem(ownerAgent, AgentConstants.QUERY_TAG_3);
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
