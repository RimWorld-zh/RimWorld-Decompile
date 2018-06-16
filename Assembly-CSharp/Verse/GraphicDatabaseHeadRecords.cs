using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000DD1 RID: 3537
	public static class GraphicDatabaseHeadRecords
	{
		// Token: 0x06004F26 RID: 20262 RVA: 0x00293074 File Offset: 0x00291474
		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		// Token: 0x06004F27 RID: 20263 RVA: 0x00293090 File Offset: 0x00291490
		private static void BuildDatabaseIfNecessary()
		{
			if (GraphicDatabaseHeadRecords.heads.Count <= 0 || GraphicDatabaseHeadRecords.skull == null || GraphicDatabaseHeadRecords.stump == null)
			{
				GraphicDatabaseHeadRecords.heads.Clear();
				foreach (string text in GraphicDatabaseHeadRecords.HeadsFolderPaths)
				{
					foreach (string str in GraphicDatabaseUtility.GraphicNamesInFolder(text))
					{
						GraphicDatabaseHeadRecords.heads.Add(new GraphicDatabaseHeadRecords.HeadGraphicRecord(text + "/" + str));
					}
				}
				GraphicDatabaseHeadRecords.skull = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.SkullPath);
				GraphicDatabaseHeadRecords.stump = new GraphicDatabaseHeadRecords.HeadGraphicRecord(GraphicDatabaseHeadRecords.StumpPath);
			}
		}

		// Token: 0x06004F28 RID: 20264 RVA: 0x00293178 File Offset: 0x00291578
		public static Graphic_Multi GetHeadNamed(string graphicPath, Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			for (int i = 0; i < GraphicDatabaseHeadRecords.heads.Count; i++)
			{
				GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads[i];
				if (headGraphicRecord.graphicPath == graphicPath)
				{
					return headGraphicRecord.GetGraphic(skinColor);
				}
			}
			Log.Message("Tried to get pawn head at path " + graphicPath + " that was not found. Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor);
		}

		// Token: 0x06004F29 RID: 20265 RVA: 0x00293200 File Offset: 0x00291600
		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white);
		}

		// Token: 0x06004F2A RID: 20266 RVA: 0x0029322C File Offset: 0x0029162C
		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor);
		}

		// Token: 0x06004F2B RID: 20267 RVA: 0x00293254 File Offset: 0x00291654
		public static Graphic_Multi GetHeadRandom(Gender gender, Color skinColor, CrownType crownType)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			Predicate<GraphicDatabaseHeadRecords.HeadGraphicRecord> predicate = (GraphicDatabaseHeadRecords.HeadGraphicRecord head) => head.crownType == crownType && head.gender == gender;
			int num = 0;
			GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord;
			for (;;)
			{
				headGraphicRecord = GraphicDatabaseHeadRecords.heads.RandomElement<GraphicDatabaseHeadRecords.HeadGraphicRecord>();
				if (predicate(headGraphicRecord))
				{
					break;
				}
				num++;
				if (num > 40)
				{
					goto Block_2;
				}
			}
			return headGraphicRecord.GetGraphic(skinColor);
			Block_2:
			foreach (GraphicDatabaseHeadRecords.HeadGraphicRecord headGraphicRecord2 in GraphicDatabaseHeadRecords.heads.InRandomOrder(null))
			{
				if (predicate(headGraphicRecord2))
				{
					return headGraphicRecord2.GetGraphic(skinColor);
				}
			}
			Log.Error("Failed to find head for gender=" + gender + ". Defaulting...", false);
			return GraphicDatabaseHeadRecords.heads.First<GraphicDatabaseHeadRecords.HeadGraphicRecord>().GetGraphic(skinColor);
		}

		// Token: 0x0400349C RID: 13468
		private static List<GraphicDatabaseHeadRecords.HeadGraphicRecord> heads = new List<GraphicDatabaseHeadRecords.HeadGraphicRecord>();

		// Token: 0x0400349D RID: 13469
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord skull;

		// Token: 0x0400349E RID: 13470
		private static GraphicDatabaseHeadRecords.HeadGraphicRecord stump;

		// Token: 0x0400349F RID: 13471
		private static readonly string[] HeadsFolderPaths = new string[]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		// Token: 0x040034A0 RID: 13472
		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		// Token: 0x040034A1 RID: 13473
		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		// Token: 0x02000DD2 RID: 3538
		private class HeadGraphicRecord
		{
			// Token: 0x06004F2D RID: 20269 RVA: 0x002933AC File Offset: 0x002917AC
			public HeadGraphicRecord(string graphicPath)
			{
				this.graphicPath = graphicPath;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(graphicPath);
				string[] array = fileNameWithoutExtension.Split(new char[]
				{
					'_'
				});
				try
				{
					this.crownType = (CrownType)ParseHelper.FromString(array[array.Length - 2], typeof(CrownType));
					this.gender = (Gender)ParseHelper.FromString(array[array.Length - 3], typeof(Gender));
				}
				catch (Exception ex)
				{
					Log.Error("Parse error with head graphic at " + graphicPath + ": " + ex.Message, false);
					this.crownType = CrownType.Undefined;
					this.gender = Gender.None;
				}
			}

			// Token: 0x06004F2E RID: 20270 RVA: 0x00293480 File Offset: 0x00291880
			public Graphic_Multi GetGraphic(Color color)
			{
				for (int i = 0; i < this.graphics.Count; i++)
				{
					if (color.IndistinguishableFrom(this.graphics[i].Key))
					{
						return this.graphics[i].Value;
					}
				}
				Graphic_Multi graphic_Multi = (Graphic_Multi)GraphicDatabase.Get<Graphic_Multi>(this.graphicPath, ShaderDatabase.CutoutSkin, Vector2.one, color);
				this.graphics.Add(new KeyValuePair<Color, Graphic_Multi>(color, graphic_Multi));
				return graphic_Multi;
			}

			// Token: 0x040034A2 RID: 13474
			public Gender gender;

			// Token: 0x040034A3 RID: 13475
			public CrownType crownType = CrownType.Undefined;

			// Token: 0x040034A4 RID: 13476
			public string graphicPath;

			// Token: 0x040034A5 RID: 13477
			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();
		}
	}
}
