using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D71 RID: 3441
	public struct MaterialRequest : IEquatable<MaterialRequest>
	{
		// Token: 0x04003377 RID: 13175
		public Shader shader;

		// Token: 0x04003378 RID: 13176
		public Texture2D mainTex;

		// Token: 0x04003379 RID: 13177
		public Color color;

		// Token: 0x0400337A RID: 13178
		public Color colorTwo;

		// Token: 0x0400337B RID: 13179
		public Texture2D maskTex;

		// Token: 0x0400337C RID: 13180
		public int renderQueue;

		// Token: 0x0400337D RID: 13181
		public List<ShaderParameter> shaderParameters;

		// Token: 0x06004D2F RID: 19759 RVA: 0x002837D9 File Offset: 0x00281BD9
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

		// Token: 0x06004D30 RID: 19760 RVA: 0x00283819 File Offset: 0x00281C19
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

		// Token: 0x06004D31 RID: 19761 RVA: 0x00283855 File Offset: 0x00281C55
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

		// Token: 0x17000C85 RID: 3205
		// (set) Token: 0x06004D32 RID: 19762 RVA: 0x0028388D File Offset: 0x00281C8D
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06004D33 RID: 19763 RVA: 0x002838A0 File Offset: 0x00281CA0
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

		// Token: 0x06004D34 RID: 19764 RVA: 0x00283914 File Offset: 0x00281D14
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06004D35 RID: 19765 RVA: 0x00283948 File Offset: 0x00281D48
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06004D36 RID: 19766 RVA: 0x002839F4 File Offset: 0x00281DF4
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x00283A14 File Offset: 0x00281E14
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06004D38 RID: 19768 RVA: 0x00283A34 File Offset: 0x00281E34
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
	}
}
