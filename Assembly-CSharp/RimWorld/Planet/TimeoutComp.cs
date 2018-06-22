using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000624 RID: 1572
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002003 RID: 8195 RVA: 0x001134A8 File Offset: 0x001118A8
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002004 RID: 8196 RVA: 0x001134CC File Offset: 0x001118CC
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002005 RID: 8197 RVA: 0x00113504 File Offset: 0x00111904
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002006 RID: 8198 RVA: 0x00113530 File Offset: 0x00111930
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : (this.timeoutEndTick - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06002007 RID: 8199 RVA: 0x00113567 File Offset: 0x00111967
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06002008 RID: 8200 RVA: 0x0011357C File Offset: 0x0011197C
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				Find.WorldObjects.Remove(this.parent);
			}
		}

		// Token: 0x06002009 RID: 8201 RVA: 0x001135A0 File Offset: 0x001119A0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x001135BC File Offset: 0x001119BC
		public override string CompInspectStringExtra()
		{
			string result;
			if (this.Active && !base.ParentHasMap)
			{
				result = "WorldObjectTimeout".Translate(new object[]
				{
					this.TicksLeft.ToStringTicksToPeriod()
				});
			}
			else
			{
				result = null;
			}
			return result;
		}

		// Token: 0x04001274 RID: 4724
		private int timeoutEndTick = -1;
	}
}
