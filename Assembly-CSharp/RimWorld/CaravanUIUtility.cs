using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class CaravanUIUtility
	{
		public const float ExtraSpaceForDaysWorthOfFoodReadout = 24f;

		public const float SpaceBetweenMassInfoAndDaysWorthOfFood = 19f;

		public static void DrawDaysWorthOfFoodInfo(Rect rect, float daysWorthOfFood, float daysUntilRot, bool canEatLocalPlants, bool alignRight = false, float truncToWidth = 3.40282347E+38f)
		{
			GUI.color = Color.gray;
			string text = (!(daysWorthOfFood >= 1000.0)) ? ((!(daysUntilRot < 1000.0)) ? "DaysWorthOfFoodInfo".Translate(daysWorthOfFood.ToString("0.#")) : "DaysWorthOfFoodInfoRot".Translate(daysWorthOfFood.ToString("0.#"), daysUntilRot.ToString("0.#"))) : "InfiniteDaysWorthOfFoodInfo".Translate();
			string text2 = text;
			if (truncToWidth != 3.4028234663852886E+38)
			{
				text2 = text.Truncate(truncToWidth, null);
			}
			Vector2 vector = Text.CalcSize(text2);
			Rect rect2 = (!alignRight) ? new Rect(rect.x, rect.y, vector.x, vector.y) : new Rect(rect.xMax - vector.x, rect.y, vector.x, vector.y);
			Widgets.Label(rect2, text2);
			string str = "";
			if (truncToWidth != 3.4028234663852886E+38)
			{
				Vector2 vector2 = Text.CalcSize(text);
				if (vector2.x > truncToWidth)
				{
					str = str + text + "\n\n";
				}
			}
			str = str + "DaysWorthOfFoodTooltip".Translate() + "\n\n";
			str = ((!canEatLocalPlants) ? (str + "DaysWorthOfFoodTooltip_CantEatLocalPlants".Translate()) : (str + "DaysWorthOfFoodTooltip_CanEatLocalPlants".Translate()));
			TooltipHandler.TipRegion(rect2, str);
			GUI.color = Color.white;
		}

		public static void CreateCaravanTransferableWidgets(List<TransferableOneWay> transferables, out TransferableOneWayWidget pawnsTransfer, out TransferableOneWayWidget itemsTransfer, string sourceLabel, string destLabel, string thingCountTip, IgnorePawnsInventoryMode ignorePawnInventoryMass, Func<float> availableMassGetter, bool ignoreCorpsesGearAndInventoryMass, int drawDaysUntilRotForTile)
		{
			pawnsTransfer = new TransferableOneWayWidget(null, sourceLabel, destLabel, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 24f, ignoreCorpsesGearAndInventoryMass, true, -1);
			CaravanUIUtility.AddPawnsSections(pawnsTransfer, transferables);
			itemsTransfer = new TransferableOneWayWidget(from x in transferables
			where x.ThingDef.category != ThingCategory.Pawn
			select x, sourceLabel, destLabel, thingCountTip, true, ignorePawnInventoryMass, false, availableMassGetter, 24f, ignoreCorpsesGearAndInventoryMass, true, drawDaysUntilRotForTile);
		}

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
			widget.AddSection("AnimalsSection".Translate(), from x in source
			where ((Pawn)x.AnyThing).RaceProps.Animal
			select x);
		}
	}
}
