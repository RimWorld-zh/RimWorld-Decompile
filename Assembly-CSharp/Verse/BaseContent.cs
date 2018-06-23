using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000ECE RID: 3790
	[StaticConstructorOnStartup]
	public static class BaseContent
	{
		// Token: 0x04003C06 RID: 15366
		public static readonly string BadTexPath = "UI/Misc/BadTexture";

		// Token: 0x04003C07 RID: 15367
		public static readonly Material BadMat = MaterialPool.MatFrom(BaseContent.BadTexPath, ShaderDatabase.Cutout);

		// Token: 0x04003C08 RID: 15368
		public static readonly Texture2D BadTex = ContentFinder<Texture2D>.Get(BaseContent.BadTexPath, true);

		// Token: 0x04003C09 RID: 15369
		public static readonly Graphic BadGraphic = GraphicDatabase.Get<Graphic_Single>(BaseContent.BadTexPath);

		// Token: 0x04003C0A RID: 15370
		public static readonly Texture2D BlackTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

		// Token: 0x04003C0B RID: 15371
		public static readonly Texture2D GreyTex = SolidColorMaterials.NewSolidColorTexture(Color.grey);

		// Token: 0x04003C0C RID: 15372
		public static readonly Texture2D WhiteTex = SolidColorMaterials.NewSolidColorTexture(Color.white);

		// Token: 0x04003C0D RID: 15373
		public static readonly Texture2D ClearTex = SolidColorMaterials.NewSolidColorTexture(Color.clear);

		// Token: 0x04003C0E RID: 15374
		public static readonly Texture2D YellowTex = SolidColorMaterials.NewSolidColorTexture(Color.yellow);

		// Token: 0x04003C0F RID: 15375
		public static readonly Material BlackMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.black, false);

		// Token: 0x04003C10 RID: 15376
		public static readonly Material WhiteMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.white, false);

		// Token: 0x04003C11 RID: 15377
		public static readonly Material ClearMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.clear, false);

		// Token: 0x060059E9 RID: 23017 RVA: 0x002E30A8 File Offset: 0x002E14A8
		public static bool NullOrBad(this Material mat)
		{
			return mat == null || mat == BaseContent.BadMat;
		}

		// Token: 0x060059EA RID: 23018 RVA: 0x002E30D8 File Offset: 0x002E14D8
		public static bool NullOrBad(this Texture2D tex)
		{
			return tex == null || tex == BaseContent.BadTex;
		}
	}
}
