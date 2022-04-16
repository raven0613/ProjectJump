using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;


namespace ProjectJump
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private TMPro.TextMeshProUGUI m_scoretext;
        [SerializeField] private int m_score;

        private void Awake()
        {
            m_score = 0;
            GameManager.OnScoreUp += this.OnScoreUp;
        }
        private void OnScoreUp()
        {
            m_score = m_score + 1;
        }

        private void Update()
        {
            m_scoretext.text = "" + m_score;
        }
    }
}
