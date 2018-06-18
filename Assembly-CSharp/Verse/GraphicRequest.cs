using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCD RID: 3533
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		// Token: 0x06004F12 RID: 20242 RVA: 0x0029294C File Offset: 0x00290D4C
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

		// Token: 0x06004F13 RID: 20243 RVA: 0x002929B4 File Offset: 0x00290DB4
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

		// Token: 0x06004F14 RID: 20244 RVA: 0x00292A58 File Offset: 0x00290E58
		public override bool Equals(object obj)
		{
			return obj is GraphicRequest && this.Equals((GraphicRequest)obj);
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x00292A8C File Offset: 0x00290E8C
		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && this.shader == other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue && this.shaderParameters == other.shaderParameters;
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x00292B5C File Offset: 0x00290F5C
		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x06004F17 RID: 20247 RVA: 0x00292B7C File Offset: 0x00290F7C
		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x0400348C RID: 13452
		public Type graphicClass;

		// Token: 0x0400348D RID: 13453
		public string path;

		// Token: 0x0400348E RID: 13454
		public Shader shader;

		// Token: 0x0400348F RID: 13455
		public Vector2 drawSize;

		// Token: 0x04003490 RID: 13456
		public Color color;

		// Token: 0x04003491 RID: 13457
		public Color colorTwo;

		// Token: 0x04003492 RID: 13458
		public GraphicData graphicData;

		// Token: 0x04003493 RID: 13459
		public int renderQueue;

		// Token: 0x04003494 RID: 13460
		public List<ShaderParameter> shaderParameters;
	}
}
