using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000626 RID: 1574
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x04001278 RID: 4728
		private int timeoutEndTick = -1;

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x06002006 RID: 8198 RVA: 0x00113860 File Offset: 0x00111C60
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x06002007 RID: 8199 RVA: 0x00113884 File Offset: 0x00111C84
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002008 RID: 8200 RVA: 0x001138BC File Offset: 0x00111CBC
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002009 RID: 8201 RVA: 0x001138E8 File Offset: 0x00111CE8
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : (this.timeoutEndTick - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x0600200A RID: 8202 RVA: 0x0011391F File Offset: 0x00111D1F
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x0600200B RID: 8203 RVA: 0x00113934 File Offset: 0x00111D34
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				Find.WorldObjects.Remove(this.parent);
			}
		}

		// Token: 0x0600200C RID: 8204 RVA: 0x00113958 File Offset: 0x00111D58
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x0600200D RID: 8205 RVA: 0x00113974 File Offset: 0x00111D74
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
