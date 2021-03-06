﻿using System;
using System.Collections.Generic;
//APP STORE LINK https://itunes.apple.com/us/app/super-jelly-troopers/id925880432?ls=1&mt=8
namespace MrBalloony
{
		public class GameConstants
		{
				public const int MINIMUM_COINS_FAILSAFE_PARACHUTE = 5;
				public const int MINIMUM_COINS_FAILSAFE_UMBRELLA = 10;

				public const string PLAYING_WORLD = "last_world";
				public const string PLAYING_LEVEL = "last_level";

				public const float WIDESCREEN_CORRECTION_VALUE = 64f;

				public const string SPIKES_LINE_BARRIER = "SpikesLine";

				//Unity Ads
				public const string UNITY_ADS_ANDROID_GAME_ID = "72442";
				public const string UNITY_ADS_IOS_GAME_ID = "72442";

				public const float MAX_SCREENSHOT_ROTATION = 45f;
				public const float MAX_SCREENSHOT_WIDTH = 256f;
				public const float MAX_SCREENSHOT_HEIGHT = 256f;
				public const float MIN_SCREENSHOT_WIDTH = 128f;
				public const float MIN_SCREENSHOT_HEIGHT = 128f;

				//the normal hitpoints for a player
				public const int NUM_LIFES_PER_LEVEL = 4;

				public const int METERS_BOUNDARY_TO_MOVE_GROUND = 50;//50/5 = 10 seconds

				public const int STARTING_TIME_IN_SECONDS = 300;
				public const int DEBUG_STARTING_TIME_IN_SECONDS = 60;
				public const int METERS_STEP = 5;//could be 4 in prod, 3 minimum

				public const int NUM_LEVELS_PER_WORLD = 1;
				public const int NUM_WORLDS = 8;

				public const string NEXT_SCENE_KEY = "NextScene";

				//THIS ARE JUST FOR THE CURRENT LEVEL
				//number of seconds left when finished the 
				public const string LEVEL_REMAINING_TIME_SECS = "remaining_level_time_secs";
				public const string LEVEL_REMAINING_LIFES = "remaining_lifes";
				public const string LEVEL_SAVED_JELLIES = "saved_jellies";

				public const string LEVEL_REMAINING_TIME_SECS_SCORE = "remaining_level_time_secs_score";
				public const string LEVEL_REMAINING_LIFES_SCORE = "remaining_lifes_score";
				public const string LEVEL_SAVED_JELLIES_SCORE = "saved_jellies_score";


				//for the game resume on game over
				public const string CURRENT_WORLD_KEY = "CurrentWorld";
				public const string CURRENT_LEVEL_KEY = "CurrentLevel";

				//Batch keys
				public const string BATCH_DEV_KEY_ANDROID ="DEV55F07A40CEA3A931948E83C6621";
				public const string BATCH_PROD_KEY_ANDROID ="55F07A40CC39F9874B7C29DDFA65F8";
				public const string BATCH_DEV_KEY_IOS ="DEV55F07A06A93469849BCA92C9389";
				public const string BATCH_PROD_KEY_IOS ="55F07A06A69E19FF8BCD5B57574764";

				//achievements same ids of GAME CENTER
				//com.pcdreams.mrballoony.achievement.level1
				public const string ACHIEVEMENT_LEVEL1_KEY = "com.pcdreams.mrballoony.achievement.level1";
				public const string ACHIEVEMENT_LEVEL2_KEY = "com.pcdreams.mrballoony.achievement.level2";
				public const string ACHIEVEMENT_LEVEL3_KEY = "com.pcdreams.mrballoony.achievement.level3";
				public const string ACHIEVEMENT_LEVEL4_KEY = "com.pcdreams.mrballoony.achievement.level4";
				public const string ACHIEVEMENT_LEVEL5_KEY = "com.pcdreams.mrballoony.achievement.level5";
				public const string ACHIEVEMENT_LEVEL6_KEY = "com.pcdreams.mrballoony.achievement.level6";
				public const string ACHIEVEMENT_LEVEL7_KEY = "com.pcdreams.mrballoony.achievement.level7";
				public const string ACHIEVEMENT_LEVEL8_KEY = "com.pcdreams.mrballoony.achievement.level8";
			
				//achievements points
				public const int ACHIEVEMENT_LEGEND_CHECKPOINT = 200; //saved 200 troopers
				public const int ACHIEVEMENT_HERO_CHECKPOINT = 150; //saved 150 troopers
				public const int ACHIEVEMENT_BRAVE_CHECKPOINT = 100; //saved 100 troopers
				//public const int ACHIEVEMENT_GURU_CHECKPOINT = END GAME :-)

				//leaderboards
				public const string LEADERBOARD_MAIN_SCORE = "com.pcdreams.mrballoony.leaderboard.mainscores";
				public const string LEADERBOARD_BEST_TIME = "jelly_troopers_best_time_leaderboard";
				public const string LEADERBOARD_LESS_DEATHS = "jelly_troopers_less_deaths_leaderboard";

				//local keys, for the leaderboards 
				//( THESE ARE GLOBALS/ENTIRE GAMEPLAY KEYS )
				public const string TOTAL_ELAPSED_TIME_SECS_KEY = "total_elapsed_time_secs";
				public const string TOTAL_LOST_LIFES_KEY = "total_lost_lifes";
				public const string TOTAL_SAVED_JELLIES_KEY = "total_saved_jellies";
		
				public const string BEST_SCORE_KEY = "best_score";//the best so far
				public const string HIGH_SCORE_KEY = "high_score";//the current score
				public const string PREVIOUS_BEST_SCORE_KEY = "previous_best_score";//the previous best

				public const string SOUND_SETTINGS_KEY = "sound_settings";
				public const string MUSIC_SETTINGS_KEY = "music_settings";
				public const string ACCELEROMETER_SETTINGS_KEY = "accelerometer_settings";

				//to track level unlocked/locked
				public const string UNLOCKED_LEVEL_KEY = "unlocked_level";//+ level number

				//missions
				public const string MISSION_1_KEY = "mission_1";
				public const string MISSION_2_KEY = "mission_2";
				public const string MISSION_3_KEY = "mission_3";
				public const string MISSION_4_KEY = "mission_4";

				//translation keys
				public const string MSG_HOW_TO_PLAY="how_to_play";
				public const string MSG_HOW_TO_PLAY_LAST_LEVEL="how_to_play_last_level";
				public const string MSG_TAP_TROOPER="tap_trooper";
				public const string MSG_TAP_LEFT_RIGHT="tap_left_right";
				public const string MSG_LAND_ALL="land_all_safely";
				public const string MSG_RESCUED="rescued";
				public const string MSG_LIFES="lifes";
				public const string MSG_INFINITE_LIFES="infinite_lifes";
				public const string MSG_TIME="time";
				public const string MSG_WORLD="world";
				public const string MSG_LEVEL="level";
				public const string MSG_HURRY_UP="hurry_up";
				public const string MSG_FAILSAFE="failsafe";
				public const string MSG_CONGRATULATIONS="congratulations";
				public const string MSG_NEXT="next";
				public const string MSG_HIGH_SCORE="high_sore";
				public const string MSG_HELPS_BOOSTERS="helps";
				public const string MSG_GET_SOME_ACTION="msg_action";
				public const string MSG_GET_LETS_DOIT="msg_doit";
				public const string MSG_GAME_CENTER_ERROR="game_center_error";
				public const string MSG_INTRUSION_ALERT="intrusion_alert";


				//IN-App
				public const string MSG_BUY_EXTRA_LIFES="buy_extra_lifes";
				public const string MSG_BUY_EXTRA_TIME="buy_extra_time";
				public const string MSG_BUY_EXTRA_SPEED="buy_extra_speed";
				public const string MSG_BUY_INFINITE_LIFES="buy_infinite_lifes";

				//in app purchases
				public const int 	IN_APP_PURCHASE_EXTRA_TIME_IN_SECONDS = 30; //extra 30 seconds
				public const int 	IN_APP_PURCHASE_EXTRA_LIFES_COUNT = 2; //2 extra lifes
				public const int 	IN_APP_PURCHASE_INFINITE_LIFES_COUNT = 1000; //1000 extra lifes, still dies because of time
				public const float 	IN_APP_PURCHASE_EXTRA_SPEED_INCREASE_FACTOR = 2.5f;//double the speed in x axis

				//#if UNITY_ANDROID && !UNITY_EDITOR


				public static Dictionary<string, string> ANDROID_DICTIONARY = new Dictionary<string, string>
		    	{
						{"jelly_troopers_main_leaderboard", "CgkI7La6roAbEAIQBg"},
						{"jelly_troopers_best_time_leaderboard", "CgkI7La6roAbEAIQBw"},
						{"jelly_troopers_less_deaths_leaderboard", "CgkI7La6roAbEAIQCA"},
						{"rescued_all_jelly_troopers", "CgkI7La6roAbEAIQAQ"},
						{"rescued_150_jelly_troopers", "CgkI7La6roAbEAIQAg"},
						{"rescued_200_jelly_troopers", "CgkI7La6roAbEAIQAw"},
						{"rescued_110_jelly_troopers", "CgkI7La6roAbEAIQBA"}
		    	};
				//#endif
		}
}

