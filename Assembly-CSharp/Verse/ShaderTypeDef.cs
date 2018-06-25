using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B73 RID: 2931
	public class ShaderTypeDef : Def
	{
		// Token: 0x04002AE2 RID: 10978
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04002AE3 RID: 10979
		[Unsaved]
		private Shader shaderInt;

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x0021B81C File Offset: 0x00219C1C
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
	}
}
