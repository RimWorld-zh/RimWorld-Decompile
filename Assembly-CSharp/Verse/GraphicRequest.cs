using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCC RID: 3532
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x04003497 RID: 13463
		public Type graphicClass;

		// Token: 0x04003498 RID: 13464
		public string path;

		// Token: 0x04003499 RID: 13465
		public Shader shader;

		// Token: 0x0400349A RID: 13466
		public Vector2 drawSize;

		// Token: 0x0400349B RID: 13467
		public Color color;

		// Token: 0x0400349C RID: 13468
		public Color colorTwo;

		// Token: 0x0400349D RID: 13469
		public GraphicData graphicData;

		// Token: 0x0400349E RID: 13470
		public int renderQueue;

		// Token: 0x0400349F RID: 13471
		public List<ShaderParameter> shaderParameters;

		// Token: 0x06004F2B RID: 20267 RVA: 0x00294054 File Offset: 0x00292454
		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue, List<ShaderParameter> shaderParameters)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
			this.shaderParameters = ((!shaderParameters.NullOrEmpty<ShaderParameter>()) ? shaderParameters : null);
		}

		// Token: 0x06004F2C RID: 20268 RVA: 0x002940BC File Offset: 0x002924BC
		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			int seed = 0;
			seed = Gen.HashCombine<Type>(seed, this.graphicClass);
			seed = Gen.HashCombine<string>(seed, this.path);
			seed = Gen.HashCombine<Shader>(seed, this.shader);
			seed = Gen.HashCombineStruct<Vector2>(seed, this.drawSize);
			seed = Gen.HashCombineStruct<Color>(seed, this.color);
			seed = Gen.HashCombineStruct<Color>(seed, this.colorTwo);
			seed = Gen.HashCombine<GraphicData>(seed, this.graphicData);
			seed = Gen.HashCombine<int>(seed, this.renderQueue);
			return Gen.HashCombine<List<ShaderParameter>>(seed, this.shaderParameters);
		}

		// Token: 0x06004F2D RID: 20269 RVA: 0x00294160 File Offset: 0x00292560
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x00294194 File Offset: 0x00292594
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x00294264 File Offset: 0x00292664
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x00294284 File Offset: 0x00292684
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}
	}
}
