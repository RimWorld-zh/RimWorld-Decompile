using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000792 RID: 1938
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x06002AFE RID: 11006 RVA: 0x0016B4FE File Offset: 0x001698FE
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B2 RID: 1714
		// (get) Token: 0x06002AFF RID: 11007 RVA: 0x0016B520 File Offset: 0x00169920
		private IEnumerable<Pawn> Patients
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (maps[i].IsPlayerHome)
					{
						bool healthyDoc = false;
						foreach (Pawn pawn in maps[i].mapPawns.FreeColonistsSpawned)
						{
							if (!pawn.Downed && pawn.workSettings != null && pawn.workSettings.WorkIsActive(WorkTypeDefOf.Doctor))
							{
								healthyDoc = true;
								break;
							}
						}
						if (!healthyDoc)
						{
							foreach (Pawn p in maps[i].mapPawns.FreeColonistsSpawned)
							{
								if ((p.Downed && p.needs.food.CurCategory < HungerCategory.Fed && p.InBed()) || HealthAIUtility.ShouldBeTendedNowByPlayer(p))
								{
									yield return p;
								}
							}
						}
					}
				}
				yield break;
			}
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x0016B544 File Offset: 0x00169944
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("NeedDoctorDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x0016B5D4 File Offset: 0x001699D4
		public override AlertReport GetReport()
		{
			AlertReport result;
			if (Find.AnyPlayerHomeMap == null)
			{
				result = false;
			}
			else
			{
				result = AlertReport.CulpritsAre(this.Patients);
			}
			return result;
		}
	}
}
