using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using DG.Tweening;
using UnityEngine.UI;
using kyon;

namespace kyon
{
    public class SkillNode : MonoBehaviour
    {
        public Bullet bullet;
        public int cost;
        public bool isAvailable = false;

        public bool isActive = false;
        
        public string skillName;
        public string explanation;

        [SerializeField] private Light2D pointLight;
        [SerializeField] private List<SkillNode> Premise = new List<SkillNode>();

        public bool IsAvailable()
        {
            foreach(SkillNode node in Premise)
            {
                if (!node.isActive) return false;
            }
            if (GameManager.rainbowPieceNum < cost) return false;

            return true;
        }

        public enum NodeType
        {
            Fire,
            Ice,
            Thunder
        }
        public NodeType type;

        public void OnClick()
        {
            SkillTree.Instance.SelectNode(this);
        }

        public void Activate()
        {
            if (isActive) return;
            isActive = true;
            PlayerShot.Instance.bulletList.Add(bullet);
            PlayerShot.Instance.bulletDictionary.Add(bullet, true);
            GameManager.rainbowPieceNum -= cost;
            InfomationCanvas.Instance.SetRainbowPieceText();
            DOTween.To(() => pointLight.pointLightOuterRadius, num => pointLight.pointLightOuterRadius = num, 2, 1);
            DOTween.To(() => pointLight.intensity, num => pointLight.intensity = num, 1, 1);
        }

        public void BecomeAvailable()
        {
            if (isActive) return;
            DOTween.To(() => pointLight.pointLightOuterRadius, num => pointLight.pointLightOuterRadius = num, 1, 1);
            DOTween.To(() => pointLight.intensity, num => pointLight.intensity = num, 1, 1);
        }
        public void BecomeInavailable()
        {
            if (isActive) return;
            DOTween.To(() => pointLight.pointLightOuterRadius, num => pointLight.pointLightOuterRadius = num, 1, 1);
            DOTween.To(() => pointLight.intensity, num => pointLight.intensity = num, 0.5f, 1);
        }
    }
}