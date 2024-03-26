using Fusion;
using UnityEngine;

public class LevelGenerationManager : NetworkBehaviour
{
    [SerializeField] private GameObject _startPrefab;
    [SerializeField] private GameObject _finishPrefab;
    [SerializeField] private GameObject[] _levelParts;
    [SerializeField] private int _levelLength = 10;
    [SerializeField] private Vector3 _levelPartSizes;


    public override void Spawned()
    {
        if (Runner.LocalPlayer.PlayerId == 1)
        {
            Vector3 startPosition = Vector3.zero;

            NetworkObject obj = Runner.Spawn(_startPrefab, startPosition);
            obj.transform.parent = transform;

            for (int i = 1; i < (_levelLength - 1); i++)
            {
                int rundomNum = Random.Range(0, _levelParts.Length);
                obj = Runner.Spawn(_levelParts[rundomNum], startPosition + _levelPartSizes * i);
                obj.transform.parent = transform;
            }

            obj = Runner.Spawn(_finishPrefab, startPosition + _levelPartSizes * (_levelLength - 1));
            obj.transform.parent = transform;
        }
    }
}
