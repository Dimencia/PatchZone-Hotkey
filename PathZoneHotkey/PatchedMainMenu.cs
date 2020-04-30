using HarmonyLib;
using Service.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserInterface;

namespace PatchZoneHotkey
{
    [HarmonyPatch(typeof(UIBase), "Tick")]
    class PatchedMainMenu
    {
        private static string workingDir = $"{Environment.CurrentDirectory}{Path.DirectorySeparatorChar}PatchZone";
        private static string path = $"{workingDir}{Path.DirectorySeparatorChar}PatchZone.GUI.exe";
        public static void Postfix(UIBase __instance)
        {
            if (__instance is UIMainMenu && ServiceMapper.userInputService.ButtonDown(Service.UserInput.Button.HideUI))
            {
                // Open PatchZone form... this will take away focus so we don't have to worry about opening it multiple times
                PatchZoneHotkey.Context.Log.Log($"Loading {path}");
                // This feels gross but it does the job.  Otherwise it has the wrong base folder and is a different instance
                var temp = Environment.CurrentDirectory;
                Environment.CurrentDirectory = workingDir;
                Process.Start(path);
                Environment.CurrentDirectory = temp;
            }
            
        }
    }
}
