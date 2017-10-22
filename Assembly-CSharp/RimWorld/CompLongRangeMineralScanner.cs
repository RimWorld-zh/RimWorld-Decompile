using RimWorld.Planet;
using System;
using System.Collections.Generic;
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

		private static readonly string MineralScannerPreciousLumpThreatTag = "MineralScannerPreciousLumpThreat";

		public CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return (CompProperties_LongRangeMineralScanner)base.props;
			}
		}

		public bool Active
		{
			get
			{
				return base.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && base.parent.Faction == Faction.OfPlayer;
			}
		}

		private float EffectiveMtbDays
		{
			get
			{
				CompProperties_LongRangeMineralScanner props = this.Props;
				float effectiveAreaPct = this.EffectiveAreaPct;
				return (float)((!(effectiveAreaPct <= 0.0010000000474974513)) ? (props.mtbDays / effectiveAreaPct) : -1.0);
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
			this.powerComp = base.parent.GetComp<CompPowerTrader>();
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
				if (!this.otherActiveMineralScanners.Any())
				{
					this.cachedEffectiveAreaPct = 1f;
				}
				else
				{
					CompProperties_LongRangeMineralScanner props = this.Props;
					WorldGrid worldGrid = Find.WorldGrid;
					Vector3 tileCenter = worldGrid.GetTileCenter(base.parent.Tile);
					float angle = worldGrid.TileRadiusToAngle(props.radius);
					int num = 0;
					int count = this.otherActiveMineralScanners.Count;
					Rand.PushState(base.parent.thingIDNumber);
					for (int i = 0; i < 400; i++)
					{
						Vector3 point = Rand.PointOnSphereCap(tileCenter, angle);
						bool flag = false;
						for (int num2 = 0; num2 < count; num2++)
						{
							Pair<Vector3, float> pair = this.otherActiveMineralScanners[num2];
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
					this.cachedEffectiveAreaPct = (float)((float)num / 400.0);
				}
			}
		}

		private void CheckTryFindMinerals(int interval)
		{
			if (this.Active)
			{
				float effectiveMtbDays = this.EffectiveMtbDays;
				if (!(effectiveMtbDays <= 0.0) && Rand.MTBEventOccurs(effectiveMtbDays, 60000f, (float)interval))
				{
					this.FoundMinerals();
				}
			}
		}

		private void FoundMinerals()
		{
			int tile = base.parent.Tile;
			int tile2 = default(int);
			if (TileFinder.TryFindNewSiteTile(out tile2, 8, 30, false, true, tile))
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PreciousLump, (!Rand.Chance(0.6f)) ? CompLongRangeMineralScanner.MineralScannerPreciousLumpThreatTag : null, null, true, null);
				if (site != null)
				{
					site.Tile = tile2;
					Find.WorldObjects.Add(site);
					Find.LetterStack.ReceiveLetter("LetterLabelFoundPreciousLump".Translate(), "LetterFoundPreciousLump".Translate(), LetterDefOf.PositiveEvent, (WorldObject)site, (string)null);
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
			return otherScanner != this && otherScanner.Active && ((this.Props.mtbDays == otherScanner.Props.mtbDays) ? (otherScanner.parent.thingIDNumber < base.parent.thingIDNumber) : (otherScanner.Props.mtbDays < this.Props.mtbDays));
		}

		public override string CompInspectStringExtra()
		{
			string result;
			if (this.Active)
			{
				this.RecacheEffectiveAreaPct();
				result = "LongRangeMineralScannerEfficiency".Translate(this.EffectiveAreaPct.ToStringPercent());
			}
			else
			{
				result = (string)null;
			}
			return result;
		}

		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
				yield break;
			yield return (Gizmo)new Command_Action
			{
				defaultLabel = "Dev: Find resources now",
				action = (Action)delegate
				{
					((_003CCompGetGizmosExtra_003Ec__Iterator0)/*Error near IL_004e: stateMachine*/)._0024this.FoundMinerals();
				}
			};
			/*Error: Unable to find new state assignment for yield return*/;
		}
	}
}
