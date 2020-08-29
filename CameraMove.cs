using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using kyon;

namespace kyon
{
    public class CameraMove : SingletonMonoBehaviour<CameraMove>
    {
        [SerializeField] private Transform player;
        private bool seek = false;
        private bool s = false;
        private float t = 0;

        private void Update()
        {
            if (seek)
            {
                if (!s)
                    transform.position = new Vector3(player.position.x, player.position.y, -10);
                else
                    transform.position = Vector3.Lerp(Vector2.zero, new Vector3(player.position.x, player.position.y, -10), t);
            }
        }

        public async void SwitchSeek()
        {
            seek = true;
            s = true;
            t = 0;
            DOTween.To(() => t, num => t = num, 1, 1);
            await GameTimer.WaitForSeconds(1);
            s = false;
        }
        public void ResetCamera()
        {
            seek = false;
            s = false;
            t = 0;
            transform.DOMove(new Vector3(0, 0, -10), 1);
        }
    }
}