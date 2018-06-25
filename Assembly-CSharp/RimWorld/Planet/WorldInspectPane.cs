using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020008EB RID: 2283
	public class WorldInspectPane : Window, IInspectPane
	{
		// Token: 0x04001C6A RID: 7274
		private static readonly WITab[] TileTabs = new WITab[]
		{
			new WITab_Terrain(),
			new WITab_Planet()
		};

		// Token: 0x04001C6B RID: 7275
		private Type openTabType;

		// Token: 0x04001C6C RID: 7276
		private float recentHeight;

		// Token: 0x04001C6D RID: 7277
		public Gizmo mouseoverGizmo;

		// Token: 0x04001C6E RID: 7278
		private static List<object> tmpObjectsList = new List<object>();

		// Token: 0x06003484 RID: 13444 RVA: 0x001C1620 File Offset: 0x001BFA20
		public WorldInspectPane()
		{
			this.layer = WindowLayer.GameUI;
			this.soundAppear = null;
			this.soundClose = null;
			this.closeOnClickedOutside = false;
			this.closeOnAccept = false;
			this.closeOnCancel = false;
			this.preventCameraMotion = false;
		}

		// Token: 0x17000871 RID: 2161
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x001C165C File Offset: 0x001BFA5C
		// (set) Token: 0x06003486 RID: 13446 RVA: 0x001C1677 File Offset: 0x001BFA77
		public Type OpenTabType
		{
			get
			{
				return this.openTabType;
			}
			set
			{
				this.openTabType = value;
			}
		}

		// Token: 0x17000872 RID: 2162
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x001C1684 File Offset: 0x001BFA84
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x001C169F File Offset: 0x001BFA9F
		public float RecentHeight
		{
			get
			{
				return this.recentHeight;
			}
			set
			{
				this.recentHeight = value;
			}
		}

		// Token: 0x17000873 RID: 2163
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x001C16AC File Offset: 0x001BFAAC
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x001C16C8 File Offset: 0x001BFAC8
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600348B RID: 13451 RVA: 0x001C16E4 File Offset: 0x001BFAE4
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x001C1704 File Offset: 0x001BFB04
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x001C1724 File Offset: 0x001BFB24
		public float PaneTopY
		{
			get
			{
				float num = (float)UI.screenHeight - 165f;
				if (Current.ProgramState == ProgramState.Playing)
				{
					num -= 35f;
				}
				return num;
			}
		}

		// Token: 0x17000878 RID: 2168
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x001C175C File Offset: 0x001BFB5C
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x001C177C File Offset: 0x001BFB7C
		private int SelectedTile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003490 RID: 13456 RVA: 0x001C179C File Offset: 0x001BFB9C
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile >= 0);
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x001C17DC File Offset: 0x001BFBDC
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06003492 RID: 13458 RVA: 0x001C17F8 File Offset: 0x001BFBF8
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003493 RID: 13459 RVA: 0x001C1814 File Offset: 0x001BFC14
		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				IEnumerable<InspectTabBase> result;
				if (this.NumSelectedObjects == 1)
				{
					result = Find.WorldSelector.SingleSelectedObject.GetInspectTabs();
				}
				else if (this.NumSelectedObjects == 0 && this.SelectedTile >= 0)
				{
					result = WorldInspectPane.TileTabs;
				}
				else
				{
					result = Enumerable.Empty<InspectTabBase>();
				}
				return result;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x001C1874 File Offset: 0x001BFC74
		private string TileInspectString
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				Vector2 vector = Find.WorldGrid.LongLatOf(this.SelectedTile);
				stringBuilder.Append(vector.y.ToStringLatitude());
				stringBuilder.Append(" ");
				stringBuilder.Append(vector.x.ToStringLongitude());
				Tile tile = Find.WorldGrid[this.SelectedTile];
				if (!tile.biome.impassable)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(tile.hilliness.GetLabelCap());
				}
				if (tile.Roads != null)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append((from rl in tile.Roads
					select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
				}
				if (!Find.World.Impassable(this.SelectedTile))
				{
					string str = (WorldPathGrid.CalculatedMovementDifficultyAt(this.SelectedTile, false, null, null) * Find.WorldGrid.GetRoadMovementDifficultyMultiplier(this.SelectedTile, -1, null)).ToString("0.#");
					stringBuilder.AppendLine();
					stringBuilder.Append("MovementDifficulty".Translate() + ": " + str);
				}
				stringBuilder.AppendLine();
				stringBuilder.Append("AvgTemp".Translate() + ": " + GenTemperature.GetAverageTemperatureLabel(this.SelectedTile));
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06003495 RID: 13461 RVA: 0x001C1A20 File Offset: 0x001BFE20
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x001C1A4C File Offset: 0x001BFE4C
		public void DrawInspectGizmos()
		{
			WorldInspectPane.tmpObjectsList.Clear();
			WorldRoutePlanner worldRoutePlanner = Find.WorldRoutePlanner;
			List<WorldObject> selected = this.Selected;
			for (int i = 0; i < selected.Count; i++)
			{
				if (!worldRoutePlanner.Active || selected[i] is RoutePlannerWaypoint)
				{
					WorldInspectPane.tmpObjectsList.Add(selected[i]);
				}
			}
			InspectGizmoGrid.DrawInspectGizmoGridFor(WorldInspectPane.tmpObjectsList, out this.mouseoverGizmo);
			WorldInspectPane.tmpObjectsList.Clear();
		}

		// Token: 0x06003497 RID: 13463 RVA: 0x001C1AD8 File Offset: 0x001BFED8
		public string GetLabel(Rect rect)
		{
			string result;
			if (this.NumSelectedObjects > 0)
			{
				result = WorldInspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
			}
			else if (this.SelectedTile >= 0)
			{
				result = Find.WorldGrid[this.SelectedTile].biome.LabelCap;
			}
			else
			{
				result = "error";
			}
			return result;
		}

		// Token: 0x06003498 RID: 13464 RVA: 0x001C1B3C File Offset: 0x001BFF3C
		public void SelectNextInCell()
		{
			if (this.AnythingSelected)
			{
				if (this.NumSelectedObjects > 0)
				{
					Find.WorldSelector.SelectFirstOrNextAt(this.Selected[0].Tile);
				}
				else
				{
					Find.WorldSelector.SelectFirstOrNextAt(this.SelectedTile);
				}
			}
		}

		// Token: 0x06003499 RID: 13465 RVA: 0x001C1B96 File Offset: 0x001BFF96
		public void DoPaneContents(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				InspectPaneFiller.DoPaneContentsFor(Find.WorldSelector.FirstSelectedObject, rect);
			}
			else if (this.SelectedTile >= 0)
			{
				InspectPaneFiller.DrawInspectString(this.TileInspectString, rect);
			}
		}

		// Token: 0x0600349A RID: 13466 RVA: 0x001C1BD4 File Offset: 0x001BFFD4
		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			if (singleSelectedObject != null || this.SelectedTile >= 0)
			{
				float x = rect.width - 48f;
				if (singleSelectedObject != null)
				{
					Widgets.InfoCardButton(x, 0f, singleSelectedObject);
				}
				else
				{
					Widgets.InfoCardButton(x, 0f, Find.WorldGrid[this.SelectedTile].biome);
				}
				lineEndWidth += 24f;
			}
		}

		// Token: 0x0600349B RID: 13467 RVA: 0x001C1C52 File Offset: 0x001C0052
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x001C1C5C File Offset: 0x001C005C
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x001C1C81 File Offset: 0x001C0081
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x001C1C90 File Offset: 0x001C0090
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x001C1C9A File Offset: 0x001C009A
		public void Reset()
		{
			this.openTabType = null;
		}
	}
}
