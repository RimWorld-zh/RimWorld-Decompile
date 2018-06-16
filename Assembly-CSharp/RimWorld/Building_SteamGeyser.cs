using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006CE RID: 1742
	public class Building_SteamGeyser : Building
	{
		// Token: 0x060025AD RID: 9645 RVA: 0x001427B8 File Offset: 0x00140BB8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this);
			this.steamSprayer.startSprayCallback = new Action(this.StartSpray);
			this.steamSprayer.endSprayCallback = new Action(this.EndSpray);
		}

		// Token: 0x060025AE RID: 9646 RVA: 0x00142808 File Offset: 0x00140C08
		private void StartSpray()
		{
			SnowUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060025AF RID: 9647 RVA: 0x00142870 File Offset: 0x00140C70
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x060025B0 RID: 9648 RVA: 0x00142894 File Offset: 0x00140C94
		public override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null)
			{
				if (Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
				{
					Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.", false);
					this.spraySustainer.End();
					this.spraySustainer = null;
				}
			}
		}

		// Token: 0x04001504 RID: 5380
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04001505 RID: 5381
		public Building harvester = null;

		// Token: 0x04001506 RID: 5382
		private Sustainer spraySustainer = null;

		// Token: 0x04001507 RID: 5383
		private int spraySustainerStartTick = -999;
	}
}
