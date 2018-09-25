using System;
using System.Collections.Generic;
using AI;
using MathExt;

namespace Tools
{
    public class EnumDictionary<TEnum, TClass> where TEnum : struct, IConvertible where TClass : _AI
    {
        private readonly Dictionary<TEnum, List<TClass>> _dictionary;

        public List<TClass> this[TEnum _enum] => _dictionary[_enum];

        public EnumDictionary()
        {
            if (!typeof(TEnum).IsEnum)
            {
                throw new ArgumentException("TEnum must be an enum");
            }
            _dictionary = new Dictionary<TEnum, List<TClass>>();

        }

        public void Add(TEnum _enum, TClass _class)
        {

            if (!ContainsKey(_enum))
            {
                _dictionary.Add(_enum, new List<TClass>());
            }
  

            _dictionary[_enum].Add(_class);
        }

        private bool ContainsKey(TEnum _enum)
        {
            return _dictionary.ContainsKey(_enum);
        }

        public void RemoveFromList(TEnum _enum, TClass _class)
        {
            _dictionary[_enum].Remove(_class);
        }

        public int EnumCount()
        {
            return _dictionary.Count;
        }

        public int ListCount(TEnum _enum)
        {
            return _dictionary[_enum].Count;


        }

        public TClass GetRandomElement(TEnum _enum)
        {
            return _dictionary[_enum].GetRandomElement();
        }

    }
}
