using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000D73 RID: 3443
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

		// Token: 0x06004D33 RID: 19763 RVA: 0x00283905 File Offset: 0x00281D05
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

		// Token: 0x06004D34 RID: 19764 RVA: 0x00283945 File Offset: 0x00281D45
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

		// Token: 0x06004D35 RID: 19765 RVA: 0x00283981 File Offset: 0x00281D81
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

		// Token: 0x17000C84 RID: 3204
		// (set) Token: 0x06004D36 RID: 19766 RVA: 0x002839B9 File Offset: 0x00281DB9
		public string BaseTexPath
		{
			set
			{
				this.mainTex = ContentFinder<Texture2D>.Get(value, true);
			}
		}

		// Token: 0x06004D37 RID: 19767 RVA: 0x002839CC File Offset: 0x00281DCC
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

		// Token: 0x06004D38 RID: 19768 RVA: 0x00283A40 File Offset: 0x00281E40
		public override bool Equals(object obj)
		{
			return obj is MaterialRequest && this.Equals((MaterialRequest)obj);
		}

		// Token: 0x06004D39 RID: 19769 RVA: 0x00283A74 File Offset: 0x00281E74
		public bool Equals(MaterialRequest other)
		{
			return other.shader == this.shader && other.mainTex == this.mainTex && other.color == this.color && other.colorTwo == this.colorTwo && other.maskTex == this.maskTex && other.renderQueue == this.renderQueue && other.shaderParameters == this.shaderParameters;
		}

		// Token: 0x06004D3A RID: 19770 RVA: 0x00283B20 File Offset: 0x00281F20
		public static bool operator ==(MaterialRequest lhs, MaterialRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004D3B RID: 19771 RVA: 0x00283B40 File Offset: 0x00281F40
		public static bool operator !=(MaterialRequest lhs, MaterialRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x06004D3C RID: 19772 RVA: 0x00283B60 File Offset: 0x00281F60
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
