using AgentApplication.AddedClasses;
using AgentLibrary;
using AgentLibrary.DialogueItems;
using AgentLibrary.Memories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace AgentApplication.AddedClasses
{
    [DataContract]
    public class ShortcutItem : DialogueItem
    {
        private OutputAction outputAction;
        private string failureID, failureContext;
        private string addressQuery, shortcutQuery;
        
        public ShortcutItem(string id, string addressQuery, string shortcutQuery ,string failureID,string failureContext)
        {
            this.addressQuery = addressQuery;
            this.shortcutQuery = shortcutQuery;
            this.id = id;
            this.failureID = failureID;
            this.failureContext = failureContext;
        }


        public override Boolean Run(List<object> parameterList, out string targetContext, out string targetID)
        {
            base.Run(parameterList, out targetContext, out targetID);

            string address = "", shortcut = "";
            MemoryItem addressMemory = ItemHandler.PopItem(ownerAgent,addressQuery);
            MemoryItem shortcutMemory = ItemHandler.PopItem(ownerAgent, shortcutQuery);

            if (addressMemory != null)
                address = addressMemory.GetContent().ToString();
            if (shortcutMemory != null)
                shortcut = shortcutMemory.GetContent().ToString();

            if (ItemHandler.ShortcutArray.Contains(address))
            {

                ownerAgent.SendSpeechOutput("Please only set addresses as shortcuts, you tried to put the shortcut < "+address+" > as a shortcut.");
                targetID = failureID;
                targetContext = failureContext;
            }
            else
            {
                if (ItemHandler.ShortcutArray.Contains(shortcut))
                {
                    string oldAddress = ItemHandler.TryGetShortcutAddress(shortcut);
                    bool storedReplaced = ItemHandler.StoreShortcut(ownerAgent, shortcut, address,true);
                    if(!storedReplaced)
                        ownerAgent.SendSpeechOutput(shortcut + " " + " will now behave as when you say " + address);
                    else
                        ownerAgent.SendSpeechOutput(shortcut + " replaced the old address  "+ oldAddress + ", and will now act as you say " + address + " instead." );

                    targetID = outputAction.TargetID;
                }
                else if(ItemHandler.TravelAddressesArray.Contains(shortcut))
                {

                    ownerAgent.SendSpeechOutput(" < " + shortcut + " > is an address and not a shortcut.");
                    targetContext = failureContext;
                    targetID = failureID;
                }
                else{
                    ownerAgent.SendSpeechOutput("Uh oh. Something went wrong when you tried to set " + address + " as address and " + shortcut + "as shortcut.");
                }
            }
            /* used if strictness iss neccessary strictness
            else
            {
                ownerAgent.SendSpeechOutput(address + " is not an address. You can only create shortcuts of an address.");
                targetID = failureID;
            }
            */


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
