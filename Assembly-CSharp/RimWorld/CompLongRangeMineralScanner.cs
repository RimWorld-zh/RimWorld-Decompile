using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public class CompLongRangeMineralScanner : ThingComp
	{
		private ThingDef targetMineable = null;

		private CompPowerTrader powerComp;

		public CompLongRangeMineralScanner()
		{
		}

		public CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return (CompProperties_LongRangeMineralScanner)this.props;
			}
		}

		public bool CanUseNow
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		public override void PostExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.targetMineable, "targetMineable");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetMineable == null)
			{
				this.SetDefaultTargetMineral();
			}
		}

		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetDefaultTargetMineral();
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
		}

		private void SetDefaultTargetMineral()
		{
			this.targetMineable = ThingDefOf.MineableGold;
		}

		public void Used(Pawn worker)
		{
			if (!this.CanUseNow)
			{
				Log.Error("Used while CanUseNow is false.", false);
			}
			if (Find.TickManager.TicksGame % 59 == 0)
			{
				float statValue = worker.GetStatValue(StatDefOf.ResearchSpeed, true);
				float mtb = this.Props.mtbDays / statValue;
				if (Rand.MTBEventOccurs(mtb, 60000f, 59f))
				{
					this.FoundMinerals(worker);
				}
			}
		}

		private void FoundMinerals(Pawn worker)
		{
			IntRange preciousLumpSiteDistanceRange = SiteTuning.PreciousLumpSiteDistanceRange;
			int tile2;
			ref int tile = ref tile2;
			int min = preciousLumpSiteDistanceRange.min;
			int max = preciousLumpSiteDistanceRange.max;
			int tile3 = this.parent.Tile;
			if (TileFinder.TryFindNewSiteTile(out tile, min, max, false, true, tile3))
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PreciousLump, (!Rand.Chance(0.6f)) ? "MineralScannerPreciousLumpThreat" : null, tile2, null, true, null, true, null);
				if (site != null)
				{
					site.sitePartsKnown = true;
					site.core.parms.preciousLumpResources = this.targetMineable;
					int randomInRange = SiteTuning.MineralScannerPreciousLumpTimeoutDaysRange.RandomInRange;
					site.GetComponent<TimeoutComp>().StartTimeout(randomInRange * 60000);
					Find.WorldObjects.Add(site);
					Find.LetterStack.ReceiveLetter("LetterLabelFoundPreciousLump".Translate(), "LetterFoundPreciousLump".Translate(new object[]
					{
						this.targetMineable.label,
						randomInRange,
						SitePartUtility.GetDescriptionDialogue(site, site.parts.FirstOrDefault<SitePart>()).CapitalizeFirst(),
						worker.LabelShort
					}), LetterDefOf.PositiveEvent, site, null, null);
				}
			}
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (this.parent.Faction == Faction.OfPlayer)
			{
				ThingDef resource = this.targetMineable.building.mineableThing;
				Command_Action setTarg = new Command_Action();
				setTarg.defaultLabel = "CommandSelectMineralToScanFor".Translate() + ": " + resource.LabelCap;
				setTarg.icon = resource.uiIcon;
				setTarg.iconAngle = resource.uiIconAngle;
				setTarg.iconOffset = resource.uiIconOffset;
				setTarg.action = delegate()
				{
					List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (ThingDef localD2 in mineables)
					{
						ThingDef localD = localD2;
						FloatMenuOption item = new FloatMenuOption(localD.building.mineableThing.LabelCap, delegate()
						{
							foreach (object obj in Find.Selector.SelectedObjects)
							{
								Thing thing = obj as Thing;
								if (thing != null)
								{
									CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
									if (compLongRangeMineralScanner != null)
									{
										compLongRangeMineralScanner.targetMineable = localD;
									}
								}
							}
						}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null);
						list.Add(item);
					}
					Find.WindowStack.Add(new FloatMenu(list));
				};
				yield return setTarg;
			}
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Find resources now",
					action = delegate()
					{
						this.FoundMinerals(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
					}
				};
			}
			yield break;
		}

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal ThingDef <resource>__1;

			internal Command_Action <setTarg>__1;

			internal Command_Action <forceFindResourcesNow>__2;

			internal CompLongRangeMineralScanner $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

			private static Action <>f__am$cache0;

			[DebuggerHidden]
			public <CompGetGizmosExtra>c__Iterator0()
			{
			}

			public bool MoveNext()
			{
				uint num = (uint)this.$PC;
				this.$PC = -1;
				switch (num)
				{
				case 0u:
					if (this.parent.Faction == Faction.OfPlayer)
					{
						resource = this.targetMineable.building.mineableThing;
						setTarg = new Command_Action();
						setTarg.defaultLabel = "CommandSelectMineralToScanFor".Translate() + ": " + resource.LabelCap;
						setTarg.icon = resource.uiIcon;
						setTarg.iconAngle = resource.uiIconAngle;
						setTarg.iconOffset = resource.uiIconOffset;
						setTarg.action = delegate()
						{
							List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
							List<FloatMenuOption> list = new List<FloatMenuOption>();
							foreach (ThingDef localD2 in mineables)
							{
								ThingDef localD = localD2;
								FloatMenuOption item = new FloatMenuOption(localD.building.mineableThing.LabelCap, delegate()
								{
									foreach (object obj in Find.Selector.SelectedObjects)
									{
										Thing thing = obj as Thing;
										if (thing != null)
										{
											CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
											if (compLongRangeMineralScanner != null)
											{
												compLongRangeMineralScanner.targetMineable = localD;
											}
										}
									}
								}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null);
								list.Add(item);
							}
							Find.WindowStack.Add(new FloatMenu(list));
						};
						this.$current = setTarg;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				case 2u:
					goto IL_17A;
				default:
					return false;
				}
				if (Prefs.DevMode)
				{
					Command_Action forceFindResourcesNow = new Command_Action();
					forceFindResourcesNow.defaultLabel = "Dev: Find resources now";
					forceFindResourcesNow.action = delegate()
					{
						base.FoundMinerals(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
					};
					this.$current = forceFindResourcesNow;
					if (!this.$disposing)
					{
						this.$PC = 2;
					}
					return true;
				}
				IL_17A:
				this.$PC = -1;
				return false;
			}

			Gizmo IEnumerator<Gizmo>.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return this.$current;
				}
			}

			[DebuggerHidden]
			public void Dispose()
			{
				this.$disposing = true;
				this.$PC = -1;
			}

			[DebuggerHidden]
			public void Reset()
			{
				throw new NotSupportedException();
			}

			[DebuggerHidden]
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this.System.Collections.Generic.IEnumerable<Verse.Gizmo>.GetEnumerator();
			}

			[DebuggerHidden]
			IEnumerator<Gizmo> IEnumerable<Gizmo>.GetEnumerator()
			{
				if (Interlocked.CompareExchange(ref this.$PC, 0, -2) == -2)
				{
					return this;
				}
				CompLongRangeMineralScanner.<CompGetGizmosExtra>c__Iterator0 <CompGetGizmosExtra>c__Iterator = new CompLongRangeMineralScanner.<CompGetGizmosExtra>c__Iterator0();
				<CompGetGizmosExtra>c__Iterator.$this = this;
				return <CompGetGizmosExtra>c__Iterator;
			}

			private static void <>m__0()
			{
				List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (ThingDef localD2 in mineables)
				{
					ThingDef localD = localD2;
					FloatMenuOption item = new FloatMenuOption(localD.building.mineableThing.LabelCap, delegate()
					{
						foreach (object obj in Find.Selector.SelectedObjects)
						{
							Thing thing = obj as Thing;
							if (thing != null)
							{
								CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
								if (compLongRangeMineralScanner != null)
								{
									compLongRangeMineralScanner.targetMineable = localD;
								}
							}
						}
					}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null);
					list.Add(item);
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}

			internal void <>m__1()
			{
				base.FoundMinerals(PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>());
			}

			private sealed class <CompGetGizmosExtra>c__AnonStorey1
			{
				internal ThingDef localD;

				public <CompGetGizmosExtra>c__AnonStorey1()
				{
				}

				internal void <>m__0()
				{
					foreach (object obj in Find.Selector.SelectedObjects)
					{
						Thing thing = obj as Thing;
						if (thing != null)
						{
							CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
							if (compLongRangeMineralScanner != null)
							{
								compLongRangeMineralScanner.targetMineable = this.localD;
							}
						}
					}
				}

				internal bool <>m__1(Rect rect)
				{
					return Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, this.localD.building.mineableThing);
				}
			}
		}
	}
}
