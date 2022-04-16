using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectJump
{
    public class PlayerManager : MonoBehaviour
    {
        private void Start()
        {
            GameManager.Instance.PlayerRegister(this);
            
            m_isOntheGround = true;
            gameObject.transform.position = new Vector3(50, -75, 1);

            GameManager.OnHeightReset += this.OnReset;
        }


        public void DetectInput()
        {
            if (Input.GetMouseButton(0))
            {
                ChargePower();
            }

            if (Input.GetMouseButtonUp(0))
            {

                Debug.Log("m_chargedpower" + " = " + m_chargedpower);
                m_isjumping = true;
                m_isOntheGround = false;
                GetChargedPower();
            }
            if (m_isjumping)
            {
                Jump(ref m_chargingpower);
                ControlDirection(300);
            }
            CheckDirection();
        }

        [SerializeField] private float m_chargingpower;
        [SerializeField] private float m_chargedpower;  

        private float m_floor;

        [SerializeField] private bool m_isjumping;
        [SerializeField] private bool m_isOntheGround;

        private void ChargePower()
        {
            m_chargingpower = m_chargingpower + 15;
            
            if (m_chargingpower >= 3000)
            {
                m_chargingpower = 3000;
            }
        }


        [SerializeField] private float m_originalPosition;

        private void Jump(ref float chargingpower)
        {
            float gravity = 19.6f;

            chargingpower = chargingpower - gravity;
            m_originalPosition = gameObject.transform.position.y;
            gameObject.transform.position = gameObject.transform.position + Vector3.up * chargingpower * Time.deltaTime;


        }
        [SerializeField] private float m_direction;
        private void CheckDirection()
        {
            if (m_isOntheGround)
            {
                if (gameObject.transform.position.x >= 0)
                {
                    m_direction= -1;
                }
                else
                {
                    m_direction = 1;
                }
            }
        }

        private void ControlDirection(float speed)
        {
            gameObject.transform.position = gameObject.transform.position + Vector3.right * speed * m_direction  * Time.deltaTime;
        }



        public bool m_isJumpedOn;
        public float m_heighteststandingPos = -1500;
        public void JumpOnSomething(float targetRangeLine , float targetLeft , float targetRight)
        {
            if (gameObject.transform.position.x >= targetLeft && gameObject.transform.position.x <= targetRight)  //如果在目標地板的左右範圍內
            {
                if(gameObject.transform.position.y < targetRangeLine && targetRangeLine < m_originalPosition)    //如果在目標地板的上方
                 {
                     if (m_chargingpower <= 0)
                     {
                         m_chargingpower = 0;

                        if(gameObject.transform.position.y <= targetRangeLine)    
                        {
                            gameObject.transform.position = new Vector3(gameObject.transform.position.x, targetRangeLine, gameObject.transform.position.z);  //讓玩家站定位


                           
                            if (targetRangeLine > m_heighteststandingPos)  //判定是不是目前踩到的最高點
                            {
                                m_heighteststandingPos = targetRangeLine;   //紀錄目前踩到的最高點
                            }
                        }

                         m_isOntheGround = true;
                         m_isjumping = false;
                        
                     }
                 }
            }
        }
        public void OnReset()
        {
            Debug.Log("player reset");
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - GameManager.Instance.PlayerResetHeight/* + 175*/, 0);
            m_heighteststandingPos = m_heighteststandingPos - GameManager.Instance.PlayerResetHeight;

        }

        //------------------------------get property-------------------------------------------

        private float GetChargedPower()
        {
            m_chargedpower = m_chargingpower;
            return m_chargedpower;
        }

        public bool GetisOntheGroundbool()
        {
            if(m_isOntheGround)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public float GetHeight()
        {
            return gameObject.transform.position.y;
        }

        public bool GetisUpingbool()
        {
            if (m_chargingpower >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
                
        }
    }
}
