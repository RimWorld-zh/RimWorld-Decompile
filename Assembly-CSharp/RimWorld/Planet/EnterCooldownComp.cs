using System;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02000620 RID: 1568
	public class EnterCooldownComp : WorldObjectComp
	{
		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06001FD1 RID: 8145 RVA: 0x00112358 File Offset: 0x00110758
		public WorldObjectCompProperties_EnterCooldown Props
		{
			get
			{
				return (WorldObjectCompProperties_EnterCooldown)this.props;
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x06001FD2 RID: 8146 RVA: 0x00112378 File Offset: 0x00110778
		public bool Active
		{
			get
			{
				return this.ticksLeft > 0;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x06001FD3 RID: 8147 RVA: 0x00112398 File Offset: 0x00110798
		public bool BlocksEntering
		{
			get
			{
				return this.Active && !base.ParentHasMap;
			}
		}

		// Token: 0x170004BF RID: 1215
		// (get) Token: 0x06001FD4 RID: 8148 RVA: 0x001123C4 File Offset: 0x001107C4
		public int TicksLeft
		{
			get
			{
				return (!this.Active) ? 0 : this.ticksLeft;
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06001FD5 RID: 8149 RVA: 0x001123F0 File Offset: 0x001107F0
		public float DaysLeft
		{
			get
			{
				return (float)this.TicksLeft / 60000f;
			}
		}

		// Token: 0x06001FD6 RID: 8150 RVA: 0x00112414 File Offset: 0x00110814
		public void Start(float? durationDays = null)
		{
			float num = (durationDays == null) ? this.Props.durationDays : durationDays.Value;
			this.ticksLeft = Mathf.RoundToInt(num * 60000f);
		}

		// Token: 0x06001FD7 RID: 8151 RVA: 0x00112458 File Offset: 0x00110858
		public void Stop()
		{
			this.ticksLeft = 0;
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x00112462 File Offset: 0x00110862
		public override void CompTick()
		{
			base.CompTick();
			if (this.Active)
			{
				this.ticksLeft--;
			}
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x00112484 File Offset: 0x00110884
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			if (this.Active)
			{
				this.Stop();
			}
		}

		// Token: 0x06001FDA RID: 8154 RVA: 0x001124A0 File Offset: 0x001108A0
		public override void PostMyMapRemoved()
		{
			base.PostMyMapRemoved();
			if (this.Props.autoStartOnMapRemoved)
			{
				this.Start(null);
			}
		}

		// Token: 0x06001FDB RID: 8155 RVA: 0x001124D3 File Offset: 0x001108D3
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ticksLeft, "ticksLeft", 0, false);
		}

		// Token: 0x0400126C RID: 4716
		private int ticksLeft;
	}
}
