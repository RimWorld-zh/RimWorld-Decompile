using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020000F8 RID: 248
	public class JobGiver_GetJoy : ThinkNode_JobGiver
	{
		// Token: 0x040002CF RID: 719
		[Unsaved]
		private DefMap<JoyGiverDef, float> joyGiverChances;

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x06000537 RID: 1335 RVA: 0x00034934 File Offset: 0x00032D34
		protected virtual bool CanDoDuringMedicalRest
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0003494C File Offset: 0x00032D4C
		protected virtual bool JoyGiverAllowed(JoyGiverDef def)
		{
			return true;
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x00034964 File Offset: 0x00032D64
		protected virtual Job TryGiveJobFromJoyGiverDefDirect(JoyGiverDef def, Pawn pawn)
		{
			return def.Worker.TryGiveJob(pawn);
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x00034985 File Offset: 0x00032D85
		public override void ResolveReferences()
		{
			this.joyGiverChances = new DefMap<JoyGiverDef, float>();
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x00034994 File Offset: 0x00032D94
		protected override Job TryGiveJob(Pawn pawn)
		{
			Job result;
			if (!this.CanDoDuringMedicalRest && pawn.InBed() && HealthAIUtility.ShouldSeekMedicalRest(pawn))
			{
				result = null;
			}
			else
			{
				Profiler.BeginSample("GetJoy");
				List<JoyGiverDef> allDefsListForReading = DefDatabase<JoyGiverDef>.AllDefsListForReading;
				JoyToleranceSet tolerances = pawn.needs.joy.tolerances;
				for (int i = 0; i < allDefsListForReading.Count; i++)
				{
					JoyGiverDef joyGiverDef = allDefsListForReading[i];
					this.joyGiverChances[joyGiverDef] = 0f;
					if (this.JoyGiverAllowed(joyGiverDef))
					{
						if (joyGiverDef.Worker.MissingRequiredCapacity(pawn) == null)
						{
							if (joyGiverDef.pctPawnsEverDo < 1f)
							{
								Rand.PushState(pawn.thingIDNumber ^ 63216713);
								if (Rand.Value >= joyGiverDef.pctPawnsEverDo)
								{
									Rand.PopState();
									goto IL_12A;
								}
								Rand.PopState();
							}
							float num = tolerances[joyGiverDef.joyKind];
							float num2 = Mathf.Pow(1f - num, 5f);
							num2 = Mathf.Max(0.001f, num2);
							this.joyGiverChances[joyGiverDef] = joyGiverDef.Worker.GetChance(pawn) * num2;
						}
					}
					IL_12A:;
				}
				for (int j = 0; j < this.joyGiverChances.Count; j++)
				{
					JoyGiverDef def;
					if (!allDefsListForReading.TryRandomElementByWeight((JoyGiverDef d) => this.joyGiverChances[d], out def))
					{
						break;
					}
					Job job = this.TryGiveJobFromJoyGiverDefDirect(def, pawn);
					if (job != null)
					{
						Profiler.EndSample();
						return job;
					}
					this.joyGiverChances[def] = 0f;
				}
				Profiler.EndSample();
				result = null;
			}
			return result;
		}
	}
}
