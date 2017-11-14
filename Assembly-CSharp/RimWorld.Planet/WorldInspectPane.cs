using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	public class WorldInspectPane : Window, IInspectPane
	{
		private static readonly WITab[] TileTabs = new WITab[2]
		{
			new WITab_Terrain(),
			new WITab_Planet()
		};

		private Type openTabType;

		private float recentHeight;

		private static List<object> tmpObjectsList = new List<object>();

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

		protected override float Margin
		{
			get
			{
				return 0f;
			}
		}

		public override Vector2 InitialSize
		{
			get
			{
				return InspectPaneUtility.PaneSizeFor(this);
			}
		}

		private List<WorldObject> Selected
		{
			get
			{
				return Find.WorldSelector.SelectedObjects;
			}
		}

		private int NumSelectedObjects
		{
			get
			{
				return Find.WorldSelector.NumSelectedObjects;
			}
		}

		public float PaneTopY
		{
			get
			{
				float num = (float)((float)UI.screenHeight - 165.0);
				if (Current.ProgramState == ProgramState.Playing)
				{
					num = (float)(num - 35.0);
				}
				return num;
			}
		}

		public bool AnythingSelected
		{
			get
			{
				return Find.WorldSelector.AnyObjectOrTileSelected;
			}
		}

		private int SelectedTile
		{
			get
			{
				return Find.WorldSelector.selectedTile;
			}
		}

		private bool SelectedSingleObjectOrTile
		{
			get
			{
				return this.NumSelectedObjects == 1 || (this.NumSelectedObjects == 0 && this.SelectedTile >= 0);
			}
		}

		public bool ShouldShowSelectNextInCellButton
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		public bool ShouldShowPaneContents
		{
			get
			{
				return this.SelectedSingleObjectOrTile;
			}
		}

		public IEnumerable<InspectTabBase> CurTabs
		{
			get
			{
				if (this.NumSelectedObjects == 1)
				{
					return Find.WorldSelector.SingleSelectedObject.GetInspectTabs();
				}
				if (this.NumSelectedObjects == 0 && this.SelectedTile >= 0)
				{
					return WorldInspectPane.TileTabs;
				}
				return Enumerable.Empty<InspectTabBase>();
			}
		}

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
				if (tile.VisibleRoads != null)
				{
					stringBuilder.Append("\n" + (from rl in tile.VisibleRoads
					select rl.road).MaxBy((RoadDef road) => road.priority).LabelCap);
				}
				return stringBuilder.ToString();
			}
		}

		public WorldInspectPane()
		{
			base.layer = WindowLayer.GameUI;
			base.soundAppear = null;
			base.soundClose = null;
			base.closeOnClickedOutside = false;
			base.closeOnEscapeKey = false;
			base.preventCameraMotion = false;
		}

		protected override void SetInitialSizeAndPosition()
		{
			base.SetInitialSizeAndPosition();
			base.windowRect.x = 0f;
			base.windowRect.y = this.PaneTopY;
		}

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
			InspectGizmoGrid.DrawInspectGizmoGridFor(WorldInspectPane.tmpObjectsList);
			WorldInspectPane.tmpObjectsList.Clear();
		}

		public string GetLabel(Rect rect)
		{
			if (this.NumSelectedObjects > 0)
			{
				return WorldInspectPaneUtility.AdjustedLabelFor(this.Selected, rect);
			}
			if (this.SelectedTile >= 0)
			{
				return Find.WorldGrid[this.SelectedTile].biome.LabelCap;
			}
			return "error";
		}

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

		public void DoInspectPaneButtons(Rect rect, ref float lineEndWidth)
		{
			WorldObject singleSelectedObject = Find.WorldSelector.SingleSelectedObject;
			if (singleSelectedObject == null && this.SelectedTile < 0)
				return;
			float x = (float)(rect.width - 48.0);
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

		public override void DoWindowContents(Rect rect)
		{
			InspectPaneUtility.InspectPaneOnGUI(rect, this);
		}

		public override void WindowUpdate()
		{
			base.WindowUpdate();
			InspectPaneUtility.UpdateTabs(this);
		}

		public override void ExtraOnGUI()
		{
			base.ExtraOnGUI();
			InspectPaneUtility.ExtraOnGUI(this);
		}

		public void CloseOpenTab()
		{
			this.openTabType = null;
		}

		public void Reset()
		{
			this.openTabType = null;
		}
	}
}
