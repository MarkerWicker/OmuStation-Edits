using Content.Shared._EinsteinEngines.Flight;
using Content.Goobstation.Shared.Dash;
using Robust.Server.GameObjects;

namespace Content.Server._Omu.Traits.Physical;

/// <summary>
///     Removes the moth dash from players who have the moth flight trait selected.
/// </summary>
public sealed class MothFlightSystem : EntitySystem
{

    [Dependency] private readonly IEntityManager _entityManager = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FlightComponent, ComponentInit>(OnSpawn);
    }

    private void OnSpawn(Entity<FlightComponent> ent, ref ComponentInit args)
    {
        _entityManager.RemoveComponent<DashActionComponent>(ent);
    }

} 
