using UnityEngine;
using Netick;

namespace Netick.Samples.Bomberman
{
    public class Block : NetworkBehaviour
    {
        // Networked properties
        [Networked]
        public bool Visible { get; set; } = true;

        [OnChanged(nameof(Visible))]
        private void OnVisibleChanged(bool previous)
        {
            // for visual components, don't use "enabled" property when you want to disable/enable it, instead use SetEnabled().
            // -- GetComponent<Renderer>().enabled = Visible; #### Not like this.

            GetComponent<Renderer>().SetEnabled(Sandbox, Visible); // #### Like this.

            GetComponent<BoxCollider>().enabled = Visible;
        }
    }
}

