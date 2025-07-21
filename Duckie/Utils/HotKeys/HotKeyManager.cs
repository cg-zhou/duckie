using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Duckie.Utils.HotKeys
{
    internal static partial class HotKeyManager
    {
        static HotKeyManager()
        {
            form = new HotKeyForm();
            form.KeyPressed += Form_KeyPressed;
        }

        private static void Form_KeyPressed(object sender, int id)
        {
            if (items.TryGetValue(id, out var item))
            {
                item.Action.Invoke();
            }
        }

        private static int id = 9527;

        private static Dictionary<int, HotKeyItem> items { get; set; } = new Dictionary<int, HotKeyItem>();
        private static HotKeyForm form = new HotKeyForm();
        private static object lockObj = new object();

        public static int Register(KeyModifiers modifiers, Keys keys, Action action)
        {
            lock (lockObj)
            {
                ++id;

                var hotKeyItem = new HotKeyItem
                {
                    Id = id,
                    Modifiers = modifiers,
                    Keys = keys,
                    Action = action
                };

                items[hotKeyItem.Id] = hotKeyItem;

                Interop.RegisterHotKey(form.Handle, hotKeyItem.Id, modifiers, keys);

                return id;
            }
        }

        public static void Unregister(IEnumerable<int> ids)
        {
            lock (lockObj)
            {
                foreach (var id in ids)
                {
                    Interop.UnregisterHotKey(form.Handle, id);
                }
            }
        }

        public static void Unregister(int id)
        {
            Unregister(new[] { id });
        }
    }
}
