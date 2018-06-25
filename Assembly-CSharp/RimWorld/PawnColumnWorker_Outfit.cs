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
	public class PawnColumnWorker_Outfit : PawnColumnWorker
	{
		private const int TopAreaHeight = 65;

		private const int ManageOutfitsButtonHeight = 32;

		[CompilerGenerated]
		private static Func<Pawn, Outfit> <>f__am$cache0;

		public PawnColumnWorker_Outfit()
		{
		}

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
			return (pawn.outfits != null && pawn.outfits.CurrentOutfit != null) ? pawn.outfits.CurrentOutfit.uniqueId : int.MinValue;
		}

		[CompilerGenerated]
		private static Outfit <DoCell>m__0(Pawn p)
		{
			return p.outfits.CurrentOutfit;
		}

		[CompilerGenerated]
		private sealed class <DoCell>c__AnonStorey1
		{
			internal Pawn pawn;

			public <DoCell>c__AnonStorey1()
			{
			}

			internal string <>m__0()
			{
				string text = "ForcedApparel".Translate() + ":\n";
				foreach (Apparel apparel in this.pawn.outfits.forcedHandler.ForcedApparel)
				{
					text = text + "\n   " + apparel.LabelCap;
				}
				return text;
			}
		}

		[CompilerGenerated]
		private sealed class <Button_GenerateMenu>c__Iterator0 : IEnumerable, IEnumerable<Widgets.DropdownMenuElement<Outfit>>, IEnumerator, IDisposable, IEnumerator<Widgets.DropdownMenuElement<Outfit>>
		{
			internal List<Outfit>.Enumerator $locvar0;

			internal Pawn pawn;

			internal Widgets.DropdownMenuElement<Outfit> $current;

			internal bool $disposing;

			internal int $PC;

			private PawnColumnWorker_Outfit.<Button_GenerateMenu>c__Iterator0.<Button_GenerateMenu>c__AnonStorey3 $locvar1;

			private PawnColumnWorker_Outfit.<Button_GenerateMenu>c__Iterator0.<Button_GenerateMenu>c__AnonStorey2 $locvar2;

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
					enumerator = Current.Game.outfitDatabase.AllOutfits.GetEnumerator();
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
						Outfit outfit = enumerator.Current;
						this.$current = new Widgets.DropdownMenuElement<Outfit>
						{
							option = new FloatMenuOption(outfit.label, delegate()
							{
								<Button_GenerateMenu>c__AnonStorey.pawn.outfits.CurrentOutfit = outfit;
							}, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = outfit
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

			Widgets.DropdownMenuElement<Outfit> IEnumerator<Widgets.DropdownMenuElement<Outfit>>.Current
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
				return this.System.Collections.Generic.IEnumerable<Verse.Widgets.DropdownMenuElement<RimWorld.Outfit>>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Widgets.DropdownMenuElement<Outfit>> IEnumerable<Widgets.DropdownMenuElement<Outfit>>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				PawnColumnWorker_Outfit.<Button_GenerateMenu>c__Iterator0 <Button_GenerateMenu>c__Iterator = new PawnColumnWorker_Outfit.<Button_GenerateMenu>c__Iterator0();
				<Button_GenerateMenu>c__Iterator.pawn = pawn;
				return <Button_GenerateMenu>c__Iterator;
			}

			private sealed class <Button_GenerateMenu>c__AnonStorey3
			{
				internal Pawn pawn;

				public <Button_GenerateMenu>c__AnonStorey3()
				{
				}
			}

			private sealed class <Button_GenerateMenu>c__AnonStorey2
			{
				internal Outfit outfit;

				internal PawnColumnWorker_Outfit.<Button_GenerateMenu>c__Iterator0.<Button_GenerateMenu>c__AnonStorey3 <>f__ref$3;

				public <Button_GenerateMenu>c__AnonStorey2()
				{
				}

				internal void <>m__0()
				{
					this.<>f__ref$3.pawn.outfits.CurrentOutfit = this.outfit;
				}
			}
		}
	}
}
