// -----------------------------------------------------------------------
// <copyright file="Config.cs" company="Build">
// Copyright (c) Build. All rights reserved.
// Licensed under the CC BY-SA 3.0 license.
// </copyright>
// -----------------------------------------------------------------------

namespace Flicker096
{
    using System.ComponentModel;
    using Exiled.API.Interfaces;

    /// <inheritdoc />
    public class Config : IConfig
    {
        /// <inheritdoc/>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Gets or sets the maximum distance between 096 and a light controller for the light to be affected.
        /// </summary>
        [Description("The maximum distance between 096 and a light controller for the light to be affected.")]
        public float MaximumDistance { get; set; } = 30f;
    }
}