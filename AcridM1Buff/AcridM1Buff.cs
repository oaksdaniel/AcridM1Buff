using BepInEx;
using Mono.Cecil.Cil;
using MonoMod.Cil;
using RoR2;
using System;
using R2API;
using R2API.Utils;
using System.Collections.Generic;
using UnityEngine;
using EntityStates;
using IL.EntityStates.Huntress.HuntressWeapon;
using EntityStates.Huntress.HuntressWeapon;
using IL.RoR2.Projectile;
using RoR2.Projectile;
using On.RoR2.Projectile;
using ProjectileDotZone = RoR2.Projectile.ProjectileDotZone;

namespace AcridM1Buff
{
    //Loads R2API Submodules
    [R2APISubmoduleDependency(nameof(LanguageAPI))]

    //This is an example plugin that can be put in BepInEx/plugins/ExamplePlugin/ExamplePlugin.dll to test out.
    //It's a small plugin that adds a relatively simple item to the game, and gives you that item whenever you press F2.

    //This attribute is required, and lists metadata for your plugin.
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]

    //This is the main declaration of our plugin class. BepInEx searches for all classes inheriting from BaseUnityPlugin to initialize on startup.
    //BaseUnityPlugin itself inherits from MonoBehaviour, so you can use this as a reference for what you can declare and use in your plugin class: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
    public class AcridM1Buff : BaseUnityPlugin
    {
        //The Plugin GUID should be a unique ID for this plugin, which is human readable (as it is used in places like the config).
        //If we see this PluginGUID as it is on thunderstore, we will deprecate this mod. Change the PluginAuthor and the PluginName !
        public const string PluginGUID = PluginAuthor + "." + PluginName;
        public const string PluginAuthor = "OakPrime";
        public const string PluginName = "AcridM1Buff";
        public const string PluginVersion = "0.1.0";

        private readonly Dictionary<string, string> DefaultLanguage = new Dictionary<string, string>();

        //The Awake() method is run at the very start when the game is initialized.
        public void Awake()
        {
            try
            {
                IL.EntityStates.Croco.Slash.AuthorityModifyOverlapAttack += (il) =>
                {
                    ILCursor c = new ILCursor(il);
                    c.TryGotoNext(
                        x => x.MatchCallOrCallvirt<EntityStates.BasicMeleeAttack>(nameof(EntityStates.BasicMeleeAttack.AuthorityModifyOverlapAttack)),
                        x => x.MatchRet()
                    );
                    c.EmitDelegate<Func<RoR2.OverlapAttack, RoR2.OverlapAttack>>(overlapAttack =>
                    {
                        overlapAttack.damageType = DamageType.BlightOnHit;
                        return overlapAttack;
                    });
                    //c.Index++;
                    /*c.EmitDelegate<Action<EntityStates.Croco.Slash, RoR2.OverlapAttack>>((slash, overlapAttack) =>
                    {
                        //CrocoDamageTypeController controller = slash.GetComponent<CrocoDamageTypeController>();
                        //DamageType damageType = /*(bool)(UnityEngine.Object)controller ? controller.GetDamageType() : DamageType.Generic;
                        //overlapAttack.damageType = damageType;
                        slash.step = 1;
                    });*/
                    /*c.Emit(OpCodes.Ldarg_0);
                    c.Emit(OpCodes.Ldfld, RoR2.CrocoDamageTypeController.En)
                    c.Index++;
                    c.Emit(OpCodes.Ldc_R4, 1.3f);
                    c.Emit(OpCodes.Mul);
                    c.TryGotoNext(
                        x => x.MatchDup(),
                        x => x.MatchLdcI4(0x42),
                        x => x.MatchStfld<DamageInfo>("damageType")
                    );
                    c.RemoveRange(3);*/



                };
                //this.ReplaceSecondaryText();
            }
            catch (Exception e)
            {
                Logger.LogError(e.Message + " - " + e.StackTrace);
            };
        }
        private void ReplaceSecondaryText()
        {
            this.ReplaceString("HUNTRESS_SECONDARY_DESCRIPTION", "Throw a seeking glaive that bounces up to <style=cIsDamage>6</style> times for <style=cIsDamage>250% damage</style>" +
                ". Damage increases by <style=cIsDamage>15%</style> per bounce.");
        }

        private void ReplaceString(string token, string newText)
        {
            this.DefaultLanguage[token] = Language.GetString(token);
            LanguageAPI.Add(token, newText);
        }
    }
}
