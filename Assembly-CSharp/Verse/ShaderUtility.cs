using UnityEngine;

namespace Verse
{
	public static class ShaderUtility
	{
		public static bool SupportsMaskTex(this Shader shader)
		{
			return (Object)shader == (Object)ShaderDatabase.CutoutComplex;
		}
	}
}
