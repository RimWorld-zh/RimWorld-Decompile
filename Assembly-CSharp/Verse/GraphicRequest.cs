using System;
using UnityEngine;

namespace Verse
{
	public struct GraphicRequest : IEquatable<GraphicRequest>
	{
		public Type graphicClass;

		public string path;

		public Shader shader;

		public Vector2 drawSize;

		public Color color;

		public Color colorTwo;

		public GraphicData graphicData;

		public int renderQueue;

		public GraphicRequest(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData graphicData, int renderQueue)
		{
			this.graphicClass = graphicClass;
			this.path = path;
			this.shader = shader;
			this.drawSize = drawSize;
			this.color = color;
			this.colorTwo = colorTwo;
			this.graphicData = graphicData;
			this.renderQueue = renderQueue;
		}

		public override int GetHashCode()
		{
			if (this.path == null)
			{
				this.path = BaseContent.BadTexPath;
			}
			int seed = 0;
			seed = Gen.HashCombine(seed, this.graphicClass);
			seed = Gen.HashCombine(seed, this.path);
			seed = Gen.HashCombine(seed, this.shader);
			seed = Gen.HashCombineStruct(seed, this.drawSize);
			seed = Gen.HashCombineStruct(seed, this.color);
			seed = Gen.HashCombineStruct(seed, this.colorTwo);
			seed = Gen.HashCombine(seed, this.graphicData);
			return Gen.HashCombine(seed, this.renderQueue);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is GraphicRequest))
			{
				return false;
			}
			return this.Equals((GraphicRequest)obj);
		}

		public bool Equals(GraphicRequest other)
		{
			return this.graphicClass == other.graphicClass && this.path == other.path && (UnityEngine.Object)this.shader == (UnityEngine.Object)other.shader && this.drawSize == other.drawSize && this.color == other.color && this.colorTwo == other.colorTwo && this.graphicData == other.graphicData && this.renderQueue == other.renderQueue;
		}

		public static bool operator ==(GraphicRequest lhs, GraphicRequest rhs)
		{
			return lhs.Equals(rhs);
		}

		public static bool operator !=(GraphicRequest lhs, GraphicRequest rhs)
		{
			return !(lhs == rhs);
		}
	}
}
