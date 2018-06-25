using System;

namespace Verse
{
	public class RoofDef : Def
	{
		public bool isNatural = false;

		public bool isThickRoof = false;

		public ThingDef collapseLeavingThingDef = null;

		public ThingDef filthLeaving = null;

		public SoundDef soundPunchThrough;

		public RoofDef()
		{
		}

		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}
	}
}
