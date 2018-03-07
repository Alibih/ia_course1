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

    [DataContract]
    public class FriendlyInputItem : InputItem
    {
        
        private OutputAction outputAction;
        InputAction inputAction;
        public FriendlyInputItem(string id, List<string> queryTagList, double timeoutInterval, int maximumRepetitionCount) : base(id, queryTagList, timeoutInterval, maximumRepetitionCount,"","") { }

        public override void Initialize(Agent ownerAgent)
        {
            base.Initialize(ownerAgent);
            foreach (Pattern pattern in InputAction.PatternList)
            {
                pattern.ProcessDefinition();
                //     pattern.ProcessDefinitionList();
            }
            foreach (Pattern failurePattern in FailureResponsePatternList)
            {
                failurePattern.ProcessDefinition();
            }
        }
        public override Boolean Run(List<object> parameterList, out string targetContext, out string targetID)
        {
            repetitionCount++;
            // These values are set in the derived classes, but must be initialized here, due to the "out" keyword.
            targetContext = "";
            targetID = "";

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
                targetContext = null;
                targetID = null;

                Boolean isMatching = inputAction.CheckMatch(inputString, tag, out matchingPattern);
                if (isMatching)
                {
                    targetContext = inputAction.TargetContext;
                    targetID = inputAction.TargetID;
                    if (doReset)
                    {
                        repetitionCount = 0; // 20171025. Once the agent moves to a different item, the repetition count should be reset.
                    }
                    List<string> queryTerms = matchingPattern.GetQueryTerms();
                    Tuple<List<string>, List<string>> queryTagLists = GetQueriesFrom(matchingPattern.Definition, QueryTagList);

                     AddQueryTermsToWorkingMemory(queryTerms,queryTagLists);

                    return true;
                }
                else { repetitionCount -= 1; }  // Does not count as a repetition if no match was found.

            }
            return false;
        }

        //creates a new list of queries with the same order as the input string
        public static Tuple<List<string>,List<string>> GetQueriesFrom(string str, List<string> queries)
        {
            List<string> matches = new List<string>();
            List<string> unmatches = new List<string>();
            string[] words = str.Split(' ');

            foreach (string word in words)
                if (queries.Contains(word))
                    matches.Add(word);
            unmatches = queries.Except(matches).ToList();
            return new Tuple<List<string>, List<string>>(matches, unmatches);
        }
        private void AddQueryTermsToWorkingMemory(List<string> queryTerms, Tuple<List<string>, List<string>> queryTagList)
        {

            //sets values at same tag as it was recieved
            foreach (string tag in queryTagList.Item1)
            {
                string term = queryTerms[0];
                queryTerms.RemoveAt(0);
                StringMemoryItem queryMemoryItem = ItemHandler.StoreTermOnTag(ownerAgent, tag, term);
            }
            foreach(string tag in queryTagList.Item2)
            {   
                //fills unfilled queries with empty string
                StringMemoryItem queryMemoryItem = ItemHandler.StoreTermOnTag(ownerAgent, tag, "");
            }
        }

        [DataMember]
        public OutputAction OutputAction
        {
            get { return outputAction; }
            set { outputAction = value; }
        }
        [DataMember]
        public InputAction InputAction
        {
            get { return inputAction; }
            set { inputAction = value; }
        }

    }
}
