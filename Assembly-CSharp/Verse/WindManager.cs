using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse.Noise;

namespace Verse
{
	// Token: 0x02000CB3 RID: 3251
	public class WindManager
	{
		// Token: 0x06004794 RID: 18324 RVA: 0x0025B434 File Offset: 0x00259834
		public WindManager(Map map)
		{
			this.map = map;
		}

		// Token: 0x17000B4B RID: 2891
		// (get) Token: 0x06004795 RID: 18325 RVA: 0x0025B458 File Offset: 0x00259858
		public float WindSpeed
		{
			get
			{
				return this.cachedWindSpeed;
			}
		}

		// Token: 0x06004796 RID: 18326 RVA: 0x0025B474 File Offset: 0x00259874
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

		// Token: 0x06004797 RID: 18327 RVA: 0x0025B57C File Offset: 0x0025997C
		public static void Notify_PlantMaterialCreated(Material newMat)
		{
			WindManager.plantMaterials.Add(newMat);
		}

		// Token: 0x06004798 RID: 18328 RVA: 0x0025B58C File Offset: 0x0025998C
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

		// Token: 0x06004799 RID: 18329 RVA: 0x0025B664 File Offset: 0x00259A64
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

		// Token: 0x0600479A RID: 18330 RVA: 0x0025B6B0 File Offset: 0x00259AB0
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

		// Token: 0x04003091 RID: 12433
		private Map map;

		// Token: 0x04003092 RID: 12434
		private static List<Material> plantMaterials = new List<Material>();

		// Token: 0x04003093 RID: 12435
		private float cachedWindSpeed;

		// Token: 0x04003094 RID: 12436
		private ModuleBase windNoise = null;

		// Token: 0x04003095 RID: 12437
		private float plantSwayHead = 0f;
	}
}
