using HarmonyLib;
using Service.Localization;
using Service.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInterface;
using UnityEngine;

namespace PatchZoneHotkey
{
    [HarmonyPatch(typeof(UIBase), "Tick")]
    class PatchedMainMenu
    {

        private static bool done = false;
        private static string workingDir = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}PatchZone";
        private static string path = $"{workingDir}{Path.DirectorySeparatorChar}PatchZone.GUI.exe";
        public static void Postfix(UIBase __instance)
        {
            if (__instance is UIMainMenu menu && !done)
            {
                // It takes some time for localization to kick in, so we basically retry every tick until it's English
                // If it's not ever English, this never works, which is probalby better than having one english button
                // If we try to change it before it localizes, it forces it to match what its enum says it should be

                // This is super messy but meh.  It works I guess.

                var components = menu.gameObject.GetComponentsInChildren<Component>();

                foreach (var c in components)
                {
                    if (c is TMPro.TextMeshProUGUI tmp)
                    {
                        if (tmp.text == "Quit Game")
                        {
                            // We want to duplicate its parent's gameobject, into its parent's parent...
                            // So first, make a copy and put it there

                            var duplicate = UnityEngine.Object.Instantiate(tmp.transform.parent.gameObject, tmp.transform.parent.parent);
                            // We'll make the old one for mods, the new one is Quit since it's going at the bottom
                            tmp.text = "Mod Setup";

                            RectTransform rt = duplicate.GetComponent<RectTransform>();
                            duplicate.transform.localPosition = new Vector3(duplicate.transform.localPosition.x, duplicate.transform.localPosition.y - rt.sizeDelta.y, duplicate.transform.localPosition.z);
                            PatchZoneHotkey.Context.Log.Log(duplicate.GetType().ToString());

                            tmp.transform.parent.GetComponent<GMButton>().onClick.RemoveAllListeners();
                            tmp.transform.parent.GetComponent<GMButton>().onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
                            {
                                var temp = Environment.CurrentDirectory;
                                Environment.CurrentDirectory = workingDir;
                                Process.Start(path);
                                Environment.CurrentDirectory = temp;

                            }));
                            done = true;
                        }
                    }
                }
            }


        }
    }


}
