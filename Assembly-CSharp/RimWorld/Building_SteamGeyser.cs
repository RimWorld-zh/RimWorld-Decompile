using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020006CA RID: 1738
	public class Building_SteamGeyser : Building
	{
		// Token: 0x04001502 RID: 5378
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x04001503 RID: 5379
		public Building harvester = null;

		// Token: 0x04001504 RID: 5380
		private Sustainer spraySustainer = null;

		// Token: 0x04001505 RID: 5381
		private int spraySustainerStartTick = -999;

		// Token: 0x060025A7 RID: 9639 RVA: 0x0014297C File Offset: 0x00140D7C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this);
			this.steamSprayer.startSprayCallback = new Action(this.StartSpray);
			this.steamSprayer.endSprayCallback = new Action(this.EndSpray);
		}

		// Token: 0x060025A8 RID: 9640 RVA: 0x001429CC File Offset: 0x00140DCC
		private void StartSpray()
		{
			SnowUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x060025A9 RID: 9641 RVA: 0x00142A34 File Offset: 0x00140E34
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x060025AA RID: 9642 RVA: 0x00142A58 File Offset: 0x00140E58
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
	}
}
