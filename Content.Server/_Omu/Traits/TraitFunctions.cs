using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Content.Shared._Omu.Traits;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Physics;

namespace Content.Server._Omu.Traits;

/// <summary>
/// Used for traits that remove a component upon a player spawning in.
/// </summary>
public sealed partial class TraitRemoveComponent : TraitFunction
{
    [DataField, AlwaysPushInheritance]
    public ComponentRegistry ComponentsToRemove { get; private set; } = new();

    public override void OnPlayerSpawn(EntityUid uid,
        IComponentFactory factory,
        IEntityManager entityManager,
        ISerializationManager serializationManager)
    {
        foreach (var (name, _) in ComponentsToRemove)
            entityManager.RemoveComponentDeferred(uid, factory.GetComponent(name).GetType());
    }
}

public sealed partial class TraitModifyDensity : TraitFunction
{
    [DataField, AlwaysPushInheritance]
    public float DensityModifier;

    [DataField, AlwaysPushInheritance]
    public bool Multiply = false;

    public override void OnPlayerSpawn(EntityUid uid,
        IComponentFactory factory,
        IEntityManager entityManager,
        ISerializationManager serializationManager)
    {
        var physicsSystem = entityManager.System<SharedPhysicsSystem>();
        if (!entityManager.TryGetComponent<FixturesComponent>(uid, out var fixturesComponent)
            || fixturesComponent.Fixtures.Count is 0)
            return;

        var fixture = fixturesComponent.Fixtures["fix1"]; // As of writing, fix1 is the fixture used for practically everything.
        var newDensity = Multiply ? fixture.Density * DensityModifier : fixture.Density + DensityModifier;
        physicsSystem.SetDensity(uid, "fix1", fixture, newDensity);
        // SetDensity handles the Dirty.
    }
}
