using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000F3A RID: 3898
	public static class GenDebug
	{
		// Token: 0x06005DF1 RID: 24049 RVA: 0x002FC83C File Offset: 0x002FAC3C
		public static void DebugPlaceSphere(Vector3 Loc, float Scale)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = Loc;
			gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
		}

		// Token: 0x06005DF2 RID: 24050 RVA: 0x002FC870 File Offset: 0x002FAC70
		public static void LogList<T>(IEnumerable<T> list)
		{
			foreach (T t in list)
			{
				Log.Message("    " + t.ToString(), false);
			}
		}

		// Token: 0x06005DF3 RID: 24051 RVA: 0x002FC8E0 File Offset: 0x002FACE0
		public static void ClearArea(CellRect r, Map map)
		{
			r.ClipInsideMap(map);
			foreach (IntVec3 c in r)
			{
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 c2 in r)
			{
				foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.destroyable)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}
	}
}
