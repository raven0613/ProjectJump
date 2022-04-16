using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectJump
{
    public class ObjectPool : MonoBehaviour                
    {
        [SerializeField] private Queue<GameObject> m_pool;
        [SerializeField] private GameObject m_prefab;
        private GameObject m_clone;
        [SerializeField] private float GroundResetHeight;
        private void Awake()
        {
            m_pool = new Queue<GameObject>();

            GameManager.OnGameStart += OnGameStart;
            GameManager.OnGroundTakeOut += OnCreate;
            GameManager.OnGroundBack += OnPutBack;

            GroundResetHeight = GameManager.Instance.PlayerResetHeight - 480;//960
        }

        [SerializeField]private bool isCreated = false;

        //---------------------------------主要功能分隔線-----------------------------------------------------------------------

        private void Create(GameObject obj , int amount) 
        {
            for(int i = 0; i < amount; i++)
            {

                GameObject _clone = Instantiate(obj);
                _clone.transform.position = new Vector3(-500, 0, 0);
                m_clone = _clone;
                m_pool.Enqueue(_clone);
                m_clone.SetActive(false);

            }
            isCreated = true;
            Debug.Log("Create done, remaining " + m_pool.Count);
        }

        [SerializeField] private bool isfirstcreated = false;
        private Vector3 m_currentPos;
        private void TakeOut(GameObject obj , int amount)  
        {
            if (m_pool.Count < amount)
            {
                int _gap = amount - m_pool.Count;
                Debug.Log("take out create");
                Create(obj , _gap);
            }
            else { }

            for (int i = 0; i < amount; i++)
            {
                m_clone = m_pool.Dequeue();
                Debug.Log("takeout, remaining" + m_pool.Count);
                if(isfirstcreated)
                {
                    m_clone.transform.position = m_currentPos + GetNewRandomPos();
                    m_currentPos = m_clone.transform.position;
                }
                else
                {
                    if (i == 0)  //第一個的位置
                    {
                        m_clone.transform.position = new Vector3(50, -140, 0);
                    }
                    else if (i >= 1)  //之後的每一個的位置
                    {
                        m_clone.transform.position = m_currentPos + GetNewRandomPos();
                        
                    }

                    m_currentPos = m_clone.transform.position;
                    
                    m_clone.SetActive(true);
                }
                m_clone.SetActive(true);
                Debug.Log(m_currentPos);
                GameManager.Instance.m_onsceneground.Add(m_clone);
                //CheckHeight(GroundResetHeight);
                Debug.Log("GroundResetHeight = "+ GroundResetHeight);
            }
            
        }

        private void Putback()
        {
            for(int i = 0; i < GameManager.Instance.m_onsceneground.Count; i++)
            {
                while(GameManager.Instance.m_onsceneground[i].transform.position.y <= GroundResetHeight)
                {
                    m_pool.Enqueue(GameManager.Instance.m_onsceneground[i]);

                    if (m_pool.Contains(GameManager.Instance.m_onsceneground[i]))
                    {
                        GameManager.Instance.m_onsceneground[i].SetActive(false);
                        GameManager.Instance.m_onsceneground[i].transform.position = new Vector3(-500, 0, 0);
                        
                    }
                    GameManager.Instance.m_onsceneground.Remove(GameManager.Instance.m_onsceneground[i]);
                }


            }
 
        }

        //---------------------------------事件處理器分隔線-----------------------------------------------------------------------

        private void OnGameStart()
        {

            Debug.Log("on GameStart(objtool)");
            isCreated = false;

            Create(m_prefab, 12);
            if (isCreated == true)
            {
                TakeOut(m_prefab, 12);
                isfirstcreated = true;
            }
        }
        private void OnPutBack()
        {
            Putback();

            OnReset();
        }
        private void OnReset()  //為什麼這邊呼叫兩次
        {
            Debug.Log("On Reset");
            m_currentPos.y = m_currentPos.y - GameManager.Instance.PlayerResetHeight;

            Debug.Log(GameManager.Instance.PlayerResetHeight);
            Debug.Log(m_currentPos);
        }

        private void OnCreate()
        {
            TakeOut(m_prefab,8);
        }




        //---------------------------------小功能分隔線-----------------------------------------------------------------------

        private Vector3 GetNewRandomPos()
        {
            float _gapfromxto0 = 0 - m_currentPos.x;
            

            float _randomx;
            if (m_currentPos.x >=0)
            {
                _randomx = Random.Range(50, 200) * -1;
            }
            else
            {
                _randomx = Random.Range(50, 200);
                
            }


            float _randomy = Random.Range(100, 250);

            return new Vector3(_gapfromxto0 + _randomx , _randomy , 0);

        }  //_randomx的數字就是下一個地板的x軸

        
        [SerializeField] List<GameObject> m_back = new List<GameObject>();
        //private void CheckHeight(float height)
        //{

        //    if (m_clone.transform.position.y < height) 
        //    {
        //        m_back.Add(m_clone);
        //    }
        //}
        //private void ResetObj()
        //{
        //    for (int i = 0; i < GameManager.Instance.m_onsceneground.Count; i++)
        //    {
        //     GameManager.Instance.m_onsceneground[i].transform.position = GameManager.Instance.m_onsceneground[i].transform.position + new Vector3(0 , - 1745 , 0);
             
        //    }
            
        //}

    }
}