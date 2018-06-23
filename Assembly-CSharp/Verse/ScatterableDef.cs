using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B6F RID: 2927
	public class ScatterableDef : Def
	{
		// Token: 0x04002AD5 RID: 10965
		[NoTranslate]
		public string texturePath;

		// Token: 0x04002AD6 RID: 10966
		public float minSize;

		// Token: 0x04002AD7 RID: 10967
		public float maxSize;

		// Token: 0x04002AD8 RID: 10968
		public float selectionWeight = 100f;

		// Token: 0x04002AD9 RID: 10969
		[NoTranslate]
		public string scatterType = "";

		// Token: 0x04002ADA RID: 10970
		public Material mat;

		// Token: 0x06003FF2 RID: 16370 RVA: 0x0021B3F0 File Offset: 0x002197F0
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
