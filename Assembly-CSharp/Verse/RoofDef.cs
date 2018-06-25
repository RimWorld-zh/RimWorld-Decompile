using System;

namespace Verse
{
	// Token: 0x02000B6B RID: 2923
	public class RoofDef : Def
	{
		// Token: 0x04002AC4 RID: 10948
		public bool isNatural = false;

		// Token: 0x04002AC5 RID: 10949
		public bool isThickRoof = false;

		// Token: 0x04002AC6 RID: 10950
		public ThingDef collapseLeavingThingDef = null;

		// Token: 0x04002AC7 RID: 10951
		public ThingDef filthLeaving = null;

		// Token: 0x04002AC8 RID: 10952
		public SoundDef soundPunchThrough;

		// Token: 0x170009BA RID: 2490
		// (get) Token: 0x06003FDF RID: 16351 RVA: 0x0021B0F4 File Offset: 0x002194F4
		public bool VanishOnCollapse
		{
			get
			{
				return !this.isThickRoof;
			}
		}
	}
}
