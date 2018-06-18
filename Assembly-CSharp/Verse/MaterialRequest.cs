using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D74 RID: 3444
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x06004D1A RID: 19738 RVA: 0x00282229 File Offset: 0x00280629
		public MaterialRequest(Texture2D tex)
		{
			this.shader = ShaderDatabase.Cutout;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06004D1B RID: 19739 RVA: 0x00282269 File Offset: 0x00280669
		public MaterialRequest(Texture2D tex, Shader shader)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = Color.white;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x06004D1C RID: 19740 RVA: 0x002822A5 File Offset: 0x002806A5
		public MaterialRequest(Texture2D tex, Shader shader, Color color)
		{
			this.shader = shader;
			this.mainTex = tex;
			this.color = color;
			this.colorTwo = Color.white;
			this.maskTex = null;
			this.renderQueue = 0;
			this.shaderParameters = null;
		}

		// Token: 0x17000C83 RID: 3203
		// (set) Token: 0x06004D1D RID: 19741 RVA: 0x002822DD File Offset: 0x002806DD
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06004D1E RID: 19742 RVA: 0x002822F0 File Offset: 0x002806F0
		public override int GetHashCode()
		{
			int seed = 0;
			seed = Gen.HashCombine<Shader>(seed, this.shader);
			seed = Gen.HashCombineStruct<Color>(seed, this.color);
			seed = Gen.HashCombineStruct<Color>(seed, this.colorTwo);
			seed = Gen.HashCombine<Texture2D>(seed, this.mainTex);
			seed = Gen.HashCombine<Texture2D>(seed, this.maskTex);
			seed = Gen.HashCombineInt(seed, this.renderQueue);
			return Gen.HashCombine<List<ShaderParameter>>(seed, this.shaderParameters);
		}

		// Token: 0x06004D1F RID: 19743 RVA: 0x00282364 File Offset: 0x00280764
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06004D20 RID: 19744 RVA: 0x00282398 File Offset: 0x00280798
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06004D21 RID: 19745 RVA: 0x00282444 File Offset: 0x00280844
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004D22 RID: 19746 RVA: 0x00282464 File Offset: 0x00280864
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06004D23 RID: 19747 RVA: 0x00282484 File Offset: 0x00280884
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"MaterialRequest(",
				this.shader.name,
				", ",
				this.mainTex.name,
				", ",
				this.color.ToString(),
				", ",
				this.colorTwo.ToString(),
				", ",
				this.maskTex.ToString(),
				", ",
				this.renderQueue.ToString(),
				")"
			});
		}

		// Token: 0x0400336C RID: 13164
		public Shader shader;

		// Token: 0x0400336D RID: 13165
		public Texture2D mainTex;

		// Token: 0x0400336E RID: 13166
		public Color color;

		// Token: 0x0400336F RID: 13167
		public Color colorTwo;

		// Token: 0x04003370 RID: 13168
		public Texture2D maskTex;

		// Token: 0x04003371 RID: 13169
		public int renderQueue;

		// Token: 0x04003372 RID: 13170
		public List<ShaderParameter> shaderParameters;
	}
}
