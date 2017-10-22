using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace Verse
{
	public static class GraphicDatabaseHeadRecords
	{
		private class HeadGraphicRecord
		{
			public Gender gender;

			public CrownType crownType;

			public string graphicPath;

			private List<KeyValuePair<Color, Graphic_Multi>> graphics = new List<KeyValuePair<Color, Graphic_Multi>>();

			public HeadGraphicRecord(string graphicPath)
			{
				this.graphicPath = graphicPath;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(graphicPath);
				string[] array = fileNameWithoutExtension.Split('_');
				try
				{
					this.crownType = (CrownType)(byte)ParseHelper.FromString(array[array.Length - 2], typeof(CrownType));
					this.gender = (Gender)(byte)ParseHelper.FromString(array[array.Length - 3], typeof(Gender));
				}
				catch (Exception ex)
				{
					Log.Error("Parse error with head graphic at " + graphicPath + ": " + ex.Message);
					this.crownType = CrownType.Undefined;
					this.gender = Gender.None;
				}
			}

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
		}

		private static List<HeadGraphicRecord> heads = new List<HeadGraphicRecord>();

		private static HeadGraphicRecord skull;

		private static HeadGraphicRecord stump;

		private static readonly string[] HeadsFolderPaths = new string[2]
		{
			"Things/Pawn/Humanlike/Heads/Male",
			"Things/Pawn/Humanlike/Heads/Female"
		};

		private static readonly string SkullPath = "Things/Pawn/Humanlike/Heads/None_Average_Skull";

		private static readonly string StumpPath = "Things/Pawn/Humanlike/Heads/None_Average_Stump";

		public static void Reset()
		{
			GraphicDatabaseHeadRecords.heads.Clear();
			GraphicDatabaseHeadRecords.skull = null;
			GraphicDatabaseHeadRecords.stump = null;
		}

		private static void BuildDatabaseIfNecessary()
		{
			if (GraphicDatabaseHeadRecords.heads.Count > 0 && GraphicDatabaseHeadRecords.skull != null && GraphicDatabaseHeadRecords.stump != null)
				return;
			GraphicDatabaseHeadRecords.heads.Clear();
			string[] headsFolderPaths = GraphicDatabaseHeadRecords.HeadsFolderPaths;
			for (int i = 0; i < headsFolderPaths.Length; i++)
			{
				string text = headsFolderPaths[i];
				foreach (string item in GraphicDatabaseUtility.GraphicNamesInFolder(text))
				{
					GraphicDatabaseHeadRecords.heads.Add(new HeadGraphicRecord(text + "/" + item));
				}
			}
			GraphicDatabaseHeadRecords.skull = new HeadGraphicRecord(GraphicDatabaseHeadRecords.SkullPath);
			GraphicDatabaseHeadRecords.stump = new HeadGraphicRecord(GraphicDatabaseHeadRecords.StumpPath);
		}

		public static Graphic_Multi GetHeadNamed(string graphicPath, Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			for (int i = 0; i < GraphicDatabaseHeadRecords.heads.Count; i++)
			{
				HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads[i];
				if (headGraphicRecord.graphicPath == graphicPath)
				{
					return headGraphicRecord.GetGraphic(skinColor);
				}
			}
			Log.Message("Tried to get pawn head at path " + graphicPath + " that was not found. Defaulting...");
			return GraphicDatabaseHeadRecords.heads.First().GetGraphic(skinColor);
		}

		public static Graphic_Multi GetSkull()
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.skull.GetGraphic(Color.white);
		}

		public static Graphic_Multi GetStump(Color skinColor)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			return GraphicDatabaseHeadRecords.stump.GetGraphic(skinColor);
		}

		public static Graphic_Multi GetHeadRandom(Gender gender, Color skinColor, CrownType crownType)
		{
			GraphicDatabaseHeadRecords.BuildDatabaseIfNecessary();
			Predicate<HeadGraphicRecord> predicate = (Predicate<HeadGraphicRecord>)delegate(HeadGraphicRecord head)
			{
				if (head.crownType != crownType)
				{
					return false;
				}
				if (head.gender != gender)
				{
					return false;
				}
				return true;
			};
			int num = 0;
			while (true)
			{
				HeadGraphicRecord headGraphicRecord = GraphicDatabaseHeadRecords.heads.RandomElement();
				if (predicate(headGraphicRecord))
				{
					return headGraphicRecord.GetGraphic(skinColor);
				}
				num++;
				if (num > 40)
					break;
			}
			foreach (HeadGraphicRecord item in GraphicDatabaseHeadRecords.heads.InRandomOrder(null))
			{
				if (predicate(item))
				{
					return item.GetGraphic(skinColor);
				}
			}
			Log.Error("Failed to find head for gender=" + gender + ". Defaulting...");
			return GraphicDatabaseHeadRecords.heads.First().GetGraphic(skinColor);
		}
	}
}
