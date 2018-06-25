using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020002E7 RID: 743
	public class TimeAssignmentDef : Def
	{
		// Token: 0x040007CC RID: 1996
		public Color color;

		// Token: 0x040007CD RID: 1997
		public bool allowRest = true;

		// Token: 0x040007CE RID: 1998
		public bool allowJoy = true;

		// Token: 0x040007CF RID: 1999
		private Texture2D colorTextureInt;

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0006CB48 File Offset: 0x0006AF48
		public Texture2D ColorTexture
		{
			get
			{
				if (this.colorTextureInt == null)
				{
					this.colorTextureInt = SolidColorMaterials.NewSolidColorTexture(this.color);
				}
				return this.colorTextureInt;
			}
		}
	}
}
