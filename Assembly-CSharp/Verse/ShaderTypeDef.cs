using System;
using UnityEngine;

namespace Verse
{
	public class ShaderTypeDef : Def
	{
		[NoTranslate]
		public string shaderPath;

		[Unsaved]
		private Shader shaderInt;

		public ShaderTypeDef()
		{
		}

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
