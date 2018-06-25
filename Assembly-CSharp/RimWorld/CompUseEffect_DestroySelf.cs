using System;
using Verse;

namespace RimWorld
{
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		public CompUseEffect_DestroySelf()
		{
		}

		public override float OrderPriority
		{
			get
			{
				return -1000f;
			}
		}

		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}
	}
}
