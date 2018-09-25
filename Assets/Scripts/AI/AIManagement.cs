'87using System;
using System.Collections;
using System.Collections.Generic;
using MathExt;
using Tools;
using UnityEngine;

namespace AI
{
    public abstract class AIManagement<TClass, TEnum , TSingleton> : Singleton<TSingleton>
        where TClass : _AI
        where TEnum : struct, IConvertible
        where TSingleton : Singleton<TSingleton>
    {


        private EnumDictionary<TEnum,TClass> _aiDictionary = new EnumDictionary<TEnum, TClass>();

        protected override void Awake()
        {
            base.Awake();
            _aiDictionary = new EnumDictionary<TEnum, TClass>();
        }

        public void Add(TEnum _enum, TClass _class)
        {
            _aiDictionary.Add(_enum, _class);    
        }

        public void RemoveFromList(TEnum _enum, TClass _class)
        {
            _aiDictionary.RemoveFromList(_enum,_class);
        }

        public void SendAllToPosition(TEnum _enum, Vector3 position)
        {
            foreach (var items in _aiDictionary[_enum])
            {
                items.SendToLocation(position);
            }
        }

        public List<TClass> GetList(TEnum _enum)
        {
            return _aiDictionary[_enum];
        }

        public void SendSomeToPosition()
        {

        } 

        public void SendRandomToRandomPosition(TEnum _enum, Transform[] positions)
        {
            var randomAI = _aiDictionary.GetRandomElement(_enum);
            Transform location = positions.GetRandom_Array();
            randomAI.SendToLocation(location.position);
        }

        public Vector3 GetRandomPosition(TEnum _enum)
        {
            return _aiDictionary[_enum].GetRandomElement().transform.position;
        }

        public void SendRandomToPosition(TEnum _enum, Vector3 positon)
        {
            _aiDictionary.GetRandomElement(_enum).SendToLocation(positon);
        }

        protected IEnumerator PatrolTransforms(TEnum _enum, int amountOfRandoms, Transform [] positions, float waitForSeconds)
        {
            
            while (true)
            {
                var lengthCount = _aiDictionary.ListCount(_enum);
                int[] randoms = Randoms.GiveRandoms(amountOfRandoms, lengthCount);
                for (int i = 0; i < randoms.Length; i++)
                {
                    _aiDictionary[_enum][i].SendToLocation(positions[i].position);
                }
                yield return new WaitForSeconds(waitForSeconds);

            }
            // ReSharper disable once IteratorNeverReturns
        }
  



    }
}
