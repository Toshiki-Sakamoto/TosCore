using System;
using NUnit.Framework;
using TosCore.Entity;

namespace TosCore.Tests.EditMode
{
    public class EntityRepositoryBaseTests
    {
        [Test]
        public void AddしたEntityをTryGetValueで取得できる()
        {
            var repository = new TestEntityRepository();
            var entity = TestEntity.Create(1);

            repository.Add(entity);
            var found = repository.TryGetValue(entity.Id, out var resolved);

            Assert.That(found, Is.True);
            Assert.That(resolved, Is.SameAs(entity));
        }

        [Test]
        public void 同じIDのEntityをAddすると例外が発生する()
        {
            var repository = new TestEntityRepository();
            var first = TestEntity.Create(10);
            var second = TestEntity.Create(10);

            repository.Add(first);

            Assert.Throws<InvalidOperationException>(() => repository.Add(second));
        }

        [Test]
        public void RemoveしたEntityは取得できない()
        {
            var repository = new TestEntityRepository();
            var entity = TestEntity.Create(5);
            repository.Add(entity);

            repository.Remove(entity);
            var found = repository.TryGetValue(entity.Id, out _);

            Assert.That(found, Is.False);
        }

        private sealed class TestEntityRepository : EntityRepositoryBase<TestEntity>
        {
        }

        private sealed class TestEntity : IEntityIdAssignable
        {
            public IEntityId Id { get; private set; }

            public void AssignId(IEntityId id)
            {
                Id = id;
            }

            public static TestEntity Create(long id)
            {
                var entity = new TestEntity();
                entity.AssignId(new EntityId<TestEntity>(id));
                return entity;
            }
        }
    }
}
