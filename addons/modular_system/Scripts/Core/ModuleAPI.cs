using System;
using Godot;

namespace ModularSystem
{
    public abstract partial class ModuleAPI : Node
    {
        protected bool IsInitialized { get; private set; } = false;

        /// <summary>
        /// Called when the module is first initialized
        /// </summary>
        public virtual void Initialize()
        {
            if (IsInitialized)
                return;

            IsInitialized = true;
        }

        /// <summary>
        /// Called when the module needs to be cleaned up
        /// </summary>
        public virtual void Cleanup()
        {
            if (!IsInitialized)
                return;

            IsInitialized = false;
        }

        /// <summary>
        /// Gets the unique identifier for this module
        /// </summary>
        public abstract string ModuleID { get; }

        /// <summary>
        /// Gets the display name of this module
        /// </summary>
        public abstract string ModuleName { get; }

        /// <summary>
        /// Gets the version of this module
        /// </summary>
        public abstract Version ModuleVersion { get; }
    }
}
