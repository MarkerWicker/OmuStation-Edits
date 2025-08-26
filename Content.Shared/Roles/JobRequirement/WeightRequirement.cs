using System.Diagnostics.CodeAnalysis;
using System.Text;
using Content.Shared.Humanoid.Prototypes;
using Content.Shared.Preferences;
using Content.Shared.Traits;
using JetBrains.Annotations;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Roles;

/// <summary>
/// Requires a character to have, or not have, certain traits
/// </summary>
[UsedImplicitly]
[Serializable, NetSerializable]
public sealed partial class WeightRequirement : JobRequirement
{
    [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly IComponentFactory _componentFactory = default!;

    [DataField(required: true)]
    public float MinimumWeight = 0;

    public override bool Check(
        IEntityManager entManager,
        IPrototypeManager protoManager,
        HumanoidCharacterProfile? profile,
        IReadOnlyDictionary<string, TimeSpan> playTimes,
        [NotNullWhen(false)] out FormattedMessage? reason)
    {
        reason = new FormattedMessage();

        if (profile is null) //the profile could be null if the player is a ghost. In this case we don't need to block the role selection for ghostrole
            return true;

        var sb = new StringBuilder();
        sb.Append("[color=yellow]");

        var species = _prototypeManager.Index<SpeciesPrototype>(profile.Species);

        _prototypeManager.Index(species.ID).TryGetComponent<FixturesComponent>(out var fixture, _componentFactory);

        if (fixture == null)
        {
            return false;
        }

        var fixtureMass = FixtureSystem.GetMassData(fixture.Fixtures["fix1"].Shape, fixture.Fixtures["fix1"].Density).Mass;
        fixtureMass *= (profile.Width + profile.Height) / 2;

        sb.Append("[/color]");

        if (!Inverted)
        {
            return fixtureMass >= MinimumWeight;
        }
        else
        {
            return fixtureMass <= MinimumWeight;
        }
    }

}
