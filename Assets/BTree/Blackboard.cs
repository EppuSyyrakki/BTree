using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BTree
{
    public class BlackboardDictionary : SerializableDictionary<BlackboardVariable, float> { }
    public class SettingDictionary : SerializableDictionary<BlackboardVariable, bool> { }

    [Serializable]
    public class Blackboard
    {
        [SerializeField, Tooltip("true = Allow negative values")]
        SettingDictionary settings = new SettingDictionary();

        [SerializeField]
        private BlackboardDictionary dict;

        public UnityEvent<BlackboardVariable, float> SetValue;
        public UnityEvent<BlackboardVariable> IncreaseValue;
        public UnityEvent<BlackboardVariable> DecreaseValue;

        public Blackboard()
        {
            dict = new BlackboardDictionary();

            foreach (BlackboardVariable variable in Enum.GetValues(typeof(BlackboardVariable)))
            {
                dict.Add(variable, 0);                
            }
        }

        public void AddListeners()
        {
            SetValue.AddListener((key, value) => dict[key] = value);
            IncreaseValue.AddListener((key) => dict[key]++);
            DecreaseValue.AddListener((key) => dict[key]--);
        }

        public void RemoveListeners()
        {
            SetValue.RemoveAllListeners();
            IncreaseValue.RemoveAllListeners();
            DecreaseValue.RemoveAllListeners();
        }

        public float GetValue(BlackboardVariable variable)
        {
            if (dict.TryGetValue(variable, out float value)) { return value; }

            return 0;
        }
        
        public void OnAgentValidate()
        {
            foreach (BlackboardVariable variable in Enum.GetValues(typeof(BlackboardVariable)))
            {
                settings.TryAdd(variable, true);
            }
        }
    }
}