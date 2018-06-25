using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class DrugPolicyUIUtility
	{
		public const string AssigningDrugsTutorHighlightTag = "ButtonAssignDrugs";

		[CompilerGenerated]
		private static Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>> <>f__mg$cache0;

		[CompilerGenerated]
		private static Func<Pawn, DrugPolicy> <>f__am$cache0;

		[CompilerGenerated]
		private static Action <>f__am$cache1;

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

		[CompilerGenerated]
		private static DrugPolicy <DoAssignDrugPolicyButtons>m__0(Pawn p)
		{
			return p.drugs.CurrentPolicy;
		}

		[CompilerGenerated]
		private static void <DoAssignDrugPolicyButtons>m__1()
		{
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.DrugPolicies, KnowledgeAmount.Total);
		}

		[CompilerGenerated]
		private sealed class <Button_GenerateMenu>c__Iterator0 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<DrugPolicy>>
		{
			internal List<DrugPolicy>.Enumerator $locvar0;

			internal Pawn pawn;

			internal Widgets.DropdownMenuElement<DrugPolicy> $current;

			internal bool $disposing;

			internal int $PC;

			private DrugPolicyUIUtility.<Button_GenerateMenu>c__Iterator0.<Button_GenerateMenu>c__AnonStorey2 $locvar1;

			private DrugPolicyUIUtility.<Button_GenerateMenu>c__Iterator0.<Button_GenerateMenu>c__AnonStorey1 $locvar2;

			[DebuggerHidden]
			public <Button_GenerateMenu>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				bool flag = false;
				switch (num)
				{
				case 0u:
					enumerator = Current.Game.drugPolicyDatabase.AllPolicies.GetEnumerator();
					num = 4294967293u;
					break;
				case 1u:
					break;
				default:
					return false;
				}
				try
				{
					switch (num)
					{
					}
					if (enumerator.MoveNext())
					{
						DrugPolicy assignedDrugs = enumerator.Current;
						this.$current = new Widgets.DropdownMenuElement<DrugPolicy>
						{
							option = new FloatMenuOption(assignedDrugs.label, delegate()
							{
								<Button_GenerateMenu>c__AnonStorey.pawn.drugs.CurrentPolicy = assignedDrugs;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = assignedDrugs
						};
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						flag = true;
						return true;
					}
				}
				finally
				{
					if (!flag)
					{
						((IDisposable)enumerator).Dispose();
					}
				}
				this.$PC = -1;
				return false;
			}

			Widgets.DropdownMenuElement<DrugPolicy> IEnumerator<Widgets.DropdownMenuElement<DrugPolicy>>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				uint num = (uint)this.$PC;
				this.$disposing = true;
				this.$PC = -1;
				switch (num)
				{
				case 1u:
					try
					{
					}
					finally
					{
						((IDisposable)enumerator).Dispose();
					}
					break;
				}
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<RimWorld.DrugPolicy>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<DrugPolicy>> IEnumerable<Widgets.DropdownMenuElement<DrugPolicy>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				DrugPolicyUIUtility.<Button_GenerateMenu>c__Iterator0 <Button_GenerateMenu>c__Iterator = new DrugPolicyUIUtility.<Button_GenerateMenu>c__Iterator0();
				<Button_GenerateMenu>c__Iterator.pawn = pawn;
				return <Button_GenerateMenu>c__Iterator;
			}

			private sealed class <Button_GenerateMenu>c__AnonStorey2
			{
				internal Pawn pawn;

				public <Button_GenerateMenu>c__AnonStorey2()
				{
				}
			}

			private sealed class <Button_GenerateMenu>c__AnonStorey1
			{
				internal DrugPolicy assignedDrugs;

				internal DrugPolicyUIUtility.<Button_GenerateMenu>c__Iterator0.<Button_GenerateMenu>c__AnonStorey2 <>f__ref$2;

				public <Button_GenerateMenu>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$2.pawn.drugs.CurrentPolicy = this.assignedDrugs;
				}
			}
		}
	}
}
