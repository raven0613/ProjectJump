using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectJump
{
    public class GroundCreator : MonoBehaviour
    {
        public List<GroundManager> _clones;

        private void Start()
        {
            _clones = new List<GroundManager>();
        }
    

      public void Create(GroundManager things, float height, float targetheight)
      {
        if (height >= targetheight)
        {
            if (_clones.Count >= 8)
            {
                return;
            }

            Instantiate(things, new Vector3(0, 2400, 0), new Quaternion(0, 0, 0, 0));

            if (_clones.Contains(things))
            {
                return;
            }
            _clones.Add(things);
        }

       }
    }

}
