using System;

namespace Steamworks
{
	// Token: 0x020000E1 RID: 225
	public static class Constants
	{
		// Token: 0x040002AA RID: 682
		public const string STEAMAPPLIST_INTERFACE_VERSION = "STEAMAPPLIST_INTERFACE_VERSION001";

		// Token: 0x040002AB RID: 683
		public const string STEAMAPPS_INTERFACE_VERSION = "STEAMAPPS_INTERFACE_VERSION007";

		// Token: 0x040002AC RID: 684
		public const string STEAMAPPTICKET_INTERFACE_VERSION = "STEAMAPPTICKET_INTERFACE_VERSION001";

		// Token: 0x040002AD RID: 685
		public const string STEAMCLIENT_INTERFACE_VERSION = "SteamClient017";

		// Token: 0x040002AE RID: 686
		public const string STEAMCONTROLLER_INTERFACE_VERSION = "STEAMCONTROLLER_INTERFACE_VERSION";

		// Token: 0x040002AF RID: 687
		public const string STEAMFRIENDS_INTERFACE_VERSION = "SteamFriends015";

		// Token: 0x040002B0 RID: 688
		public const string STEAMGAMECOORDINATOR_INTERFACE_VERSION = "SteamGameCoordinator001";

		// Token: 0x040002B1 RID: 689
		public const string STEAMGAMESERVER_INTERFACE_VERSION = "SteamGameServer012";

		// Token: 0x040002B2 RID: 690
		public const string STEAMGAMESERVERSTATS_INTERFACE_VERSION = "SteamGameServerStats001";

		// Token: 0x040002B3 RID: 691
		public const string STEAMHTMLSURFACE_INTERFACE_VERSION = "STEAMHTMLSURFACE_INTERFACE_VERSION_003";

		// Token: 0x040002B4 RID: 692
		public const string STEAMHTTP_INTERFACE_VERSION = "STEAMHTTP_INTERFACE_VERSION002";

		// Token: 0x040002B5 RID: 693
		public const string STEAMINVENTORY_INTERFACE_VERSION = "STEAMINVENTORY_INTERFACE_V001";

		// Token: 0x040002B6 RID: 694
		public const string STEAMMATCHMAKING_INTERFACE_VERSION = "SteamMatchMaking009";

		// Token: 0x040002B7 RID: 695
		public const string STEAMMATCHMAKINGSERVERS_INTERFACE_VERSION = "SteamMatchMakingServers002";

		// Token: 0x040002B8 RID: 696
		public const string STEAMMUSIC_INTERFACE_VERSION = "STEAMMUSIC_INTERFACE_VERSION001";

		// Token: 0x040002B9 RID: 697
		public const string STEAMMUSICREMOTE_INTERFACE_VERSION = "STEAMMUSICREMOTE_INTERFACE_VERSION001";

		// Token: 0x040002BA RID: 698
		public const string STEAMNETWORKING_INTERFACE_VERSION = "SteamNetworking005";

		// Token: 0x040002BB RID: 699
		public const string STEAMREMOTESTORAGE_INTERFACE_VERSION = "STEAMREMOTESTORAGE_INTERFACE_VERSION012";

		// Token: 0x040002BC RID: 700
		public const string STEAMSCREENSHOTS_INTERFACE_VERSION = "STEAMSCREENSHOTS_INTERFACE_VERSION002";

		// Token: 0x040002BD RID: 701
		public const string STEAMUGC_INTERFACE_VERSION = "STEAMUGC_INTERFACE_VERSION007";

		// Token: 0x040002BE RID: 702
		public const string STEAMUNIFIEDMESSAGES_INTERFACE_VERSION = "STEAMUNIFIEDMESSAGES_INTERFACE_VERSION001";

		// Token: 0x040002BF RID: 703
		public const string STEAMUSER_INTERFACE_VERSION = "SteamUser018";

		// Token: 0x040002C0 RID: 704
		public const string STEAMUSERSTATS_INTERFACE_VERSION = "STEAMUSERSTATS_INTERFACE_VERSION011";

		// Token: 0x040002C1 RID: 705
		public const string STEAMUTILS_INTERFACE_VERSION = "SteamUtils007";

		// Token: 0x040002C2 RID: 706
		public const string STEAMVIDEO_INTERFACE_VERSION = "STEAMVIDEO_INTERFACE_V001";

		// Token: 0x040002C3 RID: 707
		public const int k_cubAppProofOfPurchaseKeyMax = 64;

		// Token: 0x040002C4 RID: 708
		public const int k_iSteamUserCallbacks = 100;

		// Token: 0x040002C5 RID: 709
		public const int k_iSteamGameServerCallbacks = 200;

		// Token: 0x040002C6 RID: 710
		public const int k_iSteamFriendsCallbacks = 300;

		// Token: 0x040002C7 RID: 711
		public const int k_iSteamBillingCallbacks = 400;

		// Token: 0x040002C8 RID: 712
		public const int k_iSteamMatchmakingCallbacks = 500;

		// Token: 0x040002C9 RID: 713
		public const int k_iSteamContentServerCallbacks = 600;

		// Token: 0x040002CA RID: 714
		public const int k_iSteamUtilsCallbacks = 700;

		// Token: 0x040002CB RID: 715
		public const int k_iClientFriendsCallbacks = 800;

		// Token: 0x040002CC RID: 716
		public const int k_iClientUserCallbacks = 900;

		// Token: 0x040002CD RID: 717
		public const int k_iSteamAppsCallbacks = 1000;

		// Token: 0x040002CE RID: 718
		public const int k_iSteamUserStatsCallbacks = 1100;

		// Token: 0x040002CF RID: 719
		public const int k_iSteamNetworkingCallbacks = 1200;

		// Token: 0x040002D0 RID: 720
		public const int k_iClientRemoteStorageCallbacks = 1300;

		// Token: 0x040002D1 RID: 721
		public const int k_iClientDepotBuilderCallbacks = 1400;

		// Token: 0x040002D2 RID: 722
		public const int k_iSteamGameServerItemsCallbacks = 1500;

		// Token: 0x040002D3 RID: 723
		public const int k_iClientUtilsCallbacks = 1600;

		// Token: 0x040002D4 RID: 724
		public const int k_iSteamGameCoordinatorCallbacks = 1700;

		// Token: 0x040002D5 RID: 725
		public const int k_iSteamGameServerStatsCallbacks = 1800;

		// Token: 0x040002D6 RID: 726
		public const int k_iSteam2AsyncCallbacks = 1900;

		// Token: 0x040002D7 RID: 727
		public const int k_iSteamGameStatsCallbacks = 2000;

		// Token: 0x040002D8 RID: 728
		public const int k_iClientHTTPCallbacks = 2100;

		// Token: 0x040002D9 RID: 729
		public const int k_iClientScreenshotsCallbacks = 2200;

		// Token: 0x040002DA RID: 730
		public const int k_iSteamScreenshotsCallbacks = 2300;

		// Token: 0x040002DB RID: 731
		public const int k_iClientAudioCallbacks = 2400;

		// Token: 0x040002DC RID: 732
		public const int k_iClientUnifiedMessagesCallbacks = 2500;

		// Token: 0x040002DD RID: 733
		public const int k_iSteamStreamLauncherCallbacks = 2600;

		// Token: 0x040002DE RID: 734
		public const int k_iClientControllerCallbacks = 2700;

		// Token: 0x040002DF RID: 735
		public const int k_iSteamControllerCallbacks = 2800;

		// Token: 0x040002E0 RID: 736
		public const int k_iClientParentalSettingsCallbacks = 2900;

		// Token: 0x040002E1 RID: 737
		public const int k_iClientDeviceAuthCallbacks = 3000;

		// Token: 0x040002E2 RID: 738
		public const int k_iClientNetworkDeviceManagerCallbacks = 3100;

		// Token: 0x040002E3 RID: 739
		public const int k_iClientMusicCallbacks = 3200;

		// Token: 0x040002E4 RID: 740
		public const int k_iClientRemoteClientManagerCallbacks = 3300;

		// Token: 0x040002E5 RID: 741
		public const int k_iClientUGCCallbacks = 3400;

		// Token: 0x040002E6 RID: 742
		public const int k_iSteamStreamClientCallbacks = 3500;

		// Token: 0x040002E7 RID: 743
		public const int k_IClientProductBuilderCallbacks = 3600;

		// Token: 0x040002E8 RID: 744
		public const int k_iClientShortcutsCallbacks = 3700;

		// Token: 0x040002E9 RID: 745
		public const int k_iClientRemoteControlManagerCallbacks = 3800;

		// Token: 0x040002EA RID: 746
		public const int k_iSteamAppListCallbacks = 3900;

		// Token: 0x040002EB RID: 747
		public const int k_iSteamMusicCallbacks = 4000;

		// Token: 0x040002EC RID: 748
		public const int k_iSteamMusicRemoteCallbacks = 4100;

		// Token: 0x040002ED RID: 749
		public const int k_iClientVRCallbacks = 4200;

		// Token: 0x040002EE RID: 750
		public const int k_iClientReservedCallbacks = 4300;

		// Token: 0x040002EF RID: 751
		public const int k_iSteamReservedCallbacks = 4400;

		// Token: 0x040002F0 RID: 752
		public const int k_iSteamHTMLSurfaceCallbacks = 4500;

		// Token: 0x040002F1 RID: 753
		public const int k_iClientVideoCallbacks = 4600;

		// Token: 0x040002F2 RID: 754
		public const int k_iClientInventoryCallbacks = 4700;

		// Token: 0x040002F3 RID: 755
		public const int k_cchMaxFriendsGroupName = 64;

		// Token: 0x040002F4 RID: 756
		public const int k_cFriendsGroupLimit = 100;

		// Token: 0x040002F5 RID: 757
		public const int k_cEnumerateFollowersMax = 50;

		// Token: 0x040002F6 RID: 758
		public const int k_cchPersonaNameMax = 128;

		// Token: 0x040002F7 RID: 759
		public const int k_cwchPersonaNameMax = 32;

		// Token: 0x040002F8 RID: 760
		public const int k_cubChatMetadataMax = 8192;

		// Token: 0x040002F9 RID: 761
		public const int k_cchMaxRichPresenceKeys = 20;

		// Token: 0x040002FA RID: 762
		public const int k_cchMaxRichPresenceKeyLength = 64;

		// Token: 0x040002FB RID: 763
		public const int k_cchMaxRichPresenceValueLength = 256;

		// Token: 0x040002FC RID: 764
		public const int k_unServerFlagNone = 0;

		// Token: 0x040002FD RID: 765
		public const int k_unServerFlagActive = 1;

		// Token: 0x040002FE RID: 766
		public const int k_unServerFlagSecure = 2;

		// Token: 0x040002FF RID: 767
		public const int k_unServerFlagDedicated = 4;

		// Token: 0x04000300 RID: 768
		public const int k_unServerFlagLinux = 8;

		// Token: 0x04000301 RID: 769
		public const int k_unServerFlagPassworded = 16;

		// Token: 0x04000302 RID: 770
		public const int k_unServerFlagPrivate = 32;

		// Token: 0x04000303 RID: 771
		public const int k_unFavoriteFlagNone = 0;

		// Token: 0x04000304 RID: 772
		public const int k_unFavoriteFlagFavorite = 1;

		// Token: 0x04000305 RID: 773
		public const int k_unFavoriteFlagHistory = 2;

		// Token: 0x04000306 RID: 774
		public const int k_unMaxCloudFileChunkSize = 104857600;

		// Token: 0x04000307 RID: 775
		public const int k_cchPublishedDocumentTitleMax = 129;

		// Token: 0x04000308 RID: 776
		public const int k_cchPublishedDocumentDescriptionMax = 8000;

		// Token: 0x04000309 RID: 777
		public const int k_cchPublishedDocumentChangeDescriptionMax = 8000;

		// Token: 0x0400030A RID: 778
		public const int k_unEnumeratePublishedFilesMaxResults = 50;

		// Token: 0x0400030B RID: 779
		public const int k_cchTagListMax = 1025;

		// Token: 0x0400030C RID: 780
		public const int k_cchFilenameMax = 260;

		// Token: 0x0400030D RID: 781
		public const int k_cchPublishedFileURLMax = 256;

		// Token: 0x0400030E RID: 782
		public const int k_nScreenshotMaxTaggedUsers = 32;

		// Token: 0x0400030F RID: 783
		public const int k_nScreenshotMaxTaggedPublishedFiles = 32;

		// Token: 0x04000310 RID: 784
		public const int k_cubUFSTagTypeMax = 255;

		// Token: 0x04000311 RID: 785
		public const int k_cubUFSTagValueMax = 255;

		// Token: 0x04000312 RID: 786
		public const int k_ScreenshotThumbWidth = 200;

		// Token: 0x04000313 RID: 787
		public const int kNumUGCResultsPerPage = 50;

		// Token: 0x04000314 RID: 788
		public const int k_cchDeveloperMetadataMax = 5000;

		// Token: 0x04000315 RID: 789
		public const int k_cchStatNameMax = 128;

		// Token: 0x04000316 RID: 790
		public const int k_cchLeaderboardNameMax = 128;

		// Token: 0x04000317 RID: 791
		public const int k_cLeaderboardDetailsMax = 64;

		// Token: 0x04000318 RID: 792
		public const int k_cbMaxGameServerGameDir = 32;

		// Token: 0x04000319 RID: 793
		public const int k_cbMaxGameServerMapName = 32;

		// Token: 0x0400031A RID: 794
		public const int k_cbMaxGameServerGameDescription = 64;

		// Token: 0x0400031B RID: 795
		public const int k_cbMaxGameServerName = 64;

		// Token: 0x0400031C RID: 796
		public const int k_cbMaxGameServerTags = 128;

		// Token: 0x0400031D RID: 797
		public const int k_cbMaxGameServerGameData = 2048;

		// Token: 0x0400031E RID: 798
		public const int k_unSteamAccountIDMask = -1;

		// Token: 0x0400031F RID: 799
		public const int k_unSteamAccountInstanceMask = 1048575;

		// Token: 0x04000320 RID: 800
		public const int k_unSteamUserDesktopInstance = 1;

		// Token: 0x04000321 RID: 801
		public const int k_unSteamUserConsoleInstance = 2;

		// Token: 0x04000322 RID: 802
		public const int k_unSteamUserWebInstance = 4;

		// Token: 0x04000323 RID: 803
		public const int k_cchGameExtraInfoMax = 64;

		// Token: 0x04000324 RID: 804
		public const int k_nSteamEncryptedAppTicketSymmetricKeyLen = 32;

		// Token: 0x04000325 RID: 805
		public const int k_cubSaltSize = 8;

		// Token: 0x04000326 RID: 806
		public const ulong k_GIDNil = 18446744073709551615UL;

		// Token: 0x04000327 RID: 807
		public const ulong k_TxnIDNil = 18446744073709551615UL;

		// Token: 0x04000328 RID: 808
		public const ulong k_TxnIDUnknown = 0UL;

		// Token: 0x04000329 RID: 809
		public const int k_uPackageIdFreeSub = 0;

		// Token: 0x0400032A RID: 810
		public const int k_uPackageIdInvalid = -1;

		// Token: 0x0400032B RID: 811
		public const ulong k_ulAssetClassIdInvalid = 0UL;

		// Token: 0x0400032C RID: 812
		public const int k_uPhysicalItemIdInvalid = 0;

		// Token: 0x0400032D RID: 813
		public const int k_uCellIDInvalid = -1;

		// Token: 0x0400032E RID: 814
		public const int k_uPartnerIdInvalid = 0;

		// Token: 0x0400032F RID: 815
		public const int MAX_STEAM_CONTROLLERS = 16;

		// Token: 0x04000330 RID: 816
		public const int STEAM_RIGHT_TRIGGER_MASK = 1;

		// Token: 0x04000331 RID: 817
		public const int STEAM_LEFT_TRIGGER_MASK = 2;

		// Token: 0x04000332 RID: 818
		public const int STEAM_RIGHT_BUMPER_MASK = 4;

		// Token: 0x04000333 RID: 819
		public const int STEAM_LEFT_BUMPER_MASK = 8;

		// Token: 0x04000334 RID: 820
		public const int STEAM_BUTTON_0_MASK = 16;

		// Token: 0x04000335 RID: 821
		public const int STEAM_BUTTON_1_MASK = 32;

		// Token: 0x04000336 RID: 822
		public const int STEAM_BUTTON_2_MASK = 64;

		// Token: 0x04000337 RID: 823
		public const int STEAM_BUTTON_3_MASK = 128;

		// Token: 0x04000338 RID: 824
		public const int STEAM_TOUCH_0_MASK = 256;

		// Token: 0x04000339 RID: 825
		public const int STEAM_TOUCH_1_MASK = 512;

		// Token: 0x0400033A RID: 826
		public const int STEAM_TOUCH_2_MASK = 1024;

		// Token: 0x0400033B RID: 827
		public const int STEAM_TOUCH_3_MASK = 2048;

		// Token: 0x0400033C RID: 828
		public const int STEAM_BUTTON_MENU_MASK = 4096;

		// Token: 0x0400033D RID: 829
		public const int STEAM_BUTTON_STEAM_MASK = 8192;

		// Token: 0x0400033E RID: 830
		public const int STEAM_BUTTON_ESCAPE_MASK = 16384;

		// Token: 0x0400033F RID: 831
		public const int STEAM_BUTTON_BACK_LEFT_MASK = 32768;

		// Token: 0x04000340 RID: 832
		public const int STEAM_BUTTON_BACK_RIGHT_MASK = 65536;

		// Token: 0x04000341 RID: 833
		public const int STEAM_BUTTON_LEFTPAD_CLICKED_MASK = 131072;

		// Token: 0x04000342 RID: 834
		public const int STEAM_BUTTON_RIGHTPAD_CLICKED_MASK = 262144;

		// Token: 0x04000343 RID: 835
		public const int STEAM_LEFTPAD_FINGERDOWN_MASK = 524288;

		// Token: 0x04000344 RID: 836
		public const int STEAM_RIGHTPAD_FINGERDOWN_MASK = 1048576;

		// Token: 0x04000345 RID: 837
		public const int STEAM_JOYSTICK_BUTTON_MASK = 4194304;

		// Token: 0x04000346 RID: 838
		public const short MASTERSERVERUPDATERPORT_USEGAMESOCKETSHARE = -1;

		// Token: 0x04000347 RID: 839
		public const int INVALID_HTTPREQUEST_HANDLE = 0;

		// Token: 0x04000348 RID: 840
		public const byte k_nMaxLobbyKeyLength = 255;

		// Token: 0x04000349 RID: 841
		public const int k_SteamMusicNameMaxLength = 255;

		// Token: 0x0400034A RID: 842
		public const int k_SteamMusicPNGMaxLength = 65535;

		// Token: 0x0400034B RID: 843
		public const int QUERY_PORT_NOT_INITIALIZED = 65535;

		// Token: 0x0400034C RID: 844
		public const int QUERY_PORT_ERROR = 65534;
	}
}
