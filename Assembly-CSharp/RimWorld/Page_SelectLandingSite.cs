using RimWorld.Planet;
using System;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	public class Page_SelectLandingSite : Page
	{
		private const float GapBetweenBottomButtons = 10f;

		private const float UseTwoRowsIfScreenWidthBelow = 1340f;

		public override string PageTitle
		{
			get
			{
				return "SelectLandingSite".Translate();
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return Vector2.zero;
			}
		}

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		public Page_SelectLandingSite()
		{
			base.absorbInputAroundWindow = false;
			base.shadowAlpha = 0f;
			base.preventCameraMotion = false;
		}

		public override void PreOpen()
		{
			base.PreOpen();
			Find.World.renderer.wantedMode = WorldRenderMode.Planet;
			Find.WorldInterface.Reset();
			((MainButtonWorker_ToggleWorld)MainButtonDefOf.World.Worker).resetViewNextTime = true;
		}

		public override void PostOpen()
		{
			base.PostOpen();
			Find.GameInitData.ChooseRandomStartingTile();
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.WorldCameraMovement, OpportunityType.Important);
			TutorSystem.Notify_Event("PageStart-SelectLandingSite");
		}

		public override void PostClose()
		{
			base.PostClose();
			Find.World.renderer.wantedMode = WorldRenderMode.None;
		}

		public override void DoWindowContents(Rect rect)
		{
			if (Find.WorldInterface.SelectedTile >= 0)
			{
				Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;
			}
			else if (Find.WorldSelector.FirstSelectedObject != null)
			{
				Find.GameInitData.startingTile = Find.WorldSelector.FirstSelectedObject.Tile;
			}
			base.closeOnEscapeKey = !Find.WorldRoutePlanner.Active;
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			Text.Anchor = TextAnchor.UpperCenter;
			base.DrawPageTitle(new Rect(0f, 5f, (float)UI.screenWidth, 300f));
			Text.Anchor = TextAnchor.UpperLeft;
			this.DoCustomBottomButtons();
		}

		protected override bool CanDoNext()
		{
			if (!base.CanDoNext())
			{
				return false;
			}
			int selectedTile = Find.WorldInterface.SelectedTile;
			if (selectedTile < 0)
			{
				Messages.Message("MustSelectLandingSite".Translate(), MessageSound.RejectInput);
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (!TileFinder.IsValidTileForNewSettlement(selectedTile, stringBuilder))
			{
				Messages.Message(stringBuilder.ToString(), MessageSound.RejectInput);
				return false;
			}
			Tile tile = Find.WorldGrid[selectedTile];
			if (!TutorSystem.AllowAction("ChooseBiome-" + tile.biome.defName + "-" + ((Enum)(object)tile.hilliness).ToString()))
			{
				return false;
			}
			return true;
		}

		protected override void DoNext()
		{
			Find.GameInitData.startingTile = Find.WorldInterface.SelectedTile;
			base.DoNext();
		}

		private void DoCustomBottomButtons()
		{
			int num = (!TutorSystem.TutorialMode) ? 5 : 4;
			int num2 = (num < 4 || !((float)UI.screenWidth < 1340.0)) ? 1 : 2;
			int num3 = Mathf.CeilToInt((float)num / (float)num2);
			Vector2 bottomButSize = Page.BottomButSize;
			float num4 = (float)(bottomButSize.x * (float)num3 + 10.0 * (float)(num3 + 1));
			float num5 = (float)num2;
			Vector2 bottomButSize2 = Page.BottomButSize;
			float num6 = (float)(num5 * bottomButSize2.y + 10.0 * (float)(num2 + 1));
			Rect rect = new Rect((float)(((float)UI.screenWidth - num4) / 2.0), (float)((float)UI.screenHeight - num6 - 4.0), num4, num6);
			if (Find.WindowStack.IsOpen<WorldInspectPane>())
			{
				float x = rect.x;
				Vector2 paneSize = InspectPaneUtility.PaneSize;
				if (x < paneSize.x + 4.0)
				{
					Vector2 paneSize2 = InspectPaneUtility.PaneSize;
					rect.x = (float)(paneSize2.x + 4.0);
				}
			}
			Widgets.DrawWindowBackground(rect);
			float num7 = (float)(rect.xMin + 10.0);
			float num8 = (float)(rect.yMin + 10.0);
			Text.Font = GameFont.Small;
			float x2 = num7;
			float y = num8;
			Vector2 bottomButSize3 = Page.BottomButSize;
			float x3 = bottomButSize3.x;
			Vector2 bottomButSize4 = Page.BottomButSize;
			if (Widgets.ButtonText(new Rect(x2, y, x3, bottomButSize4.y), "Back".Translate(), true, false, true) && this.CanDoBack())
			{
				this.DoBack();
			}
			float num9 = num7;
			Vector2 bottomButSize5 = Page.BottomButSize;
			num7 = (float)(num9 + (bottomButSize5.x + 10.0));
			if (!TutorSystem.TutorialMode)
			{
				float x4 = num7;
				float y2 = num8;
				Vector2 bottomButSize6 = Page.BottomButSize;
				float x5 = bottomButSize6.x;
				Vector2 bottomButSize7 = Page.BottomButSize;
				if (Widgets.ButtonText(new Rect(x4, y2, x5, bottomButSize7.y), "Advanced".Translate(), true, false, true))
				{
					Find.WindowStack.Add(new Dialog_AdvancedGameConfig(Find.WorldInterface.SelectedTile));
				}
				float num10 = num7;
				Vector2 bottomButSize8 = Page.BottomButSize;
				num7 = (float)(num10 + (bottomButSize8.x + 10.0));
			}
			float x6 = num7;
			float y3 = num8;
			Vector2 bottomButSize9 = Page.BottomButSize;
			float x7 = bottomButSize9.x;
			Vector2 bottomButSize10 = Page.BottomButSize;
			if (Widgets.ButtonText(new Rect(x6, y3, x7, bottomButSize10.y), "SelectRandomSite".Translate(), true, false, true))
			{
				SoundDefOf.Click.PlayOneShotOnCamera(null);
				Find.WorldInterface.SelectedTile = TileFinder.RandomStartingTile();
				Find.WorldCameraDriver.JumpTo(Find.WorldGrid.GetTileCenter(Find.WorldInterface.SelectedTile));
			}
			float num11 = num7;
			Vector2 bottomButSize11 = Page.BottomButSize;
			num7 = (float)(num11 + (bottomButSize11.x + 10.0));
			if (num2 == 2)
			{
				num7 = (float)(rect.xMin + 10.0);
				float num12 = num8;
				Vector2 bottomButSize12 = Page.BottomButSize;
				num8 = (float)(num12 + (bottomButSize12.y + 10.0));
			}
			float x8 = num7;
			float y4 = num8;
			Vector2 bottomButSize13 = Page.BottomButSize;
			float x9 = bottomButSize13.x;
			Vector2 bottomButSize14 = Page.BottomButSize;
			if (Widgets.ButtonText(new Rect(x8, y4, x9, bottomButSize14.y), "WorldFactionsTab".Translate(), true, false, true))
			{
				Find.WindowStack.Add(new Dialog_FactionDuringLanding());
			}
			float num13 = num7;
			Vector2 bottomButSize15 = Page.BottomButSize;
			num7 = (float)(num13 + (bottomButSize15.x + 10.0));
			float x10 = num7;
			float y5 = num8;
			Vector2 bottomButSize16 = Page.BottomButSize;
			float x11 = bottomButSize16.x;
			Vector2 bottomButSize17 = Page.BottomButSize;
			if (Widgets.ButtonText(new Rect(x10, y5, x11, bottomButSize17.y), "Next".Translate(), true, false, true) && this.CanDoNext())
			{
				this.DoNext();
			}
			float num14 = num7;
			Vector2 bottomButSize18 = Page.BottomButSize;
			num7 = (float)(num14 + (bottomButSize18.x + 10.0));
			GenUI.AbsorbClicksInRect(rect);
		}
	}
}
