using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCE RID: 3534
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x06004F14 RID: 20244 RVA: 0x0029296C File Offset: 0x00290D6C
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

		// Token: 0x06004F15 RID: 20245 RVA: 0x002929D4 File Offset: 0x00290DD4
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

		// Token: 0x06004F16 RID: 20246 RVA: 0x00292A78 File Offset: 0x00290E78
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x00292AAC File Offset: 0x00290EAC
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x06004F18 RID: 20248 RVA: 0x00292B7C File Offset: 0x00290F7C
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004F19 RID: 20249 RVA: 0x00292B9C File Offset: 0x00290F9C
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400348E RID: 13454
		public Type graphicClass;

		// Token: 0x0400348F RID: 13455
		public string path;

		// Token: 0x04003490 RID: 13456
		public Shader shader;

		// Token: 0x04003491 RID: 13457
		public Vector2 drawSize;

		// Token: 0x04003492 RID: 13458
		public Color color;

		// Token: 0x04003493 RID: 13459
		public Color colorTwo;

		// Token: 0x04003494 RID: 13460
		public GraphicData graphicData;

		// Token: 0x04003495 RID: 13461
		public int renderQueue;

		// Token: 0x04003496 RID: 13462
		public List<ShaderParameter> shaderParameters;
	}
}
