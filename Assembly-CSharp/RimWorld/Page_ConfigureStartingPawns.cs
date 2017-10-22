using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class Page_ConfigureStartingPawns : Page
	{
		private const float TabAreaHeight = 30f;

		private const float RectAreaWidth = 100f;

		private const float RightRectLeftPadding = 5f;

		private Pawn curPawn;

		private static readonly Vector2 PawnPortraitSize = new Vector2(100f, 140f);

		public override string PageTitle
		{
			get
			{
				return "CreateCharacters".Translate();
			}
		}

		public override void PreOpen()
		{
			base.PreOpen();
			if (Find.GameInitData.startingPawns.Count > 0)
			{
				this.curPawn = Find.GameInitData.startingPawns[0];
			}
		}

		public override void PostOpen()
		{
			base.PostOpen();
			TutorSystem.Notify_Event("PageStart-ConfigureStartingPawns");
		}

		public override void DoWindowContents(Rect rect)
		{
			base.DrawPageTitle(rect);
			Rect mainRect = base.GetMainRect(rect, 30f, false);
			Widgets.DrawMenuSection(mainRect, true);
			TabDrawer.DrawTabs(mainRect, from c in Find.GameInitData.startingPawns
			select new TabRecord(c.LabelCap, (Action)delegate()
			{
				this.SelectPawn(c);
			}, c == this.curPawn));
			Rect rect2;
			Rect rect3 = rect2 = mainRect.ContractedBy(17f);
			rect2.width = 100f;
			float xMin = rect2.xMin;
			float width = rect2.width;
			Vector2 pawnPortraitSize = Page_ConfigureStartingPawns.PawnPortraitSize;
			double x = xMin + (width - pawnPortraitSize.x) / 2.0 - 10.0;
			double y = rect2.yMin + 20.0;
			Vector2 pawnPortraitSize2 = Page_ConfigureStartingPawns.PawnPortraitSize;
			float x2 = pawnPortraitSize2.x;
			Vector2 pawnPortraitSize3 = Page_ConfigureStartingPawns.PawnPortraitSize;
			GUI.DrawTexture(new Rect((float)x, (float)y, x2, pawnPortraitSize3.y), PortraitsCache.Get(this.curPawn, Page_ConfigureStartingPawns.PawnPortraitSize, default(Vector3), 1f));
			Rect rect4 = rect3;
			rect4.xMin = rect2.xMax;
			Rect rect5 = rect4;
			rect5.width = 475f;
			CharacterCardUtility.DrawCharacterCard(rect5, this.curPawn, new Action(this.RandomizeCurPawn));
			Rect rect6 = new Rect((float)(rect5.xMax + 5.0), (float)(rect4.y + 100.0), (float)(rect4.width - rect5.width - 5.0), 200f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect6, "Health".Translate());
			Text.Font = GameFont.Small;
			rect6.yMin += 35f;
			HealthCardUtility.DrawHediffListing(rect6, this.curPawn, true);
			Rect rect7 = new Rect(rect6.x, rect6.yMax, rect6.width, 200f);
			Text.Font = GameFont.Medium;
			Widgets.Label(rect7, "Relations".Translate());
			Text.Font = GameFont.Small;
			rect7.yMin += 35f;
			SocialCardUtility.DrawRelationsAndOpinions(rect7, this.curPawn);
			base.DoBottomButtons(rect, "Start".Translate(), (string)null, null, true);
		}

		private void RandomizeCurPawn()
		{
			if (TutorSystem.AllowAction("RandomizePawn"))
			{
				int num = 0;
				while (true)
				{
					this.curPawn = StartingPawnUtility.RandomizeInPlace(this.curPawn);
					num++;
					if (num <= 15)
					{
						if (StartingPawnUtility.WorkTypeRequirementsSatisfied())
							break;
						continue;
					}
					return;
				}
				TutorSystem.Notify_Event("RandomizePawn");
			}
		}

		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			List<Pawn>.Enumerator enumerator = Find.GameInitData.startingPawns.GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					Pawn current = enumerator.Current;
					if (!current.Name.IsValid)
					{
						Messages.Message("EveryoneNeedsValidName".Translate(), MessageSound.RejectInput);
						return false;
					}
				}
			}
			finally
			{
				((IDisposable)(object)enumerator).Dispose();
			}
			PortraitsCache.Clear();
			return true;
		}

		public void SelectPawn(Pawn c)
		{
			if (c != this.curPawn)
			{
				this.curPawn = c;
			}
		}
	}
}
