using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using kyon;

namespace kyon
{
    public class SkillTree : SingletonMonoBehaviour<SkillTree>
    {
        [SerializeField] private Camera skillTreeCamera;
        private SkillNode selectedNode;

        [SerializeField] private GameObject ExplanationWindow;
        [SerializeField] private TextMeshProUGUI SkillNameText;
        [SerializeField] private TextMeshProUGUI ExplanationText;
        [SerializeField] private TextMeshProUGUI CostText;

        [SerializeField] private TextMeshProUGUI rainbowPieceText;

        [SerializeField] private Button ActivateButton;

        [SerializeField] private SkillNode[] nodes = new SkillNode[12];

        public static int fireLevel = 0;
        public static int iceLevel = 0;
        public static int thunderLevel = 0;

        protected override void Awake()
        {
            ResetNode();
        }

        private void OnEnable()
        {
            foreach (SkillNode node in nodes)
            {
                if (node.IsAvailable() && !node.isActive)
                    node.BecomeAvailable();
                else
                    node.BecomeInavailable();
            }
        }

        public bool IsExistsAvailableNode()
        {
            foreach (SkillNode node in nodes)
            {
                if (node.IsAvailable() && !node.isActive)
                    return true;
            }
            return false;
        }

        public void ResetNode()
        {
            foreach (SkillNode node in nodes)
            {
                node.isActive = false;
                node.BecomeInavailable();
            }
            ExplanationWindow.SetActive(false);
            selectedNode = null;
            skillTreeCamera.transform.position = new Vector3(transform.position.x, transform.position.y, -10);
            ActivateButton.gameObject.SetActive(false);

            fireLevel = 0;
            iceLevel = 0;
            thunderLevel = 0;
        }

        public void SelectNode(SkillNode node)
        {
            selectedNode = node;
            skillTreeCamera.transform.DOMove(new Vector3(node.transform.position.x, node.transform.position.y, -10), 1);
            ExplanationWindow.SetActive(true);
            SkillNameText.text = node.skillName;
            ExplanationText.text = node.explanation;
            CostText.text = "Cost:" + node.cost.ToString();
            if ((node.isActive || !selectedNode.IsAvailable()) && !GameManager.Instance.isDebug)
                ActivateButton.gameObject.SetActive(false);
            else
                ActivateButton.gameObject.SetActive(true);
        }

        public void ActivateNode()
        {
            if (!selectedNode.IsAvailable() && ! GameManager.Instance.isDebug) return;
            selectedNode.Activate();
            ActivateButton.gameObject.SetActive(false);

            switch (selectedNode.type)
            {
                case SkillNode.NodeType.Fire:
                    if (fireLevel == 0)
                    {
                        BallManager.Instance.availableBallList.Add(BallManager.BallType.Multi);
                    }
                    fireLevel++;
                    break;
                case SkillNode.NodeType.Ice:
                    if (iceLevel == 0)
                    {
                        BallManager.Instance.availableBallList.Add(BallManager.BallType.Big);
                    }
                    iceLevel++;
                    break;
                case SkillNode.NodeType.Thunder:
                    if (thunderLevel == 0)
                    {
                        BallManager.Instance.availableBallList.Add(BallManager.BallType.Small);
                    }
                    thunderLevel++;
                    break;
            }

            foreach (SkillNode node in nodes)
            {
                if (node.IsAvailable() && !node.isActive)
                    node.BecomeAvailable();
                else
                    node.BecomeInavailable();
            }
        }

        public void Exit()
        {
            ExplanationWindow.SetActive(true);
            selectedNode = null;
            skillTreeCamera.transform.position = transform.position;
            ActivateButton.gameObject.SetActive(false);
        }

        private void Update()
        {
            rainbowPieceText.text = InfomationCanvas.Instance.rainbowPieceNum.ToString();
        }
    }
}