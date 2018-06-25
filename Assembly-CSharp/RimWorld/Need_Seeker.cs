using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000506 RID: 1286
	public abstract class Need_Seeker : Need
	{
		// Token: 0x04000DB4 RID: 3508
		private const float GUIArrowTolerance = 0.05f;

		// Token: 0x0600171C RID: 5916 RVA: 0x000C9A2E File Offset: 0x000C7E2E
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600171D RID: 5917 RVA: 0x000C9A38 File Offset: 0x000C7E38
		public override int GUIChangeArrow
		{
			get
			{
				int result;
				if (!this.pawn.Awake())
				{
					result = 0;
				}
				else
				{
					float curInstantLevelPercentage = base.CurInstantLevelPercentage;
					if (curInstantLevelPercentage > base.CurLevelPercentage + 0.05f)
					{
						result = 1;
					}
					else if (curInstantLevelPercentage < base.CurLevelPercentage - 0.05f)
					{
						result = -1;
					}
					else
					{
						result = 0;
					}
				}
				return result;
			}
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x000C9AA0 File Offset: 0x000C7EA0
		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				float curInstantLevel = this.CurInstantLevel;
				if (curInstantLevel > this.CurLevel)
				{
					this.CurLevel += this.def.seekerRisePerHour * 0.06f;
					this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
				}
				if (curInstantLevel < this.CurLevel)
				{
					this.CurLevel -= this.def.seekerFallPerHour * 0.06f;
					this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);
				}
			}
		}
	}
}
