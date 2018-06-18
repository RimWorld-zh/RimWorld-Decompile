using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020006C3 RID: 1731
	public class Filth : Thing
	{
		// Token: 0x1700059D RID: 1437
		// (get) Token: 0x0600254B RID: 9547 RVA: 0x0013FCA8 File Offset: 0x0013E0A8
		public bool CanFilthAttachNow
		{
			get
			{
				return this.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		// Token: 0x1700059E RID: 1438
		// (get) Token: 0x0600254C RID: 9548 RVA: 0x0013FCFC File Offset: 0x0013E0FC
		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		// Token: 0x1700059F RID: 1439
		// (get) Token: 0x0600254D RID: 9549 RVA: 0x0013FD1C File Offset: 0x0013E11C
		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		// Token: 0x170005A0 RID: 1440
		// (get) Token: 0x0600254E RID: 9550 RVA: 0x0013FD44 File Offset: 0x0013E144
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

		// Token: 0x0600254F RID: 9551 RVA: 0x0013FDD4 File Offset: 0x0013E1D4
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

		// Token: 0x06002550 RID: 9552 RVA: 0x0013FE3C File Offset: 0x0013E23C
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

		// Token: 0x06002551 RID: 9553 RVA: 0x0013FEAC File Offset: 0x0013E2AC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x0013FEE0 File Offset: 0x0013E2E0
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

		// Token: 0x06002553 RID: 9555 RVA: 0x0013FF70 File Offset: 0x0013E370
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

		// Token: 0x06002554 RID: 9556 RVA: 0x0013FFD4 File Offset: 0x0013E3D4
		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < this.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00140023 File Offset: 0x0013E423
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

		// Token: 0x06002556 RID: 9558 RVA: 0x0014005E File Offset: 0x0013E45E
		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x00140084 File Offset: 0x0013E484
		public bool CanDropAt(IntVec3 c, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
			return terrainDef.acceptFilth && (!this.def.filth.terrainSourced || terrainDef.acceptTerrainSourceFilth);
		}

		// Token: 0x040014BB RID: 5307
		public int thickness = 1;

		// Token: 0x040014BC RID: 5308
		public List<string> sources = null;

		// Token: 0x040014BD RID: 5309
		private int growTick;

		// Token: 0x040014BE RID: 5310
		private const int MaxThickness = 5;

		// Token: 0x040014BF RID: 5311
		private const int MinAgeToPickUp = 400;

		// Token: 0x040014C0 RID: 5312
		private const int MaxNumSources = 3;
	}
}
