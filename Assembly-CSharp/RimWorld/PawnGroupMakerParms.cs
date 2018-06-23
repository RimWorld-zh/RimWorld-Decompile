using System;

namespace RimWorld
{
	// Token: 0x0200048F RID: 1167
	public class PawnGroupMakerParms
	{
		// Token: 0x04000C72 RID: 3186
		public PawnGroupKindDef groupKind;

		// Token: 0x04000C73 RID: 3187
		public int tile = -1;

		// Token: 0x04000C74 RID: 3188
		public bool inhabitants;

		// Token: 0x04000C75 RID: 3189
		public float points;

		// Token: 0x04000C76 RID: 3190
		public Faction faction;

		// Token: 0x04000C77 RID: 3191
		public TraderKindDef traderKind;

		// Token: 0x04000C78 RID: 3192
		public bool generateFightersOnly;

		// Token: 0x04000C79 RID: 3193
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x04000C7A RID: 3194
		public RaidStrategyDef raidStrategy;

		// Token: 0x04000C7B RID: 3195
		public bool forceOneIncap;

		// Token: 0x060014B4 RID: 5300 RVA: 0x000B6148 File Offset: 0x000B4548
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"groupKind=",
				this.groupKind,
				", tile=",
				this.tile,
				", inhabitants=",
				this.inhabitants,
				", points=",
				this.points,
				", faction=",
				this.faction,
				", traderKind=",
				this.traderKind,
				", generateFightersOnly=",
				this.generateFightersOnly,
				", dontUseSingleUseRocketLaunchers=",
				this.dontUseSingleUseRocketLaunchers,
				", raidStrategy=",
				this.raidStrategy,
				", forceOneIncap=",
				this.forceOneIncap
			});
		}
	}
}
