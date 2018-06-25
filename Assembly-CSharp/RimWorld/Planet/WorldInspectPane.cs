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
		// Token: 0x04001C70 RID: 7280
		private static readonly WITab[] TileTabs = new WITab[]
		{
			new WITab_Terrain(),
			new WITab_Planet()
		};

		// Token: 0x04001C71 RID: 7281
		private Type openTabType;

		// Token: 0x04001C72 RID: 7282
		private float recentHeight;

		// Token: 0x04001C73 RID: 7283
		public Gizmo mouseoverGizmo;

		// Token: 0x04001C74 RID: 7284
		private static List<object> tmpObjectsList = new List<object>();

		// Token: 0x06003484 RID: 13444 RVA: 0x001C18F4 File Offset: 0x001BFCF4
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
		// (get) Token: 0x06003485 RID: 13445 RVA: 0x001C1930 File Offset: 0x001BFD30
		// (set) Token: 0x06003486 RID: 13446 RVA: 0x001C194B File Offset: 0x001BFD4B
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
		// (get) Token: 0x06003487 RID: 13447 RVA: 0x001C1958 File Offset: 0x001BFD58
		// (set) Token: 0x06003488 RID: 13448 RVA: 0x001C1973 File Offset: 0x001BFD73
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
		// (get) Token: 0x06003489 RID: 13449 RVA: 0x001C1980 File Offset: 0x001BFD80
		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x17000874 RID: 2164
		// (get) Token: 0x0600348A RID: 13450 RVA: 0x001C199C File Offset: 0x001BFD9C
		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		// Token: 0x17000875 RID: 2165
		// (get) Token: 0x0600348B RID: 13451 RVA: 0x001C19B8 File Offset: 0x001BFDB8
		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		// Token: 0x17000876 RID: 2166
		// (get) Token: 0x0600348C RID: 13452 RVA: 0x001C19D8 File Offset: 0x001BFDD8
		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		// Token: 0x17000877 RID: 2167
		// (get) Token: 0x0600348D RID: 13453 RVA: 0x001C19F8 File Offset: 0x001BFDF8
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
		// (get) Token: 0x0600348E RID: 13454 RVA: 0x001C1A30 File Offset: 0x001BFE30
		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		// Token: 0x17000879 RID: 2169
		// (get) Token: 0x0600348F RID: 13455 RVA: 0x001C1A50 File Offset: 0x001BFE50
		private int SelectedTile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		// Token: 0x1700087A RID: 2170
		// (get) Token: 0x06003490 RID: 13456 RVA: 0x001C1A70 File Offset: 0x001BFE70
		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile >= 0);
			}
		}

		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06003491 RID: 13457 RVA: 0x001C1AB0 File Offset: 0x001BFEB0
		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06003492 RID: 13458 RVA: 0x001C1ACC File Offset: 0x001BFECC
		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06003493 RID: 13459 RVA: 0x001C1AE8 File Offset: 0x001BFEE8
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
		// (get) Token: 0x06003494 RID: 13460 RVA: 0x001C1B48 File Offset: 0x001BFF48
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

		// Token: 0x06003495 RID: 13461 RVA: 0x001C1CF4 File Offset: 0x001C00F4
		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			this.windowRect.x = 0f;
			this.windowRect.y = this.PaneTopY;
		}

		// Token: 0x06003496 RID: 13462 RVA: 0x001C1D20 File Offset: 0x001C0120
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

		// Token: 0x06003497 RID: 13463 RVA: 0x001C1DAC File Offset: 0x001C01AC
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

		// Token: 0x06003498 RID: 13464 RVA: 0x001C1E10 File Offset: 0x001C0210
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

		// Token: 0x06003499 RID: 13465 RVA: 0x001C1E6A File Offset: 0x001C026A
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

		// Token: 0x0600349A RID: 13466 RVA: 0x001C1EA8 File Offset: 0x001C02A8
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

		// Token: 0x0600349B RID: 13467 RVA: 0x001C1F26 File Offset: 0x001C0326
		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		// Token: 0x0600349C RID: 13468 RVA: 0x001C1F30 File Offset: 0x001C0330
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
			if (this.mouseoverGizmo != null)
			{
				this.mouseoverGizmo.GizmoUpdateOnMouseover();
			}
		}

		// Token: 0x0600349D RID: 13469 RVA: 0x001C1F55 File Offset: 0x001C0355
		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x001C1F64 File Offset: 0x001C0364
		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x001C1F6E File Offset: 0x001C036E
		public void Reset()
		{
			this.openTabType = null;
		}
	}
}
