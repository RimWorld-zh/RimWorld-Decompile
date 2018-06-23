using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000317 RID: 791
	public class IncidentParms : IExposable
	{
		// Token: 0x04000898 RID: 2200
		public IIncidentTarget target;

		// Token: 0x04000899 RID: 2201
		public float points = -1f;

		// Token: 0x0400089A RID: 2202
		public Faction faction;

		// Token: 0x0400089B RID: 2203
		public bool forced;

		// Token: 0x0400089C RID: 2204
		public IntVec3 spawnCenter = IntVec3.Invalid;

		// Token: 0x0400089D RID: 2205
		public Rot4 spawnRotation = Rot4.South;

		// Token: 0x0400089E RID: 2206
		public bool generateFightersOnly;

		// Token: 0x0400089F RID: 2207
		public bool dontUseSingleUseRocketLaunchers;

		// Token: 0x040008A0 RID: 2208
		public RaidStrategyDef raidStrategy;

		// Token: 0x040008A1 RID: 2209
		public PawnsArrivalModeDef raidArrivalMode;

		// Token: 0x040008A2 RID: 2210
		public bool raidForceOneIncap;

		// Token: 0x040008A3 RID: 2211
		public bool raidNeverFleeIndividual;

		// Token: 0x040008A4 RID: 2212
		public bool raidArrivalModeForQuickMilitaryAid;

		// Token: 0x040008A5 RID: 2213
		public Dictionary<Pawn, int> pawnGroups;

		// Token: 0x040008A6 RID: 2214
		public TraderKindDef traderKind;

		// Token: 0x040008A7 RID: 2215
		public int podOpenDelay = 140;

		// Token: 0x040008A8 RID: 2216
		private List<Pawn> tmpPawns;

		// Token: 0x040008A9 RID: 2217
		private List<int> tmpGroups;

		// Token: 0x06000D6B RID: 3435 RVA: 0x00073494 File Offset: 0x00071894
		public void ExposeData()
		{
			Scribe_References.Look<IIncidentTarget>(ref this.target, "target", false);
			Scribe_Values.Look<float>(ref this.points, "threatPoints", 0f, false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_Values.Look<bool>(ref this.forced, "forced", false, false);
			Scribe_Values.Look<IntVec3>(ref this.spawnCenter, "spawnCenter", default(IntVec3), false);
			Scribe_Values.Look<Rot4>(ref this.spawnRotation, "spawnRotation", default(Rot4), false);
			Scribe_Values.Look<bool>(ref this.generateFightersOnly, "generateFightersOnly", false, false);
			Scribe_Values.Look<bool>(ref this.dontUseSingleUseRocketLaunchers, "dontUseSingleUseRocketLaunchers", false, false);
			Scribe_Defs.Look<RaidStrategyDef>(ref this.raidStrategy, "raidStrategy");
			Scribe_Defs.Look<PawnsArrivalModeDef>(ref this.raidArrivalMode, "raidArrivalMode");
			Scribe_Values.Look<bool>(ref this.raidForceOneIncap, "raidForceIncap", false, false);
			Scribe_Values.Look<bool>(ref this.raidNeverFleeIndividual, "raidNeverFleeIndividual", false, false);
			Scribe_Values.Look<bool>(ref this.raidArrivalModeForQuickMilitaryAid, "raidArrivalModeForQuickMilitaryAid", false, false);
			Scribe_Collections.Look<Pawn, int>(ref this.pawnGroups, "pawnGroups", LookMode.Reference, LookMode.Value, ref this.tmpPawns, ref this.tmpGroups);
			Scribe_Defs.Look<TraderKindDef>(ref this.traderKind, "traderKind");
			Scribe_Values.Look<int>(ref this.podOpenDelay, "podOpenDelay", 140, false);
		}

		// Token: 0x06000D6C RID: 3436 RVA: 0x000735E0 File Offset: 0x000719E0
		public override string ToString()
		{
			string text = "(";
			if (this.target != null)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"target=",
					this.target,
					" "
				});
			}
			if (this.points >= 0f)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"points=",
					this.points,
					" "
				});
			}
			if (this.generateFightersOnly)
			{
				string text2 = text;
				text = string.Concat(new object[]
				{
					text2,
					"generateFightersOnly=",
					this.generateFightersOnly,
					" "
				});
			}
			if (this.raidStrategy != null)
			{
				text = text + "raidStrategy=" + this.raidStrategy.defName + " ";
			}
			return text + ")";
		}
	}
}
