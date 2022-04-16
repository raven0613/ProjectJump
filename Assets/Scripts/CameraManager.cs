using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectJump
{
    public class CameraManager : MonoBehaviour
    {
        private void Awake()
        {
            GameManager.OnHeightReset += this.OnReset;

        }

        public void Move(PlayerManager player) 
        {
            if(gameObject.transform.position.y >= GameManager.Instance.m_lowest)
            {
                gameObject.transform.position = new Vector3(0, player.transform.position.y + 75, -10);
            }
            
        }

        public float GetUpEdge() //active ground
        {
            return gameObject.transform.position.y + 600;
        }
        
        //public float GetDestroyUpEdge() //destroy ground
        //{
        //    return gameObject.transform.position.y - 800;
        //}
        public float GetDownEdge() //destroy ground
        {
            return gameObject.transform.position.y - 480;
        }

        public void OnReset()
        {
            Debug.Log("Camera Reset");
            if (gameObject.transform.position.y >= GameManager.Instance.PlayerResetHeight)
            {
                gameObject.transform.position = new Vector3(0, 0, -10);
            }
        }
    }
}
