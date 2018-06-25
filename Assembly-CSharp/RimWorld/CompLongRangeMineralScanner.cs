using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200071B RID: 1819
	public class CompLongRangeMineralScanner : ThingComp
	{
		// Token: 0x040015EC RID: 5612
		private CompPowerTrader powerComp;

		// Token: 0x040015ED RID: 5613
		private List<Pair<Vector3, float>> otherActiveMineralScanners = new List<Pair<Vector3, float>>();

		// Token: 0x040015EE RID: 5614
		private float cachedEffectiveAreaPct;

		// Token: 0x040015EF RID: 5615
		private const float NoSitePartChance = 0.6f;

		// Token: 0x040015F0 RID: 5616
		private static readonly string MineralScannerPreciousLumpThreatTag = "MineralScannerPreciousLumpThreat";

		// Token: 0x1700061B RID: 1563
		// (get) Token: 0x06002815 RID: 10261 RVA: 0x00156EB4 File Offset: 0x001552B4
		public CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return (CompProperties_LongRangeMineralScanner)this.props;
			}
		}

		// Token: 0x1700061C RID: 1564
		// (get) Token: 0x06002816 RID: 10262 RVA: 0x00156ED4 File Offset: 0x001552D4
		public bool Active
		{
			get
			{
				return this.parent.Spawned && (this.powerComp == null || this.powerComp.PowerOn) && this.parent.Faction == Faction.OfPlayer;
			}
		}

		// Token: 0x1700061D RID: 1565
		// (get) Token: 0x06002817 RID: 10263 RVA: 0x00156F34 File Offset: 0x00155334
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

		// Token: 0x1700061E RID: 1566
		// (get) Token: 0x06002818 RID: 10264 RVA: 0x00156F78 File Offset: 0x00155378
		private float EffectiveAreaPct
		{
			get
			{
				return this.cachedEffectiveAreaPct;
			}
		}

		// Token: 0x06002819 RID: 10265 RVA: 0x00156F93 File Offset: 0x00155393
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.RecacheEffectiveAreaPct();
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x00156FB4 File Offset: 0x001553B4
		public override void PostExposeData()
		{
			base.PostExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.RecacheEffectiveAreaPct();
			}
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x00156FCE File Offset: 0x001553CE
		public override void CompTickRare()
		{
			base.CompTickRare();
			this.RecacheEffectiveAreaPct();
			this.CheckTryFindMinerals(250);
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x00156FE8 File Offset: 0x001553E8
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

		// Token: 0x0600281D RID: 10269 RVA: 0x00157124 File Offset: 0x00155524
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

		// Token: 0x0600281E RID: 10270 RVA: 0x00157174 File Offset: 0x00155574
		private void FoundMinerals()
		{
			int tile2;
			ref int tile = ref tile2;
			int tile3 = this.parent.Tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, true, tile3))
			{
				Site site = SiteMaker.TryMakeSite_SingleSitePart(SiteCoreDefOf.PreciousLump, (!Rand.Chance(0.6f)) ? CompLongRangeMineralScanner.MineralScannerPreciousLumpThreatTag : null, null, true, null, true);
				if (site != null)
				{
					site.Tile = tile2;
					Find.WorldObjects.Add(site);
					Find.LetterStack.ReceiveLetter("LetterLabelFoundPreciousLump".Translate(), "LetterFoundPreciousLump".Translate(), LetterDefOf.PositiveEvent, site, null, null);
				}
			}
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x00157218 File Offset: 0x00155618
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

		// Token: 0x06002820 RID: 10272 RVA: 0x001572E4 File Offset: 0x001556E4
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

		// Token: 0x06002821 RID: 10273 RVA: 0x0015736C File Offset: 0x0015576C
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

		// Token: 0x06002822 RID: 10274 RVA: 0x001573B8 File Offset: 0x001557B8
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
	}
}
