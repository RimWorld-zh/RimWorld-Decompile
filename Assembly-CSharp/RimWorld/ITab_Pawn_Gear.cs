using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200084E RID: 2126
	public class ITab_Pawn_Gear : ITab
	{
		// Token: 0x06003025 RID: 12325 RVA: 0x001A2A80 File Offset: 0x001A0E80
		public ITab_Pawn_Gear()
		{
			this.size = new Vector2(460f, 450f);
			this.labelKey = "TabGear";
			this.tutorTag = "Gear";
		}

		// Token: 0x170007AB RID: 1963
		// (get) Token: 0x06003026 RID: 12326 RVA: 0x001A2AD8 File Offset: 0x001A0ED8
		public override bool IsVisible
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return this.ShouldShowInventory(selPawnForGear) || this.ShouldShowApparel(selPawnForGear) || this.ShouldShowEquipment(selPawnForGear);
			}
		}

		// Token: 0x170007AC RID: 1964
		// (get) Token: 0x06003027 RID: 12327 RVA: 0x001A2B18 File Offset: 0x001A0F18
		private bool CanControl
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return !selPawnForGear.Downed && !selPawnForGear.InMentalState && (selPawnForGear.Faction == Faction.OfPlayer || selPawnForGear.IsPrisonerOfColony) && (!selPawnForGear.IsPrisonerOfColony || !selPawnForGear.Spawned || selPawnForGear.Map.mapPawns.AnyFreeColonistSpawned) && (!selPawnForGear.IsPrisonerOfColony || (!PrisonBreakUtility.IsPrisonBreaking(selPawnForGear) && (selPawnForGear.CurJob == null || !selPawnForGear.CurJob.exitMapOnArrival)));
			}
		}

		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06003028 RID: 12328 RVA: 0x001A2BE0 File Offset: 0x001A0FE0
		private bool CanControlColonist
		{
			get
			{
				return this.CanControl && this.SelPawnForGear.IsColonistPlayerControlled;
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x06003029 RID: 12329 RVA: 0x001A2C10 File Offset: 0x001A1010
		private Pawn SelPawnForGear
		{
			get
			{
				Pawn result;
				if (base.SelPawn != null)
				{
					result = base.SelPawn;
				}
				else
				{
					Corpse corpse = base.SelThing as Corpse;
					if (corpse == null)
					{
						throw new InvalidOperationException("Gear tab on non-pawn non-corpse " + base.SelThing);
					}
					result = corpse.InnerPawn;
				}
				return result;
			}
		}

		// Token: 0x0600302A RID: 12330 RVA: 0x001A2C6C File Offset: 0x001A106C
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f);
			Rect rect2 = rect.ContractedBy(10f);
			Rect position = new Rect(rect2.x, rect2.y, rect2.width, rect2.height);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, position.width, position.height);
			Rect viewRect = new Rect(0f, 0f, position.width - 16f, this.scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			this.TryDrawMassInfo(ref num, viewRect.width);
			this.TryDrawComfyTemperatureRange(ref num, viewRect.width);
			if (this.ShouldShowOverallArmor(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "OverallArmor".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Sharp, "ArmorSharp".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Blunt, "ArmorBlunt".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Heat, "ArmorHeat".Translate());
			}
			if (this.ShouldShowEquipment(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Equipment".Translate());
				foreach (ThingWithComps thing in this.SelPawnForGear.equipment.AllEquipmentListForReading)
				{
					this.DrawThingRow(ref num, viewRect.width, thing, false);
				}
			}
			if (this.ShouldShowApparel(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Apparel".Translate());
				foreach (Apparel thing2 in from ap in this.SelPawnForGear.apparel.WornApparel
				orderby ap.def.apparel.bodyPartGroups[0].listOrder descending
				select ap)
				{
					this.DrawThingRow(ref num, viewRect.width, thing2, false);
				}
			}
			if (this.ShouldShowInventory(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Inventory".Translate());
				ITab_Pawn_Gear.workingInvList.Clear();
				ITab_Pawn_Gear.workingInvList.AddRange(this.SelPawnForGear.inventory.innerContainer);
				for (int i = 0; i < ITab_Pawn_Gear.workingInvList.Count; i++)
				{
					this.DrawThingRow(ref num, viewRect.width, ITab_Pawn_Gear.workingInvList[i], true);
				}
				ITab_Pawn_Gear.workingInvList.Clear();
			}
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x0600302B RID: 12331 RVA: 0x001A3008 File Offset: 0x001A1408
		private void DrawThingRow(ref float y, float width, Thing thing, bool inventory = false)
		{
			Rect rect = new Rect(0f, y, width, 28f);
			Widgets.InfoCardButton(rect.width - 24f, y, thing);
			rect.width -= 24f;
			if (this.CanControl && (inventory || this.CanControlColonist || (this.SelPawnForGear.Spawned && !this.SelPawnForGear.Map.IsPlayerHome)))
			{
				Rect rect2 = new Rect(rect.width - 24f, y, 24f, 24f);
				TooltipHandler.TipRegion(rect2, "DropThing".Translate());
				if (Widgets.ButtonImage(rect2, TexButton.Drop))
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.InterfaceDrop(thing);
				}
				rect.width -= 24f;
			}
			if (this.CanControlColonist)
			{
				if ((thing.def.IsNutritionGivingIngestible || thing.def.IsNonMedicalDrug) && thing.IngestibleNow && base.SelPawn.RaceProps.CanEverEat(thing))
				{
					Rect rect3 = new Rect(rect.width - 24f, y, 24f, 24f);
					TooltipHandler.TipRegion(rect3, "ConsumeThing".Translate(new object[]
					{
						thing.LabelNoCount
					}));
					if (Widgets.ButtonImage(rect3, TexButton.Ingest))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.InterfaceIngest(thing);
					}
				}
				rect.width -= 24f;
			}
			Rect rect4 = rect;
			rect4.xMin = rect4.xMax - 60f;
			CaravanThingsTabUtility.DrawMass(thing, rect4);
			rect.width -= 60f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_Gear.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thing.def.DrawMatSingle != null && thing.def.DrawMatSingle.mainTexture != null)
			{
				Widgets.ThingIcon(new Rect(4f, y, 28f, 28f), thing, 1f);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_Gear.ThingLabelColor;
			Rect rect5 = new Rect(36f, y, rect.width - 36f, rect.height);
			string text = thing.LabelCap;
			Apparel apparel = thing as Apparel;
			if (apparel != null && this.SelPawnForGear.outfits != null && this.SelPawnForGear.outfits.forcedHandler.IsForced(apparel))
			{
				text = text + ", " + "ApparelForcedLower".Translate();
			}
			Text.WordWrap = false;
			Widgets.Label(rect5, text.Truncate(rect5.width, null));
			Text.WordWrap = true;
			string text2 = thing.DescriptionDetailed;
			if (thing.def.useHitPoints)
			{
				string text3 = text2;
				text2 = string.Concat(new object[]
				{
					text3,
					"\n",
					thing.HitPoints,
					" / ",
					thing.MaxHitPoints
				});
			}
			TooltipHandler.TipRegion(rect, text2);
			y += 28f;
		}

		// Token: 0x0600302C RID: 12332 RVA: 0x001A3394 File Offset: 0x001A1794
		private void TryDrawOverallArmor(ref float curY, float width, StatDef stat, string label)
		{
			float num = 0f;
			float num2 = Mathf.Clamp01(this.SelPawnForGear.GetStatValue(stat, true));
			List<BodyPartRecord> allParts = this.SelPawnForGear.RaceProps.body.AllParts;
			List<Apparel> list = (this.SelPawnForGear.apparel == null) ? null : this.SelPawnForGear.apparel.WornApparel;
			for (int i = 0; i < allParts.Count; i++)
			{
				float num3 = 1f - num2;
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].def.apparel.CoversBodyPart(allParts[i]))
						{
							float num4 = Mathf.Clamp01(list[j].GetStatValue(stat, true));
							num3 *= 1f - num4;
						}
					}
				}
				num += allParts[i].coverageAbs * (1f - num3);
			}
			num = Mathf.Clamp01(num);
			Rect rect = new Rect(0f, curY, width, 100f);
			Widgets.Label(rect, label.Truncate(120f, null));
			rect.xMin += 120f;
			Widgets.Label(rect, num.ToStringPercent());
			curY += 22f;
		}

		// Token: 0x0600302D RID: 12333 RVA: 0x001A3504 File Offset: 0x001A1904
		private void TryDrawMassInfo(ref float curY, float width)
		{
			if (!this.SelPawnForGear.Dead && this.ShouldShowInventory(this.SelPawnForGear))
			{
				Rect rect = new Rect(0f, curY, width, 22f);
				float num = MassUtility.GearAndInventoryMass(this.SelPawnForGear);
				float num2 = MassUtility.Capacity(this.SelPawnForGear, null);
				Widgets.Label(rect, "MassCarried".Translate(new object[]
				{
					num.ToString("0.##"),
					num2.ToString("0.##")
				}));
				curY += 22f;
			}
		}

		// Token: 0x0600302E RID: 12334 RVA: 0x001A35A4 File Offset: 0x001A19A4
		private void TryDrawComfyTemperatureRange(ref float curY, float width)
		{
			if (!this.SelPawnForGear.Dead)
			{
				Rect rect = new Rect(0f, curY, width, 22f);
				float statValue = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMin, true);
				float statValue2 = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
				Widgets.Label(rect, string.Concat(new string[]
				{
					"ComfyTemperatureRange".Translate(),
					": ",
					statValue.ToStringTemperature("F0"),
					" ~ ",
					statValue2.ToStringTemperature("F0")
				}));
				curY += 22f;
			}
		}

		// Token: 0x0600302F RID: 12335 RVA: 0x001A3654 File Offset: 0x001A1A54
		private void InterfaceDrop(Thing t)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			Apparel apparel = t as Apparel;
			if (apparel != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.WornApparel.Contains(apparel))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(new Job(JobDefOf.RemoveApparel, apparel), JobTag.Misc);
			}
			else if (thingWithComps != null && this.SelPawnForGear.equipment != null && this.SelPawnForGear.equipment.AllEquipmentListForReading.Contains(thingWithComps))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(new Job(JobDefOf.DropEquipment, thingWithComps), JobTag.Misc);
			}
			else if (!t.def.destroyOnDrop)
			{
				Thing thing;
				this.SelPawnForGear.inventory.innerContainer.TryDrop(t, this.SelPawnForGear.Position, this.SelPawnForGear.Map, ThingPlaceMode.Near, out thing, null, null);
			}
		}

		// Token: 0x06003030 RID: 12336 RVA: 0x001A3764 File Offset: 0x001A1B64
		private void InterfaceIngest(Thing t)
		{
			Job job = new Job(JobDefOf.Ingest, t);
			job.count = Mathf.Min(t.stackCount, t.def.ingestible.maxNumToIngestAtOnce);
			job.count = Mathf.Min(job.count, FoodUtility.WillIngestStackCountOf(this.SelPawnForGear, t.def, t.GetStatValue(StatDefOf.Nutrition, true)));
			this.SelPawnForGear.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		// Token: 0x06003031 RID: 12337 RVA: 0x001A37E8 File Offset: 0x001A1BE8
		private bool ShouldShowInventory(Pawn p)
		{
			return p.RaceProps.Humanlike || p.inventory.innerContainer.Any;
		}

		// Token: 0x06003032 RID: 12338 RVA: 0x001A3820 File Offset: 0x001A1C20
		private bool ShouldShowApparel(Pawn p)
		{
			return p.apparel != null && (p.RaceProps.Humanlike || p.apparel.WornApparel.Any<Apparel>());
		}

		// Token: 0x06003033 RID: 12339 RVA: 0x001A386C File Offset: 0x001A1C6C
		private bool ShouldShowEquipment(Pawn p)
		{
			return p.equipment != null;
		}

		// Token: 0x06003034 RID: 12340 RVA: 0x001A3890 File Offset: 0x001A1C90
		private bool ShouldShowOverallArmor(Pawn p)
		{
			return p.RaceProps.Humanlike || this.ShouldShowApparel(p) || p.GetStatValue(StatDefOf.ArmorRating_Sharp, true) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Blunt, true) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Heat, true) > 0f;
		}

		// Token: 0x04001A0C RID: 6668
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04001A0D RID: 6669
		private float scrollViewHeight = 0f;

		// Token: 0x04001A0E RID: 6670
		private const float TopPadding = 20f;

		// Token: 0x04001A0F RID: 6671
		public static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x04001A10 RID: 6672
		public static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x04001A11 RID: 6673
		private const float ThingIconSize = 28f;

		// Token: 0x04001A12 RID: 6674
		private const float ThingRowHeight = 28f;

		// Token: 0x04001A13 RID: 6675
		private const float ThingLeftX = 36f;

		// Token: 0x04001A14 RID: 6676
		private const float StandardLineHeight = 22f;

		// Token: 0x04001A15 RID: 6677
		private static List<Thing> workingInvList = new List<Thing>();
	}
}
