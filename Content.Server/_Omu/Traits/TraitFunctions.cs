using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Content.Shared._Omu.Traits;

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
