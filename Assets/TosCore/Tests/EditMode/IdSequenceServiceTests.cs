using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TosCore.Entity;

namespace TosCore.Tests.EditMode
{
    public class IdSequenceServiceTests
    {
        [Test]
        public void 復元済み状態から採番が継続される()
        {
            var store = new TestIdSequenceStateStore(
                new Dictionary<TypeToken, long>
                {
                    [new TypeToken(typeof(SampleTypeA))] = 5
                });
            var service = new IdSequenceService(store);

            var actual = service.NextFor<SampleTypeA>();

            Assert.That(actual, Is.EqualTo(6));
        }

        [Test]
        public void PeekForは値を進めない()
        {
            var store = new TestIdSequenceStateStore();
            var service = new IdSequenceService(store);

            var peek = service.PeekFor<SampleTypeB>();
            var next = service.NextFor<SampleTypeB>();

            Assert.That(peek, Is.EqualTo(1));
            Assert.That(next, Is.EqualTo(1));
        }

        [Test]
        public void SaveStateで現在状態がストアへ保存される()
        {
            var store = new TestIdSequenceStateStore();
            var service = new IdSequenceService(store);
            service.NextFor<SampleTypeA>();
            service.NextFor<SampleTypeA>();
            service.NextFor<SampleTypeB>();

            service.SaveState();

            var snapshot = store.GetStates()
                .ToDictionary(pair => pair.Key.AssemblyQualifiedName, pair => pair.Value);

            Assert.That(snapshot[typeof(SampleTypeA).AssemblyQualifiedName], Is.EqualTo(2));
            Assert.That(snapshot[typeof(SampleTypeB).AssemblyQualifiedName], Is.EqualTo(1));
        }

        private sealed class TestIdSequenceStateStore : IIdSequenceStateStore
        {
            private readonly Dictionary<TypeToken, long> _stateMap;

            public TestIdSequenceStateStore()
                : this(new Dictionary<TypeToken, long>())
            {
            }

            public TestIdSequenceStateStore(Dictionary<TypeToken, long> initialState)
            {
                _stateMap = initialState;
            }

            public IReadOnlyDictionary<TypeToken, long> GetStates()
            {
                return _stateMap;
            }

            public void SetStates(IReadOnlyDictionary<TypeToken, long> state)
            {
                _stateMap.Clear();
                foreach (var pair in state)
                {
                    _stateMap[pair.Key] = pair.Value;
                }
            }
        }

        private sealed class SampleTypeA
        {
        }

        private sealed class SampleTypeB
        {
        }
    }
}
