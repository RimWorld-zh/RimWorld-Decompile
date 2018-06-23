using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000749 RID: 1865
	[HasDebugOutput]
	public static class QualityUtility
	{
		// Token: 0x0400168B RID: 5771
		public static List<QualityCategory> AllQualityCategories = new List<QualityCategory>();

		// Token: 0x06002955 RID: 10581 RVA: 0x0015F6D0 File Offset: 0x0015DAD0
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

		// Token: 0x06002956 RID: 10582 RVA: 0x0015F750 File Offset: 0x0015DB50
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

		// Token: 0x06002957 RID: 10583 RVA: 0x0015F7A4 File Offset: 0x0015DBA4
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

		// Token: 0x06002958 RID: 10584 RVA: 0x0015F850 File Offset: 0x0015DC50
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

		// Token: 0x06002959 RID: 10585 RVA: 0x0015F8FC File Offset: 0x0015DCFC
		public static bool FollowQualityThingFilter(this ThingDef def)
		{
			return def.stackLimit == 1 || def.HasComp(typeof(CompQuality));
		}

		// Token: 0x0600295A RID: 10586 RVA: 0x0015F944 File Offset: 0x0015DD44
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

		// Token: 0x0600295B RID: 10587 RVA: 0x0015F9A0 File Offset: 0x0015DDA0
		public static QualityCategory GenerateQualityRandomEqualChance()
		{
			return QualityUtility.AllQualityCategories.RandomElement<QualityCategory>();
		}

		// Token: 0x0600295C RID: 10588 RVA: 0x0015F9C0 File Offset: 0x0015DDC0
		public static QualityCategory GenerateQualityReward()
		{
			return QualityUtility.GenerateFromGaussian(1.1f, QualityCategory.Legendary, QualityCategory.Excellent, QualityCategory.Excellent);
		}

		// Token: 0x0600295D RID: 10589 RVA: 0x0015F9E4 File Offset: 0x0015DDE4
		public static QualityCategory GenerateQualityGift()
		{
			return QualityUtility.GenerateFromGaussian(1f, QualityCategory.Legendary, QualityCategory.Normal, QualityCategory.Normal);
		}

		// Token: 0x0600295E RID: 10590 RVA: 0x0015FA08 File Offset: 0x0015DE08
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

		// Token: 0x0600295F RID: 10591 RVA: 0x0015FA68 File Offset: 0x0015DE68
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

		// Token: 0x06002960 RID: 10592 RVA: 0x0015FAA0 File Offset: 0x0015DEA0
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

		// Token: 0x06002961 RID: 10593 RVA: 0x0015FB24 File Offset: 0x0015DF24
		public static QualityCategory GenerateQualityCreatedByPawn(int relevantSkillLevel, bool inspired)
		{
			float centerX = -1f;
			switch (relevantSkillLevel)
			{
			case 0:
				centerX = 0.5f;
				break;
			case 1:
				centerX = 0.833f;
				break;
			case 2:
				centerX = 1.166f;
				break;
			case 3:
				centerX = 1.5f;
				break;
			case 4:
				centerX = 1.7f;
				break;
			case 5:
				centerX = 1.9f;
				break;
			case 6:
				centerX = 2.1f;
				break;
			case 7:
				centerX = 2.3f;
				break;
			case 8:
				centerX = 2.5f;
				break;
			case 9:
				centerX = 2.65f;
				break;
			case 10:
				centerX = 2.8f;
				break;
			case 11:
				centerX = 2.95f;
				break;
			case 12:
				centerX = 3.1f;
				break;
			case 13:
				centerX = 3.25f;
				break;
			case 14:
				centerX = 3.4f;
				break;
			case 15:
				centerX = 3.5f;
				break;
			case 16:
				centerX = 3.6f;
				break;
			case 17:
				centerX = 3.7f;
				break;
			case 18:
				centerX = 3.8f;
				break;
			case 19:
				centerX = 3.9f;
				break;
			case 20:
				centerX = 4f;
				break;
			}
			int num = (int)Rand.GaussianAsymmetric(centerX, 0.7f, 0.96f);
			num = Mathf.Clamp(num, 0, 5);
			if (num == 5 && Rand.Value < 0.5f)
			{
				num = (int)Rand.GaussianAsymmetric(centerX, 0.6f, 0.95f);
				num = Mathf.Clamp(num, 0, 5);
			}
			QualityCategory qualityCategory = (QualityCategory)num;
			if (inspired)
			{
				qualityCategory = QualityUtility.AddLevels(qualityCategory, 2);
			}
			return qualityCategory;
		}

		// Token: 0x06002962 RID: 10594 RVA: 0x0015FCE8 File Offset: 0x0015E0E8
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

		// Token: 0x06002963 RID: 10595 RVA: 0x0015FD44 File Offset: 0x0015E144
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

		// Token: 0x06002964 RID: 10596 RVA: 0x0015FD84 File Offset: 0x0015E184
		private static QualityCategory AddLevels(QualityCategory quality, int levels)
		{
			return (QualityCategory)Mathf.Min((int)(quality + (byte)levels), 6);
		}

		// Token: 0x06002965 RID: 10597 RVA: 0x0015FDA4 File Offset: 0x0015E1A4
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

		// Token: 0x06002966 RID: 10598 RVA: 0x0015FE64 File Offset: 0x0015E264
		[DebugOutput]
		internal static void QualityGenerationData()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Qualities of items/buildings generated from various sources");
			stringBuilder.AppendLine("---------------------------------------------------------------------");
			stringBuilder.AppendLine("Rewards (quests, etc...? )");
			stringBuilder.AppendLine(QualityUtility.DebugQualitiesString(() => QualityUtility.GenerateQualityReward()));
			stringBuilder.AppendLine("Trader items");
			stringBuilder.AppendLine(QualityUtility.DebugQualitiesString(() => QualityUtility.GenerateQualityTraderItem()));
			stringBuilder.AppendLine("Map generation items and buildings (usually NPC bases)");
			stringBuilder.AppendLine(QualityUtility.DebugQualitiesString(() => QualityUtility.GenerateQualityBaseGen()));
			stringBuilder.AppendLine("Gifts");
			stringBuilder.AppendLine(QualityUtility.DebugQualitiesString(() => QualityUtility.GenerateQualityGift()));
			using (IEnumerator<PawnKindDef> enumerator = (from k in DefDatabase<PawnKindDef>.AllDefs
			orderby k.combatPower
			select k).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PawnKindDef pk = enumerator.Current;
					if (pk.RaceProps.Humanlike)
					{
						stringBuilder.AppendLine(string.Concat(new object[]
						{
							"Items generated to equip pawn kind: ",
							pk.defName,
							" (",
							pk.combatPower.ToString("F0"),
							" points, itemQuality ",
							pk.itemQuality,
							")"
						}));
						stringBuilder.Append(QualityUtility.DebugQualitiesString(() => QualityUtility.GenerateQualityGeneratingPawn(pk)));
						stringBuilder.AppendLine();
					}
				}
			}
			int level;
			for (level = 0; level <= 20; level++)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Items/buildings made by crafter/builder at skill level " + level);
				stringBuilder.Append(QualityUtility.DebugQualitiesString(() => QualityUtility.GenerateQualityCreatedByPawn(level, false)));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x06002967 RID: 10599 RVA: 0x001600F4 File Offset: 0x0015E4F4
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
	}
}
