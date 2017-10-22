using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnCapacityFactor
	{
		private const float MaxReciprocal = 5f;

		public PawnCapacityDef capacity;

		public float weight = 1f;

		public bool useReciprocal;

		public float max = 9999f;

		public float GetFactor(float capacityEfficiency)
		{
			float num = capacityEfficiency;
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
