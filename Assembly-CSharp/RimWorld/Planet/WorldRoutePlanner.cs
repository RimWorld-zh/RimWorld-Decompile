using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020008F1 RID: 2289
	[StaticConstructorOnStartup]
	public class WorldRoutePlanner
	{
		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x060034B6 RID: 13494 RVA: 0x001C229C File Offset: 0x001C069C
		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x060034B7 RID: 13495 RVA: 0x001C22B8 File Offset: 0x001C06B8
		private bool ShouldStop
		{
			get
			{
				return !this.active || !WorldRendererUtility.WorldRenderedNow || (Current.ProgramState == ProgramState.Playing && Find.TickManager.CurTimeSpeed != TimeSpeed.Paused);
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x060034B8 RID: 13496 RVA: 0x001C2314 File Offset: 0x001C0714
		private int CaravanTicksPerMove
		{
			get
			{
				CaravanTicksPerMoveUtility.CaravanInfo? caravanInfo = this.CaravanInfo;
				int result;
				if (caravanInfo != null && caravanInfo.Value.pawns.Any<Pawn>())
				{
					result = CaravanTicksPerMoveUtility.GetTicksPerMove(caravanInfo.Value, null);
				}
				else
				{
					result = 3674;
				}
				return result;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x060034B9 RID: 13497 RVA: 0x001C2370 File Offset: 0x001C0770
		private CaravanTicksPerMoveUtility.CaravanInfo? CaravanInfo
		{
			get
			{
				CaravanTicksPerMoveUtility.CaravanInfo? result;
				if (this.currentFormCaravanDialog != null)
				{
					result = this.caravanInfoFromFormCaravanDialog;
				}
				else
				{
					Caravan caravanAtTheFirstWaypoint = this.CaravanAtTheFirstWaypoint;
					if (caravanAtTheFirstWaypoint != null)
					{
						result = new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(caravanAtTheFirstWaypoint));
					}
					else
					{
						result = null;
					}
				}
				return result;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x060034BA RID: 13498 RVA: 0x001C23C4 File Offset: 0x001C07C4
		private Caravan CaravanAtTheFirstWaypoint
		{
			get
			{
				Caravan result;
				if (!this.waypoints.Any<RoutePlannerWaypoint>())
				{
					result = null;
				}
				else
				{
					result = Find.WorldObjects.PlayerControlledCaravanAt(this.waypoints[0].Tile);
				}
				return result;
			}
		}

		// Token: 0x060034BB RID: 13499 RVA: 0x001C240C File Offset: 0x001C080C
		public void Start()
		{
			if (this.active)
			{
				this.Stop();
			}
			this.active = true;
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.World.renderer.wantedMode = WorldRenderMode.Planet;
				Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
			}
		}

		// Token: 0x060034BC RID: 13500 RVA: 0x001C245C File Offset: 0x001C085C
		public void Start(Dialog_FormCaravan formCaravanDialog)
		{
			if (this.active)
			{
				this.Stop();
			}
			this.currentFormCaravanDialog = formCaravanDialog;
			this.caravanInfoFromFormCaravanDialog = new CaravanTicksPerMoveUtility.CaravanInfo?(new CaravanTicksPerMoveUtility.CaravanInfo(formCaravanDialog));
			formCaravanDialog.choosingRoute = true;
			Find.WindowStack.TryRemove(formCaravanDialog, true);
			this.Start();
			this.TryAddWaypoint(formCaravanDialog.CurrentTile, true);
			this.cantRemoveFirstWaypoint = true;
		}

		// Token: 0x060034BD RID: 13501 RVA: 0x001C24C4 File Offset: 0x001C08C4
		public void Stop()
		{
			this.active = false;
			WorldObjectsHolder worldObjects = Find.WorldObjects;
			for (int i = 0; i < this.waypoints.Count; i++)
			{
				worldObjects.Remove(this.waypoints[i]);
			}
			this.waypoints.Clear();
			this.cachedTicksToWaypoint.Clear();
			if (this.currentFormCaravanDialog != null)
			{
				this.currentFormCaravanDialog.Notify_NoLongerChoosingRoute();
			}
			this.caravanInfoFromFormCaravanDialog = null;
			this.currentFormCaravanDialog = null;
			this.cantRemoveFirstWaypoint = false;
			this.ReleasePaths();
		}

		// Token: 0x060034BE RID: 13502 RVA: 0x001C2560 File Offset: 0x001C0960
		public void WorldRoutePlannerUpdate()
		{
			if (this.active && this.ShouldStop)
			{
				this.Stop();
			}
			if (this.active)
			{
				for (int i = 0; i < this.paths.Count; i++)
				{
					this.paths[i].DrawPath(null);
				}
			}
		}

		// Token: 0x060034BF RID: 13503 RVA: 0x001C25CC File Offset: 0x001C09CC
		public void WorldRoutePlannerOnGUI()
		{
			if (this.active)
			{
				if (KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					if (this.currentFormCaravanDialog != null)
					{
						Find.WindowStack.Add(this.currentFormCaravanDialog);
					}
					else
					{
						SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
					}
					this.Stop();
					Event.current.Use();
				}
				else
				{
					GenUI.DrawMouseAttachment(WorldRoutePlanner.MouseAttachment);
					if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
					{
						Caravan caravan = Find.WorldSelector.SelectableObjectsUnderMouse().FirstOrDefault<WorldObject>() as Caravan;
						int tile = (caravan == null) ? GenWorld.MouseTile(true) : caravan.Tile;
						if (tile >= 0)
						{
							RoutePlannerWaypoint waypoint = this.MostRecentWaypointAt(tile);
							if (waypoint != null)
							{
								if (waypoint == this.waypoints[this.waypoints.Count - 1])
								{
									this.TryRemoveWaypoint(waypoint, true);
								}
								else
								{
									List<FloatMenuOption> list = new List<FloatMenuOption>();
									list.Add(new FloatMenuOption("AddWaypoint".Translate(), delegate()
									{
										this.TryAddWaypoint(tile, true);
									}, MenuOptionPriority.Default, null, null, 0f, null, null));
									list.Add(new FloatMenuOption("RemoveWaypoint".Translate(), delegate()
									{
										this.TryRemoveWaypoint(waypoint, true);
									}, MenuOptionPriority.Default, null, null, 0f, null, null));
									Find.WindowStack.Add(new FloatMenu(list));
								}
							}
							else
							{
								this.TryAddWaypoint(tile, true);
							}
							Event.current.Use();
						}
					}
					this.DoRouteDetailsBox();
					if (!this.DoChooseRouteButton())
					{
						this.DoTileTooltips();
					}
				}
			}
		}

		// Token: 0x060034C0 RID: 13504 RVA: 0x001C27BC File Offset: 0x001C0BBC
		private void DoRouteDetailsBox()
		{
			WorldRoutePlanner.<DoRouteDetailsBox>c__AnonStorey2 <DoRouteDetailsBox>c__AnonStorey = new WorldRoutePlanner.<DoRouteDetailsBox>c__AnonStorey2();
			<DoRouteDetailsBox>c__AnonStorey.$this = this;
			<DoRouteDetailsBox>c__AnonStorey.rect = new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomWindowSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f, WorldRoutePlanner.BottomWindowSize.x, WorldRoutePlanner.BottomWindowSize.y);
			if (Current.ProgramState == ProgramState.Entry)
			{
				WorldRoutePlanner.<DoRouteDetailsBox>c__AnonStorey2 <DoRouteDetailsBox>c__AnonStorey2 = <DoRouteDetailsBox>c__AnonStorey;
				<DoRouteDetailsBox>c__AnonStorey2.rect.y = <DoRouteDetailsBox>c__AnonStorey2.rect.y - 22f;
			}
			Find.WindowStack.ImmediateWindow(1373514241, <DoRouteDetailsBox>c__AnonStorey.rect, WindowLayer.Dialog, delegate
			{
				if (<DoRouteDetailsBox>c__AnonStorey.$this.active)
				{
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperCenter;
					Text.Font = GameFont.Small;
					float num = 6f;
					if (<DoRouteDetailsBox>c__AnonStorey.$this.waypoints.Count >= 2)
					{
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerEstTimeToFinalDest".Translate(new object[]
						{
							<DoRouteDetailsBox>c__AnonStorey.$this.GetTicksToWaypoint(<DoRouteDetailsBox>c__AnonStorey.$this.waypoints.Count - 1).ToStringTicksToDays("0.#")
						}));
					}
					else if (<DoRouteDetailsBox>c__AnonStorey.$this.cantRemoveFirstWaypoint)
					{
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerAddOneOrMoreWaypoints".Translate());
					}
					else
					{
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerAddTwoOrMoreWaypoints".Translate());
					}
					num += 20f;
					if (<DoRouteDetailsBox>c__AnonStorey.$this.CaravanInfo == null || !<DoRouteDetailsBox>c__AnonStorey.$this.CaravanInfo.Value.pawns.Any<Pawn>())
					{
						GUI.color = new Color(0.8f, 0.6f, 0.6f);
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerUsingAverageTicksPerMoveWarning".Translate());
					}
					else if (<DoRouteDetailsBox>c__AnonStorey.$this.currentFormCaravanDialog == null && <DoRouteDetailsBox>c__AnonStorey.$this.CaravanAtTheFirstWaypoint != null)
					{
						GUI.color = Color.gray;
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerUsingTicksPerMoveOfCaravan".Translate(new object[]
						{
							<DoRouteDetailsBox>c__AnonStorey.$this.CaravanAtTheFirstWaypoint.LabelCap
						}));
					}
					num += 20f;
					GUI.color = Color.gray;
					Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerPressRMBToAddAndRemoveWaypoints".Translate());
					num += 20f;
					if (<DoRouteDetailsBox>c__AnonStorey.$this.currentFormCaravanDialog != null)
					{
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerPressEscapeToReturnToCaravanFormationDialog".Translate());
					}
					else
					{
						Widgets.Label(new Rect(0f, num, <DoRouteDetailsBox>c__AnonStorey.rect.width, 25f), "RoutePlannerPressEscapeToExit".Translate());
					}
					num += 20f;
					GUI.color = Color.white;
					Text.Anchor = TextAnchor.UpperLeft;
				}
			}, true, false, 1f);
		}

		// Token: 0x060034C1 RID: 13505 RVA: 0x001C287C File Offset: 0x001C0C7C
		private bool DoChooseRouteButton()
		{
			bool result;
			if (this.currentFormCaravanDialog == null || this.waypoints.Count < 2)
			{
				result = false;
			}
			else
			{
				Rect rect = new Rect(((float)UI.screenWidth - WorldRoutePlanner.BottomButtonSize.x) / 2f, (float)UI.screenHeight - WorldRoutePlanner.BottomWindowSize.y - 45f - 10f - WorldRoutePlanner.BottomButtonSize.y, WorldRoutePlanner.BottomButtonSize.x, WorldRoutePlanner.BottomButtonSize.y);
				if (Widgets.ButtonText(rect, "ChooseRouteButton".Translate(), true, false, true))
				{
					Find.WindowStack.Add(this.currentFormCaravanDialog);
					this.currentFormCaravanDialog.Notify_ChoseRoute(this.waypoints[1].Tile);
					this.Stop();
					result = true;
				}
				else
				{
					result = false;
				}
			}
			return result;
		}

		// Token: 0x060034C2 RID: 13506 RVA: 0x001C2974 File Offset: 0x001C0D74
		private void DoTileTooltips()
		{
			if (!Mouse.IsInputBlockedNow)
			{
				int num = GenWorld.MouseTile(true);
				if (num != -1)
				{
					for (int i = 0; i < this.paths.Count; i++)
					{
						if (this.paths[i].NodesReversed.Contains(num))
						{
							string str = this.GetTileTip(num, i);
							Text.Font = GameFont.Small;
							Vector2 size = Text.CalcSize(str);
							size.x += 20f;
							size.y += 20f;
							Vector2 mouseAttachedWindowPos = GenUI.GetMouseAttachedWindowPos(size.x, size.y);
							Rect rect = new Rect(mouseAttachedWindowPos, size);
							Find.WindowStack.ImmediateWindow(1859615246, rect, WindowLayer.Super, delegate
							{
								Text.Font = GameFont.Small;
								Rect rect = rect.AtZero().ContractedBy(10f);
								Widgets.Label(rect, str);
							}, true, false, 1f);
							break;
						}
					}
				}
			}
		}

		// Token: 0x060034C3 RID: 13507 RVA: 0x001C2A80 File Offset: 0x001C0E80
		private string GetTileTip(int tile, int pathIndex)
		{
			int num = this.paths[pathIndex].NodesReversed.IndexOf(tile);
			int num2;
			if (num > 0)
			{
				num2 = this.paths[pathIndex].NodesReversed[num - 1];
			}
			else if (pathIndex < this.paths.Count - 1 && this.paths[pathIndex + 1].NodesReversed.Count >= 2)
			{
				num2 = this.paths[pathIndex + 1].NodesReversed[this.paths[pathIndex + 1].NodesReversed.Count - 2];
			}
			else
			{
				num2 = -1;
			}
			int num3 = this.cachedTicksToWaypoint[pathIndex] + CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.paths[pathIndex].FirstNode, tile, this.paths[pathIndex], 0f, this.CaravanTicksPerMove, GenTicks.TicksAbs + this.cachedTicksToWaypoint[pathIndex]);
			int num4 = GenTicks.TicksAbs + num3;
			StringBuilder stringBuilder = new StringBuilder();
			if (num3 != 0)
			{
				stringBuilder.AppendLine("EstimatedTimeToTile".Translate(new object[]
				{
					num3.ToStringTicksToDays("0.##")
				}));
			}
			stringBuilder.AppendLine("ForagedFoodAmount".Translate() + ": " + Find.WorldGrid[tile].biome.forageability.ToStringPercent());
			stringBuilder.Append(VirtualPlantsUtility.GetVirtualPlantsStatusExplanationAt(tile, num4));
			if (num2 != -1)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine();
				StringBuilder stringBuilder2 = new StringBuilder();
				float num5 = WorldPathGrid.CalculatedMovementDifficultyAt(num2, false, new int?(num4), stringBuilder2);
				float roadMovementDifficultyMultiplier = Find.WorldGrid.GetRoadMovementDifficultyMultiplier(tile, num2, stringBuilder2);
				stringBuilder.Append("TileMovementDifficulty".Translate() + ":\n" + stringBuilder2.ToString().Indented("  "));
				stringBuilder.AppendLine();
				stringBuilder.Append("  = ");
				stringBuilder.Append((num5 * roadMovementDifficultyMultiplier).ToString("0.#"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x060034C4 RID: 13508 RVA: 0x001C2CB4 File Offset: 0x001C10B4
		public void DoRoutePlannerButton(ref float curBaseY)
		{
			float num = (float)WorldRoutePlanner.ButtonTex.width;
			float num2 = (float)WorldRoutePlanner.ButtonTex.height;
			Rect rect = new Rect((float)UI.screenWidth - 10f - num, curBaseY - 10f - num2, num, num2);
			if (Widgets.ButtonImage(rect, WorldRoutePlanner.ButtonTex, Color.white, new Color(0.8f, 0.8f, 0.8f)))
			{
				if (this.active)
				{
					this.Stop();
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
				else
				{
					this.Start();
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegion(rect, "RoutePlannerButtonTip".Translate());
			curBaseY -= num2 + 20f;
		}

		// Token: 0x060034C5 RID: 13509 RVA: 0x001C2D80 File Offset: 0x001C1180
		public int GetTicksToWaypoint(int index)
		{
			return this.cachedTicksToWaypoint[index];
		}

		// Token: 0x060034C6 RID: 13510 RVA: 0x001C2DA4 File Offset: 0x001C11A4
		private void TryAddWaypoint(int tile, bool playSound = true)
		{
			if (Find.World.Impassable(tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseImpassable".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else if (this.waypoints.Any<RoutePlannerWaypoint>() && !Find.WorldReachability.CanReach(this.waypoints[this.waypoints.Count - 1].Tile, tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseUnreachable".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else if (this.waypoints.Count >= 25)
			{
				Messages.Message("MessageCantAddWaypointBecauseLimit".Translate(new object[]
				{
					25
				}), MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				RoutePlannerWaypoint routePlannerWaypoint = (RoutePlannerWaypoint)WorldObjectMaker.MakeWorldObject(WorldObjectDefOf.RoutePlannerWaypoint);
				routePlannerWaypoint.Tile = tile;
				Find.WorldObjects.Add(routePlannerWaypoint);
				this.waypoints.Add(routePlannerWaypoint);
				this.RecreatePaths();
				if (playSound)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060034C7 RID: 13511 RVA: 0x001C2EB8 File Offset: 0x001C12B8
		public void TryRemoveWaypoint(RoutePlannerWaypoint point, bool playSound = true)
		{
			if (this.cantRemoveFirstWaypoint && this.waypoints.Any<RoutePlannerWaypoint>() && point == this.waypoints[0])
			{
				Messages.Message("MessageCantRemoveWaypointBecauseFirst".Translate(), MessageTypeDefOf.RejectInput, false);
			}
			else
			{
				Find.WorldObjects.Remove(point);
				this.waypoints.Remove(point);
				for (int i = this.waypoints.Count - 1; i >= 1; i--)
				{
					if (this.waypoints[i].Tile == this.waypoints[i - 1].Tile)
					{
						Find.WorldObjects.Remove(this.waypoints[i]);
						this.waypoints.RemoveAt(i);
					}
				}
				this.RecreatePaths();
				if (playSound)
				{
					SoundDefOf.Tick_Low.PlayOneShotOnCamera(null);
				}
			}
		}

		// Token: 0x060034C8 RID: 13512 RVA: 0x001C2FAC File Offset: 0x001C13AC
		private void ReleasePaths()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].ReleaseToPool();
			}
			this.paths.Clear();
		}

		// Token: 0x060034C9 RID: 13513 RVA: 0x001C2FF4 File Offset: 0x001C13F4
		private void RecreatePaths()
		{
			this.ReleasePaths();
			WorldPathFinder worldPathFinder = Find.WorldPathFinder;
			for (int i = 1; i < this.waypoints.Count; i++)
			{
				this.paths.Add(worldPathFinder.FindPath(this.waypoints[i - 1].Tile, this.waypoints[i].Tile, null, null));
			}
			this.cachedTicksToWaypoint.Clear();
			int num = 0;
			int caravanTicksPerMove = this.CaravanTicksPerMove;
			for (int j = 0; j < this.waypoints.Count; j++)
			{
				if (j == 0)
				{
					this.cachedTicksToWaypoint.Add(0);
				}
				else
				{
					num += CaravanArrivalTimeEstimator.EstimatedTicksToArrive(this.waypoints[j - 1].Tile, this.waypoints[j].Tile, this.paths[j - 1], 0f, caravanTicksPerMove, GenTicks.TicksAbs + num);
					this.cachedTicksToWaypoint.Add(num);
				}
			}
		}

		// Token: 0x060034CA RID: 13514 RVA: 0x001C310C File Offset: 0x001C150C
		private RoutePlannerWaypoint MostRecentWaypointAt(int tile)
		{
			for (int i = this.waypoints.Count - 1; i >= 0; i--)
			{
				if (this.waypoints[i].Tile == tile)
				{
					return this.waypoints[i];
				}
			}
			return null;
		}

		// Token: 0x04001C79 RID: 7289
		private bool active;

		// Token: 0x04001C7A RID: 7290
		private CaravanTicksPerMoveUtility.CaravanInfo? caravanInfoFromFormCaravanDialog;

		// Token: 0x04001C7B RID: 7291
		private Dialog_FormCaravan currentFormCaravanDialog;

		// Token: 0x04001C7C RID: 7292
		private List<WorldPath> paths = new List<WorldPath>();

		// Token: 0x04001C7D RID: 7293
		private List<int> cachedTicksToWaypoint = new List<int>();

		// Token: 0x04001C7E RID: 7294
		public List<RoutePlannerWaypoint> waypoints = new List<RoutePlannerWaypoint>();

		// Token: 0x04001C7F RID: 7295
		private bool cantRemoveFirstWaypoint;

		// Token: 0x04001C80 RID: 7296
		private const int MaxCount = 25;

		// Token: 0x04001C81 RID: 7297
		private static readonly Texture2D ButtonTex = ContentFinder<Texture2D>.Get("UI/Misc/WorldRoutePlanner", true);

		// Token: 0x04001C82 RID: 7298
		private static readonly Texture2D MouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/WaypointMouseAttachment", true);

		// Token: 0x04001C83 RID: 7299
		private static readonly Vector2 BottomWindowSize = new Vector2(500f, 95f);

		// Token: 0x04001C84 RID: 7300
		private static readonly Vector2 BottomButtonSize = new Vector2(160f, 40f);

		// Token: 0x04001C85 RID: 7301
		private const float BottomWindowBotMargin = 45f;

		// Token: 0x04001C86 RID: 7302
		private const float BottomWindowEntryExtraBotMargin = 22f;
	}
}
