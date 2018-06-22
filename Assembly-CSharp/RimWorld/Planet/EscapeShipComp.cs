using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061E RID: 1566
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x06001FD8 RID: 8152 RVA: 0x00112624 File Offset: 0x00110A24
		public override void CompTick()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				List<Pawn> allPawnsSpawned = mapParent.Map.mapPawns.AllPawnsSpawned;
				bool flag = mapParent.Map.mapPawns.FreeColonistsSpawnedOrInPlayerEjectablePodsCount != 0;
				bool flag2 = false;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn = allPawnsSpawned[i];
					if (pawn.RaceProps.Humanlike)
					{
						if (pawn.HostFaction == null)
						{
							if (!pawn.Downed)
							{
								if (pawn.Faction != null && pawn.Faction.HostileTo(Faction.OfPlayer))
								{
									flag2 = true;
								}
							}
						}
					}
				}
				if (flag2 && !flag)
				{
					Find.LetterStack.ReceiveLetter("EscapeShipLostLabel".Translate(), "EscapeShipLost".Translate(), LetterDefOf.NegativeEvent, null);
					Find.WorldObjects.Remove(this.parent);
				}
			}
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x0011273C File Offset: 0x00110B3C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption f in CaravanArrivalAction_VisitEscapeShip.GetFloatMenuOptions(caravan, (MapParent)this.parent))
			{
				yield return f;
			}
			yield break;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x00112770 File Offset: 0x00110B70
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MapParent mapParent = (MapParent)this.parent;
			if (mapParent.HasMap)
			{
				if (mapParent.Map.listerThings.ThingsOfDef(ThingDefOf.Ship_Reactor).Any((Thing reactor) => reactor.TryGetComp<CompHibernatable>().Running))
				{
					yield return SettleInExistingMapUtility.SettleCommand(((MapParent)this.parent).Map, true);
				}
			}
			yield break;
		}
	}
}
