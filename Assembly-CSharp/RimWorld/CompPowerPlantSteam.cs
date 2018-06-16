using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200041B RID: 1051
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x0600123C RID: 4668 RVA: 0x0009E306 File Offset: 0x0009C706
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x0600123D RID: 4669 RVA: 0x0009E324 File Offset: 0x0009C724
		public override void CompTick()
		{
			base.CompTick();
			if (this.geyser == null)
			{
				this.geyser = (Building_SteamGeyser)this.parent.Map.thingGrid.ThingAt(this.parent.Position, ThingDefOf.SteamGeyser);
			}
			if (this.geyser != null)
			{
				this.geyser.harvester = (Building)this.parent;
				this.steamSprayer.SteamSprayerTick();
			}
		}

		// Token: 0x0600123E RID: 4670 RVA: 0x0009E3A1 File Offset: 0x0009C7A1
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}

		// Token: 0x04000B0D RID: 2829
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04000B0E RID: 2830
		private Building_SteamGeyser geyser = null;
	}
}
