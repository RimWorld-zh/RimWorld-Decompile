using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	public class Filth : Thing
	{
		public int thickness = 1;

		public List<string> sources = null;

		private int growTick;

		private const int MaxThickness = 5;

		private const int MinAgeToPickUp = 400;

		private const int MaxNumSources = 3;

		public bool CanFilthAttachNow
		{
			get
			{
				return base.def.filth.canFilthAttach && this.thickness > 1 && Find.TickManager.TicksGame - this.growTick > 400;
			}
		}

		public bool CanBeThickened
		{
			get
			{
				return this.thickness < 5;
			}
		}

		public int TicksSinceThickened
		{
			get
			{
				return Find.TickManager.TicksGame - this.growTick;
			}
		}

		public override string Label
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.Label);
				if (!this.sources.NullOrEmpty())
				{
					stringBuilder.Append(" " + "OfLower".Translate() + " ");
					stringBuilder.Append(GenText.ToCommaList(this.sources, true));
				}
				stringBuilder.Append(" x" + this.thickness);
				return stringBuilder.ToString();
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.thickness, "thickness", 1, false);
			Scribe_Values.Look<int>(ref this.growTick, "growTick", 0, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.sources == null)
				return;
			Scribe_Collections.Look<string>(ref this.sources, "sources", LookMode.Value, new object[0]);
		}

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

		public override void DeSpawn()
		{
			Map map = base.Map;
			base.DeSpawn();
			if (Current.ProgramState == ProgramState.Playing)
			{
				map.listerFilthInHomeArea.Notify_FilthDespawned(this);
			}
		}

		public void AddSource(string newSource)
		{
			if (this.sources == null)
			{
				this.sources = new List<string>();
			}
			int num = 0;
			while (num < this.sources.Count)
			{
				if (!(this.sources[num] == newSource))
				{
					num++;
					continue;
				}
				return;
			}
			while (this.sources.Count > 3)
			{
				this.sources.RemoveAt(0);
			}
			this.sources.Add(newSource);
		}

		public void AddSources(IEnumerable<string> sources)
		{
			if (sources != null)
			{
				foreach (string item in sources)
				{
					this.AddSource(item);
				}
			}
		}

		public virtual void ThickenFilth()
		{
			this.growTick = Find.TickManager.TicksGame;
			if (this.thickness < base.def.filth.maxThickness)
			{
				this.thickness++;
				this.UpdateMesh();
			}
		}

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

		private void UpdateMesh()
		{
			if (base.Spawned)
			{
				base.Map.mapDrawer.MapMeshDirty(base.Position, MapMeshFlag.Things);
			}
		}

		public bool CanDropAt(IntVec3 c, Map map)
		{
			TerrainDef terrainDef = map.terrainGrid.TerrainAt(c);
			return (byte)(terrainDef.acceptFilth ? ((!base.def.filth.terrainSourced || terrainDef.acceptTerrainSourceFilth) ? 1 : 0) : 0) != 0;
		}
	}
}
