using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class QualityUtility
	{
		public static List<QualityCategory> AllQualityCategories;

		static QualityUtility()
		{
			QualityUtility.AllQualityCategories = new List<QualityCategory>();
			IEnumerator enumerator = Enum.GetValues(typeof(QualityCategory)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					QualityCategory item = (QualityCategory)enumerator.Current;
					QualityUtility.AllQualityCategories.Add(item);
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}

		public static bool TryGetQuality(this Thing t, out QualityCategory qc)
		{
			MinifiedThing minifiedThing = t as MinifiedThing;
			CompQuality compQuality = (minifiedThing == null) ? t.TryGetComp<CompQuality>() : minifiedThing.InnerThing.TryGetComp<CompQuality>();
			bool result;
			if (compQuality == null)
			{
				qc = QualityCategory.Normal;
				result = false;
			}
			else
			{
				qc = compQuality.Quality;
				result = true;
			}
			return result;
		}

		public static string GetLabel(this QualityCategory cat)
		{
			string result;
			switch (cat)
			{
			case QualityCategory.Awful:
			{
				result = "QualityCategory_Awful".Translate();
				break;
			}
			case QualityCategory.Shoddy:
			{
				result = "QualityCategory_Shoddy".Translate();
				break;
			}
			case QualityCategory.Poor:
			{
				result = "QualityCategory_Poor".Translate();
				break;
			}
			case QualityCategory.Normal:
			{
				result = "QualityCategory_Normal".Translate();
				break;
			}
			case QualityCategory.Good:
			{
				result = "QualityCategory_Good".Translate();
				break;
			}
			case QualityCategory.Excellent:
			{
				result = "QualityCategory_Excellent".Translate();
				break;
			}
			case QualityCategory.Superior:
			{
				result = "QualityCategory_Superior".Translate();
				break;
			}
			case QualityCategory.Masterwork:
			{
				result = "QualityCategory_Masterwork".Translate();
				break;
			}
			case QualityCategory.Legendary:
			{
				result = "QualityCategory_Legendary".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}

		public static string GetLabelShort(this QualityCategory cat)
		{
			string result;
			switch (cat)
			{
			case QualityCategory.Awful:
			{
				result = "QualityCategoryShort_Awful".Translate();
				break;
			}
			case QualityCategory.Shoddy:
			{
				result = "QualityCategoryShort_Shoddy".Translate();
				break;
			}
			case QualityCategory.Poor:
			{
				result = "QualityCategoryShort_Poor".Translate();
				break;
			}
			case QualityCategory.Normal:
			{
				result = "QualityCategoryShort_Normal".Translate();
				break;
			}
			case QualityCategory.Good:
			{
				result = "QualityCategoryShort_Good".Translate();
				break;
			}
			case QualityCategory.Excellent:
			{
				result = "QualityCategoryShort_Excellent".Translate();
				break;
			}
			case QualityCategory.Superior:
			{
				result = "QualityCategoryShort_Superior".Translate();
				break;
			}
			case QualityCategory.Masterwork:
			{
				result = "QualityCategoryShort_Masterwork".Translate();
				break;
			}
			case QualityCategory.Legendary:
			{
				result = "QualityCategoryShort_Legendary".Translate();
				break;
			}
			default:
			{
				throw new ArgumentException();
			}
			}
			return result;
		}

		public static bool FollowQualityThingFilter(this ThingDef def)
		{
			return (byte)((def.stackLimit == 1) ? 1 : (def.HasComp(typeof(CompQuality)) ? 1 : 0)) != 0;
		}

		public static QualityCategory RandomQuality()
		{
			return QualityUtility.AllQualityCategories.RandomElement();
		}

		public static QualityCategory RandomCreationQuality(int relevantSkillLevel)
		{
			float centerX = -1f;
			switch (relevantSkillLevel)
			{
			case 0:
			{
				centerX = 0.167f;
				break;
			}
			case 1:
			{
				centerX = 0.5f;
				break;
			}
			case 2:
			{
				centerX = 0.833f;
				break;
			}
			case 3:
			{
				centerX = 1.166f;
				break;
			}
			case 4:
			{
				centerX = 1.5f;
				break;
			}
			case 5:
			{
				centerX = 1.833f;
				break;
			}
			case 6:
			{
				centerX = 2.166f;
				break;
			}
			case 7:
			{
				centerX = 2.5f;
				break;
			}
			case 8:
			{
				centerX = 2.833f;
				break;
			}
			case 9:
			{
				centerX = 3.166f;
				break;
			}
			case 10:
			{
				centerX = 3.5f;
				break;
			}
			case 11:
			{
				centerX = 3.75f;
				break;
			}
			case 12:
			{
				centerX = 4f;
				break;
			}
			case 13:
			{
				centerX = 4.25f;
				break;
			}
			case 14:
			{
				centerX = 4.5f;
				break;
			}
			case 15:
			{
				centerX = 4.7f;
				break;
			}
			case 16:
			{
				centerX = 4.9f;
				break;
			}
			case 17:
			{
				centerX = 5.1f;
				break;
			}
			case 18:
			{
				centerX = 5.3f;
				break;
			}
			case 19:
			{
				centerX = 5.5f;
				break;
			}
			case 20:
			{
				centerX = 5.7f;
				break;
			}
			}
			float value = Rand.Gaussian(centerX, 1.25f);
			value = Mathf.Clamp(value, 0f, (float)((float)QualityUtility.AllQualityCategories.Count - 0.5));
			return (QualityCategory)(byte)(int)value;
		}

		public static QualityCategory RandomTraderItemQuality()
		{
			QualityCategory result;
			if (Rand.Value < 0.25)
			{
				result = QualityCategory.Normal;
			}
			else
			{
				float value = Rand.Gaussian(3.5f, 1.13f);
				value = Mathf.Clamp(value, 0f, (float)((float)QualityUtility.AllQualityCategories.Count - 0.5));
				result = (QualityCategory)(byte)(int)value;
			}
			return result;
		}

		public static QualityCategory RandomBaseGenItemQuality()
		{
			return QualityUtility.RandomTraderItemQuality();
		}

		public static QualityCategory RandomGeneratedGearQuality(PawnKindDef pawnKind)
		{
			QualityCategory result;
			if (pawnKind.forceNormalGearQuality)
			{
				result = QualityCategory.Normal;
			}
			else if (Rand.Value < 0.25)
			{
				result = pawnKind.itemQuality;
			}
			else
			{
				float centerX = (float)((float)(int)pawnKind.itemQuality + 0.5);
				float value = Rand.GaussianAsymmetric(centerX, 1.25f, 1.07f);
				value = Mathf.Clamp(value, 0f, (float)((float)QualityUtility.AllQualityCategories.Count - 0.5));
				result = (QualityCategory)(byte)(int)value;
			}
			return result;
		}

		public static QualityCategory AddLevels(this QualityCategory quality, int levels)
		{
			return (QualityCategory)(byte)Mathf.Min((int)quality + levels, 8);
		}

		internal static void LogGenerationData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Qualities for trader items");
			stringBuilder.AppendLine(QualityUtility.DebugQualitiesString((Func<QualityCategory>)(() => QualityUtility.RandomTraderItemQuality())));
			foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				if (allDef.RaceProps.Humanlike)
				{
					stringBuilder.AppendLine("Qualities for items generated for pawn kind " + allDef.defName);
					stringBuilder.Append(QualityUtility.DebugQualitiesString((Func<QualityCategory>)(() => QualityUtility.RandomGeneratedGearQuality(allDef))));
					stringBuilder.AppendLine();
				}
			}
			int level;
			for (level = 0; level <= 20; level++)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Creation qualities for worker at level " + level);
				stringBuilder.Append(QualityUtility.DebugQualitiesString((Func<QualityCategory>)(() => QualityUtility.RandomCreationQuality(level))));
			}
			Log.Message(stringBuilder.ToString());
		}

		private static string DebugQualitiesString(Func<QualityCategory> qualityGenerator)
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<QualityCategory> list = new List<QualityCategory>();
			for (int i = 0; i < 1000; i++)
			{
				list.Add(qualityGenerator());
			}
			foreach (QualityCategory allQualityCategory in QualityUtility.AllQualityCategories)
			{
				stringBuilder.AppendLine(allQualityCategory.ToString() + " - " + (from q in list
				where q == allQualityCategory
				select q).Count().ToString());
			}
			return stringBuilder.ToString();
		}
	}
}
