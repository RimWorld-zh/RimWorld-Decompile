using System;

namespace Verse
{
	public class WorldGenStepDef : Def
	{
		public float order;

		public WorldGenStep worldGenStep;

		public WorldGenStepDef()
		{
		}

		public override void PostLoad()
		{
			base.PostLoad();
			this.worldGenStep.def = this;
		}
	}
}
