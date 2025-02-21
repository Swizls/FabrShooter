using System.Collections.Generic;
using UnityEngine;

namespace FabrShooter.Core
{ 
    public static class ServiceLocator
    {
        private static Dictionary<string, IService> _services = new Dictionary<string, IService>();

        public static void Register<T>(IService service) where T : IService
        {
            string key = typeof(T).Name;
            _services.Add(key, service);
        }

        public static void Unregister<T>(IService service) where T : IService
        {
            string key = typeof(T).Name;
            _services.Remove(key);
        }

        public static void Clear()
        {
            _services.Clear();
        }

        public static T Get<T>() where T : IService
        {
            string key = typeof(T).Name;
            return (T)_services[key];
        }
    }
}