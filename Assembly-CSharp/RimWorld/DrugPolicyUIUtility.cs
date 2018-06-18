using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000893 RID: 2195
	public static class DrugPolicyUIUtility
	{
		// Token: 0x06003211 RID: 12817 RVA: 0x001AF46C File Offset: 0x001AD86C
		public static void DoAssignDrugPolicyButtons(Rect rect, Pawn pawn)
		{
			int num = Mathf.FloorToInt((rect.width - 4f) * 0.714285731f);
			int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
			float num3 = rect.x;
			Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
			string text = pawn.drugs.CurrentPolicy.label;
			if (pawn.story != null && pawn.story.traits != null)
			{
				Trait trait = pawn.story.traits.GetTrait(TraitDefOf.DrugDesire);
				if (trait != null)
				{
					text = text + " (" + trait.Label + ")";
				}
			}
			Rect rect3 = rect2;
			Func<Pawn, DrugPolicy> getPayload = (Pawn p) => p.drugs.CurrentPolicy;
			if (DrugPolicyUIUtility.<>f__mg$cache0 == null)
			{
				DrugPolicyUIUtility.<>f__mg$cache0 = new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>>(DrugPolicyUIUtility.Button_GenerateMenu);
			}
			Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>> menuGenerator = DrugPolicyUIUtility.<>f__mg$cache0;
			string buttonLabel = text.Truncate(rect2.width, null);
			string label = pawn.drugs.CurrentPolicy.label;
			Widgets.Dropdown<Pawn, DrugPolicy>(rect3, pawn, getPayload, menuGenerator, buttonLabel, null, label, null, delegate
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
			}, true);
			num3 += (float)num;
			num3 += 4f;
			Rect rect4 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
			if (Widgets.ButtonText(rect4, "AssignTabEdit".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_ManageDrugPolicies(pawn.drugs.CurrentPolicy));
			}
			UIHighlighter.HighlightOpportunity(rect2, "ButtonAssignDrugs");
			UIHighlighter.HighlightOpportunity(rect4, "ButtonAssignDrugs");
			num3 += (float)num2;
		}

		// Token: 0x06003212 RID: 12818 RVA: 0x001AF658 File Offset: 0x001ADA58
		private static IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>> Button_GenerateMenu(Pawn pawn)
		{
			using (List<DrugPolicy>.Enumerator enumerator = Current.Game.drugPolicyDatabase.AllPolicies.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DrugPolicy assignedDrugs = enumerator.Current;
					yield return new Widgets.DropdownMenuElement<DrugPolicy>
					{
						option = new FloatMenuOption(assignedDrugs.label, delegate()
						{
							pawn.drugs.CurrentPolicy = assignedDrugs;
						}, MenuOptionPriority.Default, null, null, 0f, null, null),
						payload = assignedDrugs
					};
				}
			}
			yield break;
		}

		// Token: 0x04001AD8 RID: 6872
		public const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";

		// Token: 0x04001AD9 RID: 6873
		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>> <>f__mg$cache0;
	}
}
