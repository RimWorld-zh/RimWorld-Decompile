using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000628 RID: 1576
	public class TimeoutComp : WorldObjectComp
	{
		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x0600200E RID: 8206 RVA: 0x00113528 File Offset: 0x00111928
		public bool Active
		{
			get
			{
				return this.timeoutEndTick != -1;
			}
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x0600200F RID: 8207 RVA: 0x0011354C File Offset: 0x0011194C
		public bool Passed
		{
			get
			{
				return this.Active && Find.TickManager.TicksGame >= this.timeoutEndTick;
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x06002010 RID: 8208 RVA: 0x00113584 File Offset: 0x00111984
		private bool ShouldRemoveWorldObjectNow
		{
			get
			{
				return this.Passed && !base.ParentHasMap;
			}
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x06002011 RID: 8209 RVA: 0x001135B0 File Offset: 0x001119B0
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : (this.timeoutEndTick - Find.TickManager.TicksGame);
			}
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x001135E7 File Offset: 0x001119E7
		public void StartTimeout(int ticks)
		{
			this.timeoutEndTick = Find.TickManager.TicksGame + ticks;
		}

		// Token: 0x06002013 RID: 8211 RVA: 0x001135FC File Offset: 0x001119FC
		public override void CompTick()
		{
			base.CompTick();
			if (this.ShouldRemoveWorldObjectNow)
			{
				Find.WorldObjects.Remove(this.parent);
			}
		}

		// Token: 0x06002014 RID: 8212 RVA: 0x00113620 File Offset: 0x00111A20
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.timeoutEndTick, "timeoutEndTick", 0, false);
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x0011363C File Offset: 0x00111A3C
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

		// Token: 0x0400127A RID: 4730
		private int timeoutEndTick = -1;
	}
}
