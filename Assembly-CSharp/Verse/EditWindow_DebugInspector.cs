using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	public class EditWindow_DebugInspector : EditWindow
	{
		private StringBuilder debugStringBuilder = new StringBuilder();

		public bool fullMode;

		private float columnWidth = 360f;

		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		public EditWindow_DebugInspector()
		{
			base.optionalTitle = "Debug inspector";
		}

		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (Current.ProgramState == ProgramState.Playing)
			{
				GenUI.RenderMouseoverBracket();
			}
		}

		public override void DoWindowContents(Rect inRect)
		{
			if (KeyBindingDefOf.ToggleDebugInspector.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			widgetRow.ToggleableIcon(ref this.fullMode, TexButton.InspectModeToggle, "Toggle deep inspection mode for things on the map.", (SoundDef)null, (string)null);
			widgetRow.ToggleableIcon(ref DebugViewSettings.writeCellContents, TexButton.InspectModeToggle, "Toggle shallow inspection for things on the map.", (SoundDef)null, (string)null);
			if (widgetRow.ButtonText("Visibility", "Toggle what information should be reported by the inspector.", true, false))
			{
				Find.WindowStack.Add(new Dialog_DebugSettingsMenu());
			}
			if (widgetRow.ButtonText("Column Width +", "Make the columns wider.", true, false))
			{
				this.columnWidth += 20f;
				this.columnWidth = Mathf.Clamp(this.columnWidth, 200f, 1600f);
			}
			if (widgetRow.ButtonText("Column Width -", "Make the columns narrower.", true, false))
			{
				this.columnWidth -= 20f;
				this.columnWidth = Mathf.Clamp(this.columnWidth, 200f, 1600f);
			}
			inRect.yMin += 30f;
			Listing_Standard listing_Standard = new Listing_Standard(GameFont.Tiny);
			listing_Standard.ColumnWidth = Mathf.Min(this.columnWidth, inRect.width);
			listing_Standard.Begin(inRect);
			string[] array = this.debugStringBuilder.ToString().Split('\n');
			string[] array2 = array;
			foreach (string label in array2)
			{
				listing_Standard.Label(label, -1f);
				listing_Standard.Gap(-10f);
			}
			listing_Standard.End();
			if (Event.current.type == EventType.Repaint)
			{
				this.debugStringBuilder = new StringBuilder();
				this.debugStringBuilder.Append(this.CurrentDebugString());
			}
		}

		public void AppendDebugString(string str)
		{
			this.debugStringBuilder.AppendLine(str);
		}

		private string CurrentDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Pawn pawn2;
			Pawn pawn3;
			if (!WorldRendererUtility.WorldRenderedNow && Find.VisibleMap != null)
			{
				if (DebugViewSettings.writeGameConditions)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.VisibleMap.gameConditionManager.DebugString());
				}
				if (DebugViewSettings.writePlayingSounds)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Sustainers:");
					foreach (Sustainer allSustainer in Find.SoundRoot.sustainerManager.AllSustainers)
					{
						stringBuilder.AppendLine(allSustainer.DebugString());
					}
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("OneShots:");
					foreach (SampleOneShot playingOneShot in Find.SoundRoot.oneShotManager.PlayingOneShots)
					{
						stringBuilder.AppendLine(playingOneShot.ToString());
					}
				}
				if (DebugViewSettings.writeSoundEventsRecord)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Recent sound events:\n       ...");
					stringBuilder.AppendLine(DebugSoundEventsLog.EventsListingDebugString);
				}
				if (DebugViewSettings.writeGame)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine((Current.Game != null) ? Current.Game.DebugString() : "Current.Game = null");
				}
				if (DebugViewSettings.writeSteamItems)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(WorkshopItems.DebugOutput());
				}
				if (DebugViewSettings.writeConcepts)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(LessonAutoActivator.DebugString());
				}
				if (DebugViewSettings.writeMemoryUsage)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Total allocated: " + Profiler.GetTotalAllocatedMemoryLong().ToStringBytes("F2"));
					stringBuilder.AppendLine("Total reserved: " + Profiler.GetTotalReservedMemoryLong().ToStringBytes("F2"));
					stringBuilder.AppendLine("Total reserved unused: " + Profiler.GetTotalUnusedReservedMemoryLong().ToStringBytes("F2"));
					stringBuilder.AppendLine("Mono heap size: " + Profiler.GetMonoHeapSizeLong().ToStringBytes("F2"));
					stringBuilder.AppendLine("Mono used size: " + Profiler.GetMonoUsedSizeLong().ToStringBytes("F2"));
				}
				if (Current.ProgramState == ProgramState.Playing)
				{
					IntVec3 intVec = UI.MouseCell();
					stringBuilder.AppendLine("Tick " + Find.TickManager.TicksGame);
					stringBuilder.AppendLine("Inspecting " + intVec.ToString());
					if (DebugViewSettings.writeCellContents || this.fullMode)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.VisibleMap))
						{
							foreach (Designation item in Find.VisibleMap.designationManager.AllDesignationsAt(intVec))
							{
								stringBuilder.AppendLine(item.ToString());
							}
							foreach (Thing item2 in Find.VisibleMap.thingGrid.ThingsAt(intVec))
							{
								if (!this.fullMode)
								{
									stringBuilder.AppendLine(item2.LabelCap + " - " + item2.ToString());
								}
								else
								{
									stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(item2));
									stringBuilder.AppendLine();
								}
							}
						}
					}
					if (DebugViewSettings.debugApparelOptimize)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing item3 in Find.VisibleMap.thingGrid.ThingsAt(intVec))
						{
							Apparel apparel = item3 as Apparel;
							if (apparel != null)
							{
								stringBuilder.AppendLine(apparel.LabelCap);
								stringBuilder.AppendLine("   raw: " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel).ToString("F2"));
								Pawn pawn = Find.Selector.SingleSelectedThing as Pawn;
								if (pawn != null)
								{
									stringBuilder.AppendLine("  Pawn: " + pawn);
									stringBuilder.AppendLine("  gain: " + JobGiver_OptimizeApparel.ApparelScoreGain(pawn, apparel).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.drawPawnDebug)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.reservationManager.DebugString());
					}
					if (DebugViewSettings.writeMoteSaturation)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Mote count: " + Find.VisibleMap.moteCounter.MoteCount);
						stringBuilder.AppendLine("Mote saturation: " + Find.VisibleMap.moteCounter.Saturation);
					}
					if (DebugViewSettings.writeSnowDepth)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Snow depth: " + Find.VisibleMap.snowGrid.GetDepth(intVec));
					}
					if (DebugViewSettings.drawDeepResources)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Deep resource def: " + Find.VisibleMap.deepResourceGrid.ThingDefAt(intVec));
						stringBuilder.AppendLine("Deep resource count: " + Find.VisibleMap.deepResourceGrid.CountAt(intVec));
					}
					if (DebugViewSettings.writeEcosystem)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.wildSpawner.DebugString());
					}
					if (DebugViewSettings.writeTotalSnowDepth)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Total snow depth: " + Find.VisibleMap.snowGrid.TotalDepth);
					}
					if (DebugViewSettings.writeCanReachColony)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("CanReachColony: " + Find.VisibleMap.reachability.CanReachColony(UI.MouseCell()));
					}
					if (DebugViewSettings.writeMentalStateCalcs)
					{
						stringBuilder.AppendLine("---");
						foreach (Pawn item4 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							stringBuilder.AppendLine(item4.mindState.mentalBreaker.DebugString());
						}
					}
					if (DebugViewSettings.writeWind)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.windManager.DebugString());
					}
					if (DebugViewSettings.writeWorkSettings)
					{
						foreach (Pawn item5 in (from t in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							if (item5.workSettings != null)
							{
								stringBuilder.AppendLine("---");
								stringBuilder.AppendLine(item5.workSettings.DebugString());
							}
						}
					}
					if (DebugViewSettings.writeApparelScore)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.VisibleMap))
						{
							foreach (Thing thing3 in intVec.GetThingList(Find.VisibleMap))
							{
								Apparel apparel2 = thing3 as Apparel;
								if (apparel2 != null)
								{
									stringBuilder.AppendLine(apparel2.Label + ": " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel2).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.writeRecentStrikes)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.mineStrikeManager.DebugStrikeRecords());
					}
					if (DebugViewSettings.writeStoryteller)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.Storyteller.DebugString());
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.StoryWatcher.DebugString());
					}
					if (DebugViewSettings.writeListRepairableBldgs)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.listerBuildingsRepairable.DebugString());
					}
					if (DebugViewSettings.writeListFilthInHomeArea)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.listerFilthInHomeArea.DebugString());
					}
					if (DebugViewSettings.writeTerrain)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.terrainGrid.DebugStringAt(intVec));
					}
					if (DebugViewSettings.writeListHaulables)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.VisibleMap.listerHaulables.DebugString());
					}
					if (DebugViewSettings.drawLords)
					{
						foreach (Lord lord in Find.VisibleMap.lordManager.lords)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine(lord.DebugString());
						}
					}
					if (DebugViewSettings.writeMusicManagerPlay)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.MusicManagerPlay.DebugString());
					}
					if (DebugViewSettings.writeAttackTargets)
					{
						foreach (Pawn item6 in Find.VisibleMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("Potential attack targets for " + item6.LabelShort + ":");
							List<IAttackTarget> potentialTargetsFor = Find.VisibleMap.attackTargetsCache.GetPotentialTargetsFor(item6);
							for (int i = 0; i < potentialTargetsFor.Count; i++)
							{
								Thing thing = (Thing)potentialTargetsFor[i];
								stringBuilder.AppendLine(thing.LabelShort + ", " + thing.Faction + ((!potentialTargetsFor[i].ThreatDisabled()) ? string.Empty : " (threat disabled)"));
							}
						}
					}
					if (intVec.InBounds(Find.VisibleMap))
					{
						if (DebugViewSettings.drawRegions)
						{
							stringBuilder.AppendLine("---");
							Region regionAt_NoRebuild_InvalidAllowed = Find.VisibleMap.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
							stringBuilder.AppendLine("Region:\n" + ((regionAt_NoRebuild_InvalidAllowed == null) ? "null" : regionAt_NoRebuild_InvalidAllowed.DebugString));
						}
						if (DebugViewSettings.drawRooms)
						{
							stringBuilder.AppendLine("---");
							Room room = intVec.GetRoom(Find.VisibleMap, RegionType.Set_All);
							if (room != null)
							{
								stringBuilder.AppendLine(room.DebugString());
							}
							else
							{
								stringBuilder.AppendLine("(no room)");
							}
						}
						if (DebugViewSettings.drawRoomGroups)
						{
							stringBuilder.AppendLine("---");
							RoomGroup roomGroup = intVec.GetRoomGroup(Find.VisibleMap);
							if (roomGroup != null)
							{
								stringBuilder.AppendLine(roomGroup.DebugString());
							}
							else
							{
								stringBuilder.AppendLine("(no room group)");
							}
						}
						if (DebugViewSettings.drawGlow)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("Game glow: " + Find.VisibleMap.glowGrid.GameGlowAt(intVec, false));
							stringBuilder.AppendLine("Psych glow: " + Find.VisibleMap.glowGrid.PsychGlowAt(intVec));
							stringBuilder.AppendLine("Visual Glow: " + Find.VisibleMap.glowGrid.VisualGlowAt(intVec));
							stringBuilder.AppendLine("GlowReport:\n" + ((SectionLayer_LightingOverlay)Find.VisibleMap.mapDrawer.SectionAt(intVec).GetLayer(typeof(SectionLayer_LightingOverlay))).GlowReportAt(intVec));
							stringBuilder.AppendLine("SkyManager.CurSkyGlow: " + Find.VisibleMap.skyManager.CurSkyGlow);
						}
						if (DebugViewSettings.writePathCosts)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("Perceived path cost: " + Find.VisibleMap.pathGrid.PerceivedPathCostAt(intVec));
							stringBuilder.AppendLine("Real path cost: " + Find.VisibleMap.pathGrid.CalculatedCostAt(intVec, false, IntVec3.Invalid));
						}
						if (DebugViewSettings.writeFertility)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("\nFertility: " + Find.VisibleMap.fertilityGrid.FertilityAt(intVec).ToString("##0.00"));
						}
						if (DebugViewSettings.writeLinkFlags)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("\nLinkFlags: ");
							IEnumerator enumerator11 = Enum.GetValues(typeof(LinkFlags)).GetEnumerator();
							try
							{
								while (enumerator11.MoveNext())
								{
									object current11 = enumerator11.Current;
									if ((Find.VisibleMap.linkGrid.LinkFlagsAt(intVec) & (LinkFlags)current11) != 0)
									{
										stringBuilder.Append(" " + current11);
									}
								}
							}
							finally
							{
								IDisposable disposable;
								if ((disposable = (enumerator11 as IDisposable)) != null)
								{
									disposable.Dispose();
								}
							}
						}
						if (DebugViewSettings.writeSkyManager)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine(Find.VisibleMap.skyManager.DebugString());
						}
						if (DebugViewSettings.writeCover)
						{
							stringBuilder.AppendLine("---");
							stringBuilder.Append("Cover: ");
							Thing thing2 = Find.VisibleMap.coverGrid[intVec];
							if (thing2 == null)
							{
								stringBuilder.AppendLine("null");
							}
							else
							{
								stringBuilder.AppendLine(thing2.ToString());
							}
						}
						if (DebugViewSettings.drawPower)
						{
							stringBuilder.AppendLine("---");
							foreach (Thing item7 in Find.VisibleMap.thingGrid.ThingsAt(intVec))
							{
								ThingWithComps thingWithComps = item7 as ThingWithComps;
								if (thingWithComps != null && thingWithComps.GetComp<CompPowerTrader>() != null)
								{
									stringBuilder.AppendLine(" " + thingWithComps.GetComp<CompPowerTrader>().DebugString);
								}
							}
							PowerNet powerNet = Find.VisibleMap.powerNetGrid.TransmittedPowerNetAt(intVec);
							if (powerNet != null)
							{
								stringBuilder.AppendLine(string.Empty + powerNet.DebugString());
							}
							else
							{
								stringBuilder.AppendLine("(no PowerNet here)");
							}
						}
						if (DebugViewSettings.drawPreyInfo)
						{
							pawn2 = (Find.Selector.SingleSelectedThing as Pawn);
							if (pawn2 != null)
							{
								List<Thing> thingList = intVec.GetThingList(Find.VisibleMap);
								for (int j = 0; j < thingList.Count; j++)
								{
									pawn3 = (thingList[j] as Pawn);
									if (pawn3 != null)
										goto IL_107b;
								}
							}
						}
					}
				}
				goto IL_10de;
			}
			stringBuilder.AppendLine("World view");
			return stringBuilder.ToString();
			IL_107b:
			stringBuilder.AppendLine("---");
			if (FoodUtility.IsAcceptablePreyFor(pawn2, pawn3))
			{
				stringBuilder.AppendLine("Prey score: " + FoodUtility.GetPreyScoreFor(pawn2, pawn3));
			}
			else
			{
				stringBuilder.AppendLine("Prey score: None");
			}
			goto IL_10de;
			IL_10de:
			return stringBuilder.ToString();
		}
	}
}
