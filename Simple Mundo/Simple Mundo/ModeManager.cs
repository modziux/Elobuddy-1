﻿using System;
using System.Collections.Generic;
using EloBuddy;
using EloBuddy.SDK.Enumerations;
using EloBuddy.SDK.Utils;
using Simple_Mundo.Modes;

namespace Simple_Mundo
{
    public static class ModeManager
    {
        static ModeManager()
        {
            // Initialize properties
            Modes = new List<ModeBase>();

            // Load all modes manually since we are in a sandbox which does not allow reflection
            // Order matter here! You would want something like PermaActive being called first
            Modes.AddRange(new ModeBase[]
            {
                new PermaActive(),
                new Combo(),
                new Harass(),
                new LaneClear(),
                new JungleClear(),
                new LastHit()
            });

            // Listen to events we need
            Game.OnTick += OnTick;
        }

        private static List<ModeBase> Modes { get; }

        public static void Initialize()
        {
            // Let the static initializer do the job, this way we avoid multiple init calls aswell
        }

        private static void OnTick(EventArgs args)
        {
            // Execute all modes
            Modes.ForEach(mode =>
            {
                try
                {
                    // Precheck if the mode should be executed
                    if (mode.ShouldBeExecuted())
                        mode.Execute();
                }
                catch (Exception e)
                {
                    // Please enable the debug window to see and solve the exceptions that might occur!
                    Logger.Log(LogLevel.Error, "Error executing mode '{0}'\n{1}", mode.GetType().Name, e);
                }
            });
        }
    }
}