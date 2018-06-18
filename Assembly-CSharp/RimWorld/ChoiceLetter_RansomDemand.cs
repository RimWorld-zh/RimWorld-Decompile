using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000846 RID: 2118
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		// Token: 0x17000795 RID: 1941
		// (get) Token: 0x06002FEA RID: 12266 RVA: 0x001A07AC File Offset: 0x0019EBAC
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.OK;
					yield break;
				}
				DiaOption accept = new DiaOption("RansomDemand_Accept".Translate());
				accept.action = delegate()
				{
					this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
					Find.WorldPawns.RemovePawn(this.kidnapped);
					IntVec3 intVec;
					if (this.faction.def.techLevel < TechLevel.Industrial)
					{
						if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
						{
							Log.Warning("Could not find any edge cell.", false);
							intVec = DropCellFinder.TradeDropSpot(this.map);
						}
						GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
					}
					else
					{
						intVec = DropCellFinder.TradeDropSpot(this.map);
						TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
					}
					CameraJumper.TryJump(intVec, this.map);
					TradeUtility.LaunchSilver(this.map, this.fee);
					Find.LetterStack.RemoveLetter(this);
				};
				accept.resolveTree = true;
				if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					accept.Disable("NeedSilverLaunchable".Translate(new object[]
					{
						this.fee.ToString()
					}));
				}
				yield return accept;
				yield return base.Reject;
				yield return base.Postpone;
				yield break;
			}
		}

		// Token: 0x17000796 RID: 1942
		// (get) Token: 0x06002FEB RID: 12267 RVA: 0x001A07D8 File Offset: 0x0019EBD8
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		// Token: 0x06002FEC RID: 12268 RVA: 0x001A0838 File Offset: 0x0019EC38
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}

		// Token: 0x040019EA RID: 6634
		public Map map;

		// Token: 0x040019EB RID: 6635
		public Faction faction;

		// Token: 0x040019EC RID: 6636
		public Pawn kidnapped;

		// Token: 0x040019ED RID: 6637
		public int fee;
	}
}
