using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Modification
{
    public class Stat
    {
        public float BaseValue;
        public float MinimumValue;
        public float MaximumValue;
        
        public float RealValue => Mathf.Clamp(BaseValue + modifiers.Sum(m => m.Value), MinimumValue, MaximumValue);

        private List<Modifier<float>> modifiers = new List<Modifier<float>>();

        public Action<float> OnChanged;

        public Stat(float baseValue, (float, float) minMax)
        {
            BaseValue = baseValue;
            MinimumValue = minMax.Item1;
            MaximumValue = minMax.Item2;
        }
        
        public void AddModifier(Modifier<float> mod)
        {
            modifiers.Add(mod);
            OnChanged?.Invoke(RealValue);
        }

        public void RemoveModifier(string id)
        {
            modifiers.RemoveAll(m => m.Id == id);
            OnChanged?.Invoke(RealValue);
        }
    }
}