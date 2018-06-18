using System;

namespace RimWorld
{
	// Token: 0x02000493 RID: 1171
	public class PawnGroupMakerParms
	{
		// Token: 0x060014BD RID: 5309 RVA: 0x000B614C File Offset: 0x000B454C
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"groupKind=",
				this.groupKind,
				"tile=",
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

		// Token: 0x04000C75 RID: 3189
		public PawnGroupKindDef groupKind;

		// Token: 0x04000C76 RID: 3190
		public int tile = -1;

		// Token: 0x04000C77 RID: 3191
		public bool inhabitants;

		// Token: 0x04000C78 RID: 3192
		public float points;

		// Token: 0x04000C79 RID: 3193
		public Faction faction;

		// Token: 0x04000C7A RID: 3194
		public TraderKindDef traderKind;

		// Token: 0x04000C7B RID: 3195
		public bool generateFightersOnly;

		// Token: 0x04000C7C RID: 3196
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x04000C7D RID: 3197
		public RaidStrategyDef raidStrategy;

		// Token: 0x04000C7E RID: 3198
		public bool forceOneIncap;
	}
}
