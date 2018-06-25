using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200061E RID: 1566
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x0400126D RID: 4717
		private int ticksLeft;

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001FCD RID: 8141 RVA: 0x001127DC File Offset: 0x00110BDC
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001FCE RID: 8142 RVA: 0x001127FC File Offset: 0x00110BFC
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001FCF RID: 8143 RVA: 0x0011281C File Offset: 0x00110C1C
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001FD0 RID: 8144 RVA: 0x00112848 File Offset: 0x00110C48
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : this.ticksLeft;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x00112874 File Offset: 0x00110C74
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x00112898 File Offset: 0x00110C98
		public void Start(float? durationDays = null)
		{
			float num = (durationDays == null) ? this.Props.durationDays : durationDays.Value;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x001128DC File Offset: 0x00110CDC
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x001128E6 File Offset: 0x00110CE6
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x06001FD5 RID: 8149 RVA: 0x00112908 File Offset: 0x00110D08
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x00112924 File Offset: 0x00110D24
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x00112957 File Offset: 0x00110D57
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}
	}
}
