using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005CE RID: 1486
	public class CaravanArrivalAction_Enter : CaravanArrivalAction
	{
		// Token: 0x06001CD2 RID: 7378 RVA: 0x000F7333 File Offset: 0x000F5733
		public CaravanArrivalAction_Enter()
		{
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000F733C File Offset: 0x000F573C
		public CaravanArrivalAction_Enter(MapParent mapParent)
		{
			this.mapParent = mapParent;
		}

		// Token: 0x17000430 RID: 1072
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x000F734C File Offset: 0x000F574C
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
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x000F7380 File Offset: 0x000F5780
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

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000F73B4 File Offset: 0x000F57B4
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

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000F7418 File Offset: 0x000F5818
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

		// Token: 0x06001CD8 RID: 7384 RVA: 0x000F7554 File Offset: 0x000F5954
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x06001CD9 RID: 7385 RVA: 0x000F7570 File Offset: 0x000F5970
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

		// Token: 0x06001CDA RID: 7386 RVA: 0x000F75F0 File Offset: 0x000F59F0
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan, MapParent mapParent)
		{
			return CaravanArrivalActionUtility.GetFloatMenuOptions<CaravanArrivalAction_Enter>(() => CaravanArrivalAction_Enter.CanEnter(caravan, mapParent), () => new CaravanArrivalAction_Enter(mapParent), "EnterMap".Translate(new object[]
			{
				mapParent.Label
			}), caravan, mapParent.Tile, mapParent);
		}

		// Token: 0x04001154 RID: 4436
		private MapParent mapParent;
	}
}
