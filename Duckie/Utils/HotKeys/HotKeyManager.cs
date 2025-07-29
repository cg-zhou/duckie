using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys
{
    internal static partial class HotKeyManager
    {
        public static IHotKeyService[] GetHotKeyServices()
        {
            return typeof(HotKeyManager).Assembly
                .GetTypes()
                .Where(x => x.GetInterfaces().Contains(typeof(IHotKeyService)))
                .Select(x => Activator.CreateInstance(x))
                .OfType<IHotKeyService>()
                .ToArray();
        }

        public static void RegisterServices()
        {
            var hotKeyServices = GetHotKeyServices();
            foreach (var hotKeyService in hotKeyServices)
            {
                Register(hotKeyService, hotKeyService.Modifiers, hotKeyService.Keys);
            }
        }

        static HotKeyManager()
        {
            form = new HotKeyForm();
            form.KeyPressed += Form_KeyPressed;
        }

        private static void Form_KeyPressed(object sender, int id)
        {
            foreach (var item in items.Where(x => x.Id == id))
            {
                item.Service.Run();
            }
        }

        private static int id = 9527;

        private static List<HotKeyItem> items { get; set; } = new List<HotKeyItem>();
        private static HotKeyForm form = new HotKeyForm();
        private static object lockObj = new object();

        public static int Register(IHotKeyService hotKeyService, KeyModifiers modifiers, Keys keys)
        {
            Unregister(hotKeyService);

            lock (lockObj)
            {
                ++id;

                var item = new HotKeyItem
                {
                    Id = id,
                    Service = hotKeyService
                };

                items.Add(item);

                Interop.RegisterHotKey(form.Handle, item.Id, modifiers, keys);

                return id;
            }
        }

        public static void Unregister(IHotKeyService hotKeyService)
        {
            lock (lockObj)
            {
                var removedIds = new List<int>();
                foreach (var item in items)
                {
                    if (item.Service.Name == hotKeyService.Name)
                    {
                        removedIds.Add(item.Id);
                        Interop.UnregisterHotKey(form.Handle, item.Id);
                    }
                }

                items.RemoveAll(x => removedIds.Contains(x.Id));
            }
        }
    }
}
