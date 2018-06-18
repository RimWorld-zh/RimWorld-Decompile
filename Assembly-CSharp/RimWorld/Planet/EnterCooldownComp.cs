using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000620 RID: 1568
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x001123D0 File Offset: 0x001107D0
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x001123F0 File Offset: 0x001107F0
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x00112410 File Offset: 0x00110810
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001FD6 RID: 8150 RVA: 0x0011243C File Offset: 0x0011083C
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : this.ticksLeft;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001FD7 RID: 8151 RVA: 0x00112468 File Offset: 0x00110868
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x0011248C File Offset: 0x0011088C
		public void Start(float? durationDays = null)
		{
			float num = (durationDays == null) ? this.Props.durationDays : durationDays.Value;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x001124D0 File Offset: 0x001108D0
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x001124DA File Offset: 0x001108DA
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x001124FC File Offset: 0x001108FC
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x00112518 File Offset: 0x00110918
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x0011254B File Offset: 0x0011094B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x0400126C RID: 4716
		private int ticksLeft;
	}
}
