using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061C RID: 1564
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001FCA RID: 8138 RVA: 0x00112424 File Offset: 0x00110824
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001FCB RID: 8139 RVA: 0x00112444 File Offset: 0x00110844
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001FCC RID: 8140 RVA: 0x00112464 File Offset: 0x00110864
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x00112490 File Offset: 0x00110890
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : this.ticksLeft;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x001124BC File Offset: 0x001108BC
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x001124E0 File Offset: 0x001108E0
		public void Start(float? durationDays = null)
		{
			float num = (durationDays == null) ? this.Props.durationDays : durationDays.Value;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x00112524 File Offset: 0x00110924
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x0011252E File Offset: 0x0011092E
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00112550 File Offset: 0x00110950
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x0011256C File Offset: 0x0011096C
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x0011259F File Offset: 0x0011099F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x04001269 RID: 4713
		private int ticksLeft;
	}
}
