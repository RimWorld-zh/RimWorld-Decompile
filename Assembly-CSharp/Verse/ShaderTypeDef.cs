using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B70 RID: 2928
	public class ShaderTypeDef : Def
	{
		// Token: 0x170009C0 RID: 2496
		// (get) Token: 0x06003FF5 RID: 16373 RVA: 0x0021B460 File Offset: 0x00219860
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

		// Token: 0x04002ADB RID: 10971
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04002ADC RID: 10972
		[Unsaved]
		private Shader shaderInt;
	}
}
