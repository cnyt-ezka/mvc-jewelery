using System.Collections.Generic;
using Runtime.Entity.Source.Processors;
using Runtime.Enums.Source;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Runtime.Signals
{
    public class SourceSignals
    {
        public Signal<List<AssetID>> InstallationCompleted = new ();
        public Signal<Transform> TrashProductActivated = new ();
        public Signal<Transform, IProcessor> TryToConsumeForProcessor = new();
    }
}