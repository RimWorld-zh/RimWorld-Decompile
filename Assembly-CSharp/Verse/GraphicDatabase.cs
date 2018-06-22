using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DCB RID: 3531
	[HasDebugOutput]
	public static class GraphicDatabase
	{
		// Token: 0x06004F2D RID: 20269 RVA: 0x00294178 File Offset: 0x00292578
		public static Graphic Get<T>(string path) where T : Graphic, new()
		{
			GraphicRequest req = new GraphicRequest(typeof(T), path, ShaderDatabase.Cutout, Vector2.one, Color.white, Color.white, null, 0, null);
			return GraphicDatabase.GetInner<T>(req);
		}

		// Token: 0x06004F2E RID: 20270 RVA: 0x002941C4 File Offset: 0x002925C4
		public static Graphic Get<T>(string path, Shader shader) where T : Graphic, new()
		{
			GraphicRequest req = new GraphicRequest(typeof(T), path, shader, Vector2.one, Color.white, Color.white, null, 0, null);
			return GraphicDatabase.GetInner<T>(req);
		}

		// Token: 0x06004F2F RID: 20271 RVA: 0x0029420C File Offset: 0x0029260C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color) where T : Graphic, new()
		{
			GraphicRequest req = new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, 0, null);
			return GraphicDatabase.GetInner<T>(req);
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x0029424C File Offset: 0x0029264C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, int renderQueue) where T : Graphic, new()
		{
			GraphicRequest req = new GraphicRequest(typeof(T), path, shader, drawSize, color, Color.white, null, renderQueue, null);
			return GraphicDatabase.GetInner<T>(req);
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x0029428C File Offset: 0x0029268C
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo) where T : Graphic, new()
		{
			GraphicRequest req = new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, null, 0, null);
			return GraphicDatabase.GetInner<T>(req);
		}

		// Token: 0x06004F32 RID: 20274 RVA: 0x002942C8 File Offset: 0x002926C8
		public static Graphic Get<T>(string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data) where T : Graphic, new()
		{
			GraphicRequest req = new GraphicRequest(typeof(T), path, shader, drawSize, color, colorTwo, data, 0, null);
			return GraphicDatabase.GetInner<T>(req);
		}

		// Token: 0x06004F33 RID: 20275 RVA: 0x00294304 File Offset: 0x00292704
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo)
		{
			return GraphicDatabase.Get(graphicClass, path, shader, drawSize, color, colorTwo, null, null);
		}

		// Token: 0x06004F34 RID: 20276 RVA: 0x00294328 File Offset: 0x00292728
		public static Graphic Get(Type graphicClass, string path, Shader shader, Vector2 drawSize, Color color, Color colorTwo, GraphicData data, List<ShaderParameter> shaderParameters)
		{
			GraphicRequest graphicRequest = new GraphicRequest(graphicClass, path, shader, drawSize, color, colorTwo, data, 0, shaderParameters);
			Graphic result;
			if (graphicRequest.graphicClass == typeof(Graphic_Single))
			{
				result = GraphicDatabase.GetInner<Graphic_Single>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_Terrain))
			{
				result = GraphicDatabase.GetInner<Graphic_Terrain>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_Multi))
			{
				result = GraphicDatabase.GetInner<Graphic_Multi>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_Mote))
			{
				result = GraphicDatabase.GetInner<Graphic_Mote>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_Random))
			{
				result = GraphicDatabase.GetInner<Graphic_Random>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_Flicker))
			{
				result = GraphicDatabase.GetInner<Graphic_Flicker>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_Appearances))
			{
				result = GraphicDatabase.GetInner<Graphic_Appearances>(graphicRequest);
			}
			else if (graphicRequest.graphicClass == typeof(Graphic_StackCount))
			{
				result = GraphicDatabase.GetInner<Graphic_StackCount>(graphicRequest);
			}
			else
			{
				try
				{
					return (Graphic)GenGeneric.InvokeStaticGenericMethod(typeof(GraphicDatabase), graphicRequest.graphicClass, "GetInner", new object[]
					{
						graphicRequest
					});
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Exception getting ",
						graphicClass,
						" at ",
						path,
						": ",
						ex.ToString()
					}), false);
				}
				result = BaseContent.BadGraphic;
			}
			return result;
		}

		// Token: 0x06004F35 RID: 20277 RVA: 0x002944F0 File Offset: 0x002928F0
		private static T GetInner<T>(GraphicRequest req) where T : Graphic, new()
		{
			Graphic graphic;
			if (!GraphicDatabase.allGraphics.TryGetValue(req, out graphic))
			{
				graphic = Activator.CreateInstance<T>();
				graphic.Init(req);
				GraphicDatabase.allGraphics.Add(req, graphic);
			}
			return (T)((object)graphic);
		}

		// Token: 0x06004F36 RID: 20278 RVA: 0x0029453D File Offset: 0x0029293D
		public static void Clear()
		{
			GraphicDatabase.allGraphics.Clear();
		}

		// Token: 0x06004F37 RID: 20279 RVA: 0x0029454C File Offset: 0x0029294C
		[DebugOutput]
		[Category("System")]
		public static void AllGraphicsLoaded()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("There are " + GraphicDatabase.allGraphics.Count + " graphics loaded.");
			int num = 0;
			foreach (Graphic graphic in GraphicDatabase.allGraphics.Values)
			{
				stringBuilder.AppendLine(num + " - " + graphic.ToString());
				if (num % 50 == 49)
				{
					Log.Message(stringBuilder.ToString(), false);
					stringBuilder = new StringBuilder();
				}
				num++;
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x040034A0 RID: 13472
		private static Dictionary<GraphicRequest, Graphic> allGraphics = new Dictionary<GraphicRequest, Graphic>();
	}
}
