using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000502 RID: 1282
	public class Need_Outdoors : Need
	{
		// Token: 0x06001709 RID: 5897 RVA: 0x000CAFF4 File Offset: 0x000C93F4
		public Need_Outdoors(Pawn pawn) : base(pawn)
		{
			this.threshPercents = new List<float>();
			this.threshPercents.Add(0.8f);
			this.threshPercents.Add(0.6f);
			this.threshPercents.Add(0.4f);
			this.threshPercents.Add(0.2f);
			this.threshPercents.Add(0.05f);
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x0600170A RID: 5898 RVA: 0x000CB070 File Offset: 0x000C9470
		public override int GUIChangeArrow
		{
			get
			{
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x0600170B RID: 5899 RVA: 0x000CB090 File Offset: 0x000C9490
		public OutdoorsCategory CurCategory
		{
			get
			{
				OutdoorsCategory result;
				if (this.CurLevel > 0.8f)
				{
					result = OutdoorsCategory.Free;
				}
				else if (this.CurLevel > 0.6f)
				{
					result = OutdoorsCategory.NeedFreshAir;
				}
				else if (this.CurLevel > 0.4f)
				{
					result = OutdoorsCategory.CabinFeverLight;
				}
				else if (this.CurLevel > 0.2f)
				{
					result = OutdoorsCategory.CabinFeverSevere;
				}
				else if (this.CurLevel > 0.05f)
				{
					result = OutdoorsCategory.Trapped;
				}
				else
				{
					result = OutdoorsCategory.Entombed;
				}
				return result;
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x0600170C RID: 5900 RVA: 0x000CB11C File Offset: 0x000C951C
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x0600170D RID: 5901 RVA: 0x000CB13C File Offset: 0x000C953C
		private bool Disabled
		{
			get
			{
				return this.pawn.story.traits.HasTrait(TraitDefOf.Tunneler);
			}
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x000CB16B File Offset: 0x000C956B
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x000CB17C File Offset: 0x000C957C
		public override void NeedInterval()
		{
			if (this.Disabled)
			{
				this.CurLevel = 1f;
			}
			else if (!base.IsFrozen)
			{
				float b = 0.2f;
				bool flag = !this.pawn.Spawned || this.pawn.Position.UsesOutdoorTemperature(this.pawn.Map);
				RoofDef roofDef = (!this.pawn.Spawned) ? null : this.pawn.Position.GetRoof(this.pawn.Map);
				float num;
				if (!flag)
				{
					if (roofDef == null)
					{
						num = 2.5f;
					}
					else if (!roofDef.isThickRoof)
					{
						num = -0.3f;
					}
					else
					{
						num = -0.4f;
						b = 0f;
					}
				}
				else if (roofDef == null)
				{
					num = 5f;
				}
				else if (roofDef.isThickRoof)
				{
					num = -0.4f;
				}
				else
				{
					num = 0.7f;
				}
				if (this.pawn.InBed() && num < 0f)
				{
					num *= 0.25f;
				}
				num *= 0.0025f;
				float curLevel = this.CurLevel;
				if (num < 0f)
				{
					this.CurLevel = Mathf.Min(this.CurLevel, Mathf.Max(this.CurLevel + num, b));
				}
				else
				{
					this.CurLevel = Mathf.Min(this.CurLevel + num, 1f);
				}
				this.lastEffectiveDelta = this.CurLevel - curLevel;
			}
		}

		// Token: 0x04000D92 RID: 3474
		private const float Delta_IndoorsThickRoof = -0.4f;

		// Token: 0x04000D93 RID: 3475
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x04000D94 RID: 3476
		private const float Delta_IndoorsThinRoof = -0.3f;

		// Token: 0x04000D95 RID: 3477
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x04000D96 RID: 3478
		private const float Delta_OutdoorsThinRoof = 0.7f;

		// Token: 0x04000D97 RID: 3479
		private const float Delta_IndoorsNoRoof = 2.5f;

		// Token: 0x04000D98 RID: 3480
		private const float Delta_OutdoorsNoRoof = 5f;

		// Token: 0x04000D99 RID: 3481
		private const float DeltaFactor_InBed = 0.25f;

		// Token: 0x04000D9A RID: 3482
		private float lastEffectiveDelta = 0f;
	}
}
