using System.Collections.Generic;
using System.Linq;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace Items
{
    public class InventoryManager : Singleton<InventoryManager>
    {


        public int WeightLimit;
        public int CurrentWeight;

        private readonly List<Item> _itemList = new List<Item>();

        public Text DynamiteText1;
        public Text DynamiteText2;

        public Slider FuelSlider;


        private bool _timer;
        private bool _secondTimer;
        private float _time;
        private float _secondTime;


        public GameObject AccessItem;



        public bool AddItem(Item item)
        {
            if (CurrentWeight + item.Weight > WeightLimit) return false;
            _itemList.Add(item);
            CurrentWeight += item.Weight;
            return true;
        }

        public bool RemoveItem(Item item)
        {
            var succes = _itemList.Remove(item);
            CurrentWeight = succes ? CurrentWeight -= item.Weight : 0;
            return succes;
        }

        private bool _exist;
        public void InstansiateAccessItem(Transform yourTransform)
        {
            if(!_exist)
            Instantiate(Instance.AccessItem, new Vector3(yourTransform.position.x, GameController.Instance.GetPlayerLocation.position.y, yourTransform.position.z), yourTransform.rotation);
            _exist = true;
        }

        public void AddFuel(float value)
        {
            if (!FuelSlider.gameObject.activeSelf)
            {
                FuelSlider.gameObject.SetActive(true);
            }
            FuelSlider.value += value;

            if (FuelSlider.value >= 1)
            {
                GameController.Instance.Player.BlowUp();
            }
        }

        public float GetFuelValue()
        {
            return FuelSlider.value;
        }

        public void Update()
        {
            if (_timer)
            {
                if (_time > 0)
                {
                    _time -= Time.deltaTime;
                    DynamiteText1.text = ("Explosion in " + Mathf.RoundToInt(_time));

                }
                else
                {
                    ReturnToItem();
                    _time = 0;
                    _timer = false;
                    if (!_secondTimer)
                    {
                        DynamiteText1.gameObject.SetActive(false);
                    }

                }
            }

            if (!_secondTimer) return;
            if (!(_secondTime > 0))
            {
                _secondTimer = _secondTimer = false;
                _secondTime = 0;
                ReturnToItem();
                DynamiteText1.gameObject.SetActive(false);
                return;
            }
            _secondTime -= Time.deltaTime;
            if (!_timer)
            {
                DynamiteText1.text = ("Explosion in " + Mathf.RoundToInt(_secondTime));

                
                DynamiteText2.gameObject.SetActive(false);
            }
            else if(_secondTimer)
            {
                DynamiteText2.text = ("Explosion in " + Mathf.RoundToInt(_secondTime));
            }




        }

        public void ReturnToItem()
        {
            _itemList.OfType<DynamiteItem>().First().TimerFinished();
            _itemList.Remove(_itemList.OfType<DynamiteItem>().First());
           

        }

      

        public bool TransferDynamiteItem(GameObject cow)
        {
            if (_timer)
            {
                foreach (var item in _itemList.OfType<DynamiteItem>())
                {
                    if (item.WhereAmI != DynamiteItem.WhereAmIEnum.Player) continue;
                    item.TransferToOtherCow(cow);
                    return true;

                }
              
            }
            Debug.Log("Timer not active");
            return false;
        }

        public bool PlayerHasDynamite()
        {
            return _itemList.OfType<DynamiteItem>().Any(item => item.WhereAmI == DynamiteItem.WhereAmIEnum.Player);
        }

        public void StartTimer(float time)
        {
            if (!_timer)
            {
                _time = time;
                DynamiteText1.gameObject.SetActive(true);
                _timer = true;
            }
            else if(!_secondTimer)
            {
                _secondTime = time;
                DynamiteText2.gameObject.SetActive(true);
                _secondTimer = true;
            }
           
        }

        public bool CheckForDoorID(int doorID)
        {
            return _itemList.OfType<AccessItem>().Any(item => item.OpensDoor(doorID));
        }

        public void DebugItems()
        {
            Debug.Log("list size" + _itemList.Count);
            foreach (var item in _itemList)
            {
                Debug.Log("item weight " + item.Weight + "          item name " + item.Name);
            }
        }

    }
}

