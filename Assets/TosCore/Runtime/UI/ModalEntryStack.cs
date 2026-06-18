using System.Collections.Generic;

namespace TosCore.UI
{
    internal sealed class ModalEntryStack
    {
        private readonly List<IModalEntry> _entries = new();

        public int Count => _entries.Count;

        public void Push(IModalEntry entry)
        {
            _entries.Add(entry);
        }

        public void Remove(IModalEntry entry)
        {
            _entries.Remove(entry);
        }

        public bool TryPeek(out IModalEntry entry)
        {
            if (_entries.Count == 0)
            {
                entry = null;
                return false;
            }

            entry = _entries[^1];
            return true;
        }

        /// <summary>
        /// 逆順にして返す
        /// </summary>
        public IModalEntry[] SnapshotTopFirst()
        {
            var result = _entries.ToArray();
            System.Array.Reverse(result);
            return result;
        }
    }
}
