using System;
using System.Collections.Generic;

namespace Verse
{
	public class EffecterDef : Def
	{
		public List<SubEffecterDef> children = null;

		public float positionRadius;

		public FloatRange offsetTowardsTarget;

		public EffecterDef()
		{
		}

		public Effecter Spawn()
		{
			return new Effecter(this);
		}
	}
}
