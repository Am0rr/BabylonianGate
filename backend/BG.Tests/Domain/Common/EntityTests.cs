using BG.Domain.Common;
using FluentAssertions;
using System.Reflection;

namespace BG.Tests.Domain.Common;

public class EntityTests
{
    private class TestEntity : Entity
    {

    }

    private class AnotherEntity : Entity
    {

    }

    [Fact]
    public void Constructor_Should_GenerateId_And_Timestamp()
    {
        var entity = new TestEntity();

        entity.Id.Should().NotBeEmpty();
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Equals_Should_ReturnTrue_For_SameReference()
    {
        var entity1 = new TestEntity();
        var entity2 = entity1;

        entity1.Equals(entity2).Should().BeTrue();
        (entity1 == entity2).Should().BeTrue();
    }

    [Fact]
    public void Equals_Should_ReturnFalse_For_DifferentIds()
    {
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        entity1.Equals(entity2).Should().BeFalse();
        (entity1 == entity2).Should().BeFalse();
        (entity1 != entity2).Should().BeTrue();
    }

    [Fact]
    public void Equals_Should_ReturnTrue_For_DifferentObjects_WithSameId()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        SetId(entity1, id);
        SetId(entity2, id);

        entity1.Equals(entity2).Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_Should_BeSame_For_EqualEntities()
    {
        var id = Guid.NewGuid();
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        SetId(entity1, id);
        SetId(entity2, id);

        entity1.GetHashCode().Should().Be(entity2.GetHashCode());
    }

    [Fact]
    public void GetHashCode_Should_BeDifferent_For_DifferentEntities()
    {
        var entity1 = new TestEntity();
        var entity2 = new TestEntity();

        entity1.GetHashCode().Should().NotBe(entity2.GetHashCode());
    }

    private void SetId(Entity entity, Guid id)
    {
        var prop = typeof(Entity).GetProperty("Id", BindingFlags.Public | BindingFlags.Instance);
        if (prop != null && prop.CanWrite)
        {
            prop.SetValue(entity, id);
        }
    }
}