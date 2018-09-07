using System;
using RimWorld;

namespace Verse
{
	public class GenStepDef : Def
	{
		public SiteCoreOrPartDefBase linkWithSite;

		public float order;

		public GenStep genStep;

		public GenStepDef()
		{
		}

		public override void PostLoad()
		{
			base.PostLoad();
			this.genStep.def = this;
		}
	}
}
