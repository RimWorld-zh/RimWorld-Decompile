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
	public class Dialog_ManageOutfits : Window
	{
		private Vector2 scrollPosition;

		private Outfit selOutfitInt;

		private const float TopAreaHeight = 40f;

		private const float TopButtonHeight = 35f;

		private const float TopButtonWidth = 150f;

		private static ThingFilter apparelGlobalFilter;

		public Dialog_ManageOutfits(Outfit selectedOutfit)
		{
			this.forcePause = true;
			this.doCloseX = true;
			this.doCloseButton = true;
			this.closeOnClickedOutside = true;
			this.absorbInputAroundWindow = true;
			if (Dialog_ManageOutfits.apparelGlobalFilter == null)
			{
				Dialog_ManageOutfits.apparelGlobalFilter = new ThingFilter();
				Dialog_ManageOutfits.apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			}
			this.SelectedOutfit = selectedOutfit;
		}

		private Outfit SelectedOutfit
		{
			get
			{
				return this.selOutfitInt;
			}
			set
			{
				this.CheckSelectedOutfitHasName();
				this.selOutfitInt = value;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		private void CheckSelectedOutfitHasName()
		{
			if (this.SelectedOutfit != null && this.SelectedOutfit.label.NullOrEmpty())
			{
				this.SelectedOutfit.label = "Unnamed";
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			float num = 0f;
			Rect rect = new Rect(0f, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect, "SelectOutfit".Translate(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (Outfit localOut3 in Current.Game.outfitDatabase.AllOutfits)
				{
					Outfit localOut = localOut3;
					list.Add(new FloatMenuOption(localOut.label, delegate()
					{
						this.SelectedOutfit = localOut;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num += 10f;
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect2, "NewOutfit".Translate(), true, false, true))
			{
				this.SelectedOutfit = Current.Game.outfitDatabase.MakeNewOutfit();
			}
			num += 10f;
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num += 150f;
			if (Widgets.ButtonText(rect3, "DeleteOutfit".Translate(), true, false, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				foreach (Outfit localOut2 in Current.Game.outfitDatabase.AllOutfits)
				{
					Outfit localOut = localOut2;
					list2.Add(new FloatMenuOption(localOut.label, delegate()
					{
						AcceptanceReport acceptanceReport = Current.Game.outfitDatabase.TryDelete(localOut);
						if (!acceptanceReport.Accepted)
						{
							Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
						}
						else if (localOut == this.SelectedOutfit)
						{
							this.SelectedOutfit = null;
						}
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			Rect rect4 = new Rect(0f, 40f, inRect.width, inRect.height - 40f - this.CloseButSize.y).ContractedBy(10f);
			if (this.SelectedOutfit == null)
			{
				GUI.color = Color.grey;
				Text.Anchor = TextAnchor.MiddleCenter;
				Widgets.Label(rect4, "NoOutfitSelected".Translate());
				Text.Anchor = TextAnchor.UpperLeft;
				GUI.color = Color.white;
				return;
			}
			GUI.BeginGroup(rect4);
			Rect rect5 = new Rect(0f, 0f, 200f, 30f);
			Dialog_ManageOutfits.DoNameInputRect(rect5, ref this.SelectedOutfit.label);
			Rect rect6 = new Rect(0f, 40f, 300f, rect4.height - 45f - 10f);
			Rect rect7 = rect6;
			ref Vector2 ptr = ref this.scrollPosition;
			ThingFilter filter = this.SelectedOutfit.filter;
			ThingFilter parentFilter = Dialog_ManageOutfits.apparelGlobalFilter;
			int openMask = 16;
			IEnumerable<SpecialThingFilterDef> forceHiddenFilters = this.HiddenSpecialThingFilters();
			ThingFilterUI.DoThingFilterConfigWindow(rect7, ref ptr, filter, parentFilter, openMask, null, forceHiddenFilters, null, null);
			GUI.EndGroup();
		}

		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
			yield break;
		}

		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedOutfitHasName();
		}

		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		[CompilerGenerated]
		private sealed class <DoWindowContents>c__AnonStorey1
		{
			internal Outfit localOut;

			internal Dialog_ManageOutfits $this;

			public <DoWindowContents>c__AnonStorey1()
			{
			}

			internal void <>m__0()
			{
				this.$this.SelectedOutfit = this.localOut;
			}
		}

		[CompilerGenerated]
		private sealed class <DoWindowContents>c__AnonStorey2
		{
			internal Outfit localOut;

			internal Dialog_ManageOutfits $this;

			public <DoWindowContents>c__AnonStorey2()
			{
			}

			internal void <>m__0()
			{
				AcceptanceReport acceptanceReport = Current.Game.outfitDatabase.TryDelete(this.localOut);
				if (!acceptanceReport.Accepted)
				{
					Messages.Message(acceptanceReport.Reason, MessageTypeDefOf.RejectInput, false);
				}
				else if (this.localOut == this.$this.SelectedOutfit)
				{
					this.$this.SelectedOutfit = null;
				}
			}
		}

		[CompilerGenerated]
		private sealed class <HiddenSpecialThingFilters>c__Iterator0 : IEnumerable, IEnumerable<SpecialThingFilterDef>, IEnumerator, IDisposable, IEnumerator<SpecialThingFilterDef>
		{
			internal SpecialThingFilterDef $current;

			internal bool $disposing;

			internal int $PC;

			[DebuggerHidden]
			public <HiddenSpecialThingFilters>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					this.$current = SpecialThingFilterDefOf.AllowNonDeadmansApparel;
					if (!this.$disposing)
					{
						this.$PC = 1;
					}
					return true;
				case 1u:
					this.$PC = -1;
					break;
				}
				return false;
			}

			SpecialThingFilterDef IEnumerator<SpecialThingFilterDef>.Current
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
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.SpecialThingFilterDef>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<SpecialThingFilterDef> IEnumerable<SpecialThingFilterDef>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				return new Dialog_ManageOutfits.<HiddenSpecialThingFilters>c__Iterator0();
			}
		}
	}
}
