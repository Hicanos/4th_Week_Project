using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] private bool createDustOnWalk = true; //bool-파티클 생성여부
    [SerializeField] private ParticleSystem dustParticleSystem; //파티클 시스템 부르기

    public void CreateDustParticles()
    {
        //creatDustOnWalk =true 일때만 실행
        if (createDustOnWalk)
        {
            dustParticleSystem.Stop(); //이전 재생 정지
            dustParticleSystem.Play(); //후 재생
        }
    }
}
