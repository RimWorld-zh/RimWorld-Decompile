using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CA RID: 1482
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		// Token: 0x06001CC9 RID: 7369 RVA: 0x000F7387 File Offset: 0x000F5787
		public CaravanArrivalAction_Enter()
		{
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000F7390 File Offset: 0x000F5790
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001CCB RID: 7371 RVA: 0x000F73A0 File Offset: 0x000F57A0
		public override string Label
		{
			get
			{
				return "EnterMap".Translate(new object[]
				{
					this.mapParent.Label
				});
			}
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06001CCC RID: 7372 RVA: 0x000F73D4 File Offset: 0x000F57D4
		public override string ReportString
		{
			get
			{
				return "CaravanEntering".Translate(new object[]
				{
					this.mapParent.Label
				});
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000F7408 File Offset: 0x000F5808
		public override FloatMenuAcceptanceReport StillValid(Caravan caravan, int destinationTile)
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = base.StillValid(caravan, destinationTile);
			FloatMenuAcceptanceReport result;
			if (!floatMenuAcceptanceReport)
			{
				result = floatMenuAcceptanceReport;
			}
			else if (this.mapParent != null && this.mapParent.Tile != destinationTile)
			{
				result = false;
			}
			else
			{
				result = CaravanArrivalAction_Enter.CanEnter(caravan, this.mapParent);
			}
			return result;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000F746C File Offset: 0x000F586C
		public override void Arrived(Caravan caravan)
		{
			Map map = this.mapParent.Map;
			if (map != null)
			{
				Pawn t = caravan.PawnsListForReading[0];
				CaravanDropInventoryMode dropInventoryMode = (!map.IsPlayerHome) ? CaravanDropInventoryMode.DoNotDrop : CaravanDropInventoryMode.UnloadIndividually;
				bool draftColonists = this.mapParent.Faction != null && this.mapParent.Faction.HostileTo(Faction.OfPlayer);
				CaravanEnterMapUtility.Enter(caravan, map, CaravanEnterMode.Edge, dropInventoryMode, draftColonists, null);
				if (this.mapParent.def == WorldObjectDefOf.Ambush)
				{
					Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
					Find.LetterStack.ReceiveLetter("LetterLabelCaravanEnteredAmbushMap".Translate(), "LetterCaravanEnteredAmbushMap".Translate(new object[]
					{
						caravan.Label
					}).CapitalizeFirst(), LetterDefOf.NeutralEvent, t, null, null);
				}
				else if (caravan.IsPlayerControlled || this.mapParent.Faction == Faction.OfPlayer)
				{
					Messages.Message("MessageCaravanEnteredWorldObject".Translate(new object[]
					{
						caravan.Label,
						this.mapParent.Label
					}).CapitalizeFirst(), t, MessageTypeDefOf.TaskCompletion, true);
				}
			}
		}

		// Token: 0x06001CCF RID: 7375 RVA: 0x000F75A8 File Offset: 0x000F59A8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x06001CD0 RID: 7376 RVA: 0x000F75C4 File Offset: 0x000F59C4
		public static FloatMenuAcceptanceReport CanEnter(Caravan caravan, MapParent mapParent)
		{
			FloatMenuAcceptanceReport result;
			if (mapParent == null || !mapParent.Spawned || !mapParent.HasMap)
			{
				result = false;
			}
			else if (mapParent.EnterCooldownBlocksEntering())
			{
				result = FloatMenuAcceptanceReport.WithFailMessage("MessageEnterCooldownBlocksEntering".Translate(new object[]
				{
					mapParent.EnterCooldownDaysLeft().ToString("0.#")
				}));
			}
			else
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06001CD1 RID: 7377 RVA: 0x000F7644 File Offset: 0x000F5A44
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(new object[]
			{
				mapParent.Label
			}), caravan, mapParent.Tile, mapParent);
		}

		// Token: 0x04001151 RID: 4433
		private MapParent mapParent;
	}
}
