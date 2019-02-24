using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Manybits
{
    public abstract class Window : MonoBehaviour
    {
        private bool isOpen;

        public bool IsOpen { get { return isOpen; } }



        public virtual void Open()
        {
            isOpen = true;
        }

        public virtual void Close()
        {
            isOpen = false;
        }
    }
}
