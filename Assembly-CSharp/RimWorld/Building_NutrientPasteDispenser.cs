using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006AB RID: 1707
	public class Building_NutrientPasteDispenser : Building
	{
		// Token: 0x0400143B RID: 5179
		public CompPowerTrader powerComp;

		// Token: 0x0400143C RID: 5180
		private List<IntVec3> cachedAdjCellsCardinal;

		// Token: 0x0400143D RID: 5181
		public static int CollectDuration = 50;

		// Token: 0x17000584 RID: 1412
		// (get) Token: 0x06002491 RID: 9361 RVA: 0x0013963C File Offset: 0x00137A3C
		public bool CanDispenseNow
		{
			get
			{
				return this.powerComp.PowerOn && this.HasEnoughFeedstockInHoppers();
			}
		}

		// Token: 0x17000585 RID: 1413
		// (get) Token: 0x06002492 RID: 9362 RVA: 0x0013966C File Offset: 0x00137A6C
		private List<IntVec3> AdjCellsCardinalInBounds
		{
			get
			{
				if (this.cachedAdjCellsCardinal == null)
				{
					this.cachedAdjCellsCardinal = (from c in GenAdj.CellsAdjacentCardinal(this)
					where c.InBounds(base.Map)
					select c).ToList<IntVec3>();
				}
				return this.cachedAdjCellsCardinal;
			}
		}

		// Token: 0x17000586 RID: 1414
		// (get) Token: 0x06002493 RID: 9363 RVA: 0x001396B4 File Offset: 0x00137AB4
		public virtual ThingDef DispensableDef
		{
			get
			{
				return ThingDefOf.MealNutrientPaste;
			}
		}

		// Token: 0x17000587 RID: 1415
		// (get) Token: 0x06002494 RID: 9364 RVA: 0x001396D0 File Offset: 0x00137AD0
		public override Color DrawColor
		{
			get
			{
				Color result;
				if (!this.IsSociallyProper(null, false, false))
				{
					result = Building_Bed.SheetColorForPrisoner;
				}
				else
				{
					result = base.DrawColor;
				}
				return result;
			}
		}

		// Token: 0x06002495 RID: 9365 RVA: 0x00139704 File Offset: 0x00137B04
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.powerComp = base.GetComp<CompPowerTrader>();
		}

		// Token: 0x06002496 RID: 9366 RVA: 0x0013971C File Offset: 0x00137B1C
		public virtual Building AdjacentReachableHopper(Pawn reacher)
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				IntVec3 c = this.AdjCellsCardinalInBounds[i];
				Building edifice = c.GetEdifice(base.Map);
				if (edifice != null && edifice.def == ThingDefOf.Hopper && reacher.CanReach(edifice, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
				{
					return edifice;
				}
			}
			return null;
		}

		// Token: 0x06002497 RID: 9367 RVA: 0x0013979C File Offset: 0x00137B9C
		public virtual Thing TryDispenseFood()
		{
			Thing result;
			if (!this.CanDispenseNow)
			{
				result = null;
			}
			else
			{
				float num = this.def.building.nutritionCostPerDispense - 0.0001f;
				List<ThingDef> list = new List<ThingDef>();
				for (;;)
				{
					Thing thing = this.FindFeedInAnyHopper();
					if (thing == null)
					{
						break;
					}
					int num2 = Mathf.Min(thing.stackCount, Mathf.CeilToInt(num / thing.GetStatValue(StatDefOf.Nutrition, true)));
					num -= (float)num2 * thing.GetStatValue(StatDefOf.Nutrition, true);
					list.Add(thing.def);
					thing.SplitOff(num2);
					if (num <= 0f)
					{
						goto Block_3;
					}
				}
				Log.Error("Did not find enough food in hoppers while trying to dispense.", false);
				return null;
				Block_3:
				this.def.building.soundDispense.PlayOneShot(new TargetInfo(base.Position, base.Map, false));
				Thing thing2 = ThingMaker.MakeThing(ThingDefOf.MealNutrientPaste, null);
				CompIngredients compIngredients = thing2.TryGetComp<CompIngredients>();
				for (int i = 0; i < list.Count; i++)
				{
					compIngredients.RegisterIngredient(list[i]);
				}
				result = thing2;
			}
			return result;
		}

		// Token: 0x06002498 RID: 9368 RVA: 0x001398D4 File Offset: 0x00137CD4
		public virtual Thing FindFeedInAnyHopper()
		{
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = this.AdjCellsCardinalInBounds[i].GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.def == ThingDefOf.Hopper)
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06002499 RID: 9369 RVA: 0x0013998C File Offset: 0x00137D8C
		public virtual bool HasEnoughFeedstockInHoppers()
		{
			float num = 0f;
			for (int i = 0; i < this.AdjCellsCardinalInBounds.Count; i++)
			{
				IntVec3 c = this.AdjCellsCardinalInBounds[i];
				Thing thing = null;
				Thing thing2 = null;
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int j = 0; j < thingList.Count; j++)
				{
					Thing thing3 = thingList[j];
					if (Building_NutrientPasteDispenser.IsAcceptableFeedstock(thing3.def))
					{
						thing = thing3;
					}
					if (thing3.def == ThingDefOf.Hopper)
					{
						thing2 = thing3;
					}
				}
				if (thing != null && thing2 != null)
				{
					num += (float)thing.stackCount * thing.GetStatValue(StatDefOf.Nutrition, true);
				}
				if (num >= this.def.building.nutritionCostPerDispense)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600249A RID: 9370 RVA: 0x00139A7C File Offset: 0x00137E7C
		public static bool IsAcceptableFeedstock(ThingDef def)
		{
			return def.IsNutritionGivingIngestible && def.ingestible.preferability != FoodPreferability.Undefined && (def.ingestible.foodType & FoodTypeFlags.Plant) != FoodTypeFlags.Plant && (def.ingestible.foodType & FoodTypeFlags.Tree) != FoodTypeFlags.Tree;
		}

		// Token: 0x0600249B RID: 9371 RVA: 0x00139AE0 File Offset: 0x00137EE0
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(base.GetInspectString());
			if (!this.IsSociallyProper(null, false, false))
			{
				stringBuilder.AppendLine("InPrisonCell".Translate());
			}
			return stringBuilder.ToString().Trim();
		}
	}
}
