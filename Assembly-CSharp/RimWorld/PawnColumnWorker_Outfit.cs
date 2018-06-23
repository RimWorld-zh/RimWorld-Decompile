using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000894 RID: 2196
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		// Token: 0x04001ADF RID: 6879
		private const int TopAreaHeight = 65;

		// Token: 0x04001AE0 RID: 6880
		private const int ManageOutfitsButtonHeight = 32;

		// Token: 0x06003229 RID: 12841 RVA: 0x001B0130 File Offset: 0x001AE530
		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, rect.y + (rect.height - 65f), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageOutfits".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageOutfits(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Outfits, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageOutfits");
		}

		// Token: 0x0600322A RID: 12842 RVA: 0x001B01C0 File Offset: 0x001AE5C0
		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.outfits != null)
			{
				int num = Mathf.FloorToInt((rect.width - 4f) * 0.714285731f);
				int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
				float num3 = rect.x;
				bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
				Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
				if (somethingIsForced)
				{
					rect2.width -= 4f + (float)num2;
				}
				Rect rect3 = rect2;
				Pawn pawn2 = pawn;
				Func<Pawn, Outfit> getPayload = (Pawn p) => p.outfits.CurrentOutfit;
				Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Outfit>>> menuGenerator = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Outfit>>>(this.Button_GenerateMenu);
				string buttonLabel = pawn.outfits.CurrentOutfit.label.Truncate(rect2.width, null);
				string label = pawn.outfits.CurrentOutfit.label;
				Widgets.Dropdown<Pawn, Outfit>(rect3, pawn2, getPayload, menuGenerator, buttonLabel, null, label, null, null, true);
				num3 += rect2.width;
				num3 += 4f;
				Rect rect4 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
				if (somethingIsForced)
				{
					if (Widgets.ButtonText(rect4, "ClearForcedApparel".Translate(), true, false, true))
					{
						pawn.outfits.forcedHandler.Reset();
					}
					TooltipHandler.TipRegion(rect4, new TipSignal(delegate()
					{
						string text = "ForcedApparel".Translate() + ":\n";
						foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
						{
							text = text + "\n   " + apparel.LabelCap;
						}
						return text;
					}, pawn.GetHashCode() * 612));
					num3 += (float)num2;
					num3 += 4f;
				}
				Rect rect5 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
				if (Widgets.ButtonText(rect5, "AssignTabEdit".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new Dialog_ManageOutfits(pawn.outfits.CurrentOutfit));
				}
				num3 += (float)num2;
			}
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x001B0410 File Offset: 0x001AE810
		private IEnumerable<Widgets.DropdownMenuElement<Outfit>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<Outfit>.Enumerator enumerator = Current.Game.outfitDatabase.AllOutfits.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Outfit outfit = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<Outfit>
					{
						option = new FloatMenuOption(outfit.label, delegate()
						{
							pawn.outfits.CurrentOutfit = outfit;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = outfit
					};
				}
			}
			yield break;
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x001B043C File Offset: 0x001AE83C
		public override int GetMinWidth(PawnTable table)
		{
			return Mathf.Max(base.GetMinWidth(table), Mathf.CeilToInt(194f));
		}

		// Token: 0x0600322D RID: 12845 RVA: 0x001B0468 File Offset: 0x001AE868
		public override int GetOptimalWidth(PawnTable table)
		{
			return Mathf.Clamp(Mathf.CeilToInt(354f), this.GetMinWidth(table), this.GetMaxWidth(table));
		}

		// Token: 0x0600322E RID: 12846 RVA: 0x001B049C File Offset: 0x001AE89C
		public override int GetMinHeaderHeight(PawnTable table)
		{
			return Mathf.Max(base.GetMinHeaderHeight(table), 65);
		}

		// Token: 0x0600322F RID: 12847 RVA: 0x001B04C0 File Offset: 0x001AE8C0
		public override int Compare(Pawn a, Pawn b)
		{
			return this.GetValueToCompare(a).CompareTo(this.GetValueToCompare(b));
		}

		// Token: 0x06003230 RID: 12848 RVA: 0x001B04EC File Offset: 0x001AE8EC
		private int GetValueToCompare(Pawn pawn)
		{
			return (pawn.outfits != null && pawn.outfits.CurrentOutfit != null) ? pawn.outfits.CurrentOutfit.uniqueId : int.MinValue;
		}
	}
}
