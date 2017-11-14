using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		public Map map;

		public Faction faction;

		public Pawn kidnapped;

		public int fee;

		protected override IEnumerable<DiaOption> Choices
		{
			get
			{
				DiaOption accept = new DiaOption("RansomDemand_Accept".Translate())
				{
					action = delegate
					{
						((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.faction.kidnapped.RemoveKidnappedPawn(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.kidnapped);
						Find.WorldPawns.RemovePawn(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.kidnapped);
						IntVec3 intVec = default(IntVec3);
						if ((int)((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.faction.def.techLevel < 5)
						{
							if (!CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map) && ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map.reachability.CanReachColony(c)), ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((Predicate<IntVec3>)((IntVec3 c) => c.Standable(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map)), ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
							{
								Log.Warning("Could not find any edge cell.");
								intVec = DropCellFinder.TradeDropSpot(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map);
							}
							GenSpawn.Spawn(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.kidnapped, intVec, ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map);
						}
						else
						{
							intVec = DropCellFinder.TradeDropSpot(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map);
							TradeUtility.SpawnDropPod(intVec, ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map, ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.kidnapped);
						}
						CameraJumper.TryJump(intVec, ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map);
						TradeUtility.LaunchSilver(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.map, ((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this.fee);
						Find.LetterStack.RemoveLetter(((_003C_003Ec__Iterator0)/*Error near IL_0044: stateMachine*/)._0024this);
					},
					resolveTree = true
				};
				if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					accept.Disable("NeedSilverLaunchable".Translate(this.fee.ToString()));
				}
				yield return accept;
				/*Error: Unable to find new state assignment for yield return*/;
			}
		}

		public override bool StillValid
		{
			get
			{
				if (!base.StillValid)
				{
					return false;
				}
				if (!Find.Maps.Contains(this.map))
				{
					return false;
				}
				return this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}
	}
}
