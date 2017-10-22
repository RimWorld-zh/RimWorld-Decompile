using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Dialog_ManageOutfits : Window
	{
		private const float TopAreaHeight = 40f;

		private const float TopButtonHeight = 35f;

		private const float TopButtonWidth = 150f;

		private Vector2 scrollPosition;

		private Outfit selOutfitInt;

		private static ThingFilter apparelGlobalFilter;

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

		public Dialog_ManageOutfits(Outfit selectedOutfit)
		{
			base.forcePause = true;
			base.doCloseX = true;
			base.closeOnEscapeKey = true;
			base.doCloseButton = true;
			base.closeOnClickedOutside = true;
			base.absorbInputAroundWindow = true;
			if (Dialog_ManageOutfits.apparelGlobalFilter == null)
			{
				Dialog_ManageOutfits.apparelGlobalFilter = new ThingFilter();
				Dialog_ManageOutfits.apparelGlobalFilter.SetAllow(ThingCategoryDefOf.Apparel, true, null, null);
			}
			this.SelectedOutfit = selectedOutfit;
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
			num = (float)(num + 150.0);
			if (Widgets.ButtonText(rect, "SelectOutfit".Translate(), true, false, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				List<Outfit>.Enumerator enumerator = Current.Game.outfitDatabase.AllOutfits.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						Outfit current = enumerator.Current;
						Outfit localOut = current;
						list.Add(new FloatMenuOption(localOut.label, (Action)delegate
						{
							this.SelectedOutfit = localOut;
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator).Dispose();
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
			num = (float)(num + 10.0);
			Rect rect2 = new Rect(num, 0f, 150f, 35f);
			num = (float)(num + 150.0);
			if (Widgets.ButtonText(rect2, "NewOutfit".Translate(), true, false, true))
			{
				this.SelectedOutfit = Current.Game.outfitDatabase.MakeNewOutfit();
			}
			num = (float)(num + 10.0);
			Rect rect3 = new Rect(num, 0f, 150f, 35f);
			num = (float)(num + 150.0);
			if (Widgets.ButtonText(rect3, "DeleteOutfit".Translate(), true, false, true))
			{
				List<FloatMenuOption> list2 = new List<FloatMenuOption>();
				List<Outfit>.Enumerator enumerator2 = Current.Game.outfitDatabase.AllOutfits.GetEnumerator();
				try
				{
					while (enumerator2.MoveNext())
					{
						Outfit current2 = enumerator2.Current;
						Outfit localOut2 = current2;
						list2.Add(new FloatMenuOption(localOut2.label, (Action)delegate
						{
							AcceptanceReport acceptanceReport = Current.Game.outfitDatabase.TryDelete(localOut2);
							if (!acceptanceReport.Accepted)
							{
								Messages.Message(acceptanceReport.Reason, MessageSound.RejectInput);
							}
							else if (localOut2 == this.SelectedOutfit)
							{
								this.SelectedOutfit = null;
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null));
					}
				}
				finally
				{
					((IDisposable)(object)enumerator2).Dispose();
				}
				Find.WindowStack.Add(new FloatMenu(list2));
			}
			float width = inRect.width;
			double num2 = inRect.height - 40.0;
			Vector2 closeButSize = base.CloseButSize;
			Rect rect4 = new Rect(0f, 40f, width, (float)(num2 - closeButSize.y)).ContractedBy(10f);
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
				Rect rect6 = new Rect(0f, 40f, 300f, (float)(rect4.height - 45.0 - 10.0));
				IEnumerable<SpecialThingFilterDef> forceHiddenFilters = this.HiddenSpecialThingFilters();
				ThingFilterUI.DoThingFilterConfigWindow(rect6, ref this.scrollPosition, this.SelectedOutfit.filter, Dialog_ManageOutfits.apparelGlobalFilter, 16, (IEnumerable<ThingDef>)null, forceHiddenFilters, (List<ThingDef>)null);
				GUI.EndGroup();
			}
		}

		private IEnumerable<SpecialThingFilterDef> HiddenSpecialThingFilters()
		{
			yield return SpecialThingFilterDefOf.AllowNonDeadmansApparel;
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
	}
}
