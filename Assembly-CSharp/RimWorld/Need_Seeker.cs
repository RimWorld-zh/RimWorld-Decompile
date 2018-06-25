using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000506 RID: 1286
	public abstract class Need_Seeker : Need
	{
		// Token: 0x04000DB7 RID: 3511
		private const float GUIArrowTolerance = 0.05f;

		// Token: 0x0600171B RID: 5915 RVA: 0x000C9C2E File Offset: 0x000C802E
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x0600171C RID: 5916 RVA: 0x000C9C38 File Offset: 0x000C8038
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

		// Token: 0x0600171D RID: 5917 RVA: 0x000C9CA0 File Offset: 0x000C80A0
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
