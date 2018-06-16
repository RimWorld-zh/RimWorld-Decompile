using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ED0 RID: 3792
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x060059CA RID: 22986 RVA: 0x002E11BC File Offset: 0x002DF5BC
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x060059CB RID: 22987 RVA: 0x002E11EC File Offset: 0x002DF5EC
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}

		// Token: 0x04003BF7 RID: 15351
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x04003BF8 RID: 15352
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x04003BF9 RID: 15353
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x04003BFA RID: 15354
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x04003BFB RID: 15355
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04003BFC RID: 15356
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x04003BFD RID: 15357
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04003BFE RID: 15358
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x04003BFF RID: 15359
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x04003C00 RID: 15360
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x04003C01 RID: 15361
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x04003C02 RID: 15362
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);
	}
}
