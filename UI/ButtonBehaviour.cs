using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using kyon;

namespace kyon
{
	public class ButtonBehaviour : MonoBehaviour
	{
        private SimpleAnimation anim;

        private void Start()
        {
            anim = GetComponent<SimpleAnimation>();
        }

        public void OnPointerEnter(string clipName)
        {
            anim.Play(clipName);
        }
        public void OnPointerExit(string clipName)
        {
            anim.Play(clipName);
        }
    }
}