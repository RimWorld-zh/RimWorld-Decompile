using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED1 RID: 3793
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x04003C0E RID: 15374
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x04003C0F RID: 15375
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x04003C10 RID: 15376
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x04003C11 RID: 15377
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x04003C12 RID: 15378
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04003C13 RID: 15379
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x04003C14 RID: 15380
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04003C15 RID: 15381
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x04003C16 RID: 15382
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x04003C17 RID: 15383
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x04003C18 RID: 15384
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x04003C19 RID: 15385
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);

		// Token: 0x060059EC RID: 23020 RVA: 0x002E33E8 File Offset: 0x002E17E8
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x060059ED RID: 23021 RVA: 0x002E3418 File Offset: 0x002E1818
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}
	}
}
