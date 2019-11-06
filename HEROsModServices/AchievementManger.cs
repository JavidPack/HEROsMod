/*
using MonoMod.Cil;
using MonoMod.RuntimeDetour.HookGen;
using System;
using Terraria;
using Terraria.DataStructures;
using Terraria.GameContent.Achievements;
using Terraria.ModLoader;
using Terraria.UI;

namespace HEROsMod.HEROsModServices
{
	static class AchievementManger
	{
		//public delegate void orig_Value(CustomFloatCondition self, float value);

		internal static void Load()
		{
			On.Terraria.Achievements.AchievementCondition.Complete += AchievementCondition_Complete;
			//On.Terraria.GameContent.Achievements.CustomFloatCondition.

			//Action<Action<T, V>, T, V> where T is object type and V is property type

			//var set = new orig_Value(CustomIntCondition_Value_set);
			//HookEndpointManager.Add(typeof(CustomFloatCondition).GetProperty("ValidateOutput").GetSetMethod(), set);
			//HookEndpointManager.Add(typeof(CustomFloatCondition).GetProperty("Value").GetSetMethod(), (CustomFloatCondition self, float val) => { CustomIntCondition_Value_set(self, val); });
			Action<Action<CustomFloatCondition, float>, CustomFloatCondition, float> action = (Action<CustomFloatCondition, float> a, CustomFloatCondition self, float b) => CustomFloatCondition_Value_set(a, self, b);
			HookEndpointManager.Add(typeof(CustomFloatCondition).GetProperty("Value").GetSetMethod(), action);
	//HookEndpointManager.Add(typeof(CustomFloatCondition).GetProperty("Value").GetSetMethod(), (Action<Action<CustomFloatCondition, int>, CustomFloatCondition, int>)(Action<CustomFloatCondition, int> a, CustomFloatCondition self, int b) => CustomIntCondition_Value_set(self, b));

		}

		private static void AchievementCondition_Complete(On.Terraria.Achievements.AchievementCondition.orig_Complete orig, Terraria.Achievements.AchievementCondition self)
		{
			var config = ModContent.GetInstance<HEROsModServerConfig>();
			if (config.DisableAchievements)
				return;
			orig(self);
		}

		//private static Action<int> original_CustomIntCondition_Value_Set;
		private static void CustomFloatCondition_Value_set(Action<CustomFloatCondition, float> orig, CustomFloatCondition self, float val)
		{
			var config = ModContent.GetInstance<HEROsModServerConfig>();
			if (config.DisableAchievements)
				return;
			orig(self, val);
			//self.Value = val;
		}
	}
}


//using System;
//using System.Reflection;
//using MonoModExt.Utils;
//using MonoModExt.RuntimeDetour;

//public static class TestClass
//{
//	public static bool allowAchievements;

//	public static void Test()
//	{
//		var detour = CreateMethodHook(
//			typeof(CustomIntCondition).GetProperty(nameof(CustomIntCondition.Value), BindingFlags.Public | BindingFlags.Instance).SetMethod,
//			typeof(TestClass).GetMethod(nameof(CustomIntCondition_Value_set), BindingFlags.NonPublic | BindingFlags.Static),
//			out original_CustomIntCondition_Value_Set
//		);

//		//Dispose detour on Mod.Unload()
//	}

//	private static NativeDetour CreateMethodHook<TDelegate>(MethodInfo srcMethod, MethodInfo destMethod, out TDelegate trampolineDelegate) where TDelegate : Delegate
//	{
//		var delegateType = typeof(TDelegate);

//		detour = new NativeDetour(srcMethod.GetNativeStart(), destMethod);
//		detour.Undo();

//		var trampoline = detour.GenerateTrampoline(delegateType.GetMethod("Invoke"));
//		trampolineDelegate = trampoline.CreateDelegate(delegateType);

//		detour.Apply();

//		return detour;
//	}

//	private static Action<int> original_CustomIntCondition_Value_Set;
//	private static void CustomIntCondition_Value_set(int val)
//	{
//		if (allowAchievements)
//		{
//			original_CustomIntCondition_Value_Set(val);
//		}
//	}
//}
*/
