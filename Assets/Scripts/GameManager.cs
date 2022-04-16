using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectJump
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;
        [SerializeField]private CameraManager m_camera;
        [SerializeField] private BackgroundController m_backgroundController;
       
        [SerializeField] private List<GroundManager> m_grounds;
        [SerializeField] private List<PlayerManager> m_players;
        [SerializeField] public List<GameObject> m_onsceneground;

        private void Awake()
        {
            Instance = this;
            m_grounds = new List<GroundManager>();
            m_players = new List<PlayerManager>();
            m_onsceneground = new List<GameObject>();
            
        }
        private void Start()
        {
            Debug.Log("GameStart");
            OnGameStart?.Invoke();
            
        }
        //------------------------------------------------------------------------------
        
        private void Update()
        {
            CreateGround(CreateGroundHeight);
            
            FindLowestinSceneGround();
            

            for (int i = 0; i < m_players.Count; i++)
            {
                m_players[i].DetectInput();
                m_camera.Move(m_players[i]);
                m_backgroundController.Move(m_players[i]);
                HeightReset(PlayerResetHeight);
                GameOver(m_players[i]);

                for (int j = 0; j < m_grounds.Count; j++)
                {
                    m_players[i].JumpOnSomething(m_grounds[j].GetHeight() , m_grounds[j].GetLeft() , m_grounds[j].GetRight());
                    ScoreUpCheck(m_players[i] , m_grounds[j]);
                    
                }
            }



        }
        
        //------------------------------------------------------------------------------

        public static event System.Action OnGameStart;
        public static event System.Action OnGroundTakeOut;
        public static event System.Action OnHeightReset;
        public static event System.Action OnGroundBack;
        public static event System.Action OnScoreUp;
        public static event System.Action OnPlayerDead;

        public float PlayerResetHeight; //1920
        [SerializeField] private float CreateGroundHeight;  //480

        private void HeightReset(float height)  //到某個高度的時候重置高度
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if(m_players[i].GetHeight() >= height)
                {

                    Debug.Log("invoke reset");
                    OnGroundBack?.Invoke();
                    OnHeightReset?.Invoke();
                    
                    isCreateInvoked = false;
                    
                }
            }
            
        }


        private bool isCreateInvoked = false;
        private void CreateGround(float height)  //到某個高度的時候生成接下來的地板
        {
            for (int i = 0; i < m_players.Count; i++)
            {
                if (m_players[i].GetHeight() >= height)
                {
                    if(isCreateInvoked == false)
                    {
                        Debug.Log("invoke create");
                        OnGroundTakeOut?.Invoke();
                        isCreateInvoked = true;
                    }
                }
            }
        }
        public float m_lowest = 3000;
        private void FindLowestinSceneGround()
        {
            for (int i = 0; i < m_onsceneground.Count; i++)
            {
                if (m_onsceneground[i].transform.position.y <= m_lowest)
                {
                    m_lowest = m_onsceneground[i].transform.position.y;
                }
            }
        }


        private bool isScoreUpInvoked = false;
        private void ScoreUpCheck(PlayerManager player , GroundManager ground)  
        {
            if (player.GetisOntheGroundbool() == true)
            {
                 if (player.GetHeight() >= player.m_heighteststandingPos)   //如果玩家目前高度 >= 已知玩家踩過的最高高度   
                 {

                    if (isScoreUpInvoked == false)  //如果還沒呼叫過(因為只能一次)  並且 該地板還沒踩過
                    {
                        if (ground.transform.position.y == player.transform.position.y - 60) //確定是他腳下踩的那個
                        {
                            if (ground.isStamped == false)    //還沒被踩過
                            {
                                OnScoreUp?.Invoke();                                     
                                isScoreUpInvoked = true;
                                ground.isStamped = true;
                            }
                        }


                    }
                    
                 }
            }
            else
            {
                isScoreUpInvoked = false;
            }
        }




        private void GameOver(PlayerManager player)
        {
            if(player.transform.position.y <= m_camera.GetDownEdge() - 100)
            {
                OnPlayerDead?.Invoke();
                Debug.Log("Game Over");
            }
        }

        //------------------------------------------------------------------------------


        public void PlayerRegister(PlayerManager player)
        {
            if (m_players.Contains(player))
            {
                return;
            }

            m_players.Add(player);


        }
        public void GroundRegister(GroundManager ground)
        {
            if (m_grounds.Contains(ground))
            {
                return;
            }

            m_grounds.Add(ground);
        }


        public void UnRegister(PlayerManager player)
        {
            if (!m_players.Contains(player))
            {
                return;
            }
            m_players.Remove(player);
        }

        public void UnRegister(GroundManager ground)
        {
            if (!m_grounds.Contains(ground))
            {
                return;
            }
            m_grounds.Remove(ground);
        }


    }
}
