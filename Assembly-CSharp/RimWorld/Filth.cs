using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006BF RID: 1727
	public class Filth : Thing
	{
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x06002543 RID: 9539 RVA: 0x0013FDF4 File Offset: 0x0013E1F4
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x0013FE48 File Offset: 0x0013E248
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x06002545 RID: 9541 RVA: 0x0013FE68 File Offset: 0x0013E268
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x06002546 RID: 9542 RVA: 0x0013FE90 File Offset: 0x0013E290
		public override string Label
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.Label);
				if (!this.sources.NullOrEmpty<string>())
				{
					stringBuilder.Append(" " + "OfLower".Translate() + " ");
					stringBuilder.Append(this.sources.ToCommaList(true));
				}
				stringBuilder.Append(" x" + this.thickness);
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x0013FF20 File Offset: 0x0013E320
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.thickness, "thickness", 1, false);
			Scribe_Values.Look<int>(ref this.growTick, "growTick", 0, false);
			if (Scribe.mode != LoadSaveMode.Saving || this.sources != null)
			{
				Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, new object[0]);
			}
		}

		// Token: 0x06002548 RID: 9544 RVA: 0x0013FF88 File Offset: 0x0013E388
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (Current.ProgramState == ProgramState.Playing)
			{
				base.Map.listerFilthInHomeArea.Notify_FilthSpawned(this);
			}
			if (!respawningAfterLoad)
			{
				this.growTick = Find.TickManager.TicksGame;
			}
			if (!base.Map.terrainGrid.TerrainAt(base.Position).acceptFilth)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002549 RID: 9545 RVA: 0x0013FFF8 File Offset: 0x0013E3F8
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x0600254A RID: 9546 RVA: 0x0014002C File Offset: 0x0013E42C
		public void AddSource(string newSource)
		{
			if (this.sources == null)
			{
				this.sources = new List<string>();
			}
			for (int i = 0; i < this.sources.Count; i++)
			{
				if (this.sources[i] == newSource)
				{
					return;
				}
			}
			while (this.sources.Count > 3)
			{
				this.sources.RemoveAt(0);
			}
			this.sources.Add(newSource);
		}

		// Token: 0x0600254B RID: 9547 RVA: 0x001400BC File Offset: 0x0013E4BC
		public void AddSources(IEnumerable<string> sources)
		{
			if (sources != null)
			{
				foreach (string newSource in sources)
				{
					this.AddSource(newSource);
				}
			}
		}

		// Token: 0x0600254C RID: 9548 RVA: 0x00140120 File Offset: 0x0013E520
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x0600254D RID: 9549 RVA: 0x0014016F File Offset: 0x0013E56F
		public void ThinFilth()
		{
			this.thickness--;
			if (base.Spawned)
			{
				if (this.thickness == 0)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				else
				{
					this.UpdateMesh();
				}
			}
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x001401AA File Offset: 0x0013E5AA
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x001401D0 File Offset: 0x0013E5D0
		public bool CanDropAt(IntVec3 c, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
			return terrainDef.acceptFilth && (!this.def.filth.terrainSourced || terrainDef.acceptTerrainSourceFilth);
		}

		// Token: 0x040014B9 RID: 5305
		public int thickness = 1;

		// Token: 0x040014BA RID: 5306
		public List<string> sources = null;

		// Token: 0x040014BB RID: 5307
		private int growTick;

		// Token: 0x040014BC RID: 5308
		private const int MaxThickness = 5;

		// Token: 0x040014BD RID: 5309
		private const int MinAgeToPickUp = 400;

		// Token: 0x040014BE RID: 5310
		private const int MaxNumSources = 3;
	}
}
