using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;
    public Vector3[] positions;
    

    void Start()
    {
        foreach (var position in positions)
        {
            InstantiateParticleSystemAtPosition(position);
        }
    }

    void InstantiateParticleSystemAtPosition(Vector3 position)
    {
        // Tạo một GameObject mới để chứa Particle System
        GameObject particleObject = new GameObject("ParticleEffect");
        particleObject.transform.position = position;

        // Thêm Particle System vào GameObject
        ParticleSystem particleSystem = Instantiate(particleSystemPrefab, particleObject.transform);

        // Nếu cần, cấu hình thêm cho Particle System
        var main = particleSystem.main;
        main.startSize = 1f; // Ví dụ: thay đổi kích thước particle

        // Bắt đầu particle system
        particleSystem.Play();
    }
}