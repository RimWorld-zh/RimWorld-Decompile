using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020005F1 RID: 1521
	public static class CaravanUIUtility
	{
		// Token: 0x06001E30 RID: 7728 RVA: 0x00103BD8 File Offset: 0x00101FD8
		public static void CreateCaravanTransferableWidgets(List<TransferableOneWay> transferables, out TransferableOneWayWidget pawnsTransfer, out TransferableOneWayWidget itemsTransfer, string thingCountTip, IgnorePawnsInventoryMode ignorePawnInventoryMass, Func<float> availableMassGetter, bool ignoreSpawnedCorpsesGearAndInventoryMass, int tile, bool playerPawnsReadOnly = false)
		{
			IEnumerable<TransferableOneWay> transferables2 = null;
			string sourceLabel = null;
			string destinationLabel = null;
			bool drawMass = true;
			bool includePawnsMassInMassUsage = false;
			pawnsTransfer = new TransferableOneWayWidget(transferables2, sourceLabel, destinationLabel, thingCountTip, drawMass, ignorePawnInventoryMass, includePawnsMassInMassUsage, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, true, true, false, true, false, playerPawnsReadOnly);
			CaravanUIUtility.AddPawnsSections(pawnsTransfer, transferables);
			transferables2 = from x in transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x;
			string sourceLabel2 = null;
			destinationLabel = null;
			bool drawMass2 = true;
			bool includePawnsMassInMassUsage2 = false;
			itemsTransfer = new TransferableOneWayWidget(transferables2, sourceLabel2, destinationLabel, thingCountTip, drawMass2, ignorePawnInventoryMass, includePawnsMassInMassUsage2, availableMassGetter, 0f, ignoreSpawnedCorpsesGearAndInventoryMass, tile, true, false, false, true, false, true, false);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x00103C90 File Offset: 0x00102090
		public static void AddPawnsSections(TransferableOneWayWidget widget, List<TransferableOneWay> transferables)
		{
			IEnumerable<TransferableOneWay> source = from x in transferables
			where x.ThingDef.category == ThingCategory.Pawn
			select x;
			widget.AddSection("ColonistsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsFreeColonist
			select x);
			widget.AddSection("PrisonersSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).IsPrisoner
			select x);
			widget.AddSection("CaptureSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).Downed && CaravanUtility.ShouldAutoCapture((Pawn)x.AnyThing, Faction.OfPlayer)
			select x);
			widget.AddSection("AnimalsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).RaceProps.Animal
			select x);
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x00103D90 File Offset: 0x00102190
		private static string GetDaysWorthOfFoodLabel(Pair<float, float> daysWorthOfFood, bool multiline)
		{
			string result;
			if (daysWorthOfFood.First >= 600f)
			{
				result = "InfiniteDaysWorthOfFoodInfo".Translate();
			}
			else
			{
				string text = daysWorthOfFood.First.ToString("0.#");
				string str = (!multiline) ? " " : "\n";
				if (daysWorthOfFood.Second < 600f && daysWorthOfFood.Second < daysWorthOfFood.First)
				{
					text = text + str + "(" + "DaysWorthOfFoodInfoRot".Translate(new object[]
					{
						daysWorthOfFood.Second.ToString("0.#") + ")"
					});
				}
				result = text;
			}
			return result;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x00103E58 File Offset: 0x00102258
		private static Color GetDaysWorthOfFoodColor(Pair<float, float> daysWorthOfFood, int? ticksToArrive)
		{
			Color result;
			if (daysWorthOfFood.First >= 600f)
			{
				result = Color.white;
			}
			else
			{
				float num = Mathf.Min(daysWorthOfFood.First, daysWorthOfFood.Second);
				if (ticksToArrive != null)
				{
					result = GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodKnownRouteColor, num / ((float)ticksToArrive.Value / 60000f));
				}
				else
				{
					result = GenUI.LerpColor(CaravanUIUtility.DaysWorthOfFoodColor, num);
				}
			}
			return result;
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x00103ED4 File Offset: 0x001022D4
		public static void DrawCaravanInfo(CaravanUIUtility.CaravanInfo info, CaravanUIUtility.CaravanInfo? info2, int currentTile, int? ticksToArrive, float lastMassFlashTime, Rect rect, bool lerpMassColor = true, string extraDaysWorthOfFoodTipInfo = null, bool multiline = false)
		{
			CaravanUIUtility.tmpInfo.Clear();
			string text = string.Concat(new string[]
			{
				"MassCarriedSimple".Translate(),
				": ",
				info.massUsage.ToStringEnsureThreshold(info.massCapacity, 2),
				" ",
				"kg".Translate(),
				"\n",
				"MassCapacity".Translate(),
				": ",
				info.massCapacity.ToString("F2"),
				" ",
				"kg".Translate()
			});
			if (info2 != null)
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n <-> \n",
					"MassCarriedSimple".Translate(),
					": ",
					info2.Value.massUsage.ToStringEnsureThreshold(info2.Value.massCapacity, 2),
					" ",
					"kg".Translate(),
					"\n",
					"MassCapacity".Translate(),
					": ",
					info2.Value.massCapacity.ToString("F2"),
					" ",
					"kg".Translate()
				});
			}
			text = text + "\n\n" + "CaravanMassUsageTooltip".Translate();
			if (!info.massCapacityExplanation.NullOrEmpty())
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n\n",
					"MassCapacity".Translate(),
					":\n",
					info.massCapacityExplanation
				});
			}
			if (info2 != null && !info2.Value.massCapacityExplanation.NullOrEmpty())
			{
				string text2 = text;
				text = string.Concat(new string[]
				{
					text2,
					"\n\n-----\n\n",
					"MassCapacity".Translate(),
					":\n",
					info2.Value.massCapacityExplanation
				});
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Mass".Translate(), string.Concat(new string[]
			{
				info.massUsage.ToStringEnsureThreshold(info.massCapacity, 0),
				" / ",
				info.massCapacity.ToString("F0"),
				" ",
				"kg".Translate()
			}), CaravanUIUtility.GetMassColor(info.massUsage, info.massCapacity, lerpMassColor), text, (info2 == null) ? null : string.Concat(new string[]
			{
				info2.Value.massUsage.ToStringEnsureThreshold(info2.Value.massCapacity, 0),
				" / ",
				info2.Value.massCapacity.ToString("F0"),
				" ",
				"kg".Translate()
			}), (info2 == null) ? Color.white : CaravanUIUtility.GetMassColor(info2.Value.massUsage, info2.Value.massCapacity, lerpMassColor), lastMassFlashTime));
			string text3 = "CaravanMovementSpeedTip".Translate();
			if (!info.tilesPerDayExplanation.NullOrEmpty())
			{
				text3 = text3 + "\n\n" + info.tilesPerDayExplanation;
			}
			if (info2 != null && !info2.Value.tilesPerDayExplanation.NullOrEmpty())
			{
				text3 = text3 + "\n\n-----\n\n" + info2.Value.tilesPerDayExplanation;
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("CaravanMovementSpeed".Translate(), info.tilesPerDay.ToString("0.#") + " " + "TilesPerDay".Translate(), GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, info.tilesPerDay), text3, (info2 == null) ? null : (info2.Value.tilesPerDay.ToString("0.#") + " " + "TilesPerDay".Translate()), (info2 == null) ? Color.white : GenUI.LerpColor(CaravanUIUtility.TilesPerDayColor, info2.Value.tilesPerDay), -9999f));
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("DaysWorthOfFood".Translate(), CaravanUIUtility.GetDaysWorthOfFoodLabel(info.daysWorthOfFood, multiline), CaravanUIUtility.GetDaysWorthOfFoodColor(info.daysWorthOfFood, ticksToArrive), "DaysWorthOfFoodTooltip".Translate() + extraDaysWorthOfFoodTipInfo + "\n\n" + VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(currentTile, Find.TickManager.TicksAbs), (info2 == null) ? null : CaravanUIUtility.GetDaysWorthOfFoodLabel(info2.Value.daysWorthOfFood, multiline), (info2 == null) ? Color.white : CaravanUIUtility.GetDaysWorthOfFoodColor(info2.Value.daysWorthOfFood, ticksToArrive), -9999f));
			string text4 = info.foragedFoodPerDay.Second.ToString("0.#");
			string text5 = (info2 == null) ? null : info2.Value.foragedFoodPerDay.Second.ToString("0.#");
			string text6 = "ForagedFoodPerDayTip".Translate();
			text6 = text6 + "\n\n" + info.foragedFoodPerDayExplanation;
			if (info2 != null)
			{
				text6 = text6 + "\n\n-----\n\n" + info2.Value.foragedFoodPerDayExplanation;
			}
			if (info.foragedFoodPerDay.Second > 0f || (info2 != null && info2.Value.foragedFoodPerDay.Second > 0f))
			{
				string text7 = (!multiline) ? " " : "\n";
				if (info2 == null)
				{
					string text2 = text4;
					text4 = string.Concat(new string[]
					{
						text2,
						text7,
						"(",
						info.foragedFoodPerDay.First.label,
						")"
					});
				}
				else
				{
					string text2 = text5;
					text5 = string.Concat(new string[]
					{
						text2,
						text7,
						"(",
						info2.Value.foragedFoodPerDay.First.label.Truncate(50f, null),
						")"
					});
				}
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("ForagedFoodPerDay".Translate(), text4, Color.white, text6, text5, Color.white, -9999f));
			string text8 = "CaravanVisibilityTip".Translate();
			if (!info.visibilityExplanation.NullOrEmpty())
			{
				text8 = text8 + "\n\n" + info.visibilityExplanation;
			}
			if (info2 != null && !info2.Value.visibilityExplanation.NullOrEmpty())
			{
				text8 = text8 + "\n\n-----\n\n" + info2.Value.visibilityExplanation;
			}
			CaravanUIUtility.tmpInfo.Add(new TransferableUIUtility.ExtraInfo("Visibility".Translate(), info.visibility.ToStringPercent(), GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info.visibility), text8, (info2 == null) ? null : info2.Value.visibility.ToStringPercent(), (info2 == null) ? Color.white : GenUI.LerpColor(CaravanUIUtility.VisibilityColor, info2.Value.visibility), -9999f));
			TransferableUIUtility.DrawExtraInfo(CaravanUIUtility.tmpInfo, rect);
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x0010472C File Offset: 0x00102B2C
		private static Color GetMassColor(float massUsage, float massCapacity, bool lerpMassColor)
		{
			Color result;
			if (massCapacity == 0f)
			{
				result = Color.white;
			}
			else if (massUsage > massCapacity)
			{
				result = Color.red;
			}
			else if (lerpMassColor)
			{
				result = GenUI.LerpColor(CaravanUIUtility.MassColor, massUsage / massCapacity);
			}
			else
			{
				result = Color.white;
			}
			return result;
		}

		// Token: 0x040011D8 RID: 4568
		private static readonly List<Pair<float, Color>> MassColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.37f, Color.green),
			new Pair<float, Color>(0.82f, Color.yellow),
			new Pair<float, Color>(1f, new Color(1f, 0.6f, 0f))
		};

		// Token: 0x040011D9 RID: 4569
		private static readonly List<Pair<float, Color>> TilesPerDayColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.001f, Color.red),
			new Pair<float, Color>(1f, Color.yellow),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x040011DA RID: 4570
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(1f, Color.red),
			new Pair<float, Color>(2f, Color.white)
		};

		// Token: 0x040011DB RID: 4571
		private static readonly List<Pair<float, Color>> DaysWorthOfFoodKnownRouteColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0.3f, Color.red),
			new Pair<float, Color>(0.9f, Color.yellow),
			new Pair<float, Color>(1.02f, Color.green)
		};

		// Token: 0x040011DC RID: 4572
		private static readonly List<Pair<float, Color>> VisibilityColor = new List<Pair<float, Color>>
		{
			new Pair<float, Color>(0f, Color.white),
			new Pair<float, Color>(0.01f, Color.green),
			new Pair<float, Color>(0.2f, Color.green),
			new Pair<float, Color>(1f, Color.white),
			new Pair<float, Color>(1.2f, Color.red)
		};

		// Token: 0x040011DD RID: 4573
		private static List<TransferableUIUtility.ExtraInfo> tmpInfo = new List<TransferableUIUtility.ExtraInfo>();

		// Token: 0x020005F2 RID: 1522
		public struct CaravanInfo
		{
			// Token: 0x06001E3D RID: 7741 RVA: 0x00104A54 File Offset: 0x00102E54
			public CaravanInfo(float massUsage, float massCapacity, string massCapacityExplanation, float tilesPerDay, string tilesPerDayExplanation, Pair<float, float> daysWorthOfFood, Pair<ThingDef, float> foragedFoodPerDay, string foragedFoodPerDayExplanation, float visibility, string visibilityExplanation)
			{
				this.massUsage = massUsage;
				this.massCapacity = massCapacity;
				this.massCapacityExplanation = massCapacityExplanation;
				this.tilesPerDay = tilesPerDay;
				this.tilesPerDayExplanation = tilesPerDayExplanation;
				this.daysWorthOfFood = daysWorthOfFood;
				this.foragedFoodPerDay = foragedFoodPerDay;
				this.foragedFoodPerDayExplanation = foragedFoodPerDayExplanation;
				this.visibility = visibility;
				this.visibilityExplanation = visibilityExplanation;
			}

			// Token: 0x040011E4 RID: 4580
			public float massUsage;

			// Token: 0x040011E5 RID: 4581
			public float massCapacity;

			// Token: 0x040011E6 RID: 4582
			public string massCapacityExplanation;

			// Token: 0x040011E7 RID: 4583
			public float tilesPerDay;

			// Token: 0x040011E8 RID: 4584
			public string tilesPerDayExplanation;

			// Token: 0x040011E9 RID: 4585
			public Pair<float, float> daysWorthOfFood;

			// Token: 0x040011EA RID: 4586
			public Pair<ThingDef, float> foragedFoodPerDay;

			// Token: 0x040011EB RID: 4587
			public string foragedFoodPerDayExplanation;

			// Token: 0x040011EC RID: 4588
			public float visibility;

			// Token: 0x040011ED RID: 4589
			public string visibilityExplanation;
		}
	}
}
