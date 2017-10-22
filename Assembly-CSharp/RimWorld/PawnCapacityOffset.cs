using UnityEngine;
using Verse;

namespace RimWorld
{
	public class PawnCapacityOffset
	{
		public PawnCapacityDef capacity;

		public float scale = 1f;

		public float max = 9999f;

		public float GetOffset(float capacityEfficiency)
		{
			return (float)((Mathf.Min(capacityEfficiency, this.max) - 1.0) * this.scale);
		}
	}
}
