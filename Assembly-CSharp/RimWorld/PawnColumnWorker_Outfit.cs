using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		private const int ManageOutfitsButtonHeight = 32;

		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, (float)(rect.y + (rect.height - 65.0)), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageOutfits".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.outfits != null)
			{
				int num = Mathf.FloorToInt((float)((rect.width - 4.0) * 0.71428573131561279));
				int num2 = Mathf.FloorToInt((float)((rect.width - 4.0) * 0.28571429848670959));
				float x = rect.x;
				bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
				Rect rect2 = new Rect(x, (float)(rect.y + 2.0), (float)num, (float)(rect.height - 4.0));
				if (somethingIsForced)
				{
					rect2.width -= (float)(4.0 + (float)num2);
				}
				string label = pawn.outfits.CurrentOutfit.label.Truncate(rect2.width, null);
				if (Widgets.ButtonText(rect2, label, true, false, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (Outfit allOutfit in Current.Game.outfitDatabase.AllOutfits)
					{
						Outfit localOut = allOutfit;
						list.Add(new FloatMenuOption(localOut.label, delegate
						{
							pawn.outfits.CurrentOutfit = localOut;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
					Find.WindowStack.Add(new FloatMenu(list));
				}
				x += rect2.width;
				x = (float)(x + 4.0);
				Rect rect3 = new Rect(x, (float)(rect.y + 2.0), (float)num2, (float)(rect.height - 4.0));
				if (somethingIsForced)
				{
					if (Widgets.ButtonText(rect3, "ClearForcedApparel".Translate(), true, false, true))
					{
						pawn.outfits.forcedHandler.Reset();
					}
					TooltipHandler.TipRegion(rect3, new TipSignal(delegate
					{
						string text = "ForcedApparel".Translate() + ":\n";
						foreach (Apparel item in pawn.outfits.forcedHandler.ForcedApparel)
						{
							text = text + "\n   " + item.LabelCap;
						}
						return text;
					}, pawn.GetHashCode() * 612));
					x += (float)num2;
					x = (float)(x + 4.0);
				}
				Rect rect4 = new Rect(x, (float)(rect.y + 2.0), (float)num2, (float)(rect.height - 4.0));
				if (Widgets.ButtonText(rect4, "AssignTabEdit".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new Dialog_ManageOutfits(pawn.outfits.CurrentOutfit));
				}
				x += (float)num2;
			}
		}

		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(354f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		private int GetValueToCompare(Pawn pawn)
		{
			return (pawn.outfits != null && pawn.outfits.CurrentOutfit != null) ? pawn.outfits.CurrentOutfit.uniqueId : (-2147483648);
		}
	}
}
