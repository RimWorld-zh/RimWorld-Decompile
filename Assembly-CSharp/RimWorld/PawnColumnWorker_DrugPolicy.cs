using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnColumnWorker_DrugPolicy : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		private const int ManageDrugPoliciesButtonHeight = 32;

		private const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";

		public override void DoHeader(Rect rect, PawnTable table)
		{
			base.DoHeader(rect, table);
			Rect rect2 = new Rect(rect.x, (float)(rect.y + (rect.height - 65.0)), Mathf.Min(rect.width, 360f), 32f);
			if (Widgets.ButtonText(rect2, "ManageDrugPolicies".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(null));
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
			}
			UIHighlighter.HighlightOpportunity(rect2, "ManageDrugPolicies");
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
		}

		public override void DoCell(Rect rect, Pawn pawn, PawnTable table)
		{
			if (pawn.drugs != null)
			{
				int num = Mathf.FloorToInt((float)((rect.width - 4.0) * 0.71428573131561279));
				int num2 = Mathf.FloorToInt((float)((rect.width - 4.0) * 0.28571429848670959));
				float x = rect.x;
				Rect rect2 = new Rect(x, (float)(rect.y + 2.0), (float)num, (float)(rect.height - 4.0));
				string text = pawn.drugs.CurrentPolicy.label;
				if (pawn.story != null && pawn.story.traits != null)
				{
					Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
					if (trait != null)
					{
						text = text + " (" + trait.Label + ")";
					}
				}
				text = text.Truncate(rect2.width, null);
				if (Widgets.ButtonText(rect2, text, true, false, true))
				{
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					List<DrugPolicy>.Enumerator enumerator = Current.Game.drugPolicyDatabase.AllPolicies.GetEnumerator();
					try
					{
						while (enumerator.MoveNext())
						{
							DrugPolicy current = enumerator.Current;
							DrugPolicy localAssignedDrugs = current;
							list.Add(new FloatMenuOption(current.label, (Action)delegate()
							{
								pawn.drugs.CurrentPolicy = localAssignedDrugs;
							}, MenuOptionPriority.Default, null, null, 0f, null, null));
						}
					}
					finally
					{
						((IDisposable)(object)enumerator).Dispose();
					}
					Find.WindowStack.Add(new FloatMenu(list));
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
				}
				x += (float)num;
				x = (float)(x + 4.0);
				Rect rect3 = new Rect(x, (float)(rect.y + 2.0), (float)num2, (float)(rect.height - 4.0));
				if (Widgets.ButtonText(rect3, "AssignTabEdit".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new Dialog_ManageDrugPolicies(pawn.drugs.CurrentPolicy));
					PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
				}
				UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
				UIHighlighter.HighlightOpportunity(rect3, "ButtonAssignDrugs");
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
			return (pawn.drugs != null && pawn.drugs.CurrentPolicy != null) ? pawn.drugs.CurrentPolicy.uniqueId : (-2147483648);
		}
	}
}
