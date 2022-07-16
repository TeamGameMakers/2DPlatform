using System;
using UnityEngine;

namespace Core
{
    public class CoreComponent : MonoBehaviour
    {
        internal GameCore core;

        protected virtual void Awake()
        {
            // if (transform.parent.TryGetComponent<GameCore>(out core))
            //     Debug.LogError("Missing Core Component On Parent!");
        }

    }
}
