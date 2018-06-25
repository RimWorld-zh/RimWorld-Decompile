using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B72 RID: 2930
	public class ScatterableDef : Def
	{
		// Token: 0x04002ADC RID: 10972
		[NoTranslate]
		public string texturePath;

		// Token: 0x04002ADD RID: 10973
		public float minSize;

		// Token: 0x04002ADE RID: 10974
		public float maxSize;

		// Token: 0x04002ADF RID: 10975
		public float selectionWeight = 100f;

		// Token: 0x04002AE0 RID: 10976
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04002AE1 RID: 10977
		public Material mat;

		// Token: 0x06003FF5 RID: 16373 RVA: 0x0021B7AC File Offset: 0x00219BAC
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.defName == "UnnamedDef")
			{
				this.defName = "Scatterable_" + this.texturePath;
			}
			LongEventHandler.ExecuteWhenFinished(delegate
			{
				this.mat = MaterialPool.MatFrom(this.texturePath, ShaderDatabase.Transparent);
			});
		}
	}
}
