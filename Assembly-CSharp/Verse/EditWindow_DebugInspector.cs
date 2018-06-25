using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using UnityEngine;
using UnityEngine.Profiling;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;
using Verse.Steam;

namespace Verse
{
	// Token: 0x02000E0D RID: 3597
	public class EditWindow_DebugInspector : EditWindow
	{
		// Token: 0x0400356B RID: 13675
		private StringBuilder debugStringBuilder = new StringBuilder();

		// Token: 0x0400356C RID: 13676
		public bool fullMode = false;

		// Token: 0x0400356D RID: 13677
		private float columnWidth = 360f;

		// Token: 0x0600518D RID: 20877 RVA: 0x0029C9F9 File Offset: 0x0029ADF9
		public EditWindow_DebugInspector()
		{
			this.optionalTitle = "Debug inspector";
		}

		// Token: 0x17000D5C RID: 3420
		// (get) Token: 0x0600518E RID: 20878 RVA: 0x0029CA2C File Offset: 0x0029AE2C
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(400f, 600f);
			}
		}

		// Token: 0x17000D5D RID: 3421
		// (get) Token: 0x0600518F RID: 20879 RVA: 0x0029CA50 File Offset: 0x0029AE50
		public override bool IsDebug
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005190 RID: 20880 RVA: 0x0029CA66 File Offset: 0x0029AE66
		public override void WindowUpdate()
		{
			base.WindowUpdate();
			if (Current.ProgramState == ProgramState.Playing)
			{
				GenUI.RenderMouseoverBracket();
			}
		}

		// Token: 0x06005191 RID: 20881 RVA: 0x0029CA80 File Offset: 0x0029AE80
		public override void DoWindowContents(Rect inRect)
		{
			if (KeyBindingDefOf.Dev_ToggleDebugInspector.KeyDownEvent)
			{
				Event.current.Use();
				this.Close(true);
			}
			Text.Font = GameFont.Tiny;
			WidgetRow widgetRow = new WidgetRow(0f, 0f, UIDirection.RightThenUp, 99999f, 4f);
			widgetRow.ToggleableIcon(ref this.fullMode, TexButton.InspectModeToggle, "Toggle deep inspection mode for things on the map.", null, null);
			widgetRow.ToggleableIcon(ref DebugViewSettings.writeCellContents, TexButton.InspectModeToggle, "Toggle shallow inspection for things on the map.", null, null);
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
			string[] array = this.debugStringBuilder.ToString().Split(new char[]
			{
				'\n'
			});
			foreach (string label in array)
			{
				listing_Standard.Label(label, -1f, null);
				listing_Standard.Gap(-9f);
			}
			listing_Standard.End();
			if (Event.current.type == EventType.Repaint)
			{
				this.debugStringBuilder = new StringBuilder();
				this.debugStringBuilder.Append(this.CurrentDebugString());
			}
		}

		// Token: 0x06005192 RID: 20882 RVA: 0x0029CC7F File Offset: 0x0029B07F
		public void AppendDebugString(string str)
		{
			this.debugStringBuilder.AppendLine(str);
		}

		// Token: 0x06005193 RID: 20883 RVA: 0x0029CC90 File Offset: 0x0029B090
		private string CurrentDebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (DebugViewSettings.writeGame)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine((Current.Game != null) ? Current.Game.DebugString() : "Current.Game = null");
			}
			if (DebugViewSettings.writeMusicManagerPlay)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine(Find.MusicManagerPlay.DebugString());
			}
			if (DebugViewSettings.writePlayingSounds)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Sustainers:");
				foreach (Sustainer sustainer in Find.SoundRoot.sustainerManager.AllSustainers)
				{
					stringBuilder.AppendLine(sustainer.DebugString());
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("OneShots:");
				foreach (SampleOneShot sampleOneShot in Find.SoundRoot.oneShotManager.PlayingOneShots)
				{
					stringBuilder.AppendLine(sampleOneShot.ToString());
				}
			}
			if (DebugViewSettings.writeSoundEventsRecord)
			{
				stringBuilder.AppendLine("---");
				stringBuilder.AppendLine("Recent sound events:\n       ...");
				stringBuilder.AppendLine(DebugSoundEventsLog.EventsListingDebugString);
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
				stringBuilder.AppendLine("Tick " + Find.TickManager.TicksGame);
				if (DebugViewSettings.writeStoryteller)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.Storyteller.DebugString());
				}
			}
			if (Current.ProgramState == ProgramState.Playing && Find.CurrentMap != null)
			{
				if (DebugViewSettings.writeMapGameConditions)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.gameConditionManager.DebugString());
				}
				if (DebugViewSettings.drawPawnDebug)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.reservationManager.DebugString());
				}
				if (DebugViewSettings.writeMoteSaturation)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Mote count: " + Find.CurrentMap.moteCounter.MoteCount);
					stringBuilder.AppendLine("Mote saturation: " + Find.CurrentMap.moteCounter.Saturation);
				}
				if (DebugViewSettings.writeEcosystem)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.wildAnimalSpawner.DebugString());
				}
				if (DebugViewSettings.writeTotalSnowDepth)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine("Total snow depth: " + Find.CurrentMap.snowGrid.TotalDepth);
				}
				if (DebugViewSettings.writeWind)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.windManager.DebugString());
				}
				if (DebugViewSettings.writeRecentStrikes)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.mineStrikeManager.DebugStrikeRecords());
				}
				if (DebugViewSettings.writeListRepairableBldgs)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerBuildingsRepairable.DebugString());
				}
				if (DebugViewSettings.writeListFilthInHomeArea)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerFilthInHomeArea.DebugString());
				}
				if (DebugViewSettings.writeListHaulables)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerHaulables.DebugString());
				}
				if (DebugViewSettings.writeListMergeables)
				{
					stringBuilder.AppendLine("---");
					stringBuilder.AppendLine(Find.CurrentMap.listerMergeables.DebugString());
				}
				if (DebugViewSettings.drawLords)
				{
					foreach (Lord lord in Find.CurrentMap.lordManager.lords)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(lord.DebugString());
					}
				}
				IntVec3 intVec = UI.MouseCell();
				if (intVec.InBounds(Find.CurrentMap))
				{
					stringBuilder.AppendLine("Inspecting " + intVec.ToString());
					if (DebugViewSettings.writeTerrain)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine(Find.CurrentMap.terrainGrid.DebugStringAt(intVec));
					}
					if (DebugViewSettings.writeAttackTargets)
					{
						foreach (Pawn pawn in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell()).OfType<Pawn>())
						{
							stringBuilder.AppendLine("---");
							stringBuilder.AppendLine("Potential attack targets for " + pawn.LabelShort + ":");
							List<IAttackTarget> potentialTargetsFor = Find.CurrentMap.attackTargetsCache.GetPotentialTargetsFor(pawn);
							for (int i = 0; i < potentialTargetsFor.Count; i++)
							{
								Thing thing = (Thing)potentialTargetsFor[i];
								stringBuilder.AppendLine(string.Concat(new object[]
								{
									thing.LabelShort,
									", ",
									thing.Faction,
									(!potentialTargetsFor[i].ThreatDisabled(null)) ? "" : " (threat disabled)"
								}));
							}
						}
					}
					if (DebugViewSettings.writeSnowDepth)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Snow depth: " + Find.CurrentMap.snowGrid.GetDepth(intVec));
					}
					if (DebugViewSettings.drawDeepResources)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Deep resource def: " + Find.CurrentMap.deepResourceGrid.ThingDefAt(intVec));
						stringBuilder.AppendLine("Deep resource count: " + Find.CurrentMap.deepResourceGrid.CountAt(intVec));
					}
					if (DebugViewSettings.writeCanReachColony)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("CanReachColony: " + Find.CurrentMap.reachability.CanReachColony(UI.MouseCell()));
					}
					if (DebugViewSettings.writeMentalStateCalcs)
					{
						stringBuilder.AppendLine("---");
						foreach (Pawn pawn2 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							stringBuilder.AppendLine(pawn2.mindState.mentalBreaker.DebugString());
						}
					}
					if (DebugViewSettings.writeWorkSettings)
					{
						foreach (Pawn pawn3 in (from t in Find.CurrentMap.thingGrid.ThingsAt(UI.MouseCell())
						where t is Pawn
						select t).Cast<Pawn>())
						{
							if (pawn3.workSettings != null)
							{
								stringBuilder.AppendLine("---");
								stringBuilder.AppendLine(pawn3.workSettings.DebugString());
							}
						}
					}
					if (DebugViewSettings.writeApparelScore)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.CurrentMap))
						{
							foreach (Thing thing2 in intVec.GetThingList(Find.CurrentMap))
							{
								Apparel apparel = thing2 as Apparel;
								if (apparel != null)
								{
									stringBuilder.AppendLine(apparel.Label + ": " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.writeCellContents || this.fullMode)
					{
						stringBuilder.AppendLine("---");
						if (intVec.InBounds(Find.CurrentMap))
						{
							foreach (Designation designation in Find.CurrentMap.designationManager.AllDesignationsAt(intVec))
							{
								stringBuilder.AppendLine(designation.ToString());
							}
							foreach (Thing thing3 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
							{
								if (!this.fullMode)
								{
									stringBuilder.AppendLine(thing3.LabelCap + " - " + thing3.ToString());
								}
								else
								{
									stringBuilder.AppendLine(Scribe.saver.DebugOutputFor(thing3));
									stringBuilder.AppendLine();
								}
							}
						}
					}
					if (DebugViewSettings.debugApparelOptimize)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing thing4 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
						{
							Apparel apparel2 = thing4 as Apparel;
							if (apparel2 != null)
							{
								stringBuilder.AppendLine(apparel2.LabelCap);
								stringBuilder.AppendLine("   raw: " + JobGiver_OptimizeApparel.ApparelScoreRaw(null, apparel2).ToString("F2"));
								Pawn pawn4 = Find.Selector.SingleSelectedThing as Pawn;
								if (pawn4 != null)
								{
									stringBuilder.AppendLine("  Pawn: " + pawn4);
									stringBuilder.AppendLine("  gain: " + JobGiver_OptimizeApparel.ApparelScoreGain(pawn4, apparel2).ToString("F2"));
								}
							}
						}
					}
					if (DebugViewSettings.drawRegions)
					{
						stringBuilder.AppendLine("---");
						Region regionAt_NoRebuild_InvalidAllowed = Find.CurrentMap.regionGrid.GetRegionAt_NoRebuild_InvalidAllowed(intVec);
						stringBuilder.AppendLine("Region:\n" + ((regionAt_NoRebuild_InvalidAllowed == null) ? "null" : regionAt_NoRebuild_InvalidAllowed.DebugString));
					}
					if (DebugViewSettings.drawRooms)
					{
						stringBuilder.AppendLine("---");
						Room room = intVec.GetRoom(Find.CurrentMap, RegionType.Set_All);
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
						RoomGroup roomGroup = intVec.GetRoomGroup(Find.CurrentMap);
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
						stringBuilder.AppendLine("Game glow: " + Find.CurrentMap.glowGrid.GameGlowAt(intVec, false));
						stringBuilder.AppendLine("Psych glow: " + Find.CurrentMap.glowGrid.PsychGlowAt(intVec));
						stringBuilder.AppendLine("Visual Glow: " + Find.CurrentMap.glowGrid.VisualGlowAt(intVec));
						stringBuilder.AppendLine("GlowReport:\n" + ((SectionLayer_LightingOverlay)Find.CurrentMap.mapDrawer.SectionAt(intVec).GetLayer(typeof(SectionLayer_LightingOverlay))).GlowReportAt(intVec));
						stringBuilder.AppendLine("SkyManager.CurSkyGlow: " + Find.CurrentMap.skyManager.CurSkyGlow);
					}
					if (DebugViewSettings.writePathCosts)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("Perceived path cost: " + Find.CurrentMap.pathGrid.PerceivedPathCostAt(intVec));
						stringBuilder.AppendLine("Real path cost: " + Find.CurrentMap.pathGrid.CalculatedCostAt(intVec, false, IntVec3.Invalid));
					}
					if (DebugViewSettings.writeFertility)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.AppendLine("\nFertility: " + Find.CurrentMap.fertilityGrid.FertilityAt(intVec).ToString("##0.00"));
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
								object obj = enumerator11.Current;
								if ((Find.CurrentMap.linkGrid.LinkFlagsAt(intVec) & (LinkFlags)obj) != LinkFlags.None)
								{
									stringBuilder.Append(" " + obj);
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
						stringBuilder.AppendLine(Find.CurrentMap.skyManager.DebugString());
					}
					if (DebugViewSettings.writeCover)
					{
						stringBuilder.AppendLine("---");
						stringBuilder.Append("Cover: ");
						Thing thing5 = Find.CurrentMap.coverGrid[intVec];
						if (thing5 == null)
						{
							stringBuilder.AppendLine("null");
						}
						else
						{
							stringBuilder.AppendLine(thing5.ToString());
						}
					}
					if (DebugViewSettings.drawPower)
					{
						stringBuilder.AppendLine("---");
						foreach (Thing thing6 in Find.CurrentMap.thingGrid.ThingsAt(intVec))
						{
							ThingWithComps thingWithComps = thing6 as ThingWithComps;
							if (thingWithComps != null && thingWithComps.GetComp<CompPowerTrader>() != null)
							{
								stringBuilder.AppendLine(" " + thingWithComps.GetComp<CompPowerTrader>().DebugString);
							}
						}
						PowerNet powerNet = Find.CurrentMap.powerNetGrid.TransmittedPowerNetAt(intVec);
						if (powerNet != null)
						{
							stringBuilder.AppendLine("" + powerNet.DebugString());
						}
						else
						{
							stringBuilder.AppendLine("(no PowerNet here)");
						}
					}
					if (DebugViewSettings.drawPreyInfo)
					{
						Pawn pawn5 = Find.Selector.SingleSelectedThing as Pawn;
						if (pawn5 != null)
						{
							List<Thing> thingList = intVec.GetThingList(Find.CurrentMap);
							for (int j = 0; j < thingList.Count; j++)
							{
								Pawn pawn6 = thingList[j] as Pawn;
								if (pawn6 != null)
								{
									stringBuilder.AppendLine("---");
									if (FoodUtility.IsAcceptablePreyFor(pawn5, pawn6))
									{
										stringBuilder.AppendLine("Prey score: " + FoodUtility.GetPreyScoreFor(pawn5, pawn6));
									}
									else
									{
										stringBuilder.AppendLine("Prey score: None");
									}
									break;
								}
							}
						}
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
