using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProjectJump
{
    public class BackgroundController : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.OnHeightReset += this.OnReset;

        }
        public void Move(PlayerManager player)
        {
            if (gameObject.transform.position.y >= GameManager.Instance.m_lowest)
            {
                gameObject.transform.position = new Vector3(0, player.transform.position.y + 75, -10);
            }

        }
        public void OnReset()
        {

            if (gameObject.transform.position.y >= GameManager.Instance.PlayerResetHeight)
            {
                gameObject.transform.position = new Vector3(0, 0, -10);
            }
        }

    }
}
