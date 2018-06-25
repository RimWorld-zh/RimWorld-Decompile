using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000B72 RID: 2930
	public class ShaderTypeDef : Def
	{
		// Token: 0x04002ADB RID: 10971
		[NoTranslate]
		public string shaderPath;

		// Token: 0x04002ADC RID: 10972
		[Unsaved]
		private Shader shaderInt;

		// Token: 0x170009BF RID: 2495
		// (get) Token: 0x06003FF8 RID: 16376 RVA: 0x0021B53C File Offset: 0x0021993C
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
