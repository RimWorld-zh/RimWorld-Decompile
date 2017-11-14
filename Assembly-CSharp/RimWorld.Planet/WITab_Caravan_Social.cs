using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	public class WITab_Caravan_Social : WITab
	{
		private Vector2 scrollPosition;

		private float scrollViewHeight;

		private Pawn specificSocialTabForPawn;

		private const float RowHeight = 50f;

		private const float PawnLabelHeight = 18f;

		private const float PawnLabelColumnWidth = 100f;

		private const float SpaceAroundIcon = 4f;

		private List<Pawn> Pawns
		{
			get
			{
				return base.SelCaravan.PawnsListForReading;
			}
		}

		private float SpecificSocialTabWidth
		{
			get
			{
				if (this.specificSocialTabForPawn == null)
				{
					return 0f;
				}
				return 540f;
			}
		}

		public WITab_Caravan_Social()
		{
			base.labelKey = "TabCaravanSocial";
		}

		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 0f, base.size.x, base.size.y).ContractedBy(10f);
			Rect rect2 = new Rect(0f, 0f, (float)(rect.width - 16.0), this.scrollViewHeight);
			float num = 0f;
			Widgets.BeginScrollView(rect, ref this.scrollPosition, rect2, true);
			this.DoRows(ref num, rect2, rect);
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = (float)(num + 30.0);
			}
			Widgets.EndScrollView();
		}

		protected override void UpdateSize()
		{
			base.UpdateSize();
			base.size.x = 243f;
			base.size.y = Mathf.Min(550f, (float)(this.PaneTopY - 30.0));
		}

		protected override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			Pawn localSpecificSocialTabForPawn = this.specificSocialTabForPawn;
			if (localSpecificSocialTabForPawn != null)
			{
				Rect tabRect = base.TabRect;
				float specificSocialTabWidth = this.SpecificSocialTabWidth;
				Rect rect = new Rect((float)(tabRect.xMax - 1.0), tabRect.yMin, specificSocialTabWidth, tabRect.height);
				Find.WindowStack.ImmediateWindow(1439870015, rect, WindowLayer.GameUI, delegate
				{
					if (!localSpecificSocialTabForPawn.DestroyedOrNull())
					{
						SocialCardUtility.DrawSocialCard(rect.AtZero(), localSpecificSocialTabForPawn);
						if (Widgets.CloseButtonFor(rect.AtZero()))
						{
							this.specificSocialTabForPawn = null;
							SoundDefOf.TabClose.PlayOneShotOnCamera(null);
						}
					}
				}, true, false, 1f);
			}
		}

		public override void OnOpen()
		{
			base.OnOpen();
			if (this.specificSocialTabForPawn != null && this.Pawns.Contains(this.specificSocialTabForPawn))
				return;
			if (this.Pawns.Any())
			{
				this.specificSocialTabForPawn = this.Pawns[0];
			}
		}

		private void DoRows(ref float curY, Rect scrollViewRect, Rect scrollOutRect)
		{
			List<Pawn> pawns = this.Pawns;
			Pawn pawn = BestCaravanPawnUtility.FindBestNegotiator(base.SelCaravan);
			GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
			Widgets.Label(new Rect(0f, curY, scrollViewRect.width, 24f), string.Format("{0}: {1}", "Negotiator".Translate(), (pawn == null) ? "NoneCapable".Translate() : pawn.NameStringShort));
			curY += 24f;
			if (this.specificSocialTabForPawn != null && !pawns.Contains(this.specificSocialTabForPawn))
			{
				this.specificSocialTabForPawn = null;
			}
			bool flag = false;
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn2 = pawns[i];
				if (pawn2.RaceProps.IsFlesh && pawn2.IsColonist)
				{
					if (!flag)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanColonists".Translate());
						flag = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn2);
				}
			}
			bool flag2 = false;
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn3 = pawns[j];
				if (pawn3.RaceProps.IsFlesh && !pawn3.IsColonist)
				{
					if (!flag2)
					{
						Widgets.ListSeparator(ref curY, scrollViewRect.width, "CaravanPrisonersAndAnimals".Translate());
						flag2 = true;
					}
					this.DoRow(ref curY, scrollViewRect, scrollOutRect, pawn3);
				}
			}
		}

		private void DoRow(ref float curY, Rect viewRect, Rect scrollOutRect, Pawn p)
		{
			float num = (float)(this.scrollPosition.y - 50.0);
			float num2 = this.scrollPosition.y + scrollOutRect.height;
			if (curY > num && curY < num2)
			{
				this.DoRow(new Rect(0f, curY, viewRect.width, 50f), p);
			}
			curY += 50f;
		}

		private void DoRow(Rect rect, Pawn p)
		{
			GUI.BeginGroup(rect);
			Rect rect2 = rect.AtZero();
			CaravanPeopleAndItemsTabUtility.DoAbandonButton(rect2, p, base.SelCaravan);
			rect2.width -= 24f;
			Widgets.InfoCardButton((float)(rect2.width - 24.0), (float)((rect.height - 24.0) / 2.0), p);
			rect2.width -= 24f;
			CaravanPeopleAndItemsTabUtility.DoOpenSpecificTabButton(rect2, p, ref this.specificSocialTabForPawn);
			rect2.width -= 24f;
			if (Mouse.IsOver(rect2))
			{
				Widgets.DrawHighlight(rect2);
			}
			Rect rect3 = new Rect(4f, (float)((rect.height - 27.0) / 2.0), 27f, 27f);
			Widgets.ThingIcon(rect3, p, 1f);
			Rect bgRect = new Rect((float)(rect3.xMax + 4.0), 16f, 100f, 18f);
			GenMapUI.DrawPawnLabel(p, bgRect, 1f, 100f, null, GameFont.Small, false, false);
			if (p.Downed)
			{
				GUI.color = new Color(1f, 0f, 0f, 0.5f);
				Widgets.DrawLineHorizontal(0f, (float)(rect.height / 2.0), rect.width);
				GUI.color = Color.white;
			}
			GUI.EndGroup();
		}
	}
}
