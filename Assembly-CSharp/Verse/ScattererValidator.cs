using System;

namespace Verse
{
	public abstract class ScattererValidator
	{
		protected ScattererValidator()
		{
		}

		public abstract bool Allows(IntVec3 c, Map map);
	}
}
