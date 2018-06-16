using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000796 RID: 1942
	public class Alert_NeedDoctor : Alert
	{
		// Token: 0x06002B03 RID: 11011 RVA: 0x0016B292 File Offset: 0x00169692
		public Alert_NeedDoctor()
		{
			this.defaultLabel = "NeedDoctor".Translate();
			this.defaultPriority = AlertPriority.High;
		}

		// Token: 0x170006B1 RID: 1713
		// (get) Token: 0x06002B04 RID: 11012 RVA: 0x0016B2B4 File Offset: 0x001696B4
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

		// Token: 0x06002B05 RID: 11013 RVA: 0x0016B2D8 File Offset: 0x001696D8
		public override string GetExplanation()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Pawn pawn in this.Patients)
			{
				stringBuilder.AppendLine("    " + pawn.LabelShort);
			}
			return string.Format("NeedDoctorDesc".Translate(), stringBuilder.ToString());
		}

		// Token: 0x06002B06 RID: 11014 RVA: 0x0016B368 File Offset: 0x00169768
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
