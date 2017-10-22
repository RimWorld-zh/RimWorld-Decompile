using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnCapacityFactor
	{
		public PawnCapacityDef capacity;

		public float weight = 1f;

		public float max = 9999f;

		public bool useReciprocal = false;

		public float allowedDefect;

		private const float MaxReciprocalFactor = 5f;

		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
			if (this.allowedDefect != 0.0 && num < 1.0)
			{
				num = Mathf.InverseLerp(0f, (float)(1.0 - this.allowedDefect), num);
			}
			if (num > this.max)
			{
				num = this.max;
			}
			if (this.useReciprocal)
			{
				num = (float)((!(Mathf.Abs(num) < 0.0010000000474974513)) ? Mathf.Min((float)(1.0 / num), 5f) : 5.0);
			}
			return num;
		}
	}
}
