using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000626 RID: 1574
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x04001274 RID: 4724
		private int timeoutEndTick = -1;

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002007 RID: 8199 RVA: 0x001135F8 File Offset: 0x001119F8
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x0011361C File Offset: 0x00111A1C
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x00113654 File Offset: 0x00111A54
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x0600200A RID: 8202 RVA: 0x00113680 File Offset: 0x00111A80
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : (this.timeoutEndTick - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x001136B7 File Offset: 0x00111AB7
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x001136CC File Offset: 0x00111ACC
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				Find.WorldObjects.Remove(this.parent);
			}
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x001136F0 File Offset: 0x00111AF0
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x0600200E RID: 8206 RVA: 0x0011370C File Offset: 0x00111B0C
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
	}
}
