using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000804 RID: 2052
	public class Dialog_ManageOutfits : Window
	{
		// Token: 0x06002DCA RID: 11722 RVA: 0x001816F8 File Offset: 0x0017FAF8
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

		// Token: 0x1700074F RID: 1871
		// (get) Token: 0x06002DCB RID: 11723 RVA: 0x00181768 File Offset: 0x0017FB68
		// (set) Token: 0x06002DCC RID: 11724 RVA: 0x00181783 File Offset: 0x0017FB83
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

		// Token: 0x17000750 RID: 1872
		// (get) Token: 0x06002DCD RID: 11725 RVA: 0x00181794 File Offset: 0x0017FB94
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(700f, 700f);
			}
		}

		// Token: 0x06002DCE RID: 11726 RVA: 0x001817B8 File Offset: 0x0017FBB8
		private void CheckSelectedOutfitHasName()
		{
			if (this.SelectedOutfit != null && this.SelectedOutfit.label.NullOrEmpty())
			{
				this.SelectedOutfit.label = "Unnamed";
			}
		}

		// Token: 0x06002DCF RID: 11727 RVA: 0x001817EC File Offset: 0x0017FBEC
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
			}
			else
			{
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
		}

		// Token: 0x06002DD0 RID: 11728 RVA: 0x00181B5C File Offset: 0x0017FF5C
		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
			yield break;
		}

		// Token: 0x06002DD1 RID: 11729 RVA: 0x00181B7F File Offset: 0x0017FF7F
		public override void PreClose()
		{
			base.PreClose();
			this.CheckSelectedOutfitHasName();
		}

		// Token: 0x06002DD2 RID: 11730 RVA: 0x00181B8E File Offset: 0x0017FF8E
		public static void DoNameInputRect(Rect rect, ref string name)
		{
			name = Widgets.TextField(rect, name, 30, Outfit.ValidNameRegex);
		}

		// Token: 0x04001848 RID: 6216
		private Vector2 scrollPosition;

		// Token: 0x04001849 RID: 6217
		private Outfit selOutfitInt = null;

		// Token: 0x0400184A RID: 6218
		private const float TopAreaHeight = 40f;

		// Token: 0x0400184B RID: 6219
		private const float TopButtonHeight = 35f;

		// Token: 0x0400184C RID: 6220
		private const float TopButtonWidth = 150f;

		// Token: 0x0400184D RID: 6221
		private static ThingFilter apparelGlobalFilter;
	}
}
