using System;

namespace Verse
{
	// Token: 0x02000B6C RID: 2924
	public class RoofDef : Def
	{
		// Token: 0x170009B9 RID: 2489
		// (get) Token: 0x06003FD9 RID: 16345 RVA: 0x0021A5FC File Offset: 0x002189FC
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}

		// Token: 0x04002ABC RID: 10940
		public bool isNatural = false;

		// Token: 0x04002ABD RID: 10941
		public bool isThickRoof = false;

		// Token: 0x04002ABE RID: 10942
		public ThingDef collapseLeavingThingDef = null;

		// Token: 0x04002ABF RID: 10943
		public ThingDef filthLeaving = null;

		// Token: 0x04002AC0 RID: 10944
		public SoundDef soundPunchThrough;
	}
}
