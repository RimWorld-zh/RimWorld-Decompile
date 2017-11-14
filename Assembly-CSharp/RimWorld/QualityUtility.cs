using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	public static class QualityUtility
	{
		public static List<QualityCategory> AllQualityCategories;

		[CompilerGenerated]
		private static Func<QualityCategory> _003C_003Ef__mg_0024cache0;

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
			if (compQuality == null)
			{
				qc = QualityCategory.Normal;
				return false;
			}
			qc = compQuality.Quality;
			return true;
		}

		public static string GetLabel(this QualityCategory cat)
		{
			switch (cat)
			{
			case QualityCategory.Awful:
				return "QualityCategory_Awful".Translate();
			case QualityCategory.Shoddy:
				return "QualityCategory_Shoddy".Translate();
			case QualityCategory.Poor:
				return "QualityCategory_Poor".Translate();
			case QualityCategory.Normal:
				return "QualityCategory_Normal".Translate();
			case QualityCategory.Good:
				return "QualityCategory_Good".Translate();
			case QualityCategory.Excellent:
				return "QualityCategory_Excellent".Translate();
			case QualityCategory.Superior:
				return "QualityCategory_Superior".Translate();
			case QualityCategory.Masterwork:
				return "QualityCategory_Masterwork".Translate();
			case QualityCategory.Legendary:
				return "QualityCategory_Legendary".Translate();
			default:
				throw new ArgumentException();
			}
		}

		public static string GetLabelShort(this QualityCategory cat)
		{
			switch (cat)
			{
			case QualityCategory.Awful:
				return "QualityCategoryShort_Awful".Translate();
			case QualityCategory.Shoddy:
				return "QualityCategoryShort_Shoddy".Translate();
			case QualityCategory.Poor:
				return "QualityCategoryShort_Poor".Translate();
			case QualityCategory.Normal:
				return "QualityCategoryShort_Normal".Translate();
			case QualityCategory.Good:
				return "QualityCategoryShort_Good".Translate();
			case QualityCategory.Excellent:
				return "QualityCategoryShort_Excellent".Translate();
			case QualityCategory.Superior:
				return "QualityCategoryShort_Superior".Translate();
			case QualityCategory.Masterwork:
				return "QualityCategoryShort_Masterwork".Translate();
			case QualityCategory.Legendary:
				return "QualityCategoryShort_Legendary".Translate();
			default:
				throw new ArgumentException();
			}
		}

		public static bool FollowQualityThingFilter(this ThingDef def)
		{
			if (def.stackLimit == 1)
			{
				return true;
			}
			if (def.HasComp(typeof(CompQuality)))
			{
				return true;
			}
			return false;
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
				centerX = 0.167f;
				break;
			case 1:
				centerX = 0.5f;
				break;
			case 2:
				centerX = 0.833f;
				break;
			case 3:
				centerX = 1.166f;
				break;
			case 4:
				centerX = 1.5f;
				break;
			case 5:
				centerX = 1.833f;
				break;
			case 6:
				centerX = 2.166f;
				break;
			case 7:
				centerX = 2.5f;
				break;
			case 8:
				centerX = 2.833f;
				break;
			case 9:
				centerX = 3.166f;
				break;
			case 10:
				centerX = 3.5f;
				break;
			case 11:
				centerX = 3.75f;
				break;
			case 12:
				centerX = 4f;
				break;
			case 13:
				centerX = 4.25f;
				break;
			case 14:
				centerX = 4.5f;
				break;
			case 15:
				centerX = 4.7f;
				break;
			case 16:
				centerX = 4.9f;
				break;
			case 17:
				centerX = 5.1f;
				break;
			case 18:
				centerX = 5.3f;
				break;
			case 19:
				centerX = 5.5f;
				break;
			case 20:
				centerX = 5.7f;
				break;
			}
			float value = Rand.Gaussian(centerX, 1.25f);
			value = Mathf.Clamp(value, 0f, (float)((float)QualityUtility.AllQualityCategories.Count - 0.5));
			return (QualityCategory)(byte)(int)value;
		}

		public static QualityCategory RandomTraderItemQuality()
		{
			if (Rand.Value < 0.25)
			{
				return QualityCategory.Normal;
			}
			float value = Rand.Gaussian(3.5f, 1.13f);
			value = Mathf.Clamp(value, 0f, (float)((float)QualityUtility.AllQualityCategories.Count - 0.5));
			return (QualityCategory)(byte)(int)value;
		}

		public static QualityCategory RandomBaseGenItemQuality()
		{
			return QualityUtility.RandomTraderItemQuality();
		}

		public static QualityCategory RandomGeneratedGearQuality(PawnKindDef pawnKind)
		{
			if (pawnKind.forceNormalGearQuality)
			{
				return QualityCategory.Normal;
			}
			if (Rand.Value < 0.25)
			{
				return pawnKind.itemQuality;
			}
			float centerX = (float)((float)(int)pawnKind.itemQuality + 0.5);
			float value = Rand.GaussianAsymmetric(centerX, 1.25f, 1.07f);
			value = Mathf.Clamp(value, 0f, (float)((float)QualityUtility.AllQualityCategories.Count - 0.5));
			return (QualityCategory)(byte)(int)value;
		}

		public static QualityCategory AddLevels(this QualityCategory quality, int levels)
		{
			return (QualityCategory)(byte)Mathf.Min((int)quality + levels, 8);
		}

		internal static void LogGenerationData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Qualities for trader items");
			stringBuilder.AppendLine(QualityUtility.DebugQualitiesString(QualityUtility.RandomTraderItemQuality));
			foreach (PawnKindDef allDef in DefDatabase<PawnKindDef>.AllDefs)
			{
				if (allDef.RaceProps.Humanlike)
				{
					stringBuilder.AppendLine("Qualities for items generated for pawn kind " + allDef.defName);
					stringBuilder.Append(QualityUtility.DebugQualitiesString(() => QualityUtility.RandomGeneratedGearQuality(allDef)));
					stringBuilder.AppendLine();
				}
			}
			int level;
			for (level = 0; level <= 20; level++)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Creation qualities for worker at level " + level);
				stringBuilder.Append(QualityUtility.DebugQualitiesString(() => QualityUtility.RandomCreationQuality(level)));
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
