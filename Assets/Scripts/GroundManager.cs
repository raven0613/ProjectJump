using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectJump
{
    public class GroundManager : MonoBehaviour
    {
        public bool isStamped;

        private void Start()
        {
            GameManager.Instance.GroundRegister(this);

            GameManager.OnHeightReset += OnReset;

        }

        public float GetHeight()
        {
            return gameObject.transform.position.y + 60;
        }
        public float GetLeft()
        {
            return gameObject.transform.position.x - 50;
        }
        public float GetRight()
        {
            return gameObject.transform.position.x + 50;
        }


        private void OnReset()
        {
            isStamped = false;
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - GameManager.Instance.PlayerResetHeight, 0);

        }//1745是 兩張地圖共1920 - player初始高度175 = 1745

    }
}
