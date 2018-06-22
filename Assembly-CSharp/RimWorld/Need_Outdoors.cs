using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020004FE RID: 1278
	public class Need_Outdoors : Need
	{
		// Token: 0x06001700 RID: 5888 RVA: 0x000CAFEC File Offset: 0x000C93EC
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
		// (get) Token: 0x06001701 RID: 5889 RVA: 0x000CB068 File Offset: 0x000C9468
		public override int GUIChangeArrow
		{
			get
			{
				return Math.Sign(this.lastEffectiveDelta);
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001702 RID: 5890 RVA: 0x000CB088 File Offset: 0x000C9488
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
		// (get) Token: 0x06001703 RID: 5891 RVA: 0x000CB114 File Offset: 0x000C9514
		public override bool ShowOnNeedList
		{
			get
			{
				return !this.Disabled;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001704 RID: 5892 RVA: 0x000CB134 File Offset: 0x000C9534
		private bool Disabled
		{
			get
			{
				return this.pawn.story.traits.HasTrait(TraitDefOf.Tunneler);
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000CB163 File Offset: 0x000C9563
		public override void SetInitialLevel()
		{
			this.CurLevel = 1f;
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x000CB174 File Offset: 0x000C9574
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

		// Token: 0x04000D8F RID: 3471
		private const float Delta_IndoorsThickRoof = -0.4f;

		// Token: 0x04000D90 RID: 3472
		private const float Delta_OutdoorsThickRoof = -0.4f;

		// Token: 0x04000D91 RID: 3473
		private const float Delta_IndoorsThinRoof = -0.3f;

		// Token: 0x04000D92 RID: 3474
		private const float Minimum_IndoorsThinRoof = 0.2f;

		// Token: 0x04000D93 RID: 3475
		private const float Delta_OutdoorsThinRoof = 0.7f;

		// Token: 0x04000D94 RID: 3476
		private const float Delta_IndoorsNoRoof = 2.5f;

		// Token: 0x04000D95 RID: 3477
		private const float Delta_OutdoorsNoRoof = 5f;

		// Token: 0x04000D96 RID: 3478
		private const float DeltaFactor_InBed = 0.25f;

		// Token: 0x04000D97 RID: 3479
		private float lastEffectiveDelta = 0f;
	}
}
