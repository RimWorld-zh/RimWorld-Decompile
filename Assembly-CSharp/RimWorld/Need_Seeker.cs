using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000508 RID: 1288
	public abstract class Need_Seeker : Need
	{
		// Token: 0x06001721 RID: 5921 RVA: 0x000C98E6 File Offset: 0x000C7CE6
		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001722 RID: 5922 RVA: 0x000C98F0 File Offset: 0x000C7CF0
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

		// Token: 0x06001723 RID: 5923 RVA: 0x000C9958 File Offset: 0x000C7D58
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

		// Token: 0x04000DB7 RID: 3511
		private const float GUIArrowTolerance = 0.05f;
	}
}
