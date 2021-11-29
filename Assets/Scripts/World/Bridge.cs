using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour
{
    [SerializeField] private float _movementDuration;

    [SerializeField] private Transform _limit;

    [SerializeField] private List<Tile> _bridgeTiles;

    [SerializeField] private List<Character> _bridgeEnemies = new List<Character>();

    private Vector3 _startPosition;

    private void Awake()
    {
        foreach (var enemy in _bridgeEnemies)
        {
            enemy.DisableUnit();
        }
    }

    private void Start()
    {
        _startPosition = transform.position;
        foreach (var tile in _bridgeTiles)
        {
            tile.gameObject.SetActive(true);
            tile.RemoveFromNeighbour();
            tile.gameObject.SetActive(false);
        }
    }

    public void StartMovement()
    {
        Debug.Log("start movement");
        EffectsController.Instance.PlayParticlesEffect(this.gameObject, EnumsClass.ParticleActionType.MovingBridge);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float time = 0;

        Debug.Log("move");
        while (time <= _movementDuration)
        {
            Debug.Log("moving");
            time += Time.deltaTime;
            var normalizedTime = time / _movementDuration;

            transform.position = Vector3.Lerp(_startPosition, _limit.position, normalizedTime);

            yield return new WaitForEndOfFrame();
        }
        AudioManager.audioManagerInstance.StopSoundWithFadeOut(this.gameObject.GetComponent<AudioSource>().clip, this.gameObject);

        foreach (var tile in _bridgeTiles)
        {
            tile.gameObject.SetActive(true);
            tile.AddToNeighbour();
        }

        foreach (var character in _bridgeEnemies)
        {
            character.EnableUnit();

            var portrait = PortraitsController.Instance.GetCharacterPortrait(character);

            if (portrait) portrait.selectionButton.interactable = true;
        }
    }
}
