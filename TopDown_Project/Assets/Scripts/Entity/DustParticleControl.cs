using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleControl : MonoBehaviour
{
    [SerializeField] private bool createDustOnWalk = true; //bool-��ƼŬ ��������
    [SerializeField] private ParticleSystem dustParticleSystem; //��ƼŬ �ý��� �θ���

    public void CreateDustParticles()
    {
        //creatDustOnWalk =true �϶��� ����
        if (createDustOnWalk)
        {
            dustParticleSystem.Stop(); //���� ��� ����
            dustParticleSystem.Play(); //�� ���
        }
    }
}
