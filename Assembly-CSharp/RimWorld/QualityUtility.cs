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
	[HasDebugOutput]
	public static class QualityUtility
	{
		public static List<QualityCategory> AllQualityCategories = new List<QualityCategory>();

		[CompilerGenerated]
		private static Func<QualityCategory, string> <>f__am$cache0;

		[CompilerGenerated]
		private static Func<QualityCategory, string> <>f__am$cache1;

		[CompilerGenerated]
		private static Func<QualityCategory, string> <>f__am$cache2;

		[CompilerGenerated]
		private static Func<QualityCategory, string> <>f__am$cache3;

		[CompilerGenerated]
		private static Func<QualityCategory, string> <>f__am$cache4;

		[CompilerGenerated]
		private static Func<PawnKindDef, float> <>f__am$cache5;

		[CompilerGenerated]
		private static Func<QualityCategory> <>f__am$cache6;

		[CompilerGenerated]
		private static Func<QualityCategory> <>f__am$cache7;

		[CompilerGenerated]
		private static Func<QualityCategory> <>f__am$cache8;

		[CompilerGenerated]
		private static Func<QualityCategory> <>f__am$cache9;

		static QualityUtility()
		{
			IEnumerator enumerator = Enum.GetValues(typeof(QualityCategory)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					QualityCategory item = (QualityCategory)obj;
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
				result = "QualityCategory_Awful".Translate();
				break;
			case QualityCategory.Poor:
				result = "QualityCategory_Poor".Translate();
				break;
			case QualityCategory.Normal:
				result = "QualityCategory_Normal".Translate();
				break;
			case QualityCategory.Good:
				result = "QualityCategory_Good".Translate();
				break;
			case QualityCategory.Excellent:
				result = "QualityCategory_Excellent".Translate();
				break;
			case QualityCategory.Masterwork:
				result = "QualityCategory_Masterwork".Translate();
				break;
			case QualityCategory.Legendary:
				result = "QualityCategory_Legendary".Translate();
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}

		public static string GetLabelShort(this QualityCategory cat)
		{
			string result;
			switch (cat)
			{
			case QualityCategory.Awful:
				result = "QualityCategoryShort_Awful".Translate();
				break;
			case QualityCategory.Poor:
				result = "QualityCategoryShort_Poor".Translate();
				break;
			case QualityCategory.Normal:
				result = "QualityCategoryShort_Normal".Translate();
				break;
			case QualityCategory.Good:
				result = "QualityCategoryShort_Good".Translate();
				break;
			case QualityCategory.Excellent:
				result = "QualityCategoryShort_Excellent".Translate();
				break;
			case QualityCategory.Masterwork:
				result = "QualityCategoryShort_Masterwork".Translate();
				break;
			case QualityCategory.Legendary:
				result = "QualityCategoryShort_Legendary".Translate();
				break;
			default:
				throw new ArgumentException();
			}
			return result;
		}

		public static bool FollowQualityThingFilter(this ThingDef def)
		{
			return def.stackLimit == 1 || def.HasComp(typeof(CompQuality));
		}

		public static QualityCategory GenerateQuality(QualityGenerator qualityGenerator)
		{
			QualityCategory result;
			switch (qualityGenerator)
			{
			case QualityGenerator.BaseGen:
				result = QualityUtility.GenerateQualityBaseGen();
				break;
			case QualityGenerator.Reward:
				result = QualityUtility.GenerateQualityReward();
				break;
			case QualityGenerator.Gift:
				result = QualityUtility.GenerateQualityGift();
				break;
			default:
				throw new NotImplementedException(qualityGenerator.ToString());
			}
			return result;
		}

		public static QualityCategory GenerateQualityRandomEqualChance()
		{
			return QualityUtility.AllQualityCategories.RandomElement<QualityCategory>();
		}

		public static QualityCategory GenerateQualityReward()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Excellent, QualityCategory.Good);
		}

		public static QualityCategory GenerateQualityGift()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Normal, QualityCategory.Normal);
		}

		public static QualityCategory GenerateQualityTraderItem()
		{
			QualityCategory result;
			if (Rand.Value < 0.25f)
			{
				result = QualityCategory.Normal;
			}
			else
			{
				QualityCategory qualityCategory = QualityUtility.GenerateFromGaussian(1.18f, QualityCategory.Masterwork, QualityCategory.Normal, QualityCategory.Poor);
				if (qualityCategory == QualityCategory.Poor && Rand.Value < 0.6f)
				{
					qualityCategory = QualityUtility.GenerateFromGaussian(1.18f, QualityCategory.Masterwork, QualityCategory.Normal, QualityCategory.Poor);
				}
				result = qualityCategory;
			}
			return result;
		}

		public static QualityCategory GenerateQualityBaseGen()
		{
			QualityCategory result;
			if (Rand.Value < 0.3f)
			{
				result = QualityCategory.Normal;
			}
			else
			{
				result = QualityUtility.GenerateFromGaussian(1f, QualityCategory.Excellent, QualityCategory.Normal, QualityCategory.Awful);
			}
			return result;
		}

		public static QualityCategory GenerateQualityGeneratingPawn(PawnKindDef pawnKind)
		{
			QualityCategory result;
			if (pawnKind.forceNormalGearQuality)
			{
				result = QualityCategory.Normal;
			}
			else if (Rand.Value < 0.25f)
			{
				result = pawnKind.itemQuality;
			}
			else
			{
				float centerX = (float)pawnKind.itemQuality + 0.5f;
				int num = (int)Rand.GaussianAsymmetric(centerX, 1.08f, 1.03f);
				num = Mathf.Clamp(num, 0, 4);
				num = Mathf.Clamp(num, (int)(pawnKind.itemQuality - QualityCategory.Normal), (int)(pawnKind.itemQuality + 2));
				result = (QualityCategory)num;
			}
			return result;
		}

		public static QualityCategory GenerateQualityCreatedByPawn(int relevantSkillLevel, bool inspired)
		{
			float num = 0f;
			switch (relevantSkillLevel)
			{
			case 0:
				num += 0.7f;
				break;
			case 1:
				num += 1.1f;
				break;
			case 2:
				num += 1.5f;
				break;
			case 3:
				num += 1.8f;
				break;
			case 4:
				num += 2f;
				break;
			case 5:
				num += 2.2f;
				break;
			case 6:
				num += 2.4f;
				break;
			case 7:
				num += 2.6f;
				break;
			case 8:
				num += 2.8f;
				break;
			case 9:
				num += 2.95f;
				break;
			case 10:
				num += 3.1f;
				break;
			case 11:
				num += 3.25f;
				break;
			case 12:
				num += 3.4f;
				break;
			case 13:
				num += 3.5f;
				break;
			case 14:
				num += 3.6f;
				break;
			case 15:
				num += 3.7f;
				break;
			case 16:
				num += 3.8f;
				break;
			case 17:
				num += 3.9f;
				break;
			case 18:
				num += 4f;
				break;
			case 19:
				num += 4.1f;
				break;
			case 20:
				num += 4.2f;
				break;
			}
			int num2 = (int)Rand.GaussianAsymmetric(num, 0.6f, 0.8f);
			num2 = Mathf.Clamp(num2, 0, 5);
			if (num2 == 5 && Rand.Value < 0.5f)
			{
				num2 = (int)Rand.GaussianAsymmetric(num, 0.6f, 0.95f);
				num2 = Mathf.Clamp(num2, 0, 5);
			}
			QualityCategory qualityCategory = (QualityCategory)num2;
			if (inspired)
			{
				qualityCategory = QualityUtility.AddLevels(qualityCategory, 2);
			}
			return qualityCategory;
		}

		public static QualityCategory GenerateQualityCreatedByPawn(Pawn pawn, SkillDef relevantSkill)
		{
			int level = pawn.skills.GetSkill(relevantSkill).Level;
			bool flag = pawn.InspirationDef == InspirationDefOf.Inspired_Creativity;
			QualityCategory result = QualityUtility.GenerateQualityCreatedByPawn(level, flag);
			if (flag)
			{
				pawn.mindState.inspirationHandler.EndInspiration(InspirationDefOf.Inspired_Creativity);
			}
			return result;
		}

		private static QualityCategory GenerateFromGaussian(float widthFactor, QualityCategory max = QualityCategory.Legendary, QualityCategory center = QualityCategory.Normal, QualityCategory min = QualityCategory.Awful)
		{
			float num = Rand.Gaussian((float)center + 0.5f, widthFactor);
			if (num < (float)min)
			{
				num = (float)min;
			}
			if (num > (float)max)
			{
				num = (float)max;
			}
			return (QualityCategory)((int)num);
		}

		private static QualityCategory AddLevels(QualityCategory quality, int levels)
		{
			return (QualityCategory)Mathf.Min((int)(quality + (byte)levels), 6);
		}

		public static void SendCraftNotification(Thing thing, Pawn worker)
		{
			if (worker != null)
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					if (compQuality.Quality == QualityCategory.Masterwork)
					{
						Messages.Message("MessageCraftedMasterwork".Translate(new object[]
						{
							worker.LabelShort,
							thing.LabelShort
						}), thing, MessageTypeDefOf.PositiveEvent, true);
					}
					else if (compQuality.Quality == QualityCategory.Legendary)
					{
						Find.LetterStack.ReceiveLetter("LetterCraftedLegendaryLabel".Translate(), "LetterCraftedLegendaryMessage".Translate(new object[]
						{
							worker.LabelShort,
							thing.LabelShort
						}), LetterDefOf.PositiveEvent, thing, null, null);
					}
				}
			}
		}

		[DebugOutput]
		internal static void QualityGenerationData()
		{
			List<TableDataGetter<QualityCategory>> list = new List<TableDataGetter<QualityCategory>>();
			list.Add(new TableDataGetter<QualityCategory>("quality", (QualityCategory q) => q.ToString()));
			list.Add(new TableDataGetter<QualityCategory>("Rewards\n(quests,\netc...? )", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityReward())));
			list.Add(new TableDataGetter<QualityCategory>("Trader\nitems", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityTraderItem())));
			list.Add(new TableDataGetter<QualityCategory>("Map generation\nitems and\nbuildings\n(e.g. NPC bases)", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityBaseGen())));
			list.Add(new TableDataGetter<QualityCategory>("Gifts", (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityGift())));
			for (int i = 0; i <= 20; i++)
			{
				int localLevel = i;
				list.Add(new TableDataGetter<QualityCategory>("Made\nat skill\n" + i, (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityCreatedByPawn(localLevel, false))));
			}
			foreach (PawnKindDef localPk2 in from k in DefDatabase<PawnKindDef>.AllDefs
			orderby k.combatPower
			select k)
			{
				PawnKindDef localPk = localPk2;
				if (localPk.RaceProps.Humanlike)
				{
					list.Add(new TableDataGetter<QualityCategory>(string.Concat(new object[]
					{
						"Gear for\n",
						localPk.defName,
						"\nPower ",
						localPk.combatPower.ToString("F0"),
						"\nitemQuality:\n",
						localPk.itemQuality
					}), (QualityCategory q) => QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityGeneratingPawn(localPk))));
				}
			}
			DebugTables.MakeTablesDialog<QualityCategory>(QualityUtility.AllQualityCategories, list.ToArray());
		}

		private static string DebugQualitiesStringSingle(QualityCategory quality, Func<QualityCategory> qualityGenerator)
		{
			int num = 10000;
			List<QualityCategory> list = new List<QualityCategory>();
			for (int i = 0; i < num; i++)
			{
				list.Add(qualityGenerator());
			}
			return ((float)(from q in list
			where q == quality
			select q).Count<QualityCategory>() / (float)num).ToStringPercent();
		}

		private static string DebugQualitiesString(Func<QualityCategory> qualityGenerator)
		{
			int num = 10000;
			StringBuilder stringBuilder = new StringBuilder();
			List<QualityCategory> list = new List<QualityCategory>();
			for (int i = 0; i < num; i++)
			{
				list.Add(qualityGenerator());
			}
			using (List<QualityCategory>.Enumerator enumerator = QualityUtility.AllQualityCategories.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					QualityCategory qu = enumerator.Current;
					stringBuilder.AppendLine(qu.ToString() + " - " + ((float)(from q in list
					where q == qu
					select q).Count<QualityCategory>() / (float)num).ToStringPercent());
				}
			}
			return stringBuilder.ToString();
		}

		[CompilerGenerated]
		private static string <QualityGenerationData>m__0(QualityCategory q)
		{
			return q.ToString();
		}

		[CompilerGenerated]
		private static string <QualityGenerationData>m__1(QualityCategory q)
		{
			return QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityReward());
		}

		[CompilerGenerated]
		private static string <QualityGenerationData>m__2(QualityCategory q)
		{
			return QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityTraderItem());
		}

		[CompilerGenerated]
		private static string <QualityGenerationData>m__3(QualityCategory q)
		{
			return QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityBaseGen());
		}

		[CompilerGenerated]
		private static string <QualityGenerationData>m__4(QualityCategory q)
		{
			return QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityGift());
		}

		[CompilerGenerated]
		private static float <QualityGenerationData>m__5(PawnKindDef k)
		{
			return k.combatPower;
		}

		[CompilerGenerated]
		private static QualityCategory <QualityGenerationData>m__6()
		{
			return QualityUtility.GenerateQualityReward();
		}

		[CompilerGenerated]
		private static QualityCategory <QualityGenerationData>m__7()
		{
			return QualityUtility.GenerateQualityTraderItem();
		}

		[CompilerGenerated]
		private static QualityCategory <QualityGenerationData>m__8()
		{
			return QualityUtility.GenerateQualityBaseGen();
		}

		[CompilerGenerated]
		private static QualityCategory <QualityGenerationData>m__9()
		{
			return QualityUtility.GenerateQualityGift();
		}

		[CompilerGenerated]
		private sealed class <QualityGenerationData>c__AnonStorey0
		{
			internal int localLevel;

			public <QualityGenerationData>c__AnonStorey0()
			{
			}

			internal string <>m__0(QualityCategory q)
			{
				return QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityCreatedByPawn(this.localLevel, false));
			}

			internal QualityCategory <>m__1()
			{
				return QualityUtility.GenerateQualityCreatedByPawn(this.localLevel, false);
			}
		}

		[CompilerGenerated]
		private sealed class <QualityGenerationData>c__AnonStorey1
		{
			internal PawnKindDef localPk;

			public <QualityGenerationData>c__AnonStorey1()
			{
			}

			internal string <>m__0(QualityCategory q)
			{
				return QualityUtility.DebugQualitiesStringSingle(q, () => QualityUtility.GenerateQualityGeneratingPawn(this.localPk));
			}

			internal QualityCategory <>m__1()
			{
				return QualityUtility.GenerateQualityGeneratingPawn(this.localPk);
			}
		}

		[CompilerGenerated]
		private sealed class <DebugQualitiesStringSingle>c__AnonStorey2
		{
			internal QualityCategory quality;

			public <DebugQualitiesStringSingle>c__AnonStorey2()
			{
			}

			internal bool <>m__0(QualityCategory q)
			{
				return q == this.quality;
			}
		}

		[CompilerGenerated]
		private sealed class <DebugQualitiesString>c__AnonStorey3
		{
			internal QualityCategory qu;

			public <DebugQualitiesString>c__AnonStorey3()
			{
			}

			internal bool <>m__0(QualityCategory q)
			{
				return q == this.qu;
			}
		}
	}
}
