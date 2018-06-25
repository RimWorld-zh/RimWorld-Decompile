using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200041D RID: 1053
	public class CompPowerPlantSteam : CompPowerPlant
	{
		// Token: 0x04000B0E RID: 2830
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04000B0F RID: 2831
		private Building_SteamGeyser geyser = null;

		// Token: 0x06001240 RID: 4672 RVA: 0x0009E63A File Offset: 0x0009CA3A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this.parent);
		}

		// Token: 0x06001241 RID: 4673 RVA: 0x0009E658 File Offset: 0x0009CA58
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

		// Token: 0x06001242 RID: 4674 RVA: 0x0009E6D5 File Offset: 0x0009CAD5
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			if (this.geyser != null)
			{
				this.geyser.harvester = null;
			}
		}
	}
}
