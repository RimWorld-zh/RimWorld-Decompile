using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld.Planet
{
	[StaticConstructorOnStartup]
	public class WorldRoutePlanner
	{
		private const int MaxCount = 25;

		private const float BottomWindowBotMargin = 45f;

		private const float BottomWindowEntryExtraBotMargin = 22f;

		private const int DefaultCaravanTicksPerMove = 100;

		private bool active;

		private List<Pawn> caravanPawnsFromFormCaravanDialog = new List<Pawn>();

		private Dialog_FormCaravan currentFormCaravanDialog;

		private List<WorldPath> paths = new List<WorldPath>();

		private List<int> cachedTicksToWaypoint = new List<int>();

		public List<RoutePlannerWaypoint> waypoints = new List<RoutePlannerWaypoint>();

		private bool cantRemoveFirstWaypoint;

		private static readonly Texture2D ButtonTex = ContentFinder<Texture2D>.Get("UI/Misc/WorldRoutePlanner", true);

		private static readonly Texture2D MouseAttachment = ContentFinder<Texture2D>.Get("UI/Overlays/WaypointMouseAttachment", true);

		private static readonly Vector2 BottomWindowSize = new Vector2(500f, 95f);

		public bool Active
		{
			get
			{
				return this.active;
			}
		}

		private bool ShouldStop
		{
			get
			{
				if (!this.active)
				{
					return true;
				}
				if (!WorldRendererUtility.WorldRenderedNow)
				{
					return true;
				}
				if (((Current.ProgramState == ProgramState.Playing) ? Find.TickManager.CurTimeSpeed : TimeSpeed.Paused) != 0)
				{
					return true;
				}
				return false;
			}
		}

		private int CaravanTicksPerMove
		{
			get
			{
				List<Pawn> caravanPawns = this.CaravanPawns;
				if (!caravanPawns.NullOrEmpty())
				{
					return CaravanTicksPerMoveUtility.GetTicksPerMove(caravanPawns);
				}
				return 3000;
			}
		}

		private List<Pawn> CaravanPawns
		{
			get
			{
				if (this.currentFormCaravanDialog != null)
				{
					return this.caravanPawnsFromFormCaravanDialog;
				}
				Caravan caravanAtTheFirstWaypoint = this.CaravanAtTheFirstWaypoint;
				if (caravanAtTheFirstWaypoint != null)
				{
					return caravanAtTheFirstWaypoint.PawnsListForReading;
				}
				return null;
			}
		}

		private Caravan CaravanAtTheFirstWaypoint
		{
			get
			{
				if (!this.waypoints.Any())
				{
					return null;
				}
				return Find.WorldObjects.PlayerControlledCaravanAt(this.waypoints[0].Tile);
			}
		}

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

		public void Start(Dialog_FormCaravan formCaravanDialog)
		{
			if (this.active)
			{
				this.Stop();
			}
			this.currentFormCaravanDialog = formCaravanDialog;
			this.caravanPawnsFromFormCaravanDialog.AddRange(TransferableUtility.GetPawnsFromTransferables(formCaravanDialog.transferables));
			Find.WindowStack.TryRemove(formCaravanDialog, true);
			this.Start();
			this.TryAddWaypoint(formCaravanDialog.CurrentTile, true);
			this.cantRemoveFirstWaypoint = true;
		}

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
			this.caravanPawnsFromFormCaravanDialog.Clear();
			this.currentFormCaravanDialog = null;
			this.cantRemoveFirstWaypoint = false;
			this.ReleasePaths();
		}

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

		public void WorldRoutePlannerOnGUI()
		{
			if (this.active)
			{
				if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
				{
					if (this.currentFormCaravanDialog != null)
					{
						Find.WindowStack.Add(this.currentFormCaravanDialog);
					}
					else
					{
						SoundDefOf.TickLow.PlayOneShotOnCamera(null);
					}
					this.Stop();
					Event.current.Use();
				}
				else
				{
					GenUI.DrawMouseAttachment(WorldRoutePlanner.MouseAttachment);
					if (Event.current.type == EventType.MouseDown && Event.current.button == 1)
					{
						Caravan caravan = Find.WorldSelector.SelectableObjectsUnderMouse().FirstOrDefault() as Caravan;
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
									list.Add(new FloatMenuOption("AddWaypoint".Translate(), (Action)delegate
									{
										this.TryAddWaypoint(tile, true);
									}, MenuOptionPriority.Default, null, null, 0f, null, null));
									list.Add(new FloatMenuOption("RemoveWaypoint".Translate(), (Action)delegate
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
					float num = (float)UI.screenWidth;
					Vector2 bottomWindowSize = WorldRoutePlanner.BottomWindowSize;
					double x = (num - bottomWindowSize.x) / 2.0;
					float num2 = (float)UI.screenHeight;
					Vector2 bottomWindowSize2 = WorldRoutePlanner.BottomWindowSize;
					double y = num2 - bottomWindowSize2.y - 45.0;
					Vector2 bottomWindowSize3 = WorldRoutePlanner.BottomWindowSize;
					float x2 = bottomWindowSize3.x;
					Vector2 bottomWindowSize4 = WorldRoutePlanner.BottomWindowSize;
					Rect rect = new Rect((float)x, (float)y, x2, bottomWindowSize4.y);
					if (Current.ProgramState == ProgramState.Entry)
					{
						rect.y -= 22f;
					}
					Find.WindowStack.ImmediateWindow(1373514241, rect, WindowLayer.Dialog, (Action)delegate
					{
						if (this.active)
						{
							GUI.color = Color.white;
							Text.Anchor = TextAnchor.UpperCenter;
							Text.Font = GameFont.Small;
							float num3 = 6f;
							if (this.waypoints.Count >= 2)
							{
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerEstTimeToFinalDest".Translate(this.GetTicksToWaypoint(this.waypoints.Count - 1).ToStringTicksToDays("0.#")));
							}
							else if (this.cantRemoveFirstWaypoint)
							{
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerAddOneOrMoreWaypoints".Translate());
							}
							else
							{
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerAddTwoOrMoreWaypoints".Translate());
							}
							num3 = (float)(num3 + 20.0);
							if (this.CaravanPawns.NullOrEmpty())
							{
								GUI.color = new Color(0.8f, 0.6f, 0.6f);
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerUsingAverageTicksPerMoveWarning".Translate());
							}
							else if (this.currentFormCaravanDialog == null && this.CaravanAtTheFirstWaypoint != null)
							{
								GUI.color = Color.gray;
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerUsingTicksPerMoveOfCaravan".Translate(this.CaravanAtTheFirstWaypoint.LabelCap));
							}
							num3 = (float)(num3 + 20.0);
							GUI.color = Color.gray;
							Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerPressRMBToAddAndRemoveWaypoints".Translate());
							num3 = (float)(num3 + 20.0);
							if (this.currentFormCaravanDialog != null)
							{
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerPressEscapeToReturnToCaravanFormationDialog".Translate());
							}
							else
							{
								Widgets.Label(new Rect(0f, num3, rect.width, 25f), "RoutePlannerPressEscapeToExit".Translate());
							}
							num3 = (float)(num3 + 20.0);
							GUI.color = Color.white;
							Text.Anchor = TextAnchor.UpperLeft;
						}
					}, true, false, 1f);
				}
			}
		}

		public void DoRoutePlannerButton(ref float curBaseY)
		{
			float num = (float)WorldRoutePlanner.ButtonTex.width;
			float num2 = (float)WorldRoutePlanner.ButtonTex.height;
			Rect rect = new Rect((float)((float)UI.screenWidth - 10.0 - num), (float)(curBaseY - 10.0 - num2), num, num2);
			if (Widgets.ButtonImage(rect, WorldRoutePlanner.ButtonTex, Color.white, new Color(0.8f, 0.8f, 0.8f)))
			{
				if (this.active)
				{
					this.Stop();
					SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				}
				else
				{
					this.Start();
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				}
			}
			TooltipHandler.TipRegion(rect, "RoutePlannerButtonTip".Translate());
			curBaseY -= (float)(num2 + 20.0);
		}

		public int GetTicksToWaypoint(int index)
		{
			return this.cachedTicksToWaypoint[index];
		}

		private void TryAddWaypoint(int tile, bool playSound = true)
		{
			if (Find.World.Impassable(tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseImpassable".Translate(), MessageSound.RejectInput);
			}
			else if (this.waypoints.Any() && !Find.WorldReachability.CanReach(this.waypoints[this.waypoints.Count - 1].Tile, tile))
			{
				Messages.Message("MessageCantAddWaypointBecauseUnreachable".Translate(), MessageSound.RejectInput);
			}
			else if (this.waypoints.Count >= 25)
			{
				Messages.Message("MessageCantAddWaypointBecauseLimit".Translate(25), MessageSound.RejectInput);
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
					SoundDefOf.TickHigh.PlayOneShotOnCamera(null);
				}
			}
		}

		public void TryRemoveWaypoint(RoutePlannerWaypoint point, bool playSound = true)
		{
			if (this.cantRemoveFirstWaypoint && this.waypoints.Any() && point == this.waypoints[0])
			{
				Messages.Message("MessageCantRemoveWaypointBecauseFirst".Translate(), MessageSound.RejectInput);
			}
			else
			{
				Find.WorldObjects.Remove(point);
				this.waypoints.Remove(point);
				for (int num = this.waypoints.Count - 1; num >= 1; num--)
				{
					if (this.waypoints[num].Tile == this.waypoints[num - 1].Tile)
					{
						Find.WorldObjects.Remove(this.waypoints[num]);
						this.waypoints.RemoveAt(num);
					}
				}
				this.RecreatePaths();
				if (playSound)
				{
					SoundDefOf.TickLow.PlayOneShotOnCamera(null);
				}
			}
		}

		private void ReleasePaths()
		{
			for (int i = 0; i < this.paths.Count; i++)
			{
				this.paths[i].ReleaseToPool();
			}
			this.paths.Clear();
		}

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

		private RoutePlannerWaypoint MostRecentWaypointAt(int tile)
		{
			for (int num = this.waypoints.Count - 1; num >= 0; num--)
			{
				if (this.waypoints[num].Tile == tile)
				{
					return this.waypoints[num];
				}
			}
			return null;
		}
	}
}
