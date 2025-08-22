using Robust.Shared.Configuration;

namespace Content.Shared.CCVar;

public sealed partial class CCVars
{
    /// <summary>
    ///     The maximum number of traits that the player is allowed to select in the profile editor.
    /// </summary>
    public static readonly CVarDef<int>
        TraitsMaxTraits = CVarDef.Create("traits.maxtraits", 14, CVar.SERVER | CVar.REPLICATED);
}
