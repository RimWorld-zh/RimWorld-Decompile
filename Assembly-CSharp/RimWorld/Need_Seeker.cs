using UnityEngine;
using Verse;

namespace RimWorld
{
	public abstract class Need_Seeker : Need
	{
		private const float GUIArrowTolerance = 0.05f;

		public override int GUIChangeArrow
		{
			get
			{
				int result;
				if (!base.pawn.Awake())
				{
					result = 0;
				}
				else
				{
					float curInstantLevelPercentage = base.CurInstantLevelPercentage;
					result = ((curInstantLevelPercentage > base.CurLevelPercentage + 0.05000000074505806) ? 1 : ((curInstantLevelPercentage < base.CurLevelPercentage - 0.05000000074505806) ? (-1) : 0));
				}
				return result;
			}
		}

		public Need_Seeker(Pawn pawn) : base(pawn)
		{
		}

		public override void NeedInterval()
		{
			if (!base.IsFrozen)
			{
				float curInstantLevel = this.CurInstantLevel;
				if (curInstantLevel > this.CurLevel)
				{
					this.CurLevel += (float)(base.def.seekerRisePerHour * 0.059999998658895493);
					this.CurLevel = Mathf.Min(this.CurLevel, curInstantLevel);
				}
				if (curInstantLevel < this.CurLevel)
				{
					this.CurLevel -= (float)(base.def.seekerFallPerHour * 0.059999998658895493);
					this.CurLevel = Mathf.Max(this.CurLevel, curInstantLevel);
				}
			}
		}
	}
}
