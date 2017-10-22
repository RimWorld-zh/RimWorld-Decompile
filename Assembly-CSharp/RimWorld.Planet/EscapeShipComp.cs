using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class EscapeShipComp : WorldObjectComp
	{
		public bool raidBeaconEnabled;

		public override void CompTick()
		{
			MapParent mapParent = base.parent as MapParent;
			if (mapParent.HasMap)
			{
				List<Pawn> allPawnsSpawned = mapParent.Map.mapPawns.AllPawnsSpawned;
				bool flag = false;
				bool flag2 = false;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					Pawn pawn = allPawnsSpawned[i];
					if (pawn.RaceProps.Humanlike && pawn.HostFaction == null)
					{
						if (pawn.Faction == Faction.OfPlayer)
						{
							flag = true;
						}
						else if (pawn.Faction.HostileTo(Faction.OfPlayer) && !pawn.Downed)
						{
							flag2 = true;
						}
					}
				}
				if (flag2 && !flag)
				{
					Find.LetterStack.ReceiveLetter("EscapeShipLostLabel".Translate(), "EscapeShipLost".Translate(), LetterDefOf.NegativeEvent, (string)null);
					Find.WorldObjects.Remove(base.parent);
				}
			}
		}

		public override IEnumerable<IncidentTargetTypeDef> AcceptedTypes()
		{
			using (IEnumerator<IncidentTargetTypeDef> enumerator = this._003CAcceptedTypes_003E__BaseCallProxy0().GetEnumerator())
			{
				if (enumerator.MoveNext())
				{
					IncidentTargetTypeDef type = enumerator.Current;
					yield return type;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
			if (!this.raidBeaconEnabled)
				yield break;
			yield return IncidentTargetTypeDefOf.MapRaidBeacon;
			/*Error: Unable to find new state assignment for yield return*/;
			IL_00f2:
			/*Error near IL_00f3: Unexpected return in MoveNext()*/;
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.raidBeaconEnabled, "raidBeaconEnabled", false, false);
		}

		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			_003CGetFloatMenuOptions_003Ec__Iterator1 _003CGetFloatMenuOptions_003Ec__Iterator = (_003CGetFloatMenuOptions_003Ec__Iterator1)/*Error near IL_0036: stateMachine*/;
			string label = "VisitEscapeShip".Translate(base.parent.Label);
			Action action = (Action)delegate()
			{
				caravan.pather.StartPath(_003CGetFloatMenuOptions_003Ec__Iterator._0024this.parent.Tile, new CaravanArrivalAction_VisitEscapeShip(_003CGetFloatMenuOptions_003Ec__Iterator._0024this.parent as MapParent), true);
			};
			WorldObject parent = base.parent;
			yield return new FloatMenuOption(label, action, MenuOptionPriority.Default, null, null, 0f, null, parent);
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
