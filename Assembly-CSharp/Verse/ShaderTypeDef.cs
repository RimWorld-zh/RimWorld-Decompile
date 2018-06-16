using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B74 RID: 2932
	public class ShaderTypeDef : Def
	{
		// Token: 0x170009BE RID: 2494
		// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x0021ACF0 File Offset: 0x002190F0
		public Shader Shader
		{
			get
			{
				if (this.shaderInt == null)
				{
					this.shaderInt = ShaderDatabase.LoadShader(this.shaderPath);
				}
				return this.shaderInt;
			}
		}

		// Token: 0x04002AD6 RID: 10966
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04002AD7 RID: 10967
		[Unsaved]
		private Shader shaderInt;
	}
}
