using System;
using System.Collections.Generic;
using Metozis.TeTwo.Internal.Entities.Meta;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Metozis.TeTwo.Internal.Entities
{
    public abstract class Entity : SerializedMonoBehaviour
    {
        public string Name;
        public EntityMeta Meta;
        
        private readonly HashSet<IModule> modules = new HashSet<IModule>();
        private readonly HashSet<IRuntimeModule> runtimeModules = new HashSet<IRuntimeModule>();
        private readonly Dictionary<Type, IModule> modulesCache = new Dictionary<Type, IModule>();
        
        public void AddModule<T>(T module) where T: class, IModule
        {
            modules.Add(module);
            modulesCache[module.GetType()] = module;
            if (module is IRuntimeModule runtimeModule)
            {
                runtimeModules.Add(runtimeModule);
            }
        }

        public bool HasModule<T>() where T : IModule
        {
            return modulesCache.ContainsKey(typeof(T));
        }

        public bool TryRemoveModule(Type moduleType)
        {
            try
            {
                var module = modulesCache[moduleType];
                modules.Remove(module);
                modulesCache.Remove(moduleType);
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }

        public T GetModule<T>() where T : IModule
        {
            try
            {
                return (T) modulesCache[typeof(T)];
            }
            catch (KeyNotFoundException)
            {
                return default;
            }
        }

        protected virtual void Update()
        {
            runtimeModules.ForEach(module => module.Update());
        }
    }
}