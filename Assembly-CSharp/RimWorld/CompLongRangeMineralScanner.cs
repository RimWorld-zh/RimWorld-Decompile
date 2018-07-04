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
		private CompPowerTrader powerComp;

		private List<Pair<Vector3, float>> otherActiveMineralScanners = new List<Pair<Vector3, float>>();

		private float cachedEffectiveAreaPct;

		private const float NoSitePartChance = 0.6f;

		private const float SiteTimeoutDays = 30f;

		private static readonly string MineralScannerPreciousLumpThreatTag = "MineralScannerPreciousLumpThreat";

		[CompilerGenerated]
		private static Func<ThingOption, float> <>f__am$cache0;

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

		public bool Active
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		private float EffectiveMtbDays
		{
			get
			{
				CompProperties_LongRangeMineralScanner props = this.Props;
				float effectiveAreaPct = this.EffectiveAreaPct;
				float result;
				if (effectiveAreaPct <= 0.001f)
				{
					result = -1f;
				}
				else
				{
					result = props.mtbDays / effectiveAreaPct;
				}
				return result;
			}
		}

		private float EffectiveAreaPct
		{
			get
			{
				return this.cachedEffectiveAreaPct;
			}
		}

		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.RecacheEffectiveAreaPct();
		}

		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheEffectiveAreaPct();
			}
		}

		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecacheEffectiveAreaPct();
			this.CheckTryFindMinerals(250);
		}

		private void RecacheEffectiveAreaPct()
		{
			if (!this.Active)
			{
				this.cachedEffectiveAreaPct = 0f;
			}
			else
			{
				this.CalculateOtherActiveMineralScanners();
				if (!this.otherActiveMineralScanners.Any<Pair<Vector3, float>>())
				{
					this.cachedEffectiveAreaPct = 1f;
				}
				else
				{
					CompProperties_LongRangeMineralScanner props = this.Props;
					WorldGrid worldGrid = Find.WorldGrid;
					Vector3 tileCenter = worldGrid.GetTileCenter(this.parent.Tile);
					float angle = worldGrid.TileRadiusToAngle(props.radius);
					int num = 0;
					int count = this.otherActiveMineralScanners.Count;
					Rand.PushState(this.parent.thingIDNumber);
					for (int i = 0; i < 400; i++)
					{
						Vector3 point = Rand.PointOnSphereCap(tileCenter, angle);
						bool flag = false;
						for (int j = 0; j < count; j++)
						{
							Pair<Vector3, float> pair = this.otherActiveMineralScanners[j];
							if (MeshUtility.Visible(point, 1f, pair.First, pair.Second))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							num++;
						}
					}
					Rand.PopState();
					this.cachedEffectiveAreaPct = (float)num / 400f;
				}
			}
		}

		private void CheckTryFindMinerals(int interval)
		{
			if (this.Active)
			{
				float effectiveMtbDays = this.EffectiveMtbDays;
				if (effectiveMtbDays > 0f)
				{
					if (Rand.MTBEventOccurs(effectiveMtbDays, 60000f, (float)interval))
					{
						this.FoundMinerals();
					}
				}
			}
		}

		private void FoundMinerals()
		{
			int tile2;
			ref int tile = ref tile2;
			int tile3 = this.parent.Tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, tile3))
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PreciousLump, (!Rand.Chance(0.6f)) ? CompLongRangeMineralScanner.MineralScannerPreciousLumpThreatTag : null, tile2, null, true, null, true);
				if (site != null)
				{
					site.sitePartsKnown = true;
					ThingDef thingDef = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables.RandomElementByWeight((ThingOption x) => x.weight).thingDef;
					site.core.parms.preciousLumpResources = thingDef;
					site.GetComponent<TimeoutComp>().StartTimeout(1800000);
					Find.WorldObjects.Add(site);
					Find.LetterStack.ReceiveLetter("LetterLabelFoundPreciousLump".Translate(), "LetterFoundPreciousLump".Translate(new object[]
					{
						thingDef.label,
						30f.ToString("F0"),
						SitePartUtility.GetDescriptionDialogue(site, site.parts.FirstOrDefault<SitePart>()).CapitalizeFirst()
					}), LetterDefOf.PositiveEvent, site, null, null);
				}
			}
		}

		private void CalculateOtherActiveMineralScanners()
		{
			this.otherActiveMineralScanners.Clear();
			List<Map> maps = Find.Maps;
			WorldGrid worldGrid = Find.WorldGrid;
			for (int i = 0; i < maps.Count; i++)
			{
				List<Thing> list = maps[i].listerThings.ThingsInGroup(ThingRequestGroup.LongRangeMineralScanner);
				for (int j = 0; j < list.Count; j++)
				{
					CompLongRangeMineralScanner compLongRangeMineralScanner = list[j].TryGetComp<CompLongRangeMineralScanner>();
					if (this.InterruptsMe(compLongRangeMineralScanner))
					{
						Vector3 tileCenter = worldGrid.GetTileCenter(maps[i].Tile);
						float second = worldGrid.TileRadiusToAngle(compLongRangeMineralScanner.Props.radius);
						this.otherActiveMineralScanners.Add(new Pair<Vector3, float>(tileCenter, second));
					}
				}
			}
		}

		private bool InterruptsMe(CompLongRangeMineralScanner otherScanner)
		{
			bool result;
			if (otherScanner == this)
			{
				result = false;
			}
			else if (!otherScanner.Active)
			{
				result = false;
			}
			else if (this.Props.mtbDays != otherScanner.Props.mtbDays)
			{
				result = (otherScanner.Props.mtbDays < this.Props.mtbDays);
			}
			else
			{
				result = (otherScanner.parent.thingIDNumber < this.parent.thingIDNumber);
			}
			return result;
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.Active)
			{
				this.RecacheEffectiveAreaPct();
				result = "LongRangeMineralScannerEfficiency".Translate(new object[]
				{
					this.EffectiveAreaPct.ToStringPercent()
				});
			}
			else
			{
				result = null;
			}
			return result;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Dev: Find resources now",
					action = delegate()
					{
						this.FoundMinerals();
					}
				};
			}
			yield break;
		}

		// Note: this type is marked as 'beforefieldinit'.
		static CompLongRangeMineralScanner()
		{
		}

		[CompilerGenerated]
		private static float <FoundMinerals>m__0(ThingOption x)
		{
			return x.weight;
		}

		[CompilerGenerated]
		private sealed class <CompGetGizmosExtra>c__Iterator0 : IEnumerable, IEnumerable<Gizmo>, IEnumerator, IDisposable, IEnumerator<Gizmo>
		{
			internal Command_Action <forceFindResourcesNow>__1;

			internal CompLongRangeMineralScanner $this;

			internal Gizmo $current;

			internal bool $disposing;

			internal int $PC;

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
					if (Prefs.DevMode)
					{
						Command_Action forceFindResourcesNow = new Command_Action();
						forceFindResourcesNow.defaultLabel = "Dev: Find resources now";
						forceFindResourcesNow.action = delegate()
						{
							base.FoundMinerals();
						};
						this.$current = forceFindResourcesNow;
						if (!this.$disposing)
						{
							this.$PC = 1;
						}
						return true;
					}
					break;
				case 1u:
					break;
				default:
					return false;
				}
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

			internal void <>m__0()
			{
				base.FoundMinerals();
			}
		}
	}
}
