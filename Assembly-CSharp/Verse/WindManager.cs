using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000CB2 RID: 3250
	public class WindManager
	{
		// Token: 0x06004792 RID: 18322 RVA: 0x0025B40C File Offset: 0x0025980C
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4A RID: 2890
		// (get) Token: 0x06004793 RID: 18323 RVA: 0x0025B430 File Offset: 0x00259830
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x06004794 RID: 18324 RVA: 0x0025B44C File Offset: 0x0025984C
		public void WindManagerTick()
		{
			this.cachedWindSpeed = this.BaseWindSpeedAt(Find.TickManager.TicksAbs) * this.map.weatherManager.CurWindSpeedFactor;
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.WindSource);
			for (int i = 0; i < list.Count; i++)
			{
				CompWindSource compWindSource = list[i].TryGetComp<CompWindSource>();
				this.cachedWindSpeed = Mathf.Max(this.cachedWindSpeed, compWindSource.wind);
			}
			if (Prefs.PlantWindSway)
			{
				this.plantSwayHead += Mathf.Min(this.WindSpeed, 1f);
			}
			else
			{
				this.plantSwayHead = 0f;
			}
			if (Find.CurrentMap == this.map)
			{
				for (int j = 0; j < WindManager.plantMaterials.Count; j++)
				{
					WindManager.plantMaterials[j].SetFloat(ShaderPropertyIDs.SwayHead, this.plantSwayHead);
				}
			}
		}

		// Token: 0x06004795 RID: 18325 RVA: 0x0025B554 File Offset: 0x00259954
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0025B564 File Offset: 0x00259964
		private float BaseWindSpeedAt(int ticksAbs)
		{
			if (this.windNoise == null)
			{
				int seed = Gen.HashCombineInt(this.map.Tile, 122049541) ^ Find.World.info.Seed;
				this.windNoise = new Perlin(3.9999998989515007E-05, 2.0, 0.5, 4, seed, QualityMode.Medium);
				this.windNoise = new ScaleBias(1.5, 0.5, this.windNoise);
				this.windNoise = new Clamp(0.039999999105930328, 2.0, this.windNoise);
			}
			return (float)this.windNoise.GetValue((double)ticksAbs, 0.0, 0.0);
		}

		// Token: 0x06004797 RID: 18327 RVA: 0x0025B63C File Offset: 0x00259A3C
		public string DebugString()
		{
			return string.Concat(new object[]
			{
				"WindSpeed: ",
				this.WindSpeed,
				"\nplantSwayHead: ",
				this.plantSwayHead
			});
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0025B688 File Offset: 0x00259A88
		public void LogWindSpeeds()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("Upcoming wind speeds:");
			for (int i = 0; i < 72; i++)
			{
				stringBuilder.AppendLine(string.Concat(new object[]
				{
					"Hour ",
					i,
					" - ",
					this.BaseWindSpeedAt(Find.TickManager.TicksAbs + 2500 * i).ToString("F2")
				}));
			}
			Log.Message(stringBuilder.ToString(), false);
		}

		// Token: 0x0400308F RID: 12431
		private Map map;

		// Token: 0x04003090 RID: 12432
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x04003091 RID: 12433
		private float cachedWindSpeed;

		// Token: 0x04003092 RID: 12434
		private ModuleBase windNoise = null;

		// Token: 0x04003093 RID: 12435
		private float plantSwayHead = 0f;
	}
}
