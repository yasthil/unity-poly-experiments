using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

public class CustomMarkerEventHandler : DefaultTrackableEventHandler
{
    protected override void OnTrackingFound()
    {
        base.OnTrackingFound();
        ModelLoader.Instance.LoadAssetsIntoContainer();
    }

}
