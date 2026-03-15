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

        public IModalEntry[] SnapshotTopFirst()
        {
            var result = _entries.ToArray();
            for (var left = 0; left < result.Length / 2; left++)
            {
                var right = result.Length - left - 1;
                (result[left], result[right]) = (result[right], result[left]);
            }

            return result;
        }
    }
}
