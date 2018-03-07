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
    public class TravelItem : DialogueItem
    {
        private OutputAction outputAction;
        private List<string> speechQueries;


        private Random random = new Random();
        private string[] partialFailurePrefix = new string[] { "Can you tell me your", "please specify your", "please tell me your", "Can you specify your"};



        private string failureID, partialFailureID;

        public TravelItem(string id, List<string> queries, string partialFailureID, string failureID)
        {
            this.speechQueries = queries;
            this.id = id;
            this.failureID = failureID;
            this.partialFailureID = partialFailureID;
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
        
        public override Boolean Run(List<object> parameterList, out string targetContext, out string targetID )
        {
            base.Run(parameterList, out targetContext, out targetID);

            string prefixString = outputAction.GetString(ownerAgent.RandomNumberGenerator, null);   //The one outputaction pattern
            string travelDialogue = "";
            string prevString = "";
            string travelInfo = "";
            string currOrigin = "";
            string currDestination = "";
            string currTime = "";
            foreach (string query in speechQueries)
            {
                MemoryItem itemSought = ItemHandler.PopItem(ownerAgent,query);

                if (itemSought == null)
                {
                    prevString = "";
                    continue;
                }
                string itemSoughtString = (string)itemSought.GetContent();
                Console.WriteLine(itemSought.ToString() + "     " + itemSought.GetContent());
                if (itemSoughtString == "")
                {
                    prevString = "";
                    continue;
                }

                if (prevString == "to")
                { 
                    currDestination = itemSoughtString;
                    travelInfo += ItemHandler.SEP + ItemHandler.DESTINATION + currDestination;
                }
                else if (prevString == "from")
                {
                    currOrigin = itemSoughtString;
                    travelInfo += ItemHandler.SEP + ItemHandler.ORIGIN + currOrigin;
                }
                else if (prevString == "at")
                {
                    currTime = itemSoughtString;
                    travelInfo += ItemHandler.SEP + ItemHandler.TIME + currTime;
                }
                travelDialogue += " "+ itemSoughtString;
                prevString = itemSoughtString;
            }

            /*
            //clears all queries
            StringMemoryItem matchMemoryClean = new StringMemoryItem();
            matchMemoryClean.TagList = speechQueries;
            matchMemoryClean.SetContent("");
            ownerAgent.WorkingMemory.AddItem(matchMemoryClean);
            */

            //puts <Q3> = currOrigin, <Q4> = currDestination, <Q5> = currTime

            ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_3, currOrigin);
            ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_4, currDestination);
            ItemHandler.StoreTermOnTag(ownerAgent, AgentConstants.QUERY_TAG_5, currTime);

            Console.WriteLine(currDestination + " " + currOrigin + " " + currTime);
            if (currOrigin != "" && currDestination != "")
            {
                ownerAgent.SendExpression("squinteyes");
                //ownerAgent.SendSpeechOutput(prefixString + travelDialogue);
               //mapControl.NavigateDestination(currDestination, currOrigin, currTime);
                targetID = outputAction.TargetID;
            }
            else if(currOrigin == "" && currDestination == "")
            {
                ownerAgent.SendExpression("shakehead");
                ownerAgent.SendSpeechOutput("I'm sorry I could not make out your origin or destination.");
                targetContext = "";
                targetID = "";
                return false;
            }
            else
            {
                string missingLocation = "";
                if (currOrigin != "")
                    missingLocation = "destination";
                else
                    missingLocation = "origin";


                ownerAgent.SendSpeechOutput(partialFailurePrefix[random.Next(partialFailurePrefix.Length-1)] + missingLocation);
                targetID = partialFailureID;
            }
            //(TRAVEL_MARK + travelInfo);
            Console.WriteLine(travelInfo);
            targetContext = outputAction.TargetContext;


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
