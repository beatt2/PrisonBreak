using System;
using System.Collections.Generic;
using AI;
using Items;
using Tools;
using UnityEngine;

namespace Door
{

    public class DoorManager : Singleton<DoorManager>
    {
        private readonly Dictionary<int, List<Doors>> _doorDictionary = new Dictionary<int, List<Doors>>();
        private readonly List<Doors> _googleMemory = new List<Doors>();
        private GetTheGoogle _getTheGoogle;

        public GameObject UiGameObject;


        private void Start()
        {
            _getTheGoogle = new GetTheGoogle();
            Invoke(nameof(DelayedStart), 10);
            
        }

        private void DelayedStart()
        { 
            //opens the first door
            OpenDoor(1); 
        }

        public void AddDoor(Doors door)
        {
            if (!_doorDictionary.ContainsKey(door.ID()))
            {
                _doorDictionary.Add(door.ID(), new List<Doors>());
            }
            _doorDictionary[door.ID()].Add(door);
        }

        /// <summary>
        /// Opens door need ID and AccessRule
        /// </summary>
        /// <param name="doorID"></param>
        /// <param name="type"></param>
        public void OpenDoor(int doorID, Doors.AccessRuleEnum type)
        {
            if (!_doorDictionary.ContainsKey(doorID))
            {
                Debug.Log("Key not in Dictionary");
                return;
            }

            foreach (var door in _doorDictionary[doorID])
            {
                switch (door.OpenRule)
                {
                    case Doors.OpenRuleEnum.ID:
                        if (InventoryManager.Instance.CheckForDoorID(door.DoorID))
                        {
                            OpenPrivate(door);
                        }
                        break;
                    case Doors.OpenRuleEnum.Rule:
                        if (type == door.AccessRule || door.AccessRule == Doors.AccessRuleEnum.All)
                            OpenPrivate(door);
                        break;
                    case Doors.OpenRuleEnum.RuleAndID:
                        if (type == door.AccessRule && InventoryManager.Instance.CheckForDoorID(door.DoorID))
                            OpenPrivate(door);
                        break;
                    case Doors.OpenRuleEnum.API:
                        if (type == Doors.AccessRuleEnum.Player)
                        {
                            _getTheGoogle.GetGoogle();
                            _googleMemory.Add(door);
                            UiGameObject.SetActive(true);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
               
            }
      
        }
        public void OpenDoor(int doorID)
        {
            if (!_doorDictionary.ContainsKey(doorID))
            {
                Debug.Log("Key not in Dictionary");
                return;
            }

            foreach (var door in _doorDictionary[doorID])
            {
                OpenPrivate(door);
               
            }
        }

        public void GoogleValueFound(int value)
        {
            Debug.Log("Amount of users on website = " + value);
            foreach (var door in _googleMemory)
            {
                if (door.AmountOfUsers <= value)
                {
                    OpenPrivate(door);
                   
                    UiGameObject.SetActive(false);
                    GameController.Instance.ToggleHint();
                }
            }

            _googleMemory.Clear();
        }


        private static void OpenPrivate(Doors door)
        {
            door.Open();
            if (door.CloseRule == Doors.CloseRuleEnum.CloseAfter)
            {
                CloseDoorAfter(door);
            }

            if (door.DoorID == 1)
            {
                CowManager.Instance.SetAllCowsToButcher();
            }
         
        }

        public void CloseDoorTrigger(Doors door, Doors.AccessRuleEnum type)
        {
            if (type == door.AccessRuleClose)
            {
                door.Close();
            }
        }

        private static void CloseDoorAfter(IDoorBehaviour door)
        {
            door.Close();
        }
    }
}
