using System;
using RimWorld;

namespace Verse
{
	public class GenStepDef : Def
	{
		public SiteDefBase linkWithSite;

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
