using System;

namespace Steamworks
{
	// Token: 0x0200013D RID: 317
	public static class SteamHTMLSurface
	{
		// Token: 0x0600056B RID: 1387 RVA: 0x00007420 File Offset: 0x00005620
		public static bool Init()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Init();
		}

		// Token: 0x0600056C RID: 1388 RVA: 0x00007440 File Offset: 0x00005640
		public static bool Shutdown()
		{
			InteropHelp.TestIfAvailableClient();
			return NativeMethods.ISteamHTMLSurface_Shutdown();
		}

		// Token: 0x0600056D RID: 1389 RVA: 0x00007460 File Offset: 0x00005660
		public static SteamAPICall_t CreateBrowser(string pchUserAgent, string pchUserCSS)
		{
			InteropHelp.TestIfAvailableClient();
			SteamAPICall_t result;
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchUserAgent))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchUserCSS))
				{
					result = (SteamAPICall_t)NativeMethods.ISteamHTMLSurface_CreateBrowser(utf8StringHandle, utf8StringHandle2);
				}
			}
			return result;
		}

		// Token: 0x0600056E RID: 1390 RVA: 0x000074CC File Offset: 0x000056CC
		public static void RemoveBrowser(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_RemoveBrowser(unBrowserHandle);
		}

		// Token: 0x0600056F RID: 1391 RVA: 0x000074DC File Offset: 0x000056DC
		public static void LoadURL(HHTMLBrowser unBrowserHandle, string pchURL, string pchPostData)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchURL))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchPostData))
				{
					NativeMethods.ISteamHTMLSurface_LoadURL(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x06000570 RID: 1392 RVA: 0x00007548 File Offset: 0x00005748
		public static void SetSize(HHTMLBrowser unBrowserHandle, uint unWidth, uint unHeight)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetSize(unBrowserHandle, unWidth, unHeight);
		}

		// Token: 0x06000571 RID: 1393 RVA: 0x00007558 File Offset: 0x00005758
		public static void StopLoad(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopLoad(unBrowserHandle);
		}

		// Token: 0x06000572 RID: 1394 RVA: 0x00007566 File Offset: 0x00005766
		public static void Reload(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_Reload(unBrowserHandle);
		}

		// Token: 0x06000573 RID: 1395 RVA: 0x00007574 File Offset: 0x00005774
		public static void GoBack(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoBack(unBrowserHandle);
		}

		// Token: 0x06000574 RID: 1396 RVA: 0x00007582 File Offset: 0x00005782
		public static void GoForward(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GoForward(unBrowserHandle);
		}

		// Token: 0x06000575 RID: 1397 RVA: 0x00007590 File Offset: 0x00005790
		public static void AddHeader(HHTMLBrowser unBrowserHandle, string pchKey, string pchValue)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchKey))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchValue))
				{
					NativeMethods.ISteamHTMLSurface_AddHeader(unBrowserHandle, utf8StringHandle, utf8StringHandle2);
				}
			}
		}

		// Token: 0x06000576 RID: 1398 RVA: 0x000075FC File Offset: 0x000057FC
		public static void ExecuteJavascript(HHTMLBrowser unBrowserHandle, string pchScript)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchScript))
			{
				NativeMethods.ISteamHTMLSurface_ExecuteJavascript(unBrowserHandle, utf8StringHandle);
			}
		}

		// Token: 0x06000577 RID: 1399 RVA: 0x00007644 File Offset: 0x00005844
		public static void MouseUp(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseUp(unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000578 RID: 1400 RVA: 0x00007653 File Offset: 0x00005853
		public static void MouseDown(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDown(unBrowserHandle, eMouseButton);
		}

		// Token: 0x06000579 RID: 1401 RVA: 0x00007662 File Offset: 0x00005862
		public static void MouseDoubleClick(HHTMLBrowser unBrowserHandle, EHTMLMouseButton eMouseButton)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseDoubleClick(unBrowserHandle, eMouseButton);
		}

		// Token: 0x0600057A RID: 1402 RVA: 0x00007671 File Offset: 0x00005871
		public static void MouseMove(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseMove(unBrowserHandle, x, y);
		}

		// Token: 0x0600057B RID: 1403 RVA: 0x00007681 File Offset: 0x00005881
		public static void MouseWheel(HHTMLBrowser unBrowserHandle, int nDelta)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_MouseWheel(unBrowserHandle, nDelta);
		}

		// Token: 0x0600057C RID: 1404 RVA: 0x00007690 File Offset: 0x00005890
		public static void KeyDown(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyDown(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x0600057D RID: 1405 RVA: 0x000076A0 File Offset: 0x000058A0
		public static void KeyUp(HHTMLBrowser unBrowserHandle, uint nNativeKeyCode, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyUp(unBrowserHandle, nNativeKeyCode, eHTMLKeyModifiers);
		}

		// Token: 0x0600057E RID: 1406 RVA: 0x000076B0 File Offset: 0x000058B0
		public static void KeyChar(HHTMLBrowser unBrowserHandle, uint cUnicodeChar, EHTMLKeyModifiers eHTMLKeyModifiers)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_KeyChar(unBrowserHandle, cUnicodeChar, eHTMLKeyModifiers);
		}

		// Token: 0x0600057F RID: 1407 RVA: 0x000076C0 File Offset: 0x000058C0
		public static void SetHorizontalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetHorizontalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x06000580 RID: 1408 RVA: 0x000076CF File Offset: 0x000058CF
		public static void SetVerticalScroll(HHTMLBrowser unBrowserHandle, uint nAbsolutePixelScroll)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetVerticalScroll(unBrowserHandle, nAbsolutePixelScroll);
		}

		// Token: 0x06000581 RID: 1409 RVA: 0x000076DE File Offset: 0x000058DE
		public static void SetKeyFocus(HHTMLBrowser unBrowserHandle, bool bHasKeyFocus)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetKeyFocus(unBrowserHandle, bHasKeyFocus);
		}

		// Token: 0x06000582 RID: 1410 RVA: 0x000076ED File Offset: 0x000058ED
		public static void ViewSource(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_ViewSource(unBrowserHandle);
		}

		// Token: 0x06000583 RID: 1411 RVA: 0x000076FB File Offset: 0x000058FB
		public static void CopyToClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_CopyToClipboard(unBrowserHandle);
		}

		// Token: 0x06000584 RID: 1412 RVA: 0x00007709 File Offset: 0x00005909
		public static void PasteFromClipboard(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_PasteFromClipboard(unBrowserHandle);
		}

		// Token: 0x06000585 RID: 1413 RVA: 0x00007718 File Offset: 0x00005918
		public static void Find(HHTMLBrowser unBrowserHandle, string pchSearchStr, bool bCurrentlyInFind, bool bReverse)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchSearchStr))
			{
				NativeMethods.ISteamHTMLSurface_Find(unBrowserHandle, utf8StringHandle, bCurrentlyInFind, bReverse);
			}
		}

		// Token: 0x06000586 RID: 1414 RVA: 0x00007760 File Offset: 0x00005960
		public static void StopFind(HHTMLBrowser unBrowserHandle)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_StopFind(unBrowserHandle);
		}

		// Token: 0x06000587 RID: 1415 RVA: 0x0000776E File Offset: 0x0000596E
		public static void GetLinkAtPosition(HHTMLBrowser unBrowserHandle, int x, int y)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_GetLinkAtPosition(unBrowserHandle, x, y);
		}

		// Token: 0x06000588 RID: 1416 RVA: 0x00007780 File Offset: 0x00005980
		public static void SetCookie(string pchHostname, string pchKey, string pchValue, string pchPath = "/", uint nExpires = 0u, bool bSecure = false, bool bHTTPOnly = false)
		{
			InteropHelp.TestIfAvailableClient();
			using (InteropHelp.UTF8StringHandle utf8StringHandle = new InteropHelp.UTF8StringHandle(pchHostname))
			{
				using (InteropHelp.UTF8StringHandle utf8StringHandle2 = new InteropHelp.UTF8StringHandle(pchKey))
				{
					using (InteropHelp.UTF8StringHandle utf8StringHandle3 = new InteropHelp.UTF8StringHandle(pchValue))
					{
						using (InteropHelp.UTF8StringHandle utf8StringHandle4 = new InteropHelp.UTF8StringHandle(pchPath))
						{
							NativeMethods.ISteamHTMLSurface_SetCookie(utf8StringHandle, utf8StringHandle2, utf8StringHandle3, utf8StringHandle4, nExpires, bSecure, bHTTPOnly);
						}
					}
				}
			}
		}

		// Token: 0x06000589 RID: 1417 RVA: 0x0000783C File Offset: 0x00005A3C
		public static void SetPageScaleFactor(HHTMLBrowser unBrowserHandle, float flZoom, int nPointX, int nPointY)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetPageScaleFactor(unBrowserHandle, flZoom, nPointX, nPointY);
		}

		// Token: 0x0600058A RID: 1418 RVA: 0x0000784D File Offset: 0x00005A4D
		public static void SetBackgroundMode(HHTMLBrowser unBrowserHandle, bool bBackgroundMode)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_SetBackgroundMode(unBrowserHandle, bBackgroundMode);
		}

		// Token: 0x0600058B RID: 1419 RVA: 0x0000785C File Offset: 0x00005A5C
		public static void AllowStartRequest(HHTMLBrowser unBrowserHandle, bool bAllowed)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_AllowStartRequest(unBrowserHandle, bAllowed);
		}

		// Token: 0x0600058C RID: 1420 RVA: 0x0000786B File Offset: 0x00005A6B
		public static void JSDialogResponse(HHTMLBrowser unBrowserHandle, bool bResult)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_JSDialogResponse(unBrowserHandle, bResult);
		}

		// Token: 0x0600058D RID: 1421 RVA: 0x0000787A File Offset: 0x00005A7A
		public static void FileLoadDialogResponse(HHTMLBrowser unBrowserHandle, IntPtr pchSelectedFiles)
		{
			InteropHelp.TestIfAvailableClient();
			NativeMethods.ISteamHTMLSurface_FileLoadDialogResponse(unBrowserHandle, pchSelectedFiles);
		}
	}
}
