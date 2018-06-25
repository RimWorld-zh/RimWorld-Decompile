using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000620 RID: 1568
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		// Token: 0x06001FDC RID: 8156 RVA: 0x00112774 File Offset: 0x00110B74
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

		// Token: 0x06001FDD RID: 8157 RVA: 0x0011288C File Offset: 0x00110C8C
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption f in CaravanArrivalAction_VisitEscapeShip.GetFloatMenuOptions(caravan, (MapParent)this.parent))
			{
				yield return f;
			}
			yield break;
		}

		// Token: 0x06001FDE RID: 8158 RVA: 0x001128C0 File Offset: 0x00110CC0
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
