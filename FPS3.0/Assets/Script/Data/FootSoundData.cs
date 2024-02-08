using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FPS3_GameBase
{
    [System.Serializable]
    public class SoundData
    {
        public string soundTag;
        public AudioClip[] audioClipList;
    }

    [CreateAssetMenu(menuName = "FPS3.0/Foot Sound Data")]
    public class FootSoundData : ScriptableObject
    {
        public SoundData[] footSoundList;

        private SoundData tac;
        public AudioClip GetAudioClip(string _tag)
        {
            //首先判断新传入值是否与上次传入值一致
            if (tac != null && _tag == tac.soundTag)
            {
                return tac.audioClipList[Random.Range(0, tac.audioClipList.Length)];
            }
            else
            {
                foreach (SoundData ac in footSoundList)
                {
                    if (ac.soundTag == _tag)
                    {
                        tac = ac;

                        return ac.audioClipList[Random.Range(0, ac.audioClipList.Length)];
                    }
                }
            }

            return null;
        }
    }
}
