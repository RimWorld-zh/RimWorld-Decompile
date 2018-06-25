using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020008A4 RID: 2212
	[StaticConstructorOnStartup]
	public static class TexCommand
	{
		// Token: 0x04001B23 RID: 6947
		public static readonly Texture2D DesirePower = ContentFinder<Texture2D>.Get("UI/Commands/DesirePower", true);

		// Token: 0x04001B24 RID: 6948
		public static readonly Texture2D Draft = ContentFinder<Texture2D>.Get("UI/Commands/Draft", true);

		// Token: 0x04001B25 RID: 6949
		public static readonly Texture2D ReleaseAnimals = ContentFinder<Texture2D>.Get("UI/Commands/ReleaseAnimals", true);

		// Token: 0x04001B26 RID: 6950
		public static readonly Texture2D HoldOpen = ContentFinder<Texture2D>.Get("UI/Commands/HoldOpen", true);

		// Token: 0x04001B27 RID: 6951
		public static readonly Texture2D Forbidden = ContentFinder<Texture2D>.Get("UI/Commands/Forbidden", true);

		// Token: 0x04001B28 RID: 6952
		public static readonly Texture2D GatherSpotActive = ContentFinder<Texture2D>.Get("UI/Commands/GatherSpotActive", true);

		// Token: 0x04001B29 RID: 6953
		public static readonly Texture2D Install = ContentFinder<Texture2D>.Get("UI/Commands/Install", true);

		// Token: 0x04001B2A RID: 6954
		public static readonly Texture2D SquadAttack = ContentFinder<Texture2D>.Get("UI/Commands/SquadAttack", true);

		// Token: 0x04001B2B RID: 6955
		public static readonly Texture2D AttackMelee = ContentFinder<Texture2D>.Get("UI/Commands/AttackMelee", true);

		// Token: 0x04001B2C RID: 6956
		public static readonly Texture2D Attack = ContentFinder<Texture2D>.Get("UI/Commands/Attack", true);

		// Token: 0x04001B2D RID: 6957
		public static readonly Texture2D FireAtWill = ContentFinder<Texture2D>.Get("UI/Commands/FireAtWill", true);

		// Token: 0x04001B2E RID: 6958
		public static readonly Texture2D ToggleVent = ContentFinder<Texture2D>.Get("UI/Commands/Vent", true);

		// Token: 0x04001B2F RID: 6959
		public static readonly Texture2D PauseCaravan = ContentFinder<Texture2D>.Get("UI/Commands/PauseCaravan", true);

		// Token: 0x04001B30 RID: 6960
		public static readonly Texture2D RearmTrap = ContentFinder<Texture2D>.Get("UI/Designators/RearmTrap", true);

		// Token: 0x04001B31 RID: 6961
		public static readonly Texture2D TreeChop = ContentFinder<Texture2D>.Get("UI/Designators/HarvestWood", true);

		// Token: 0x04001B32 RID: 6962
		public static readonly Texture2D CannotShoot = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04001B33 RID: 6963
		public static readonly Texture2D ClearPrioritizedWork = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x04001B34 RID: 6964
		public static readonly Texture2D RemoveRoutePlannerWaypoint = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);
	}
}
