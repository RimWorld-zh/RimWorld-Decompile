using System;
using Verse;

namespace RimWorld.Planet
{
	public class SitePart : SiteCoreOrPartBase
	{
		public SitePartDef def;

		public SitePart()
		{
		}

		public SitePart(SitePartDef def, SiteCoreOrPartParams parms)
		{
			this.def = def;
			this.parms = parms;
		}

		public override SiteCoreOrPartDefBase Def
		{
			get
			{
				return this.def;
			}
		}

		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<SitePartDef>(ref this.def, "def");
		}
	}
}
