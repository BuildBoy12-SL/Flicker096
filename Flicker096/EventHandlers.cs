// -----------------------------------------------------------------------
// <copyright file="EventHandlers.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Flicker096
{
    using System.Collections.Generic;
    using Exiled.API.Features;
    using Exiled.API.Features.Roles;
    using Exiled.Events.EventArgs;
    using MEC;
    using PlayableScps;

    /// <summary>
    /// General event handlers.
    /// </summary>
    public class EventHandlers
    {
        private readonly Plugin plugin;
        private CoroutineHandle coroutineHandle;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlers"/> class.
        /// </summary>
        /// <param name="plugin">The <see cref="Plugin{TConfig}"/> class reference.</param>
        public EventHandlers(Plugin plugin) => this.plugin = plugin;

        /// <inheritdoc cref="Exiled.Events.Handlers.Scp096.OnAddingTarget(AddingTargetEventArgs)"/>
        public void OnAddingTarget(AddingTargetEventArgs ev)
        {
            if (coroutineHandle.IsRunning || !ev.Scp096.Role.Is(out Scp096Role scp096Role))
                return;

            List<FlickerableLightController> lights = new List<FlickerableLightController>();
            foreach (FlickerableLightController light in FlickerableLightController.Instances)
            {
                if ((light.gameObject.transform.position - ev.Scp096.Position).magnitude < plugin.Config.MaximumDistance)
                    lights.Add(light);
            }

            coroutineHandle = Timing.RunCoroutine(CrankLights(lights, scp096Role));
        }

        private static IEnumerator<float> CrankLights(List<FlickerableLightController> lights, Scp096Role scp096Role)
        {
            yield return Timing.WaitUntilTrue(() => scp096Role.State == Scp096PlayerState.Enraging);

            Dictionary<FlickerableLightController, float> previousIntensities = new Dictionary<FlickerableLightController, float>();
            foreach (FlickerableLightController light in lights)
                previousIntensities.Add(light, light.LightIntensityMultiplier);

            while (scp096Role.State == Scp096PlayerState.Enraging)
            {
                foreach (FlickerableLightController light in lights)
                    light.LightIntensityMultiplier = (float)Exiled.Loader.Loader.Random.NextDouble();

                yield return Timing.WaitForOneFrame;
            }

            foreach (KeyValuePair<FlickerableLightController, float> kvp in previousIntensities)
                kvp.Key.LightIntensityMultiplier = kvp.Value;
        }
    }
}